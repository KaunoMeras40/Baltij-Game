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

	public string description;

	public bool door;
	public int doorPrice;
	[HideInInspector] public bool hasOpened = false;

	public bool vendingMachine;
	[SerializeField] GameObject vendingMachineObject;

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
		if (door == true && hasOpened == false)
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
		else if (vendingMachine == true)
        {
			PlayerManager.Instance.OpenVM(vendingMachineObject);
		}
		else
        {
			hasInteracted = true;
			item.Equip();
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
