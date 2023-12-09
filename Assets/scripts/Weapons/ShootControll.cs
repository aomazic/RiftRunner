using DG.Tweening;
using System;
using UnityEngine;

public class ShootControll : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] bool isShootgun;
    [Header("Bullet Object")]
    [SerializeField] GameObject bullet;

    [Header("Bullet Force")]
    [SerializeField] float shootForce;

    [Header("Gun stats")]
    [SerializeField] float timeBetweenShooting, spread, bulletsPerTap;
    [SerializeField] bool allowButtonHold;

    [Header("Sniper settings")]
    [SerializeField] float maxChargeTime;
    [SerializeField] float maxDamageMultiplier;
    [SerializeField] float maxBulletSpeedMultiplier;
    [SerializeField] float maxBulletRangeMultiplier;


    bool shooting, readyToShoot;
    float currentChargeTime;
    Camera fpsCam;

    [Header("References")]
    [SerializeField] Transform attackPoint;
    [SerializeField] Animator weaponAnimator;
  


    [Header("Graphics")]
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] GameObject chargingParticlesPrefab;
    private GameObject chargingParticles;
    [SerializeField] AudioClip shootAudioClip;
    [SerializeField] AudioClip chargeAudioClip;
    [SerializeField] bool allowInvoke = true;

    private void Awake()
    {
        fpsCam = Camera.main;
        readyToShoot = true;
        audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {
        MyInput();
    }

    private void MyInput()
    {
        if (Input.GetKey(KeyCode.Mouse0) && currentChargeTime < maxChargeTime && allowInvoke)
        {
            currentChargeTime += Time.deltaTime;
            float chargeAnimationSpeed = Mathf.Lerp(1f, 15f, currentChargeTime / maxChargeTime);
            weaponAnimator.speed = chargeAnimationSpeed;
            weaponAnimator.SetBool("charge", true);
            UpdateChargingParticles();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && allowInvoke)
        {
            weaponAnimator.speed = 1f;
            weaponAnimator.SetBool("charge", false);
            weaponAnimator.SetBool("fire", true);
            Shoot(currentChargeTime);
            currentChargeTime = 0f;
        }
    }

    private void PlayShootAudio()
    {
        if (audioSource != null && shootAudioClip != null)
        {
            audioSource.clip = shootAudioClip;
            audioSource.PlayOneShot(shootAudioClip);
        }
    }

    private void Shoot(float chargeTime)
    {
        readyToShoot = false;

        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        int layerMask = ~LayerMask.GetMask("whatIsPlayer", "whatIsGun", "UI", "whatIsBullet", "whatIsEnemyProjectil");
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }

        Vector3 shootingDirection = targetPoint - attackPoint.position;
        shootingDirection.Normalize();

        for (int i = 0; i < bulletsPerTap; i++)
        {
            float x = UnityEngine.Random.Range(-spread, spread);
            float y = UnityEngine.Random.Range(-spread, spread);
            Vector3 directionWithSpread = shootingDirection + new Vector3(x, y, 0); 
            GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.LookRotation(directionWithSpread.normalized));
            audioSource.Stop();
            PlayShootAudio();
            float currentShootForce = shootForce;
            float chargeRatio = chargeTime / maxChargeTime;
            float damageMultiplier = 1f + (chargeRatio * maxDamageMultiplier);
            float bulletSpeedMultiplier = 1f + (chargeRatio * maxBulletSpeedMultiplier);
            float bulletRangeMultiplier = 1f + (chargeRatio * maxBulletRangeMultiplier);
            currentShootForce *= bulletSpeedMultiplier;
            currentBullet.GetComponent<ProjectilControll>().ModifyBullet(damageMultiplier, bulletRangeMultiplier);
            currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * currentShootForce, ForceMode.Impulse);
        }

        if (muzzleFlash != null)
            muzzleFlash.Play();

        if (chargingParticles != null)
        {
            DOTween.KillAll();
            Destroy(chargingParticles);
        }
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }
    }

    private void ResetShot()
    {
        weaponAnimator.SetBool("fire", false);
        readyToShoot = true;
        allowInvoke = true;
    }

    private void UpdateChargingParticles()
    {
        if (chargingParticles == null)
        {
            chargingParticles = Instantiate(chargingParticlesPrefab, attackPoint.position, Quaternion.identity);
            chargingParticles.transform.parent = attackPoint;
  
            chargingParticles.transform.DOScale(new Vector3(0.2f, 0.2f, 0.2f) , maxChargeTime);
            audioSource.PlayOneShot(chargeAudioClip);
        }

 
    }


}
