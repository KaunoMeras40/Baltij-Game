using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    private void Start()
    {
        playerMoney = 20;
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
    }



}
