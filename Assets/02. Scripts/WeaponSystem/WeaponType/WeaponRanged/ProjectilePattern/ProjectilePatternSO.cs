using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ProjectilePatternSO : ScriptableObject
{
    public abstract void ProjectileLaunch(ProjectileData projectileData, List<BonusEffectSO> bonusEffects, Vector2 direction, Weapon weapon);
}