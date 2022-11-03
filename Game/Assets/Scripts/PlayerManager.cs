using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    public GameObject player;

    [SerializeField] private Item currentWeapon;

    public new List<GameObject> weaponList;

    [SerializeField] TextMeshProUGUI weaponName;
    [SerializeField] TextMeshProUGUI moneyAmount;
    PlayerAimController aimController;

    public int playerMoney;
    public bool canShoot = true;

    public List<Item> VM_Items;
    public List<Item> VM_Weapons;

    GameObject[] vms;
    GameObject[] weapons;
    GameObject[] allInteractables;

    [SerializeField] List<Item> powerups;
    private void Start()
    {
        playerMoney = 500;
        aimController = PlayerAimController.instance;
    }

    public void switchWeapon(Item weapon)
    {
        foreach (GameObject item in weaponList)
        {
            item.SetActive(true);
            WeaponManager manager = item.GetComponent<WeaponManager>();

            if (manager.item == weapon.item)
            {
                if (currentWeapon != null)
                {
                    aimController.LHandIK.weight = 0f;
                    aimController.LHandIKAimed.weight = 0f;
                    aimController.GetComponent<CharacterStats>().RemoveDamage(currentWeapon.damageModifier);
                    Instantiate(currentWeapon.itemPrefab, player.transform.position, Quaternion.identity);
                }
                currentWeapon = weapon;
                aimController.GetComponent<CharacterStats>().AddDamage(weapon.damageModifier);
                item.SetActive(true);
                player.GetComponent<PlayerAimController>().SwitchWeapon(weapon);
            }
            else
            {
                item.SetActive(false);
            }
        }
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;
    }

    private void Update()
    {
        if (currentWeapon != null)
        {
            weaponName.text = currentWeapon.itemName;
        }
        moneyAmount.text = "Money: " + playerMoney + "€";

        vms = GameObject.FindGameObjectsWithTag("VendingMachine");
        weapons = GameObject.FindGameObjectsWithTag("PlayerWeapon");
        allInteractables = vms.Concat(weapons).ToArray();
        foreach (GameObject item in allInteractables)
        {
            Interactable itr = item.GetComponent<Interactable>();
            if (itr != null)
            {
                float distance = Vector3.Distance(player.transform.position, itr.interactionTransform.position);
                if (distance <= itr.radius)
                {
                    aimController.currentInteractable = itr;
                    break;
                }
                else
                {
                    aimController.currentInteractable = null;
                }
            }
        }
    }

    public void AddPowerup(Item powerup)
    {
        CharacterStats charStats = aimController.GetComponent<CharacterStats>();
        if (powerup.consumableType == itemType.Pepsi)
        {
            float healthAmount = charStats.maxHealth.GetValue() * 0.1f;
            charStats.AddMaxHealth(Mathf.RoundToInt(healthAmount));
            powerups.Add(powerup);
        }
        else if (powerup.consumableType == itemType.Redbull)
        {
            aimController.SpeedModifier.AddModifier(0.5f);
            float healthAmount = charStats.maxHealth.GetValue() * 0.1f;
            charStats.AddMaxHealth(Mathf.RoundToInt(healthAmount));
            powerups.Add(powerup);
        }
    }

    public void OpenVM(GameObject VM)
    {
        VM.SetActive(true);
        VM.GetComponent<VendingMachine>().SetupVM();
    }

    public void VendingMachine_Purchase(Item item, bool weapon, int price)
    {
        if (playerMoney >= price)
        {
            playerMoney -= price;
            if (weapon)
            {
                switchWeapon(item);
            }
            else
            {
                AddPowerup(item);
            }
        } 
    }

}
