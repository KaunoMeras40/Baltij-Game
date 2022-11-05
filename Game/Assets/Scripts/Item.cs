using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    [Header("Item Information")]
    public string itemName;

    public int damageModifier;

    public itemTypeSlot item;

    public weaponType weaponType;

    public itemType consumableType;

    public GameObject itemPrefab;

    public bool pistol;

    public Sprite itemIcon;
    public int itemPrice;

    public string description;
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

public enum itemTypeSlot { M4A1, NERF_PISTOL, MOSSBERG, M14, CONSUMABLE }

public enum weaponType { Rifle, Pistol, Shotgun, NONE }
public enum itemType { Pepsi, Redbull, Xanax, Zaza, Meth, THC, NONE }