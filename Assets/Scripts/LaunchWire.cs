using UnityEngine;

public class LaunchWire : MonoBehaviour
{
    //Wireのオブジェクト
    public GameObject wirePrefab;
    bool isWireLaunched = false;

    HingeJoint2D hinge;

    //Wireのスケール
    public Vector3 wireScale = new Vector3(1, 1, 1);
    Rigidbody2D rb;
    //ジャンプする時に与える力
    public float jumpForce = 10f;

    private bool onGround = false; // 地面にいるかどうかのフラグ


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //マウスの左クリックを検出し、クリックした位置にワイヤーを生成（2D用に修正）
        if (Input.GetMouseButtonDown(0))
        {
            if (onGround) // 地面にいる場合のみジャンプを実行
            {
                Jump(); // ジャンプを実行
            }else{
                if(!isWireLaunched){
                    isWireLaunched = true; // ワイヤーが生成されたことを記録
                    //マウスの位置を取得し、2Dワールド座標に変換
                    Vector3 mousePosition = Input.mousePosition;
                    Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 0f));
                    mouseWorldPosition.z = 0f; // 2Dなのでz座標は0に固定
                    //ワイヤーを生成
                    LaunchWirePrefab(mouseWorldPosition);
                }
            }
        }
        if(Input.GetMouseButtonUp(0) && isWireLaunched)
        {
            if(hinge != null)
            {
                Destroy(hinge); // 既存のHingeJoint2Dを削除
            }
            isWireLaunched = false; // マウスボタンが離されたらワイヤー生成フラグをリセット
            Jump(); // ジャンプを実行
        }
    }
    //wirePrefabを生成し、生成したオブジェクトのY軸をクリックした地点の方に向ける
    public void LaunchWirePrefab(Vector3 mousePosition)
    {
        GameObject wire = Instantiate(wirePrefab, new Vector3 (transform.position.x, transform.position.y,0), Quaternion.identity);
        // ワイヤーのスケールを設定
        wire.transform.localScale = wireScale;
        // 自分の位置からマウス位置への方向ベクトル
        Vector2 toMouse = mousePosition - transform.position;
        // ワイヤーのY軸（上方向）がマウス方向を向くようにZ軸回転を計算
        float angle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg - 90f;
        wire.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //prefabのHinge2Dを取得しConnecttedRigidBodyとしてこのオブジェクトを設定
        hinge = wire.GetComponent<HingeJoint2D>();
        if (hinge != null)
        {
            hinge.connectedBody = rb;
        }
        else
        {
            Debug.LogError("HingeJoint2D component not found on the wire prefab.");
        }
    }

    //Y軸方向にJumpForceの力を与える
    public void Jump()
    {
        if (!isWireLaunched)
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 地面に接触したときの処理
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = true; // 地面にいるフラグを立てる
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        // 地面から離れたときの処理
        if (collision.gameObject.CompareTag("Ground"))
        {
            onGround = false; // 地面にいないフラグをオフ
        }
    }
}
