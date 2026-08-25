using UnityEngine;

public class PrototypeVehicleUpgradeItem : MonoBehaviour
{
    // アイテムの種類分類
    enum ItemType
    {
        MaxSpeed,
        MotorPower
    }

    // 各種パワーアップパラメーター（最初は最高速のみ）
    [SerializeField] private float _addMaxSpeed;

    // プレイヤーとの当たりを取得
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // プレイヤーならばコントローラーを取得
            PrototypeCarController player = other.GetComponentInParent<PrototypeCarController>();

            // プレイヤーだったら
            if (player != null)
            {
                // 自身のアイテム種別に従ってパラメーターを強化する
                player.IncreaseMaxSpeed(_addMaxSpeed);
                Destroy(gameObject);
            }

            // 自身を破壊する
            Destroy(gameObject);
        }
    }

}
