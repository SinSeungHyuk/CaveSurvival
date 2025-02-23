using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Linq;

public class UltimateSkillBehaviourKai : UltimateSkillBehaviour
{
    private UltimateSkillKai ultimateSkillData;
    private Tilemap tilemap;

    public UltimateSkillBehaviourKai(Player player, UltimateSkillKai so) : base(player, so)
    {
        ultimateSkillData = so;
    }

    public override void Apply()
    {
        Time.timeScale = 0f; // 시간 정지
        ApplyUltimateRoutine().Forget();
    }

    private async UniTask ApplyUltimateRoutine()
    {
        // 1. 타일맵 찾기
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        Color startColor = tilemap.color;
        Color targetColor = ultimateSkillData.TileColor;

        // 2. n초에 걸쳐서 타일맵 색상 변경
        await SetTilemapColorRoutine(tilemap, startColor, targetColor, 3f);

        // 3. 몬스터들의 머테리얼 변경
        await SetMonsterMaterialRoutine();
    }

    private async UniTask SetMonsterMaterialRoutine()
    {
        PlaySoundEffect(1);

        var monsters = GetMonstersInUltimateRange(); // 범위 내의 모든 몬스터 가져오기
        SetMonsterMaterial(monsters); // 해당 몬스터들 머테리얼 변경

        // 4. 머테리얼 n초에 걸쳐서 연출 구현
        await SetMaterialRoutine(monsters);

        // 5. 몬스터들에게 데미지 주고 종료
        ApplyDamageToMonsters(monsters);
    }

    private List<Monster> GetMonstersInUltimateRange()
    {
        var playerPosition = player.transform.position;
        var colliders = Physics2D.OverlapCircleAll(playerPosition, ultimateSkillData.Range, Settings.monsterLayer);

        return colliders
            .Where(collider => !collider.GetComponent<Monster>().IsDead)
            .Select(collider => collider.GetComponent<Monster>())
            .ToList();
    }

    private void SetMonsterMaterial(List<Monster> monsters)
    {
        foreach (var monster in monsters)
        {
            monster.Sprite.material = ultimateSkillData.MonsterMaterial;
        }
    }

    private async UniTask SetMaterialRoutine(List<Monster> monsters)
    {
        float elapsedTime = 0f;

        while (elapsedTime < 2f)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float materialValue = Mathf.Sin(elapsedTime * (Mathf.PI / 4f)) - 0.25f; // sin 그래프 => -1 ~ 1 사이

            foreach (var monster in monsters)
            {
                monster.Sprite.material.SetFloat("_SplitValue", materialValue);
            }

            await UniTask.Yield();
        }
    }

    private void ApplyDamageToMonsters(List<Monster> monsters)
    {
        Time.timeScale = 1f; // 정지 해제
        tilemap.color = Color.white;

        int damage = ultimateSkillData.BaseDamage + player.Stat.RangeDamage;

        foreach (var monster in monsters)
        {
            monster.Sprite.material = monster.EnemyDetails.enemyStandardMaterial;
            monster.TakeDamage(damage);

            SpawnHitEffect(monster);
        }

        PlaySoundEffect();
    }

    private void SpawnHitEffect(Monster monster)
    {
        var hitEffect = ObjectPoolManager.Instance.Get(EPool.FireHit, monster.transform.position, Quaternion.identity).GetComponent<HitEffect>();
        hitEffect.InitializeHitEffect(EPool.FireHit);
    }

    private async UniTask SetTilemapColorRoutine(Tilemap tilemap, Color startColor, Color targetColor, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / duration;
            tilemap.color = Color.Lerp(startColor, targetColor, t); // 두 색상 사이의 지점 t로 보간

            await UniTask.Yield();
        }

        tilemap.color = targetColor;
    }
}