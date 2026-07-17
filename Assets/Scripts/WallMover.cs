using UnityEngine;

/// <summary>
/// スポーンされた個々のWallを左方向へ流す。
/// GameSpeedManagerの速度を参照するので、同時に何個Wallが存在しても全て同じ速度で流れる。
/// Rigidbody2D（Kinematic推奨）のMovePositionで動かすため、物理エンジンとの相性・衝突判定の精度が向上する。
/// Wallプレハブ本体にアタッチしてください。
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class WallMover : MonoBehaviour
{
	[Tooltip("これより左に出たら画面外とみなして自壊する（カメラの見える範囲より少し外側に設定）")]
	[SerializeField] private float m_despawnX = -12f;

	private Rigidbody2D m_rigidbody2D;

	private void Awake()
	{
		m_rigidbody2D = GetComponent<Rigidbody2D>();

		// Kinematicでない場合、スクリプトでの位置制御と物理挙動が競合するため注意喚起する
		if (m_rigidbody2D.bodyType != RigidbodyType2D.Kinematic)
		{
			Debug.LogWarning("WallMover: Rigidbody2DのBody TypeはKinematicを推奨します（" + gameObject.name + "）");
		}
	}

	private void FixedUpdate()
	{
		if (Time.timeScale == 0f || GameSpeedManager.Instance == null)
		{
			return;
		}

		float speed = GameSpeedManager.Instance.ScrollSpeed;

		// 現在位置から、今回のFixedUpdate分だけ左へ移動した位置を計算してMovePositionに渡す
		Vector2 nextPosition = m_rigidbody2D.position + Vector2.left * speed * Time.fixedDeltaTime;
		m_rigidbody2D.MovePosition(nextPosition);

		// 画面外に出たら自分自身を破棄してメモリを解放
		if (nextPosition.x < m_despawnX)
		{
			Destroy(gameObject);
		}
	}
}
