using UnityEngine;

public class PrototypeCameraShake : MonoBehaviour
{
    [SerializeField] private float _shakeTime;
    [SerializeField] private float _shakeIntensity;
    [SerializeField] private float _currentTime;
    [SerializeField] private bool _isShaked = false;
  

    void Update()
    {
        if(_isShaked)
        {
            Shake();
        }

    }

    public void StartShake(float shakeIntensity)
    {
        // 強さとフラグを立てる
        _shakeIntensity = shakeIntensity;
        _isShaked = true;

        // 長さを初期化
        _currentTime = _shakeTime;
    }

    void Shake()
    {
        if (_currentTime > 0)
        {
            // 360度からランダムに選択し、揺れの強さを掛け合わせる
            Vector2 randomPos = Random.insideUnitCircle * _shakeIntensity;

            // ローカル座標を移動させる
            transform.localPosition = new Vector3(randomPos.x, randomPos.y, 0f);

            _currentTime -= Time.deltaTime;
        }
        else
        {
            transform.localPosition = Vector3.zero;
            _isShaked = false;
        }
    }
}
