using UnityEngine;

public class PrototypeGameSession : MonoBehaviour
{
    // 合計スコアを持つ
    [SerializeField] private int _currentScore = 0;

    /// <summary>
    /// 現在の得点を加算するメソッド 外部から呼び出す
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        // 現在の合計スコアを加算する
        _currentScore += score;
    }

    /// <summary>
    /// 現在のスコアを返すゲッターメソッド
    /// </summary>
    /// <returns></returns>
    public int GetCurrentScore()
    {
        return _currentScore;
    }
}
