using UnityEngine;

public class PrototypeCarController : MonoBehaviour
{

    enum Drive
    {
        FrontDrive,
        RearDrive,
        AllDrive

    }

    [SerializeField] private float _steerAngle; // ハンドルの最高舵角
    [SerializeField] private float _maxBrake;   // ブレーキトルク
    [SerializeField] private WheelCollider _fL, _fR, _rL, _rR;  // 4輪分のWheelCollider
    [SerializeField] private Drive _drive;                      // 駆動方式を管理

    [SerializeField] private PrototypeGameSession _gameSession; // 現在のゲーム進行情報を取得する

    [SerializeField] private float _baseMaxSpeed = 40f;   // 最高速の初期値
    [SerializeField] private float _baseMaxTorque = 1000f;
    [SerializeField] private float _currentMaxSpeed;                       // プレイ中に変化する最高速度
    [SerializeField] private float _currentMaxTorque;                       // プレイ中に変化するモータートルク

    [SerializeField] private Rigidbody _rb;                 // 車のリジッドボディ

    private void Start()
    {
        _gameSession = FindFirstObjectByType<PrototypeGameSession>();

        // エラー処理
        if (_gameSession == null)
        {
            Debug.LogError("PrototypeGameSessionが見つかりません。");
        }

        _rb = GetComponent<Rigidbody>();
        if(_rb == null)
        {
            Debug.LogError("車にRigidbodyを設定してください。");
        }

        // 最高速の初期値を設定
        _currentMaxSpeed = _baseMaxSpeed;
        _currentMaxTorque = _baseMaxTorque;
    }


    private void Update()
    {
        // ゲームセッション側の進行情報を基に、操作するかを判定
        if(_gameSession.GetIsPlaying() == true)
        {
            Driving();
            Braking();
        }
        else
        {
            _fL.motorTorque = 0f;
            _fR.motorTorque = 0f;
            _rL.motorTorque = 0f;
            _rR.motorTorque = 0f;

            _fL.brakeTorque = 0f;
            _fR.brakeTorque = 0f;
            _rL.brakeTorque = 0f;
            _rR.brakeTorque = 0f;
        }

    }

    void Driving()
    {
        // 入力
        float power = _currentMaxTorque * Input.GetAxis("Vertical");
        float steering = _steerAngle * Input.GetAxis("Horizontal");

        // ハンドル操作
        _fL.steerAngle = steering;
        _fR.steerAngle = steering;

        // 速度を取得
        float speed = _rb.linearVelocity.magnitude * 3.6f;  // 取得する値が m/s のため、km/h に変換
        // Debug.Log("現在の速度：" + speed.ToString("F2") + " km/h");

        // 駆動
        // 現在の速度が最高速度であれば加算しない
        if(_currentMaxSpeed > speed)
        {
            if (_drive == Drive.FrontDrive)
            {
                _fL.motorTorque = power * 0.5f;
                _fR.motorTorque = power * 0.5f;
            }
            else if (_drive == Drive.RearDrive)
            {
                _rL.motorTorque = power * 0.5f;
                _rR.motorTorque = power * 0.5f;
            }
            else
            {
                _fL.motorTorque = power * 0.25f;
                _fR.motorTorque = power * 0.25f;
                _rL.motorTorque = power * 0.25f;
                _rR.motorTorque = power * 0.25f;
            }
        }
        else
        {
            _fL.motorTorque = 0f;
            _fR.motorTorque = 0f;
            _rL.motorTorque = 0f;
            _rR.motorTorque = 0f;
        }
        

    }

    void Braking()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _fL.brakeTorque = _maxBrake;
            _fR.brakeTorque = _maxBrake;
            _rL.brakeTorque = _maxBrake;
            _rR.brakeTorque = _maxBrake;
        }
        else
        {
            _fL.brakeTorque = 0;
            _fR.brakeTorque = 0;
            _rL.brakeTorque = 0;
            _rR.brakeTorque = 0;
        }

    }

    /// <summary>
    /// 最高速度強化アイテムを取った時の処理
    /// </summary>
    public void IncreaseMaxSpeed(float addMaxSpeed)
    {
        // 現在の最高速度を更新
        _currentMaxSpeed += addMaxSpeed;
    }

    public void IncreaseMaxTorque(float addMaxTorque)
    {
        // 現在のモータートルクを更新
        _currentMaxTorque += addMaxTorque;
    }

}
