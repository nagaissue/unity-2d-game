using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー操作・制御クラス
/// </summary>
public class PlayerController : MonoBehaviour
{
	// オブジェクト・コンポーネント参照
	private Rigidbody2D rigidbody2D;
	private SpriteRenderer spriteRenderer;

	// 移動関連変数
	[HideInInspector] public float xSpeed; // X方向移動速度
	[HideInInspector] public bool rightFacing; // 向いている方向(true.右向き false:左向き)
	[SerializeField] float jumpPower = 5.0f; // ジャンプ力

	//private Sensor_ActorController   groundSensor;
	private bool isGrounded = true; // 地面に接地しているかどうか
	//private bool isWall = false; // 壁に接触しているかどうか

	// Start（オブジェクト有効化時に1度実行）
	void Start()
	{
		// コンポーネント参照取得
		rigidbody2D = GetComponent<Rigidbody2D> ();
		spriteRenderer = GetComponent<SpriteRenderer> ();
		//groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_ActorController>();
		
		// 変数初期化
		rightFacing = true; // 最初は右向き
	}

    // Update（1フレームごとに1度ずつ実行）
    void Update()
	{
		// 左右移動処理
		MoveUpdate ();
		// ジャンプ入力処理
		JumpUpdate();
	}
	
	/// <summary>
	/// Updateから呼び出される左右移動入力処理
	/// </summary>
	private void MoveUpdate ()
	{
		// X方向移動入力
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.D))
		{// 右方向の移動入力
			// X方向移動速度をプラスに設定
			xSpeed = 6.0f;

			// 右向きフラグon
			rightFacing = true;

			// スプライトを通常の向きで表示
			spriteRenderer.flipX = false;
		}
		else if (Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.A))
		{// 左方向の移動入力
			// X方向移動速度をマイナスに設定
			xSpeed = -6.0f;

			// 右向きフラグoff
			rightFacing = false;

			// スプライトを左右反転した向きで表示
			spriteRenderer.flipX = true;
		}
		else
		{// 入力なし
			// X方向の移動を停止
			xSpeed = 0.0f;
		}
	}

	/// <summary>
	/// Updateから呼び出されるジャンプ入力処理
	/// </summary>
	private void JumpUpdate ()
	{
		/*
		// 接地していない状態で、接地センサーが接地している場合
		if (!isGrounded && groundSensor.State())
        {
            isGrounded = true;
        }

        // 接地している状態で、接地センサーが接地していない場合
        if (isGrounded && !groundSensor.State())
        {
            isGrounded = false;
        }
		*/

		// ジャンプ操作
		if (Input.GetKeyDown("space") && isGrounded) // スペースキーが押され、かつ接地している場合
		{// ジャンプ開始
			//isGrounded = false; // 接地フラグをfalseにする
			// ジャンプ力を適用
			rigidbody2D.linearVelocity = new Vector2 (rigidbody2D.linearVelocity.x, jumpPower);
			//groundSensor.Disable(0.2f); // 接地センサーを0.2秒間無効化
		}
	}

	// FixedUpdate（一定時間ごとに1度ずつ実行・物理演算用）
	private void FixedUpdate ()
	{
		// 移動速度ベクトルを現在値から取得
		Vector2 velocity = rigidbody2D.linearVelocity;
		// X方向の速度を入力から決定
		velocity.x = xSpeed;

		// 計算した移動速度ベクトルをRigidbody2Dに反映
		rigidbody2D.linearVelocity = velocity;
	}

	/*
	/// <summary>
	/// コライダーを持つ他のオブジェクトと接触した瞬間に呼び出されるイベント
	/// </summary>
	private void OnCollisionEnter2D(Collision2D collision)
	{
		// 接触した相手のタグが "Wall" だった場合、壁接触フラグを true にする
		if (collision.gameObject.CompareTag("Wall"))
		{
			isWall = true;
			Debug.Log("壁に接触しました");
		}
	}

	/// <summary>
	/// コライダーを持つ他のオブジェクトから離れた瞬間に呼び出されるイベント
	/// </summary>
	private void OnCollisionExit2D(Collision2D collision)
	{
		// 離れた相手のタグが "Wall" だった場合、壁接触フラグを false にする
		if (collision.gameObject.CompareTag("Wall"))
		{
			isWall = false;
			Debug.Log("壁から離れました");
		}
	}
	*/
}