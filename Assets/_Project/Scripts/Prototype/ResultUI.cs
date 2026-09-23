using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private TextMeshProUGUI _resultScoreText;
    private GameSession _gameSession;              // スコア・ゲームの進行状況を取得
    private bool _hasShownResult = false;                   // リザルトパネルを1回だけ表示させる

    [Header("パネル移動用")]
    public bool _isRanking = false;
    public float _sliderSpeed = 5f;
    [SerializeField] private RectTransform _scorePanel;
    [SerializeField] private RectTransform _rankingPanel;


    private void Start()
    {
        // パネル・テキストは手動で設定
        // エラー処理
        if (_resultPanel == null)
        {
            Debug.LogError("リザルトパネルが設定されていません。");
        }
        if( _resultScoreText == null)
        {
            Debug.LogError("リザルトスコアテキストが設定されていません。");
        }

        _resultPanel.SetActive(false);  // 明示的にfalseにする

        // シーン上からゲームセッションを取得
        _gameSession = FindFirstObjectByType<GameSession>();
        if (_gameSession == null)
        {
            Debug.LogError("PrototypeGameSessionが見つかりません。");
        }
    }

    private void Update()
    {
        // 将来的にリトライ処理を整理するため、1つのメソッドにまとめる
        if(_gameSession._gameState == GameSession.GameState.Result && _hasShownResult == false)
        {
            ShowResult();
            _hasShownResult = true;
        }

        MovePanel();

    }

    /// <summary>
    /// リザルト画面を表示するメソッド
    /// </summary>
    private void ShowResult()
    {
        // リザルトパネルを有効化
        _resultPanel.SetActive(true);

        // スコアを取得
        int score = _gameSession.GetCurrentScore();

        // スコアを表示
        _resultScoreText.text = "SCORE : " + score;
    }

    /// <summary>
    /// ボタン用：クリックしたらシーンを再読み込みする
    /// </summary>
    public void OnClickRetry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnClickReturn()
    {
        SceneManager.LoadScene("Title");
    }

    // ボタンを押されたら移動させる
    public void OnClickRanking()
    {
        if(_isRanking)
            _isRanking = false;
        else
            _isRanking = true;
    }

    public void OnClickScore()
    {
        _isRanking = false;
    }

    private void MovePanel()
    {
        if (_isRanking)
        {
            if (_scorePanel.anchoredPosition.x > -800f)
            {
                // パネルを移動させる
                _scorePanel.anchoredPosition += new Vector2(-_sliderSpeed, 0f) * Time.deltaTime;
                _rankingPanel.anchoredPosition += new Vector2(-_sliderSpeed, 0f) * Time.deltaTime;
            }
            else if (_rankingPanel.anchoredPosition.x != 0f)
            {
                _scorePanel.anchoredPosition = new Vector2(-800f, 0f);
                _rankingPanel.anchoredPosition = new Vector2(0f, 0f);
            }
        }
        else
        {
            if (_rankingPanel.anchoredPosition.x < 800f)
            {
                // パネルを移動させる
                _scorePanel.anchoredPosition -= new Vector2(-_sliderSpeed, 0f) * Time.deltaTime;
                _rankingPanel.anchoredPosition -= new Vector2(-_sliderSpeed, 0f) * Time.deltaTime;
            }
            else if (_scorePanel.anchoredPosition.x != 0f)
            {
                _scorePanel.anchoredPosition = new Vector2(0f, 0f);
                _rankingPanel.anchoredPosition = new Vector2(800f, 0f);
            }

        }


    }
}
