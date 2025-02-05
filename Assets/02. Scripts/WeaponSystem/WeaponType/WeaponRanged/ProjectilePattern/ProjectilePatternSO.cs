using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ProjectilePatternSO : ScriptableObject
{
    public abstract void ProjectileLaunch(ProjectileDetailsSO projectileDetails, List<BonusEffectSO> bonusEffects, Vector2 direction, Weapon weapon);
}