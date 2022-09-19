using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Item Information")]
    public string itemName;

    public int damageModifier;

    public itemTypeSlot item;

    public GameObject itemPrefab;

    public bool pistol;
    public virtual void Equip()
    {
        Debug.Log("Equipping " + name);
        PlayerManager.Instance.switchWeapon(this);
    }

    public virtual void Unequip()
    {
        Debug.Log("Unequipping " + name);
    }
}

public enum itemTypeSlot { M4A1, NERF_PISTOL }
