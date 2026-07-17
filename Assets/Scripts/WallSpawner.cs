using UnityEngine;

/// <summary>
/// Wallを一定間隔（ランダム幅あり）で画面右外に生成するスポナー。
/// 複数サイズのWallプレハブを配列に登録しておけば、ランダムに選ばれて流れてくる。
/// シーン内の空のGameObject（例："WallSpawner"）にアタッチしてください。
/// </summary>
public class WallSpawner : MonoBehaviour
{
	[Header("生成するWallプレハブ（複数サイズ・複数種類を登録可能）")]
	[SerializeField] private GameObject[] m_wallPrefabs;

	[Header("スポーン位置のX座標（画面右外）")]
	[SerializeField] private float m_spawnX = 12f;

	[Header("スポーンするY座標の範囲（地形の高さに合わせて調整）")]
	[SerializeField] private float m_minY = -1f;
	[SerializeField] private float m_maxY = 1f;

	[Header("スポーン間隔（秒）のランダム範囲")]
	[SerializeField] private float m_minInterval = 0.8f;
	[SerializeField] private float m_maxInterval = 1.8f;

	private float m_timer;
	private float m_nextInterval;

	private void Start()
	{
		SetNextInterval();
	}

	private void Update()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}

		m_timer += Time.deltaTime;
		//Debug.Log("WallSpawner Update中: timer=" + m_timer + " / next=" + m_nextInterval); // ← 追加

		if (m_timer >= m_nextInterval)
		{
			SpawnWall();
			m_timer = 0f;
			SetNextInterval();
		}
	}

	private void SpawnWall()
	{
		if (m_wallPrefabs == null || m_wallPrefabs.Length == 0)
		{
			Debug.Log("SpawnWall呼ばれた"); // ← 追加
			
			Debug.LogWarning("WallSpawner: m_wallPrefabsが未設定です（配列のSizeを0以上にしてください）");
			return;
		}

		var prefab = m_wallPrefabs[Random.Range(0, m_wallPrefabs.Length)];

		// 配列のサイズはあるが、選ばれた要素自体がNone（未割り当て）の場合を防ぐ
		if (prefab == null)
		{
			Debug.LogWarning("WallSpawner: m_wallPrefabsの中に未割り当て（None）の要素があります。Inspectorで全てのElementにプレハブを設定してください");
			return;
		}

		float y = Random.Range(m_minY, m_maxY);
		Instantiate(prefab, new Vector3(m_spawnX, y, 0f), Quaternion.identity);
	}

	private void SetNextInterval()
	{
		m_nextInterval = Random.Range(m_minInterval, m_maxInterval);
	}
}
