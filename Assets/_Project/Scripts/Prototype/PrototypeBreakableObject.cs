using UnityEngine;

public class PrototypeBreakableObject : MonoBehaviour
{
    [SerializeField] private float _breakingSpeed;  // 必要破壊速度
    [SerializeField] private int _scoreValue;       // このオブジェクトを破壊したら入る得点

    private PrototypeGameSession _gameSession; // 破壊された際にスコア加算を行う


    private void Start()
    {
        // ゲームセッションスクリプトを持つオブジェクトをシーン上から取得する
        _gameSession = FindFirstObjectByType<PrototypeGameSession>();

        // エラー処理
        if(_gameSession == null)
        {
            Debug.LogError("PrototypeGameSessionが見つかりません。");
        }
    }


    // プレイヤーと衝突した際に、破壊速度かどうかを判定する
    private void OnCollisionEnter(Collision collision)
    {
        // プレイヤーかどうかを判定
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("接触");

            // プレイヤーと自身の相対速度を取得
            float relativeSpeed = collision.relativeVelocity.magnitude;
            Debug.Log("相対速度：" + relativeSpeed);

            // 一定速度以上なら破壊、一定速度未満なら破壊しない
            if (relativeSpeed >= _breakingSpeed)
            {
                Debug.Log("必要破壊速度以上なので破壊します。");
                Break();
            }
            else
            {
                Debug.Log("必要破壊速度未満です。破壊しません。");
            }

        }
    }

    /// <summary>
    /// 建物を破壊する処理
    /// </summary>
    private void Break()
    {
        // 現在は仮コードのため、自身を破壊する処理と、ゲームセッション側の得点加算メソッドの呼び出しだけ
        // 将来的に、破片に砕ける処理などにする
        _gameSession.AddScore(_scoreValue);

        Destroy(gameObject);
    }

}
