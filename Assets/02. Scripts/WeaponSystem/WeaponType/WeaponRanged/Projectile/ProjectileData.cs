using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct ProjectileData
{
    // 투사체 이미지
    public Sprite sprite;
    // 투사체 머테리얼
    public Material material;

    // 투사체 적중시 효과 (폭발, 관통 등)
    public ProjectileEffectSO projectileEffect;
    // 투사체 통과 여부
    public bool isPiercing;
    // 투사체 통과 카운트
    public int piercingCount;
    // 투사체 속도
    public int projectileSpeed;
}
