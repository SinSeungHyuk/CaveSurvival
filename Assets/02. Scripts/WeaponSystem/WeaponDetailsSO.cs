using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails_", menuName = "Scriptable Objects/Weapon/Weapon")]
public class WeaponDetailsSO : IdentifiedObject
{
    [Header("Weapon Base Details")]
    public string weaponName;
    public Sprite weaponSprite;
    [TextArea] public string description; // ���� �⺻����
    [TextArea] public string upgradeDesc; // ���� ���׷��̵� ����

    [Header("Weapon Type")]
    public WeaponTypeDetailsSO weaponType; // �ٰŸ� , ���Ÿ�
    public WeaponTypeDetailsSO upgradeType; // ������ ���׷��̵�
    public WeaponDetectorSO detectorType; // ���� ����Ÿ�� (�� ������, ü�� ���� ������ ��)

    [Header("Weapon Base Stats")]
    public int weaponBaseDamage = 20;   // �⺻������
    public int weaponCriticChance = 10; // ġ��Ÿ Ȯ�� (%)
    public int weaponCriticDamage = 150; // ġ��Ÿ ���� (%)
    public float weaponFireRate = 0.5f; // ���ݼӵ�
    public int weaponRange = 0; // ��Ÿ�
    public int weaponKnockback = 0; // �˹�Ÿ�

    [Header("Weapon Configuration")]
    public EPool weaponParticle;
    public SoundEffectSO weaponFiringSoundEffect; // ���� ���� ȿ����
}
