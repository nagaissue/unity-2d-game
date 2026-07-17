using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(SpriteRenderer))]
public class BackGroundMover : MonoBehaviour
{
	private const float k_maxLength = 1f;
	private const string k_propName = "_MainTex";

	[SerializeField]
	private Vector2 m_offsetSpeed;

	[SerializeField]
	private string m_sortingLayerName = "Background";

	[SerializeField]
	private int m_orderInLayer = -100;

	public Vector2 OffsetSpeed => m_offsetSpeed;

	private Material m_copiedMaterial;

	private void Start()
	{
		var spriteRenderer = GetComponent<SpriteRenderer>();

		// ワールド空間の背景として SpriteRenderer を使用する
		// 元のマテリアルを複製し、自分用にセットすることで共有マテリアルを汚さない
		m_copiedMaterial = Instantiate(spriteRenderer.material);
		spriteRenderer.material = m_copiedMaterial;

		spriteRenderer.sortingLayerName = m_sortingLayerName;
		spriteRenderer.sortingOrder = m_orderInLayer;

		Assert.IsNotNull(m_copiedMaterial);
	}

	private void Update()
	{
		if (Time.timeScale == 0f)
		{
			return;
		}

		// xとyの値が0 ～ 1でリピートするようにする
		var x = Mathf.Repeat(Time.time * m_offsetSpeed.x, k_maxLength);
		var y = Mathf.Repeat(Time.time * m_offsetSpeed.y, k_maxLength);
		var offset = new Vector2(x, y);
		m_copiedMaterial.SetTextureOffset(k_propName, offset);
	}

	private void OnDestroy()
	{
		// 複製したマテリアルを安全に破棄します。
		if(m_copiedMaterial != null)
		{
			// ゲームオブジェクト破壊時にマテリアルのコピーも消しておく
			Destroy(m_copiedMaterial);
			m_copiedMaterial = null;
		}
	}
}