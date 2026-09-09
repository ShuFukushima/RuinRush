using UnityEngine;

// アイテムベースクラスを継承
public class MaxSpeedUpItem : ItemBase
{
    /// <summary>最高速を加算</summary>
    [SerializeField] private int _addMaxSpeed;

    protected override void ApplyEffect(CarController player)
    {
        player.IncreaseMaxSpeed(_addMaxSpeed);
    }
}
