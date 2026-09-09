using UnityEngine;

public class ShaderFeedbackTest : MonoBehaviour
{
    [SerializeField] private float _glowStrength;

    private Renderer _renderer;
    private Material _material;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
    }

    private void Update()
    {
        _material.SetFloat("_GlowStrength", _glowStrength);        
    }
}
