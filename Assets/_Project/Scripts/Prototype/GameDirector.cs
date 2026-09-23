using UnityEngine;
using TMPro;

public class GameDirector : MonoBehaviour
{
    // 配列で5つまでスコアを持つ
    [SerializeField] private int[] _scoreRank = new int[] { -1, -1, -1, -1, -1 };

    private void Awake()
    {
        // 既に同じオブジェクトが存在している場合は重複して生成されないようにする処理
        int num = FindObjectsByType<GameDirector>(FindObjectsSortMode.None).Length;
        if (num > 1)
        {
            Destroy(gameObject);
            return;
        }

        // シーンをまたいでも破棄しない
        DontDestroyOnLoad(gameObject);
    }

    // 配列に得点順にいれる
    public void CheckScoreRanking(int score)
    {
        int index = -1;

        // 配列に高い順にいれる
        for(int i = 0; i < _scoreRank.Length; i++)
        {
            if(score > _scoreRank[i])
            {
                index = i;
                break;
            }
        }

        if (index == -1)
            return;

        for(int i = _scoreRank.Length - 1; i > index; i--)
        {
            // 1つずつずらす
            _scoreRank[i] = _scoreRank[i - 1];
        }

        _scoreRank[index] = score;
    }

    /// <summary>
    /// 指定した要素数の順位を記述する
    /// </summary>
    /// <param name="index"></param>
    /// <param name="scoreText"></param>
    public void DrawRanking(int index, TextMeshProUGUI scoreText)
    {
        int rank = index + 1;
        if (_scoreRank[index] != -1)
        {
            scoreText.text = rank.ToString() + "位：" + _scoreRank[index];
        }
        else
        {
            scoreText.text = rank.ToString() + "位：---";
        }
    }
}
