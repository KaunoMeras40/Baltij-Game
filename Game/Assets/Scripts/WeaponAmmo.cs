using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using TMPro;
public class WeaponAmmo : MonoBehaviour
{
    public int clipSize;
    public int extraAmmo;

    [SerializeField] TextMeshProUGUI ammoText;

    [HideInInspector] public int currentAmmo;
    public ammoTypes ammoType;

    Animator animator;
    StarterAssetsInputs Inputs;
    PlayerAimController aimController;
    void Start()
    {
        currentAmmo = clipSize;
        animator = PlayerAimController.instance.animator;
        Inputs = StarterAssetsInputs.instance;
        aimController = PlayerAimController.instance;
    }

    private void Update()
    {
        if (Inputs.reload)
        {
            if (aimController.reloading == false)
            {
                Reload();
                Invoke(nameof(reloadingOff), 3f);
            }
            Inputs.reload = false;
        }
        ammoText.text = "Ammo: " + currentAmmo + "/" + extraAmmo;
    }

    void reloadingOff()
    {
        aimController.reloading = false;
    }

    public void Reload()
    {
        animator.SetTrigger("Reload");
        aimController.reloading = true;
        if (extraAmmo >= clipSize)
        {
            int ammoToReload = clipSize - currentAmmo;
            extraAmmo -= ammoToReload;
            currentAmmo += ammoToReload;
        }
        else if (extraAmmo > 0)
        {
            if (extraAmmo + currentAmmo > clipSize)
            {
                int leftOverAmmo = extraAmmo + currentAmmo - clipSize;
                extraAmmo = leftOverAmmo;
                currentAmmo = clipSize;
            }
            else
            {
                currentAmmo += extraAmmo;
                extraAmmo = 0;
            }
        }
    }

    public void RefillAmmo(int amount)
    {
        extraAmmo += amount;
    }
}

public enum ammoTypes { Light, Medium, Heavy, Slugs}