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
        Time.timeScale = 0f; // �ð� ����
        ApplyUltimateRoutine().Forget();
    }

    private async UniTask ApplyUltimateRoutine()
    {
        // 1. Ÿ�ϸ� ã��
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        Color startColor = tilemap.color;
        Color targetColor = ultimateSkillData.TileColor;

        // 2. n�ʿ� ���ļ� Ÿ�ϸ� ���� ����
        await SetTilemapColorRoutine(tilemap, startColor, targetColor, 3f);

        // 3. ���͵��� ���׸��� ����
        await SetMonsterMaterialRoutine();
    }

    private async UniTask SetMonsterMaterialRoutine()
    {
        PlaySoundEffect(1);

        var monsters = GetMonstersInUltimateRange(); // ���� ���� ��� ���� ��������
        SetMonsterMaterial(monsters); // �ش� ���͵� ���׸��� ����

        // 4. ���׸��� n�ʿ� ���ļ� ���� ����
        await SetMaterialRoutine(monsters);

        // 5. ���͵鿡�� ������ �ְ� ����
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
            float materialValue = Mathf.Sin(elapsedTime * (Mathf.PI / 4f)) - 0.25f; // sin �׷��� => -1 ~ 1 ����

            foreach (var monster in monsters)
            {
                monster.Sprite.material.SetFloat("_SplitValue", materialValue);
            }

            await UniTask.Yield();
        }
    }

    private void ApplyDamageToMonsters(List<Monster> monsters)
    {
        Time.timeScale = 1f; // ���� ����
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
            tilemap.color = Color.Lerp(startColor, targetColor, t); // �� ���� ������ ���� t�� ����

            await UniTask.Yield();
        }

        tilemap.color = targetColor;
    }
}