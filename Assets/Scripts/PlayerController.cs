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
	private float xSpeed; // X方向移動速度
	public bool rightFacing { get; private set; } // 向いている方向(true:右向き false:左向き)
	[SerializeField] private float jumpPower = 5.0f; // ジャンプ力
	[SerializeField] private LayerMask groundLayer = 1 << 0;
	[SerializeField] private float groundNormalMinY = 0.65f;

	private bool isGrounded;
	private int groundContactCount;

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
		// ジャンプ操作
		if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
		{
			rigidbody2D.linearVelocity = new Vector2(rigidbody2D.linearVelocity.x, jumpPower);
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		EvaluateGroundContact(collision, true);
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		EvaluateGroundContact(collision, true);
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		EvaluateGroundContact(collision, false);
	}

	private void EvaluateGroundContact(Collision2D collision, bool isEntering)
	{
		if (((1 << collision.gameObject.layer) & groundLayer) == 0)
		{
			return;
		}

		foreach (var contact in collision.contacts)
		{
			if (contact.normal.y >= groundNormalMinY)
			{
				if (isEntering)
				{
					groundContactCount++;
				}
				else if (groundContactCount > 0)
				{
					groundContactCount--;
				}
				break;
			}
		}

		isGrounded = groundContactCount > 0;
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