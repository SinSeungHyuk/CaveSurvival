
// Only Enum

public enum EPool
{
    Item,
    HitText,
    BasicHitEffect,
    LightningHitEffect,
    FireHit,
    Spawner,
    Monster,
    MonsterBullet,
    MonsterTrap,
    Projectile,
    Boss,
}


public enum EDetectorType
{
    Nearest,
    Farest,
}

public enum EGrade
{
    Normal = 4,
    Rare = 8,
    Unique = 15,
    Legend = 30
}

public enum EItemType
{
    Exp,
    Magnet,
    Bomb,
}

public enum ESceneType
{
    MainMenu,
    MainGame,
}

public enum EParticleType
{
    BasicHit,
    LightningHit,
    IceHit,
}

public enum EStatType
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
}

public enum EWeaponStatType
{
    WeaponDamage,
    WeaponCriticChance,
    WeaponCriticDamage,
    WeaponFireRate,
    WeaponRange,
    WeaponKnockback,
}

public enum ELevelUpGrade
{
    Normal = 60,
    Rare = 30,
    Unique = 7,
    Legend = 3,
}


public enum EStatsType // ��� ����
{
    WeaponTotalDamage,
    WeaponAcquiredTime, // ���⸦ ó�� ȹ���� ������ ����ð�
    WeaponAcquiredWave, // ���⸦ ó�� ȹ���� ������ ���̺� ��
    PlayerTotalKills,
}


public enum ECurrencyType
{
    Achive,
    Gold,
    Diamond,
}