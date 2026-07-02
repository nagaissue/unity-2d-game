using UnityEngine;

public class DestroyMyself : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    //これより落下したら消える
    public float destroyHeight = -7f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Y座標がdestroyHeight以下になったら自分自身を削除
        if (transform.position.y < destroyHeight)
        {
            Destroy(gameObject); // 自分自身を削除
        }
    }
}
