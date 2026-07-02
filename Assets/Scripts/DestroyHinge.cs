using UnityEngine;

public class DestroyHinge : MonoBehaviour
{
    //HingeJoint2Dのオブジェクト
    public HingeJoint2D hj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        //マウスの左クリックがリリースされたらhingeJointを削除
        if (Input.GetMouseButtonUp(0))
        {
            if (hj != null)
            {
                Destroy(hj); // HingeJoint2Dを削除
            }
        }
    }

}
