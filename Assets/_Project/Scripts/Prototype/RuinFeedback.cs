using UnityEngine;
using System.Collections.Generic;

public class RuinFeedback : MonoBehaviour
{
    private BreakableObject _breakableObject;  // 破壊可能速度を参照する
    private CarController _player;


    [SerializeField] private Material[] _materials = new Material[3];

    [SerializeField] private Renderer[] _renderers = new Renderer[3];
    [SerializeField] private float _breakingSpeed;
    [SerializeField] private float _maximumLmnInt;  // 最大発光強度
    [SerializeField] private AnimationCurve _glowCurve;

    private void Awake()
    {
        // 自身についているスクリプトを取得
        _breakableObject = GetComponent<BreakableObject>();
        _breakingSpeed = _breakableObject.GetBreakingSpeed();

        _player = GameObject.FindWithTag("Player").GetComponent<CarController>();

        // Awake 内で明示的に初期化（フィールドでの初期化は、オブジェクトをインスタンシエイトで生成した際は何故だか無効になる）
        _materials = new Material[3];
        _renderers = new Renderer[3];


        // 孫オブジェクトを取得
        List<Transform> grandchildren = GetAllGrandchildren(this.transform);

        for(int i = 0; i < grandchildren.Count; i++)
        {
            // 孫オブジェクトのレンダラーを取得
            _renderers[i] = grandchildren[i].GetComponent<Renderer>();

            // 孫オブジェクトの指定した要素数のマテリアルを取得
            Material[] childMaterials = _renderers[i].materials;

            // マテリアルプロパティで照合する
            for(int j = 0; j < childMaterials.Length; j++)
            {
                if (childMaterials[j].HasProperty("_GlowStrength"))
                {
                    _materials[i] = childMaterials[j];
                    break;
                }
            }

        }

    }

    private void Update()
    {
        // 現在速度を取得
        float currentSpeed = _player.GetPlayerCurrentSpeed();

        // InverseLerp で0～1の値に変換
        float emissionRate = Mathf.InverseLerp(0f, _breakingSpeed, currentSpeed);

        // 最大発光強度を掛ける
        emissionRate = _glowCurve.Evaluate(emissionRate);
        emissionRate = Mathf.Clamp01(emissionRate);

        emissionRate *= _maximumLmnInt;

        // SetFloat("_GlowStrength", 値);
        for(int i = 0; i < _materials.Length; i++)
        {
            _materials[i].SetFloat("_GlowStrength", emissionRate);
        }
        
    }

    /// <summary>
    /// 孫オブジェクトを返すメソッド
    /// </summary>
    /// <param name="parent"></param>
    /// <returns></returns>
    List<Transform> GetAllGrandchildren(Transform parent)
    {
        List<Transform> result = new List<Transform>();

        // 子オブジェクトをループ
        foreach (Transform child in parent)
        {
            // 孫オブジェクトをループ
            foreach (Transform grandchild in child)
            {
                result.Add(grandchild);
            }
        }

        return result;
    }
}
