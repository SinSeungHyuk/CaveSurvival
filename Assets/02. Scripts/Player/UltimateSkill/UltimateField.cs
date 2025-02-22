using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateField : MonoBehaviour
{
    private Player player;
    private UltimateSkillIvy skillSO;


    public void InitializeUltimateField(Player player, UltimateSkillIvy skillSO, GameObject ultimateField)
    {
        this.player = player;
        this.skillSO = skillSO;

        // 필요한 컴포넌트 추가
        var collier = gameObject.AddComponent<CircleCollider2D>();
        collier.isTrigger = true;
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        transform.position = player.transform.position;
        gameObject.transform.localScale = ultimateField.transform.localScale;

        var originSpriteRenderer = ultimateField.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = originSpriteRenderer.sprite;
        spriteRenderer.material = originSpriteRenderer.sharedMaterial;
        spriteRenderer.sortingLayerID = originSpriteRenderer.sortingLayerID;
        spriteRenderer.sortingOrder = originSpriteRenderer.sortingOrder;
    }

    public void DestroyField()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Monster monster))
        {
            monster.Stat.Speed = UtilitieHelper.DecreaseByPercent(monster.Stat.Speed, 80);
        }

        if (collision.TryGetComponent(out Player player))
        {
            player.Stat.PlayerStatChanged(EStatType.Dodge, skillSO.AddtionalDodge);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Monster monster))
        {
            monster.Stat.Speed = monster.EnemyDetails.speed;
        }

        if (collision.TryGetComponent(out Player player))
        {
            player.Stat.PlayerStatChanged(EStatType.Dodge, -skillSO.AddtionalDodge);
        }
    }
}
