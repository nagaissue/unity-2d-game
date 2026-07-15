using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class BackGroundMover : MonoBehaviour
{
	private const float k_maxLength = 1f;
	private const string k_propName = "_MainTex";

	[SerializeField]
	private Vector2 m_offsetSpeed;

	private Material m_copiedMaterial;

	private void Start()
	{
		var image = GetComponent<Image>();

		// 元のマテリアルを「Instantiate」を使って自分自身専用に「複製（クローン）」し、
		// それを自身のマテリアルとして再設定します。
		// これにより、Unityの共有のデフォルトマテリアル自体を傷つける心配がなくなります。
		//m_copiedMaterial = image.material;
		m_copiedMaterial = Instantiate(image.material); // マテリアルをコピーして使う
		image.material = m_copiedMaterial; // コピーしたマテリアルをImageにセットする

		// マテリアルがnullだったら例外が出ます。
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