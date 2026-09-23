using System;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    public bool _isInst = false;
    public float _sliderSpeed = 5f;
    private AudioSource _audioSource;

    [Header("パネル")]
    [SerializeField] private RectTransform _TitlePanel;
    [SerializeField] private RectTransform _InstPanel;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(_isInst && _TitlePanel.anchoredPosition.x > -800f)
        {
            // パネルを移動させる
            _TitlePanel.anchoredPosition += new Vector2(-_sliderSpeed, 0f) * Time.deltaTime;
            _InstPanel.anchoredPosition += new Vector2(-_sliderSpeed, 0f) * Time.deltaTime;
        }
        else if(_isInst && _InstPanel.anchoredPosition.x != 0f)
        {
            _TitlePanel.anchoredPosition = new Vector2(-800f, 0f);
            _InstPanel.anchoredPosition = new Vector2(0f, 0f);
        }
    }


    // ボタンを押されたら移動させる
    public void OnClickInst()
    {
        _isInst = true;
        _audioSource.volume *= 0.5f;
    }
}
