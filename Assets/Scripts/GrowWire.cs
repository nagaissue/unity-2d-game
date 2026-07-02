using UnityEngine;

public class GrowWire : MonoBehaviour
{
    //WirePregabのオブジェクト
    public GameObject wirePrefab;
    //Connectorのオブジェクト
    public GameObject connectorPrefab;
    //Prefabを生成したかどうか
    private bool isWireGrown = false;
    //Wireを生成する位置のオフセット
    public Vector3 wireOffset = new Vector3(0, 0.5f, 0);
    // Rigidbody2Dコンポーネント
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    //生成のインターバル
    public float spawnInterval = 0.1f; // 生成間隔（秒）
    private float startTime;
    //なにかに接触しているかどうかのフラグ
    private bool isColliding = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startTime = Time.time;
        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            //マウスの左クリックが離されたらワイヤーを生成
            if (!isWireGrown)
            {
                isWireGrown = true;
            }
        }
        if(!isWireGrown)
        {
            //コリジョンに触れていないかマウスの左クリックがホールドされていたらワイヤーを生成
            if (isColliding)
            {
                SetConnectorPrefab();
            }else{
                if(startTime + spawnInterval > Time.time)
                {
                    return; // 生成間隔が経過していない場合は何もしない
                }
                // マウスのワールド座標を取得
                Vector3 mouseScreen = Input.mousePosition;
                Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, 0f));
                mouseWorld.z = 0f;
                GrowWirePrefab(mouseWorld);
            }
            isWireGrown = true; // ワイヤーが生成されたことを記録
            Destroy(boxCollider); // BoxCollider2Dを削除
        }

    }

    public void SetConnectorPrefab()
    {
        // ローカル座標のオフセットをワールド座標に変換（localScaleも考慮）
        Vector3 worldOffset = transform.TransformVector(wireOffset);
        //ワイヤーの生成位置にConnectorを生成
        GameObject connector = Instantiate(connectorPrefab, transform.position + worldOffset, Quaternion.identity);
        //Connectorのスケールを設定
        connector.transform.localScale = transform.localScale;
        //prefabのHinge2Dを取得しConnecttedRigidBodyとしてこのオブジェクトを設定
        HingeJoint2D hinge = connector.GetComponent<HingeJoint2D>();
        if (hinge != null)
        {
            hinge.connectedBody = rb;
        }
        else
        {
            Debug.LogError("HingeJoint2D component not found on the wire prefab.");
        }
    }

    //ローカル座標のオフセット位置にワイヤーを生成しY軸をマウス方向に向ける
    public void GrowWirePrefab(Vector3 targetPosition)
    {
        // ローカル座標のオフセットをワールド座標に変換（localScaleも考慮）
        Vector3 worldOffset = transform.TransformVector(wireOffset);
        //ワイヤーを生成
        GameObject wire = Instantiate(wirePrefab, transform.position + worldOffset, Quaternion.identity);
        // ワイヤーのスケールを設定
        wire.transform.localScale = transform.localScale;
        // 自分の位置からマウス位置への方向ベクトル
        Vector2 toMouse = (Vector2)(targetPosition - (transform.position + worldOffset));
        float angle = Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg - 90f;
        wire.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        //prefabのHinge2Dを取得しConnecttedRigidBodyとしてこのオブジェクトを設定
        HingeJoint2D hinge = wire.GetComponent<HingeJoint2D>();
        if (hinge != null)
        {
            hinge.connectedBody = rb;
        }
        else
        {
            Debug.LogError("HingeJoint2D component not found on the wire prefab.");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        //タグがPlayerのオブジェクトに接触したときの処理
        if (collision.gameObject.CompareTag("Player"))
        {
            return; // Playerに接触した場合は何もしない
        }
        isColliding = true; // 何かに接触したときのフラグを立てる
    }


}
