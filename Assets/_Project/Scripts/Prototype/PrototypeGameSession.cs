using UnityEngine;

public class PrototypeGameSession : MonoBehaviour
{
    // 合計スコアを持つ
    [SerializeField] private int _currentScore = 0;

    // タイマー関係
    [SerializeField] private float _currentTime = 0f;   // 現在の残り時間
    [SerializeField] private float _timeLimit = 30f;    // ゲームの制限時間

    [SerializeField] private bool _isPlaying = true;      // 現在のゲーム進行状況

    private void Start()
    {
        // 現在の残り時間を初期化
        _currentTime = _timeLimit;
    }

    private void Update()
    {
        // タイマーを0になるまで減らしていく
        if(_currentTime > 0f)
        {
            _currentTime -= Time.deltaTime;
        }
        else
        {
            _isPlaying = false;  // プレイ終了
            _currentTime = 0f;  // タイマーを0秒にそろえる
        }

    }

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

    /// <summary>
    /// 現在のゲーム進行状況を返すゲッターメソッド
    /// </summary>
    /// <returns></returns>
    public bool GetIsPlaying()
    {
        return _isPlaying;
    }
}
