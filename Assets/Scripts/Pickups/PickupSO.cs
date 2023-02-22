using UnityEngine;

[CreateAssetMenu(fileName = "PickupSO", menuName = "ScriptableObjects/PickupSO", order = 1)]
public class PickupSO : ScriptableObject
{
    public enum PickupId
    {
        IRON,
        NICKEL,
        COPPER,
        GOLD,
        PLATINUM,
        DIAMOND
    }

    public PickupId pickupId;
    public string pickupName;
    public int value;
    public Sprite sprite;
}
