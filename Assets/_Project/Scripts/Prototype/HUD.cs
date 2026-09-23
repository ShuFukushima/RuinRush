using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private float _displayScore;
    [SerializeField] private float _scoreCountSpeed = 500f;
    private float _prevTargetScore = 0;
    [SerializeField] private float _time;   // UIに表示する残り時間
    [SerializeField] private float _speed;  // UIに表示する速度
    [SerializeField] private TextMeshProUGUI _scoreText;    // テキストを表示するUIオブジェクト
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private GameObject _goText;
    [SerializeField] private GameObject _hudPanel;
    [SerializeField] private Image _speedSlider;
    [SerializeField] private GameObject _returnPoint;
    private GameSession _gameSession;              // スコアと残り時間を取得
    private CarController _player;                 // 現在の速度を取得
    private bool _hasShownHUD = false;
    private bool _hasShownGo = true;
    private float _showGoTime = 2f;

    [SerializeField] private Animator _scoreAnimator;


    private void Start()
    {
        // シーン上から必要なオブジェクトを取得
        _gameSession = FindFirstObjectByType<GameSession>();
        _player = FindFirstObjectByType<CarController>();

        // Goテキストを見せる
        _goText.SetActive(true);

    }

    private void Update()
    {
        if(_gameSession._gameState == GameSession.GameState.Game)
        {
            // 最初にHUDを表示させる
            if(_hasShownHUD != true)
            {
                _hudPanel.SetActive(true);
                _hasShownHUD = true;
            }

            // Goを見せる
            if (_showGoTime <= 0f && _hasShownGo == true)
            {
                _goText.SetActive(false);
                _hasShownGo = false;
            }
            else if(_hasShownGo)
            {
                _showGoTime -= Time.deltaTime;
            }

            // 必要な情報を取得
            float targetScore = _gameSession.GetCurrentScore();
            _time = _gameSession.GetCurrentTime();
            _speed = _player.GetPlayerCurrentSpeed();

            // スコアが増えたらアニメーションとすこしずつ増やす処理を実行
            if (targetScore > _prevTargetScore)
            {
                _scoreAnimator.Play("ScoreUp");
            }
            _displayScore = Mathf.MoveTowards(_displayScore, targetScore, _scoreCountSpeed * Time.deltaTime);

            // このフレームのターゲットスコアを保存
            _prevTargetScore = targetScore;

            // スピードと最高速度に合わせてスライダーを更新
            float maxSpeed = _player.GetPlayerCurrentMaxSpeed();
            _speedSlider.fillAmount = _speed / maxSpeed;


            // UIを更新
            _scoreText.text = "SCORE : " + _displayScore.ToString("000000");
            _timeText.text = "TIME : " + _time.ToString("000.00");
            _speedText.text = _speed.ToString("000");
        }
        else
        {
            _hudPanel.SetActive(false);
        }


    }

    /// <summary>
    /// 復帰処理用のボタン
    /// </summary>
    public void OnClickReturnButton()
    {
        Rigidbody rb = _player.GetComponent<Rigidbody>();

        // プレイヤーの物理的な慣性を消す
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // 復帰ポイントに位置と回転を同期させる
        _player.transform.position = _returnPoint.transform.position;
        _player.transform.rotation = _returnPoint.transform.rotation;
    }

}
