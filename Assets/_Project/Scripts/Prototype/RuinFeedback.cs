using UnityEngine;
using System.Collections.Generic;

public class RuinFeedback : MonoBehaviour
{
    private BreakableObject _breakableObject;  // 破壊可能速度を参照する
    private CarController _player;

    private List<Material> _materials = new List<Material>();

    [SerializeField] private float _breakingSpeed;
    [SerializeField] private float _maximumLmnInt;  // 最大発光強度
    [SerializeField] private AnimationCurve _glowCurve;

    private void Awake()
    {
        // 自身についているスクリプトを取得
        _breakableObject = GetComponent<BreakableObject>();
        _breakingSpeed = _breakableObject.GetBreakingSpeed();

        _player = GameObject.FindWithTag("Player").GetComponent<CarController>();

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            foreach (Material material in renderer.materials)
            {
                if (material.HasProperty("_GlowStrength"))
                {
                    _materials.Add(material);
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
        foreach (Material material in _materials)
        {
            material.SetFloat("_GlowStrength", emissionRate);
        }

    }
}
