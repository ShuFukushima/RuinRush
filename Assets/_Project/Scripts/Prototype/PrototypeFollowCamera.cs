using UnityEngine;

public class PrototypeFollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject _followObj; // カメラが追う対象
    [SerializeField] private Vector3 _offsetPos;    // カメラ座標の調整

    private void LateUpdate()
    {
        Vector3 followPosition = _followObj.transform.TransformPoint(_offsetPos);

        // 動作確認として座標を合わせる
        gameObject.transform.position = followPosition;

        // 回転を合わせる
        // 対象オブジェクトに対して注目させる という処理
        gameObject.transform.LookAt(_followObj.transform);
    }
}
