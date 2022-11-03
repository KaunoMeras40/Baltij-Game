using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
public class VendingMachine : MonoBehaviour
{
    PlayerManager plrManager;
    StarterAssetsInputs Inputs;

    public VM_Slot slot;
    public Transform VM_Items_Parent;

    GameObject[] AllMachines;

    [SerializeField] GameObject vendingUI_1;
    [SerializeField] GameObject vendingUI_2;

    bool isSetup;
    bool closed;
    private void Start()
    {
        plrManager = PlayerManager.Instance;
        AllMachines = GameObject.FindGameObjectsWithTag("VendingMachine");
        Inputs = StarterAssetsInputs.instance;
    }

    void Update()
    {
        if (Vector3.Distance(AllMachines[0].transform.position, plrManager.player.transform.position) >= 1.5f)
        {
            if (Vector3.Distance(AllMachines[1].transform.position, plrManager.player.transform.position) >= 1.5f)
            {
                vendingUI_1.SetActive(false);
                vendingUI_2.SetActive(false);
                if (closed == false)
                {
                    close();
                }
            }
        }
    }

    void close()
    {
        Inputs.cursorLocked = true;
        plrManager.canShoot = true;
        closed = true;
    }
    void RemoveAllItems()
    {
        VM_Slot[] slots = VM_Items_Parent.GetComponentsInChildren<VM_Slot>();
        foreach (VM_Slot item in slots)
        {
            Destroy(item.gameObject);
        }
    }

    public void SetupVM()
    {
        Inputs.cursorLocked = false;
        plrManager.canShoot = false;
        closed = false;
        if (isSetup == true) return;
        RemoveAllItems();

        List<Item> allItems = new List<Item>(plrManager.VM_Items);
        List<Item> allWeapons = new List<Item>(plrManager.VM_Weapons);

        int int1 = Random.Range(0, allWeapons.Count);
        Item ranWeapon1 = allWeapons[int1];
        allWeapons.Remove(ranWeapon1);
        int int2 = Random.Range(0, allWeapons.Count);
        Item ranWeapon2 = allWeapons[int2];
        allWeapons.Remove(ranWeapon2);

        int int3 = Random.Range(0, allItems.Count);
        Item ranitem1 = allItems[int3];
        allItems.Remove(ranitem1);

        VM_Slot wpnSlot1 = Instantiate(slot, VM_Items_Parent);
        VM_Slot wpnSlot2 = Instantiate(slot, VM_Items_Parent);
        VM_Slot wpnSlot3 = Instantiate(slot, VM_Items_Parent);
        wpnSlot1.AddItem(ranWeapon1, true);
        wpnSlot2.AddItem(ranWeapon2, true);
        wpnSlot3.AddItem(ranitem1, false);

        isSetup = true;
    }

}
