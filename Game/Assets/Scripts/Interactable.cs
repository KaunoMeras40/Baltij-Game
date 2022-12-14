using UnityEngine;
using StarterAssets;

public class Interactable : MonoBehaviour
{

	public float radius = 3f;
	public Transform interactionTransform;

	PlayerAimController controller;

	Transform player;

	bool hasInteracted = false;
	public Item item;

	public int doorPrice;
	[HideInInspector] public bool hasOpened = false;

	public GameObject vendingMachineObject;

	public interactableType interactable;

	public string pickupSound;

	private void Start()
	{
		player = PlayerManager.Instance.player.transform;
		controller = PlayerAimController.instance;
		controller.onItemInteractCallback += CheckInteract;
	}

    private void Update()
	{
		if (!hasInteracted)
		{
			float distance = Vector3.Distance(player.position, interactionTransform.position);
			if (distance <= radius)
			{
				controller.currentInteractable = this;
			}
		}
	}
    public virtual void Interact()
	{
		if (interactable == interactableType.Door && hasOpened == false)
        {
			if (PlayerManager.Instance.playerMoney >= doorPrice)
			{
				// opoen
				// animation
				PlayerManager.Instance.playerMoney -= doorPrice;
				Debug.Log("OPEN");
				hasOpened = true;
			}
		}
		else if (interactable == interactableType.VendingMachine)
        {
			PlayerManager.Instance.OpenVM(vendingMachineObject);
		}
		else if (interactable == interactableType.WeaponBuy)
		{
			PlayerManager.Instance.WeaponWall_Purchase(item, GetComponentInChildren<ItemObject>().gameObject, this);
		}
		else if (interactable == interactableType.Default)
		{
			hasInteracted = true;
			item.Equip();
			FindObjectOfType<AudioManager>().PlaySound(pickupSound);
			Destroy(gameObject);
		}
	}

	void CheckInteract()
	{
		if (!hasInteracted)
		{
			float distance = Vector3.Distance(player.position, interactionTransform.position);
			if (distance <= radius)
			{
				Interact();
			}
		}
	}
	void OnDrawGizmosSelected()
	{
		if (interactionTransform == null)
			interactionTransform = transform;

		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(interactionTransform.position, radius);
	}

}

public enum interactableType { Default, Door, VendingMachine, WeaponBuy }
