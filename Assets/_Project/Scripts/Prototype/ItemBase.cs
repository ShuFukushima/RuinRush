using System;
using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{
    // 生成時にプレイヤーを取得する
    private CarController _player;
    private float _speed = 200f;

    private static float _lastPlayTime = float.NegativeInfinity;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetAudioTime()
    {
        _lastPlayTime = float.NegativeInfinity;
    }
    [SerializeField] private AudioClip _audioClip;
    [SerializeField] private float _audioVol = 1f;
    [SerializeField] private float _audioInterval = 0.7f;

    private void Awake()
    {
        _player = FindAnyObjectByType<CarController>();

    }

    private void Update()
    {
        // プレイヤーと自身の方向を取得
        Vector3 dir = (_player.transform.position - this.gameObject.transform.position).normalized;

        // プレイヤーに向かわせる
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
    }

    // プレイヤーとの当たりを取得
    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // プレイヤーならばコントローラーを取得
            CarController player = other.GetComponentInParent<CarController>();

            // プレイヤーだったら
            if (player != null)
            {
                Debug.Log("パワーアップアイテムを取得");
                // 自身のアイテム種別に従ってパラメーターを強化する
                ApplyEffect(player);
                // 効果音を鳴らす
                // オブジェクトを破壊する直前などに呼ぶ
                if (Time.time - _lastPlayTime >= _audioInterval)
                {
                    Debug.Log($"{name}  Volume:{_audioVol}  Interval:{_audioInterval}");

                    // 同時に効果音が鳴ることを防止する
                    AudioSource.PlayClipAtPoint(_audioClip, transform.position, _audioVol);

                    _lastPlayTime = Time.time;
                }
                // 自身を破壊する
                Destroy(gameObject);
            }


        }
    }


    /// <summary>
    /// プレイヤーを強化する各種の処理
    /// 各継承先のクラスで処理を書く
    /// </summary>
    /// <param name="player"></param>
    protected abstract void ApplyEffect(CarController player);
}
