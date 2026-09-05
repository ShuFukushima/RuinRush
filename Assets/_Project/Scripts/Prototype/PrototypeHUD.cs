using UnityEngine;
using TMPro;

public class PrototypeHUD : MonoBehaviour
{
    [SerializeField] private int _score;    // UIに表示するスコア
    [SerializeField] private float _time;   // UIに表示する残り時間
    [SerializeField] private float _speed;  // UIに表示する速度
    [SerializeField] private TextMeshProUGUI _scoreText;    // テキストを表示するUIオブジェクト
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _speedText;
    [SerializeField] private GameObject _hudPanel;
    private PrototypeGameSession _gameSession;              // スコアと残り時間を取得
    private PrototypeCarController _player;                 // 現在の速度を取得

    private void Start()
    {
        // シーン上から必要なオブジェクトを取得
        _gameSession = FindFirstObjectByType<PrototypeGameSession>();
        _player = FindFirstObjectByType<PrototypeCarController>();
    }

    private void Update()
    {
        // 必要な情報を取得
        _score = _gameSession.GetCurrentScore();
        _time = _gameSession.GetCurrentTime();
        _speed = _player.GetPlayerCurrentSpeed();

        // UIを更新
        _scoreText.text = "SCORE : " + _score.ToString("000000");
        _timeText.text = "TIME : " + _time.ToString("000.00");
        _speedText.text = _speed.ToString("000");

        // ゲームが終わったら非表示にする
        if (!_gameSession.GetIsPlaying()) _hudPanel.SetActive(false);
    }

}
