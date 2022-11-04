using UnityEngine;

[CreateAssetMenu(fileName = "PickupScriptableObject", menuName = "ScriptableObjects/PickupScriptableObject", order = 1)]
public class PickupScriptableObject : ScriptableObject
{
    public enum PickupId
    {
        IRON,
        NICKEL,
        COPPER
    }

    public PickupId pickupId;
    public string pickupName;
    public int value;
    public Sprite sprite;
}
