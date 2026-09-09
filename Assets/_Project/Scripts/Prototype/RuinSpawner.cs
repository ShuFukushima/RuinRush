using System.Collections.Generic;
using UnityEngine;

public class RuinSpawner : MonoBehaviour
{
    // 配列としてプレハブと生成地点のトランスフォームを持つ
    [SerializeField] private GameObject[] _ruins;
    [SerializeField] private Transform[] _spawnPoints;

    [Header("出現構成比")]
    [SerializeField] private int _smallRatio;
    [SerializeField] private int _mediumRatio;
    [SerializeField] private int _largeRatio;


    private void Start()
    {
        // 出現ポイントを子オブジェクトから自動的に取得する
        _spawnPoints = new Transform[this.gameObject.transform.childCount];

        // 子オブジェクトを順番に格納
        for(int i = 0; i < _spawnPoints.Length; i++)
        {
            _spawnPoints[i] = this.gameObject.transform.GetChild(i);

            // 生成ポイントをテレインの高さに合わせる
            Vector3 pos = _spawnPoints[i].position;
            pos.y = Terrain.activeTerrain.SampleHeight(_spawnPoints[i].position);
            _spawnPoints[i].position = pos;
        }

        // 出現構成比ごとに出現する個数を求める
        float total = _smallRatio + _mediumRatio + _largeRatio;
        // エラー処理
        if(total <= 0)
        {
            Debug.LogError("出現構成比は必ず1以上になるようにしてください");
            return;
        }

        // 単純に比率ごとに個数を決定
        float smallNum = _spawnPoints.Length * (float)_smallRatio / total;
        float mediumNum = _spawnPoints.Length * (float)_mediumRatio / total;
        float largeNum = _spawnPoints.Length * (float)_largeRatio / total;
        // 個数を整数に
        int smallCount = Mathf.FloorToInt(smallNum);
        int mediumCount = Mathf.FloorToInt(mediumNum);
        int largeCount = Mathf.FloorToInt(largeNum);
        // 余った小数部分を格納
        float smallRemainder = smallNum - smallCount;
        float mediumRemainder = mediumNum - mediumCount;
        float largeRemainder = largeNum - largeCount;

        // 切り捨てによって余った枠数
        int remainingCount = _spawnPoints.Length - (smallCount + mediumCount + largeCount);

        // 実際に生成する廃墟の大きさ別個数を格納する
        int[] counts = {smallCount,  mediumCount, largeCount};

        // 小数点以下の値を格納
        float[] remainders = {smallRemainder, mediumRemainder, largeRemainder};
        

        // 余った枠数だけループさせ、追加で生成する廃墟を決定する
        for(int i = 0; i < remainingCount; i++)
        {
            int maxIndex = -1;
            float maxRemainder = -1f;

            for (int j = 0; j < remainders.Length; j++)
            {
                if (remainders[j] > maxRemainder)
                {
                    maxRemainder = remainders[j];
                    maxIndex = j;
                }
            }

            counts[maxIndex]++;
            remainders[maxIndex] = -1f;
        }

        // 抽選袋の作成
        List<GameObject> ruinBag = new List<GameObject>();
        // 小廃墟
        for (int i = 0; i < counts[0]; i++)
        {
            ruinBag.Add(_ruins[0]);
        }
        // 中廃墟
        for (int i = 0; i < counts[1]; i++)
        {
            ruinBag.Add(_ruins[1]);
        }
        // 大廃墟
        for (int i = 0; i < counts[2]; i++)
        {
            ruinBag.Add(_ruins[2]);
        }


        // 生成地点ごとに廃墟を生成
        for (int i = 0; i < _spawnPoints.Length; i++)
        {
            // 抽選袋の要素数から、ランダムに選択する
            int randomIndex = Random.Range(0, ruinBag.Count);

            // ランダムで回転を生み出す
            float xRotOffset = (float)Random.Range(-20, 20);
            float yRotOffset = (float)Random.Range(0, 360);
            float zRotOffset = (float)Random.Range(-20, 20);

            // x、zの数値にしたがって、高さを調整する
            // 単純に増減するか…
            float yPosOffset;
            if(Mathf.Abs(xRotOffset) > Mathf.Abs(zRotOffset))
            {
                yPosOffset = -Mathf.Abs(xRotOffset) * 0.2f;
            }
            else
            {
                yPosOffset = -Mathf.Abs(zRotOffset) * 0.2f;
            }


            // 廃墟を生成
            Instantiate(ruinBag[randomIndex], _spawnPoints[i].position + new Vector3(0f, yPosOffset, 0f), _spawnPoints[i].rotation * Quaternion.Euler(xRotOffset, yRotOffset, zRotOffset));

            // 抽選袋から生成された要素数の廃墟を削除
            ruinBag.Remove(ruinBag[randomIndex]);
        }
    }

}
