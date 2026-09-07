using System;
using UnityEditor.TerrainTools;
using UnityEngine;

public class PrototypeRuinFeedback : MonoBehaviour
{
    private PrototypeBreakableObject _breakableObject;  // 破壊可能速度を参照する
    private PrototypeCarController _player;

    private Material _material;

    [SerializeField] private Renderer _renderer;
    [SerializeField] private float _breakingSpeed;
    [SerializeField] private float _maximumLmnInt;  // 最大発光強度
    [SerializeField] private AnimationCurve _glowCurve;
    [SerializeField] private int _matIndex = 0; // 指定するマテリアルインデックス

    private void Awake()
    {
        // 自身についているスクリプトを取得
        _breakableObject = GetComponent<PrototypeBreakableObject>();
        _breakingSpeed = _breakableObject.GetBreakingSpeed();

        _player = GameObject.FindWithTag("Player").GetComponent<PrototypeCarController>();
        // マテリアルを取得
        if (_renderer == null)
        {
            Debug.LogError("発光させるRendererを指定してください。", this);
            enabled = false;
            return;
        }

        Material[] materials = _renderer.materials;

        // 存在しない番号を指定していないか確認する
        if (_matIndex < 0 || _matIndex >= materials.Length)
        {
            Debug.LogError("マテリアルの番号が範囲外です。", this);
            enabled = false;
            return;
        }

        // 指定した番号のマテリアルを取得する
        _material = materials[_matIndex];
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
        _material.SetFloat("_GlowStrength", emissionRate);
    }
}
