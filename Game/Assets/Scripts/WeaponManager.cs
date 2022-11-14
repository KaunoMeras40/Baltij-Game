using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.Animations.Rigging;

public class WeaponManager : MonoBehaviour
{

    [SerializeField] float fireRate;
    float fireRateTimer;
    [SerializeField] bool semiAuto;
    bool hasShot;
    [SerializeField] int bulletsPerShot;
    StarterAssetsInputs Inputs;
    PlayerAimController aimController;

    [SerializeField] private Transform shootingPoint;
    [SerializeField] private ParticleSystem cartrigeParticle;
    [SerializeField] private GameObject BulletPrefab;

    [SerializeField] AudioClip gunShot;
    [SerializeField] AudioClip gunShotBlank;

    [SerializeField] AudioClip MagazineOutClip;
    [SerializeField] AudioClip LoadClip;
    [SerializeField] AudioClip LoadEndClip;
    [SerializeField] AudioClip PumpAfterShot;

    [SerializeField] bool shotgun;
    [SerializeField] bool cartrigesOn; 

    AudioSource audioSource;

    WeaponAmmo ammo;

    [SerializeField] private ParticleSystem ShootingEffect;

    [SerializeField] public itemTypeSlot item;

    public TwoBoneIKConstraint LHandIK;
    public TwoBoneIKConstraint LHandIKAimed;

    private CharacterStats charStats;

    PlayerManager playerManager;
    void Start()
    {
        fireRateTimer = fireRate;
        Inputs = StarterAssetsInputs.instance;
        aimController = PlayerAimController.instance;
        audioSource = GetComponent<AudioSource>();
        ammo = GetComponent<WeaponAmmo>();
        charStats = GetComponentInParent<CharacterStats>();
        playerManager = PlayerManager.Instance;
    }
    void Update()
    {
        if (ShouldFire()) Fire();
        if(Inputs.shoot == false)
        {
            hasShot = false;
        }
    }

    bool ShouldFire()
    {
        fireRateTimer += Time.deltaTime;
        aimController.animator.SetBool("Shoot", false);
        if (fireRateTimer < fireRate)
        {
            return false;
        }
        //if (ammo.currentAmmo == 0) return false;  && playerManager.canShoot
        if (Inputs.sprint == true && aimController.aimed == false) return false;
        if (aimController.reloading == true) return false;
        if (playerManager.canShoot == false) return false;
        if (semiAuto && Inputs.shoot && !hasShot) return true;
        if (!semiAuto && Inputs.shoot) return true;
        return false;
    }

    void Fire()
    {
        if (ammo.currentAmmo > 0)
        {
            if (aimController.aimed == false)
            {
                aimController.AimToTarget();
            }
            aimController.choking = false;
            hasShot = true;
            fireRateTimer = 0f;
            shootingPoint.LookAt(aimController.hitPos);
            audioSource.PlayOneShot(gunShot);
            aimController.animator.SetBool("Shoot", true);
            if (itemTypeSlot.M249 == item)
            {
                aimController.Shake(0.9f, 0.4f, 12f);
            }
            else
            {
                aimController.Shake(0.7f, 0.2f, 10f);
            }
            ammo.currentAmmo--;
            ParticleSystem.MainModule main = ShootingEffect.main;
            main.simulationSpeed = 2;
            ShootingEffect.Play();
            if (cartrigesOn)
            {
                cartrigeParticle.Play();
            }
            if (shotgun)
            {
                audioSource.PlayOneShot(PumpAfterShot);
                aimController.choking = true;
                for (int i = 0; i < bulletsPerShot; i++)
                {
                    float spreadFactor = 0.05f;
                    Vector3 newPoint = shootingPoint.forward;
                    newPoint.x += Random.Range(-spreadFactor, spreadFactor);
                    newPoint.y += Random.Range(-spreadFactor, spreadFactor);
                    newPoint.z += Random.Range(-spreadFactor, spreadFactor);

                    GameObject currentBullet = Instantiate(BulletPrefab, shootingPoint.position, Quaternion.identity);
                    int damage = charStats.damage.GetValue();
                    currentBullet.GetComponent<Bullet>().damage = Mathf.RoundToInt(damage);
                    Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
                    rb.AddForce(newPoint * 300f, ForceMode.Impulse);
                }
            }
            else
            {
                GameObject currentBullet = Instantiate(BulletPrefab, shootingPoint.position, Quaternion.identity);
                currentBullet.GetComponent<Bullet>().damage = charStats.damage.GetValue();
                Rigidbody rb = currentBullet.GetComponent<Rigidbody>();
                rb.AddForce(shootingPoint.forward * 300f, ForceMode.Impulse);
            }
        }
        else
        {
            fireRateTimer = 0f;
            audioSource.PlayOneShot(gunShotBlank);
        }
    }

    public void MagazineOut()
    {
       audioSource.PlayOneShot(MagazineOutClip);
    }

    public void Load()
    {
        audioSource.PlayOneShot(LoadClip);
    }

    public void LoadEnd()
    {
        audioSource.PlayOneShot(LoadEndClip);
    }

}
