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

	private void Start()
	{
		player = PlayerManager.Instance.player.transform;
		controller = PlayerAimController.instance;
		controller.onItemInteractCallback += CheckInteract;
	}
	public virtual void Interact()
	{
		hasInteracted = true;
		Debug.Log("Interacting with " + transform.name);
		item.Equip();
		Destroy(gameObject);
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
