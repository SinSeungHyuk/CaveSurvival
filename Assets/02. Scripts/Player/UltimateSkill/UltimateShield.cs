using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltimateShield : MonoBehaviour
{
    private Player player;
    

    public void InitializeUltimateShield(Player player, GameObject ultimateShield)
    {
        this.player = player;

        // 필요한 컴포넌트 추가
        var collier = gameObject.AddComponent<CircleCollider2D>();
        var spriteRenderer = gameObject.AddComponent<SpriteRenderer>();

        gameObject.transform.localScale = ultimateShield.transform.localScale;
        gameObject.transform.localPosition = ultimateShield.transform.position;

        var originSpriteRenderer = ultimateShield.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = originSpriteRenderer.sprite;
        spriteRenderer.color = originSpriteRenderer.color;
        spriteRenderer.material = originSpriteRenderer.sharedMaterial;
        spriteRenderer.sortingLayerID = originSpriteRenderer.sortingLayerID;
    }

    public void DestroyShield()
    {
        Destroy(gameObject);
    } 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"충돌! {collision.gameObject.name}");  

        if (collision.TryGetComponent(out Monster monster))
        {
            int damage = UtilitieHelper.IncreaseByPercent((int)player.Stat.MaxHp, player.Stat.Defense);
            monster.TakeDamage(damage);
        }
    }
}
