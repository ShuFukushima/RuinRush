using UnityEngine;

public class PrototypeCarController : MonoBehaviour
{

    enum Drive
    {
        FrontDrive,
        RearDrive,
        AllDrive

    }

    [SerializeField] private float _maxPower;   // 最高出力トルク
    [SerializeField] private float _steerAngle; // ハンドルの最高舵角
    [SerializeField] private float _maxBrake;   // ブレーキトルク
    [SerializeField] private WheelCollider _fL, _fR, _rL, _rR;  // 4輪分のWheelCollider
    [SerializeField] private Drive _drive;                      // 駆動方式を管理

    [SerializeField] private PrototypeGameSession _gameSession; // 現在のゲーム進行情報を取得する

    private void Start()
    {
        _gameSession = FindFirstObjectByType<PrototypeGameSession>();

        // エラー処理
        if (_gameSession == null)
        {
            Debug.LogError("PrototypeGameSessionが見つかりません。");
        }
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
        float power = _maxPower * Input.GetAxis("Vertical");
        float steering = _steerAngle * Input.GetAxis("Horizontal");

        // ハンドル操作
        _fL.steerAngle = steering;
        _fR.steerAngle = steering;

        // 駆動
        if(_drive == Drive.FrontDrive)
        {
            _fL.motorTorque = power * 0.5f;
            _fR.motorTorque = power * 0.5f;
        }
        else if(_drive == Drive.RearDrive)
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
}
