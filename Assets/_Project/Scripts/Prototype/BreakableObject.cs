using System;
using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    [SerializeField] private float _breakingSpeed;  // 必要破壊速度
    [SerializeField] private int _scoreValue;       // このオブジェクトを破壊したら入る得点
    [SerializeField] private GameObject _upgradeMaxSpeedItem;   // 破壊された際に生成される最高速度強化プレハブ
    [SerializeField] private GameObject _upgradeMaxTorqueItem;  // 破壊された際に生成されるモータートルク強化プレハブ
    [SerializeField] private GameObject _fragmentObj;   // 破壊された際に生成される破片
    [SerializeField] private GameObject _particle;      // 破壊された際に生成される爆発パーティクル
    [SerializeField] private float _shakeIntensity;     // 破壊された際に揺らす強さ

    [SerializeField] private float _explosionForce = 20f;
    [SerializeField] private float _explosionRadius = 1f;
    private GameSession _gameSession; // 破壊された際にスコア加算を行う
    private CameraShake _cameraShake;   // 破壊された際にカメラを揺らす

    private void Start()
    {
        // ゲームセッションスクリプトを持つオブジェクトをシーン上から取得する
        _gameSession = FindAnyObjectByType<GameSession>();
        _cameraShake = FindAnyObjectByType<CameraShake>();

        // エラー処理
        if (_gameSession == null)
        {
            Debug.LogError("PrototypeGameSessionが見つかりません。");
        }
    }
    

    // プレイヤーと衝突した際に破壊速度かどうかを判定
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーかどうか判定
        if (!other.CompareTag("Player")) return;

        // プレイヤーの速度を取得
        CarController player = other.gameObject.GetComponent<CarController>();
        float playerSpeed = player.GetPlayerCurrentSpeed();

        // 接触した瞬間のプレイヤーの座標を取得
        Vector3 playerCollisionPoint = player.transform.position;

        // 一定速度以上なら破壊、一定速度未満なら破壊しない
        if (playerSpeed >= _breakingSpeed)
        {
            Debug.Log("必要破壊速度以上なので破壊します。");
            Break(playerCollisionPoint);
        }
        else
        {
            Debug.Log("必要破壊速度未満です。破壊しません。");
        }



    }

    /// <summary>
    /// 建物を破壊する処理
    /// </summary>
    /// <param name="collisionPoint"></param>
    private void Break(Vector3 collisionPoint)
    {
        // スコアを加える
        _gameSession.AddScore(_scoreValue);

        // Breakは破壊する際の処理まとめになるので、この中でアイテムの生成を呼ぶ
        // 廃墟は階層としては下の方のため、今回はデリゲートは使わない（使うととんでもないことになるから）
        SpawnUpgradeItem();                 // アイテムを生成
        SpawnFragmentObj(collisionPoint);   // 破片を生成
        SpawnParticle(collisionPoint);      // パーティクルを生成
        _cameraShake.StartShake(_shakeIntensity);

        // 最後に自身を破壊する
        Destroy(gameObject);
    }

    /// <summary>
    /// 強化アイテムを生成する
    /// </summary>
    private void SpawnUpgradeItem()
    {
        // 確率でアイテムを選択する
        GameObject upgradeItem;
        if(UnityEngine.Random.Range(0, 2) == 0)
        {
            upgradeItem = _upgradeMaxSpeedItem;
        }
        else
        {
            upgradeItem = _upgradeMaxTorqueItem;
        }

        // アイテムを生成する
        Instantiate(upgradeItem, gameObject.transform.position, Quaternion.identity);
    }

    /// <summary>
    /// 破壊時にパーティクルを生成
    /// </summary>
    private void SpawnParticle(Vector3 collisionPoint)
    {
        // パーティクルを生成
        Instantiate(_particle, new Vector3(transform.position.x, collisionPoint.y, transform.position.z), Quaternion.identity);
    }

    /// <summary>
    /// 破片を生成する
    /// </summary>
    private void SpawnFragmentObj(Vector3 collisionPoint)
    {
        // 生成した破片オブジェクトを取得する
        GameObject brokenBuilding = Instantiate(_fragmentObj, gameObject.transform.position, transform.rotation);

        // 生成した破片オブジェクトの子オブジェクトのRigidbody をすべて取得する
        Rigidbody[] brokenFragments = brokenBuilding.GetComponentsInChildren<Rigidbody>();

        // 0～個数-1までの子を順番に配列に格納
        for (var i = 0; i < brokenFragments.Length; ++i)
        {
            // 爆発力をプレイヤーとビルの接触点（ワールド座標）に加える
            // 破片に加える力の大きさ・半径は後で入力できるようにする
            brokenFragments[i].AddExplosionForce(_explosionForce, collisionPoint, _explosionRadius, 0f, ForceMode.Impulse);
        }

    }

    /// <summary>
    /// この建物の破壊可能速度を外部から取得するゲッターメソッド
    /// </summary>
    /// <returns></returns>
    public float GetBreakingSpeed()
    {
        return _breakingSpeed;
    }

}

