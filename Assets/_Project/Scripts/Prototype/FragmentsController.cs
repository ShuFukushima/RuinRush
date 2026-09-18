using UnityEngine;

public class FragmentsController : MonoBehaviour
{
    [SerializeField] private Collider[] _fragmentsCollider;
    [SerializeField] private float _triggerTime;
    [SerializeField] private float _timer = 0f;

    // 全ての子オブジェクトのコライダー取得
    private void Awake()
    {
        _fragmentsCollider = gameObject.GetComponentsInChildren<Collider>();
    }

    // オブジェクト出現から一定時間でIsTriggerをオンにする
    private void Update()
    {
        // カウントを増やす
        _timer += Time.deltaTime;

        // 一定時間でトリガーを消す
        if( _timer >= _triggerTime)
        {
            for(int i = 0; i < _fragmentsCollider.Length; i++)
            {
                _fragmentsCollider[i].isTrigger = true;
            }
        }
    }
}
