
// Only Enum

public enum EPool // 오브젝트 풀에 등록할 풀 종류
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


public enum EDetectorType // 무기 탐지기준
{
    Nearest,
    Farest,
}

public enum EGrade // 아이템 등급 (등급별 경험치)
{
    Normal = 4,
    Rare = 8,
    Unique = 16,
    Legend = 100
}

public enum EItemType // 아이템 타입
{
    Exp,
    Magnet,
    Hp,
}

public enum ESceneType // 씬 종류
{
    MainMenu,
    MainGame,
}

public enum EParticleType // 적중시 이펙트
{
    BasicHit,
    LightningHit,
    IceHit,
}

public enum EStatType // 플레이어 스탯
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
    SpeedBonus, // 속도를 정수형으로 계산하기 위한 스탯타입
}

public enum EWeaponStatType // 무기 스탯
{
    WeaponDamage,
    WeaponCriticChance,
    WeaponCriticDamage,
    WeaponFireRate,
    WeaponRange,
    WeaponKnockback,
}

public enum ELevelUpGrade // 레벨업 (레벨업시 선택지 확률 포함)
{
    Normal = 60,
    Rare = 30,
    Unique = 7,
    Legend = 3,
}


public enum EStatsType // 통계
{
    WeaponTotalDamage,
    WeaponAcquiredTime, // 무기를 처음 획득한 시점의 경과시간
    WeaponAcquiredWave, // 무기를 처음 획득한 시점의 웨이브 수
    PlayerTotalKills,
}


public enum ECurrencyType // 화폐
{
    Achive,
    Gold,
    Diamond,
}


public enum EDebuffType // 몬스터 디버프
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