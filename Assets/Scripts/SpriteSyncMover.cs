using UnityEngine;

public class SpriteSyncMover : MonoBehaviour
{
    [Header("同期させたい背景オブジェクト")]
    [SerializeField] private BackGroundMover m_backgroundMover;

    [Header("背景1ループ(UV値1.0)あたりにオブジェクトが移動するワールド距離")]
    [SerializeField] private float m_loopWorldDistance = 20f; 
    // 💡 画面幅や背景画像の大きさに合わせて調整してください（例: 2Dカメラの横幅など）

    private Vector2 m_offsetSpeed;
    private Vector3 m_startPosition;

    private void Start()
    {
        m_startPosition = transform.position;

        // 背景スクリプトからリフレクション（または直接アクセス）で速度を取得する
        if (m_backgroundMover != null)
        {
            // BackGroundMoverのm_offsetSpeedを取得
            // (リフレクションを使って非公開フィールドから安全に読み込みます)
            var field = typeof(BackGroundMover).GetField("m_offsetSpeed", 
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            
            if (field != null)
            {
                m_offsetSpeed = (Vector2)field.GetValue(m_backgroundMover);
            }
        }
        else
        {
            Debug.LogError("背景（BackGroundMover）がセットされていません！");
        }
    }

    private void Update()
    {
        if (Time.timeScale == 0f || m_backgroundMover == null)
        {
            return;
        }

        // 背景のMathf.Repeatと全く同じ計算式で、現在の進行度（0～1）を計算
        float progressX = Mathf.Repeat(Time.time * m_offsetSpeed.x, 1f);
        float progressY = Mathf.Repeat(Time.time * m_offsetSpeed.y, 1f);

        // 進行度（0～1）にワールドの移動距離を掛けて、移動位置を決定
        // (UVオフセットがプラスに進む＝テクスチャは逆方向に流れるため、- をかけて同期させます)
        Vector3 currentOffset = new Vector3(
            -progressX * m_loopWorldDistance,
            -progressY * m_loopWorldDistance,
            0f
        );

        // 初期位置から同期した位置へ移動
        transform.position = m_startPosition + currentOffset;
    }
}