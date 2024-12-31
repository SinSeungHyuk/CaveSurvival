using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/Weapon/Weapon")]
public class WeaponDetailsSO : IdentifiedObject
{
    [Header("Weapon Base Details")]
    public string weaponName;
    public Sprite weaponSprite;
    public string description; // 무기 기본설명
    [TextArea] public string upgradeDesc; // 무기 업그레이드 설명

    [Header("Weapon Type")]
    public WeaponTypeDetailsSO weaponType; // 근거리 , 원거리
    public WeaponTypeDetailsSO upgradeType; // 무기의 업그레이드
    public WeaponDetectorSO detectorType; // 무기 감지타입 (먼 적부터, 체력 낮은 적부터 등)

    [Header("Weapon Base Stats")]
    public int weaponBaseDamage = 20;   // 기본데미지
    public int weaponCriticChance = 10; // 치명타 확률 (%)
    public int weaponCriticDamage = 150; // 치명타 피해 (%)
    public float weaponFireRate = 0.5f; // 공격속도
    public int weaponRange = 0; // 사거리
    public int weaponKnockback = 0; // 넉백거리

    [Header("Weapon Configuration")]
    public EPool weaponParticle;
    //public List<GameObject> weaponAmmo;
    //[TextArea] public string upgradeDescription;
    //public bool isTrail; // Trail 렌더러 여부
    //public Material ammoTrailMaterial;
    //public float ammoTrailStartWidth;
    //public float ammoTrailEndWidth;
    //public float ammoTrailTime;
    //public SoundEffectSO weaponFiringSoundEffect;
}
