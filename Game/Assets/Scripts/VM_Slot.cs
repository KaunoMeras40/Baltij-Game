using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VM_Slot : MonoBehaviour
{
	public Image icon;
	public TextMeshProUGUI nameText;
	public Item item;
	PlayerManager plrManager;

	bool Weapon;

	private void Start()
	{
		plrManager = PlayerManager.Instance;
	}
	public void AddItem(Item weapon, bool isWeapon)
	{
		item = weapon;
		//icon.enabled = true;
		//icon.sprite = item.itemIcon;
		nameText.text = item.itemName + " - " + item.itemPrice + "€";
		Weapon = isWeapon;
	}

	public void BuyItem()
	{
		if (item != null)
		{
			if (plrManager.playerMoney >= item.itemPrice)
            {
				plrManager.VendingMachine_Purchase(item, Weapon, item.itemPrice);
				GetComponent<TextMeshProUGUI>().enabled = false;
				GetComponentInChildren<Button>().gameObject.SetActive(false);
				FindObjectOfType<AudioManager>().PlaySound("VM_Purchase");
			}
		}
	}
}
