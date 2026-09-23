using TMPro;
using UnityEngine;

public class ReadyUI : MonoBehaviour
{
    [SerializeField] private GameSession _gameSession;  // カウントダウン用
    [SerializeField] private GameObject _readyPanel;

    [Header("テキスト")]
    [SerializeField] private TextMeshProUGUI _countdownText;

    private bool _hasShownReady = false;

    private void Awake()
    {
        // ゲームセッションを取得
        _gameSession = FindAnyObjectByType<GameSession>();
    }

    private void Update()
    {
        if(_gameSession._gameState == GameSession.GameState.Ready)
        {
            if(_hasShownReady != true)
            {
                _readyPanel.SetActive(true);
            }

            int displayCount = Mathf.CeilToInt(_gameSession.GetCountdown());
            _countdownText.text = Mathf.Max(displayCount, 0).ToString();
        }
        else
        {
            _readyPanel.SetActive(false);
        }
    }
}
