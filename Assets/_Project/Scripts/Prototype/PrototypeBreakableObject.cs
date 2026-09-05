using UnityEngine;

public class PrototypeBreakableObject : MonoBehaviour
{
    [SerializeField] private float _breakingSpeed;  // 必要破壊速度
    [SerializeField] private int _scoreValue;       // このオブジェクトを破壊したら入る得点
    [SerializeField] private GameObject _upgradeMaxSpeedItem;   // 破壊された際に生成される最高速度強化プレハブ
    [SerializeField] private GameObject _upgradeMaxTorqueItem;  // 破壊された際に生成されるモータートルク強化プレハブ
    [SerializeField] private GameObject _fragmentObj;   // 破壊された際に生成される破片

    [SerializeField] private float _explosionForce = 20f;
    [SerializeField] private float _explosionRadius = 1f;
    private PrototypeGameSession _gameSession; // 破壊された際にスコア加算を行う


    private void Start()
    {
        // ゲームセッションスクリプトを持つオブジェクトをシーン上から取得する
        _gameSession = FindFirstObjectByType<PrototypeGameSession>();

        // エラー処理
        if (_gameSession == null)
        {
            Debug.LogError("PrototypeGameSessionが見つかりません。");
        }
    }


    // プレイヤーと衝突した際に、破壊速度かどうかを判定する（旧バージョン）
    /*
    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーかどうかを判定
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("接触");

            // プレイヤーと自身の相対速度を取得
            float relativeSpeed = collision.relativeVelocity.magnitude * 3.6f;
            Debug.Log("相対速度：" + relativeSpeed + " km/h");

            // プレイヤーとの接触点を取得
            Vector3 playerCollisionPoint = collision.GetContact(0).point;

            // 一定速度以上なら破壊、一定速度未満なら破壊しない
            if (relativeSpeed >= _breakingSpeed)
            {
                Debug.Log("必要破壊速度以上なので破壊します。");
                Break(playerCollisionPoint);
            }
            else
            {
                Debug.Log("必要破壊速度未満です。破壊しません。");
            }

        }
    }
    */
    

    // プレイヤーと衝突した際に破壊速度かどうかを判定
    private void OnTriggerEnter(Collider other)
    {
        // プレイヤーかどうか判定
        if (!other.CompareTag("Player")) return;

        Debug.Log("接触");

        // プレイヤーの速度を取得
        PrototypeCarController player = other.gameObject.GetComponent<PrototypeCarController>();
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
        // 現在は仮コードのため、自身を破壊する処理と、ゲームセッション側の得点加算メソッドの呼び出しだけ
        // 将来的に、破片に砕ける処理などにする
        _gameSession.AddScore(_scoreValue);

        // Breakは破壊する際の処理まとめになるので、この中でアイテムの生成を呼ぶ
        SpawnUpgradeItem(); // アイテムを生成
        SpawnFragmentObj(collisionPoint); // 破片を生成

        Destroy(gameObject);
    }

    /// <summary>
    /// 強化アイテムを生成する
    /// </summary>
    private void SpawnUpgradeItem()
    {
        // 確率でアイテムを選択する
        GameObject upgradeItem;
        if(Random.Range(0, 2) == 0)
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
    /// 破片を生成する
    /// </summary>
    private void SpawnFragmentObj(Vector3 collisionPoint)
    {
        // 生成した破片オブジェクトを取得する
        GameObject brokenBuilding = Instantiate(_fragmentObj, gameObject.transform.position, transform.rotation);

        // 子オブジェクトを格納する配列作成
        Rigidbody[] brokenFragment = new Rigidbody[brokenBuilding.transform.childCount];

        // 0～個数-1までの子を順番に配列に格納
        for (var i = 0; i < brokenFragment.Length; ++i)
        {
            // 生成した破片オブジェクトの子オブジェクトのRigidbody をすべて取得する
            brokenFragment[i] = brokenBuilding.transform.GetChild(i).GetComponent<Rigidbody>();

            // 爆発力をプレイヤーとビルの接触点（ワールド座標）に加える
            // 破片に加える力の大きさ・半径は後で入力できるようにする
            brokenFragment[i].AddExplosionForce(_explosionForce, collisionPoint, _explosionRadius, 0f, ForceMode.Impulse);
        }

    }

}
