using UnityEngine;

/// <summary>
/// ゲーム全体のスクロール速度を一元管理するシングルトン。
/// 背景・壁など「流れる」要素は全てここを参照することで、速度がズレなくなる。
/// シーン内の空のGameObject（例："GameManager"）に1つだけアタッチしてください。
/// </summary>
public class GameSpeedManager : MonoBehaviour
{
	public static GameSpeedManager Instance { get; private set; }

	[Header("基本スクロール速度（ワールド単位/秒）")]
	[SerializeField] private float m_baseScrollSpeed = 4f;

	[Header("時間経過による加速（任意・難易度を上げたい場合）")]
	[SerializeField] private bool m_useTimeBasedAcceleration = false;
	[SerializeField] private float m_accelerationPerSecond = 0.05f;
	[SerializeField] private float m_maxScrollSpeed = 12f;

	// 現在のスクロール速度（外部からは読み取り専用）
	public float ScrollSpeed { get; private set; }

	private void Awake()
	{
		// シングルトン化：既に存在する場合は自分を破棄する
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}
		Instance = this;
		ScrollSpeed = m_baseScrollSpeed;
	}

	private void Update()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}

		if (m_useTimeBasedAcceleration)
		{
			// 時間が経つほど少しずつ速くなる（最大値まで）
			ScrollSpeed = Mathf.Min(ScrollSpeed + m_accelerationPerSecond * Time.deltaTime, m_maxScrollSpeed);
		}
	}
}
