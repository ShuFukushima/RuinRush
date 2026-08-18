using UnityEngine;

public class PrototypeTireVisualController : MonoBehaviour
{
    Vector3 _pos;        //タイヤの座標を管理する変数
    Quaternion _rot;     //タイヤの回転を管理する変数
    [SerializeField] WheelCollider _wc;   //それぞれのWheelCollider
    Transform _wheel;    //それぞれのタイヤ

    void Start()
    {
        _wheel = this.transform;      
    }

    void FixedUpdate()
    {
        _wc.GetWorldPose(out _pos, out _rot);  //WheelColliderから位置と回転情報を取得
        _wheel.transform.position = _pos;     //タイヤの位置を指定
        _wheel.transform.rotation = _rot;     //タイヤの角度を指定
    }
}
