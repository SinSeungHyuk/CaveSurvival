using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/Weapon/Weapon")]
public class WeaponDetailsSO : IdentifiedObject
{
    [Header("weapon base details")]
    public string weaponName;
    public WeaponTypeDetailsSO weaponType; // �ٰŸ� , ���Ÿ�
    public WeaponTypeDetailsSO upgradeType; // ������ ���׷��̵�
    public WeaponDetectorSO detectorType; // ���� ����Ÿ�� (�� ������, ü�� ���� ������ ��)
    public Sprite weaponSprite;
    public string description; // ���� �⺻����
    public string upgradeDesc; // ���� ���׷��̵� ����

    [Header("weapon base stats")]
    public int weaponBaseDamage = 20;   // �⺻������
    public int weaponCriticChance = 10; // ġ��Ÿ Ȯ�� (%)
    public int weaponCriticDamage = 150; // ġ��Ÿ ���� (%)
    public float weaponFireRate = 0.5f; // ���ݼӵ�
    public int weaponRange = 0; // ��Ÿ�
    public int weaponKnockback = 0; // �˹�Ÿ�

    [Header("weapon configuration")]
    public EParticleType weaponParticle;
    //public List<GameObject> weaponAmmo;
    //[TextArea] public string upgradeDescription;
    //public bool isTrail; // Trail ������ ����
    //public Material ammoTrailMaterial;
    //public float ammoTrailStartWidth;
    //public float ammoTrailEndWidth;
    //public float ammoTrailTime;
    //public SoundEffectSO weaponFiringSoundEffect;
}
