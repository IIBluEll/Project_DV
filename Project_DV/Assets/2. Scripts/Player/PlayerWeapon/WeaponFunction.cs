using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFunction : MonoBehaviour
{
    #region Cashing

    [SerializeField] protected PlayerAnimationControl playerAnim;
    [SerializeField] protected Camera mainCamera;

    [Space(10f), Header("## Audio Clips")] 
    [SerializeField] protected AudioClip audioClip_Fire;
    [SerializeField] protected AudioClip audioClip_Reload;
    
    [Space(10f), Header("## Fire Effect")]
    [SerializeField] protected GameObject muzzleFlashEffect;
    
    [Space(10f), Header(" ## CASING SPAWN POINT ##")]
    [SerializeField] protected Transform casingSpawnPoint;
    
    [Space(10f), Header(" ## WEAPON SETTING ##")]
    [SerializeField] protected WeaponSetting weaponSetting;

    private AudioSource gunAudioSource;
    private BulletCasing_Pool bulletCasingPool;

    #endregion
    
    protected bool isReload = false;
    protected float lastAttackTime = 0;

    protected void OnEnable()
    {
        muzzleFlashEffect.SetActive(false);
    }

    protected void Awake()
    {
        gunAudioSource = GetComponent<AudioSource>();
        bulletCasingPool = GetComponent<BulletCasing_Pool>();
        
        weaponSetting.currentAmmo = weaponSetting.maxSizeMagazine;
    }

    public void StartWeaponFire()
    {
        if (isReload) { return; }

        if (weaponSetting.isAutomaticAttack)
        {
            StartCoroutine("OnAttackLoop");
        }
        else
        {
            OnAttack();
        }
    }

    public void StopWeaponFire()
    {
        StopCoroutine("OnAttackLoop");
    }

    protected IEnumerator OnAttackLoop()
    {
        while (true)
        {
            OnAttack();
            yield return null;
        }
    }

    protected void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponSetting.attackRate)
        {
            lastAttackTime = Time.time;

            if (weaponSetting.currentAmmo <= 0)
            {
                // 재장전
                return;
            }

            weaponSetting.currentAmmo--;
            
            // 총기 발사 애니메이션
            playerAnim.PlayAnim("Fire",1,0);
            // 발사 사운드
            PlaySound(audioClip_Fire);
            // 총구 화염 이펙트 코루틴
            StartCoroutine("OnMuzzleFlashEffect");
            // 탄피 생성
            bulletCasingPool.SpawnCasing(casingSpawnPoint.position,transform.right);
            // 레이를 이용한 총알 발사
        }
    }

    protected IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);

        yield return new WaitForSeconds(weaponSetting.attackRate * 0.3f);
        
        muzzleFlashEffect.SetActive(false);
    }

    protected void PlaySound(AudioClip clip)
    {
        gunAudioSource.Stop();
        gunAudioSource.clip = clip;
        gunAudioSource.Play();
    }
}
