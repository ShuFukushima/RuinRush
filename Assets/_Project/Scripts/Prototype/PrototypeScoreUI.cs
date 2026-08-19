using UnityEngine;
using TMPro;

public class PrototypeScoreUI : MonoBehaviour
{
    [SerializeField] private int _score;    // UIに表示するスコア
    [SerializeField] private TextMeshProUGUI _scoreText;    // テキストを表示するUIオブジェクト
    private PrototypeGameSession _gameSession;              // スコアを取得

    private void Start()
    {
        // シーン上からゲームセッションを取得
        _gameSession = FindFirstObjectByType<PrototypeGameSession>();
    }

    private void Update()
    {
        // スコアを取得
        _score = _gameSession.GetCurrentScore();

        // UIを更新
        _scoreText.text = "SCORE : " + _score;
    }

}
