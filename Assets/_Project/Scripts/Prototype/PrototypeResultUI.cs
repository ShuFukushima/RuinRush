using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrototypeResultUI : MonoBehaviour
{
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private TextMeshProUGUI _resultScoreText;
    private PrototypeGameSession _gameSession;              // スコア・ゲームの進行状況を取得
    private bool _hasShownResult = false;                   // リザルトパネルを1回だけ表示させる


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
        _gameSession = FindFirstObjectByType<PrototypeGameSession>();
        if (_gameSession == null)
        {
            Debug.LogError("PrototypeGameSessionが見つかりません。");
        }
    }

    private void Update()
    {
        // 将来的にリトライ処理を整理するため、1つのメソッドにまとめる
        if(_gameSession.GetIsPlaying() == false && _hasShownResult == false)
        {
            ShowResult();
            _hasShownResult = true;
        }
        
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
}
