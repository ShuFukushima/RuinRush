using UnityEngine;

public class VehicleUpgradeItem : MonoBehaviour
{
    // アイテムの種類分類
    enum ItemType
    {
        MaxSpeed,
        MotorPower
    }

    // アイテムの種類を定義
    [SerializeField] private ItemType _itemType;

    // 各種パワーアップパラメーター（最初は最高速のみ）
    [SerializeField] private float _addMaxSpeed;
    [SerializeField] private float _addMaxTorque;

    // プレイヤーとの当たりを取得
    private void OnTriggerEnter(Collider other)
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
                if(_itemType == ItemType.MaxSpeed)
                {
                    player.IncreaseMaxSpeed(_addMaxSpeed);
                }
                else if(_itemType == ItemType.MotorPower)
                {
                    player.IncreaseMaxTorque(_addMaxTorque);
                }

                // 自身を破壊する
                Destroy(gameObject);
            }


        }
    }

}
