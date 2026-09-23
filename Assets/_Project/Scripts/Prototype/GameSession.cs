using System;
using TMPro;
using UnityEngine;

public class GameSession : MonoBehaviour
{
    /// <summary>
    /// ゲーム中のステート
    /// </summary>
    public enum GameState
    {
        Ready,
        Game,
        Result
    }

    // 合計スコアを持つ
    [SerializeField] private int _currentScore = 0;

    // タイマー関係
    [SerializeField] private float _currentTime = 0f;   // 現在の残り時間
    [SerializeField] private float _timeLimit = 30f;    // ゲームの制限時間
    [SerializeField] private float _countdownTime = 5f; // ゲーム開始前のカウントダウン

    [SerializeField] public GameState _gameState = GameState.Ready; // 現在のゲーム状況

    // スコアランキング用
    [SerializeField] private GameDirector _gameDir;
    [SerializeField] private TextMeshProUGUI[] _rankingTexts;
    private bool _isScoreing = false;


    private void Start()
    {
        Application.targetFrameRate = 60;

        // シーン上からディレクターを取得
        _gameDir = FindAnyObjectByType<GameDirector>();

        // 残り時間を初期化
        _countdownTime = 5f;
        _currentTime = _timeLimit;
    }

    private void Update()
    {

        // ステートで状態管理する
        switch( _gameState)
        {
            case GameState.Ready:
                // 0になったらスタート
                _countdownTime -= Time.deltaTime;
                Debug.Log("カウントダウン：" + _countdownTime);
                if(_countdownTime <= 0f)
                {
                    ChangeState(GameState.Game);
                }
                
                break;
            case GameState.Game:
                // タイマーを0になるまで減らしていく
                if (_currentTime > 0f)
                {
                    _currentTime -= Time.deltaTime;
                }
                else
                {
                    ChangeState(GameState.Result);  // リザルトステートへ
                    _currentTime = 0f;  // タイマーを0秒にそろえる
                }
                break;
            case GameState.Result:
                // ゲームディレクター側のメソッドをたたいて更新する
                if(_isScoreing != true)
                {
                    _gameDir.CheckScoreRanking(_currentScore);
                    for(int i = 0; i < 5; i++)
                    {
                        _gameDir.DrawRanking(i, _rankingTexts[i]);
                    }
                    _isScoreing = true;
                }

                break;

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
    /// 現在の残り時間を返すゲッターメソッド
    /// </summary>
    /// <returns></returns>
    public float GetCurrentTime()
    {
        return _currentTime;
    }

    /// <summary>
    /// デリゲートに入れる
    /// </summary>
    /// <param name="score"></param>
    private void OnRuinBraking(int score)
    {
        // スコアを加算する
        _currentScore += score;
    }

    /// <summary>
    /// ステートを変化させる共通の入り口
    /// </summary>
    /// <param name="state"></param>
    private void ChangeState(GameState state)
    {
        _gameState = state;
    }

    /// <summary>
    /// カウントダウンを返すゲッターメソッド
    /// </summary>
    /// <returns></returns>
    public float GetCountdown()
    {
        return _countdownTime;
    }
}
