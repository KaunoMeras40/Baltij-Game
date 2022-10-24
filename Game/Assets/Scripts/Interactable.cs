using UnityEngine;
using StarterAssets;

public class Interactable : MonoBehaviour
{

	public float radius = 3f;
	public Transform interactionTransform;

	PlayerAimController controller;

	Transform player;

	bool hasInteracted = false;
	[HideInInspector] public bool hasOpened = false;

	public Item item;

	[SerializeField] bool door;
	public int doorPrice;

	private void Start()
	{
		player = PlayerManager.Instance.player.transform;
		controller = PlayerAimController.instance;
		controller.onItemInteractCallback += CheckInteract;
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
