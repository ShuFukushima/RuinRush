using UnityEngine;

public abstract class ItemBase : MonoBehaviour
{

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
