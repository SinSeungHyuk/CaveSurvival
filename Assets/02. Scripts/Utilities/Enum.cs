
// Only Enum

public enum EPool // ������Ʈ Ǯ�� ����� Ǯ ����
{
    Item,
    HitText,
    BasicHitEffect,
    LightningHitEffect,
    FireHit,
    IceHit,
    Spawner,
    Monster,
    MonsterBullet,
    MonsterTrap,
    Projectile,
    Boss,
    SoundEffect,
}


public enum EDetectorType // ���� Ž������
{
    Nearest,
    Farest,
}

public enum EGrade // ������ ��� (��޺� ����ġ)
{
    Normal = 4,
    Rare = 8,
    Unique = 16,
    Legend = 100
}

public enum EItemType // ������ Ÿ��
{
    Exp,
    Magnet,
    Hp,
}

public enum ESceneType // �� ����
{
    MainMenu,
    MainGame,
}

public enum EParticleType // ���߽� ����Ʈ
{
    BasicHit,
    LightningHit,
    IceHit,
}

public enum EStatType // �÷��̾� ����
{
    Hp,
    HpRegen,
    Defense,
    BonusDamage,
    MeleeDamage,
    RangeDamage,
    Speed,
    Dodge,
    PickUpRange,
    ExpBonus,
    SpeedBonus, // �ӵ��� ���������� ����ϱ� ���� ����Ÿ��
}

public enum EWeaponStatType // ���� ����
{
    WeaponDamage,
    WeaponCriticChance,
    WeaponCriticDamage,
    WeaponFireRate,
    WeaponRange,
    WeaponKnockback,
}

public enum ELevelUpGrade // ������ (�������� ������ Ȯ�� ����)
{
    Normal = 60,
    Rare = 30,
    Unique = 7,
    Legend = 3,
}


public enum EStatsType // ���
{
    WeaponTotalDamage,
    WeaponAcquiredTime, // ���⸦ ó�� ȹ���� ������ ����ð�
    WeaponAcquiredWave, // ���⸦ ó�� ȹ���� ������ ���̺� ��
    PlayerTotalKills,
}


public enum ECurrencyType // ȭ��
{
    Achive,
    Gold,
    Diamond,
}


public enum EDebuffType // ���� �����
{
    Slow,
    AttackDebuff,
}


public enum EHitType
{
    Normal,
    Critical,
    Dodge,
    PlayerHit
}


public enum EStageType
{
    SlimeCave,
    ShadeCave,
}