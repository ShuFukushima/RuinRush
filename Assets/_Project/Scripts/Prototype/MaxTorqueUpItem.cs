using UnityEngine;

// アイテムベースクラスを継承
public class MaxTorqueUpItem : ItemBase
{
    [SerializeField] private int _addMaxTorque;

    // トルクを加算
    protected override void ApplyEffect(CarController player)
    {
        player.IncreaseMaxTorque(_addMaxTorque);
    }
}
