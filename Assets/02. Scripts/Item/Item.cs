using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;
using System.Threading;


public class Item : MonoBehaviour  // 아이템에 연결할 클래스
{
    // 자석 이벤트가 호출되면 전역의 모든 아이템이 반응해야함
    private static event Action OnMagnet;

    private SpriteRenderer spriteRenderer;
    private ParticleSystem particle;
    private SoundEffectSO soundEffect;
    private EItemType itemType;
    private int gainExp;

    // 비활성화 되어도 유니태스크는 계속 실행되므로 관리해줘야함@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
    private CancellationTokenSource disableCancellation = new CancellationTokenSource();
    private Player player;
    private Rigidbody2D rigid;
    private CircleCollider2D circleCollider;
    private Vector2 moveVec;
    private bool isFirstTrigger;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        particle = GetComponent<ParticleSystem>();
        rigid = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void OnEnable()
    {
        // 활성화 될때마다 토큰 새롭게 초기화시켜주기      
        disableCancellation = new CancellationTokenSource();
    }
    private void OnDisable()
    {
        // 비활성화 될때 유니태스크 취소명령
        disableCancellation?.Cancel();
        disableCancellation?.Dispose();
    }


    public void InitializeItem(ItemDetailsSO data)
    {
        spriteRenderer.sprite = data.ItemSprite;
        player = GameManager.Instance.Player;
        this.soundEffect = data.soundEffect;

        ParticleSystem.MainModule main = particle.main; // 파티클 시스템의 MainModule로 색상변경 가능
        main.startColor = UtilitieHelper.GetGradeColor(data.itemGrade, data.itemType);

        itemType = data.itemType;
        gainExp = (int)data.itemGrade; // 해당 등급에 맞는 경험치 획득 (등급마다 경험치 정해져있음)

        circleCollider.enabled = true;
        isFirstTrigger = false;

        // 활성화 되면서 현재 스테이지의 웨이브 종료 이벤트 구독
        StageManager.Instance.CurrentStage.MonsterSpawnEvent.OnWaveFinish += Stage_OnWaveFinished;
        OnMagnet += Item_OnMagnet;
    }

    private void Stage_OnWaveFinished(MonsterSpawnEvent @event)
    {
        // 웨이브가 끝나면 아이템 소멸 (아이템 획득 효과는 적용x)
        ReleaseItem();
    }

    private void Item_OnMagnet()
        => MoveToOutsideDir();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 최초 충돌 + 플레이어 자석범위와 충돌
        if (!isFirstTrigger && (Settings.itemDetectorLayer & (1 << collision.gameObject.layer)) != 0)
            MoveToOutsideDir();

        // 두번째 충돌 + 플레이어와 충돌
        if (isFirstTrigger && (Settings.itemPickUpLayer & (1 << collision.gameObject.layer)) != 0)            
            ItemAcquire();
    }

    private void MoveToOutsideDir()
    {
        circleCollider.enabled = false;
        isFirstTrigger = true;
        
        // 플레이어 바깥을 향하는 방향벡터
        var outsideDir = (transform.position - player.transform.position).normalized;
        // 바깥쪽으로 이동할 목표 위치 계산 (현재 위치에서 방향 벡터의 2.5배 거리)
        var outsideDesiredPos = transform.position + outsideDir * 2.5f;

        // 바깥쪽으로 천천히 이동 (0.5초)
        transform.DOMove(outsideDesiredPos, 0.5f).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // 플레이어 향해 이동
                MoveToPlayer().Forget();
            });
    }

    private async UniTask MoveToPlayer()
    {
        circleCollider.enabled = true;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {    
            // 0.1초마다 플레이어 위치 갱신하면서 빠르게 이동
            moveVec = (player.transform.position - transform.position).normalized;
            rigid.velocity = moveVec * 18f;
            elapsedTime += 0.1f;

            await UniTask.Delay(100, cancellationToken:disableCancellation.Token);
        }

        ItemAcquire();
    }

    private void ItemAcquire()
    {
        // 아이템을 획득했을때 호출되는 함수 (효과, 구독해지, 풀 반환)

        switch (itemType)
        {
            case EItemType.Magnet:
                OnMagnet.Invoke(); // 자석 아이템이면 모든 아이템들이 움직이기 시작
                break;
            case EItemType.Exp:
                player.Stat.AddExp(gainExp + player.Stat.ExpBonus);
                break;
            case EItemType.Hp:
                player.HpRecoveryPercent(Settings.hpRecoveryValue);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        SoundEffectManager.Instance.PlaySoundEffect(soundEffect);
        ReleaseItem();
    }

    private void ReleaseItem()
    {
        // 아이템에 구독되어있던 함수 구독해지
        StageManager.Instance.CurrentStage.MonsterSpawnEvent.OnWaveFinish -= Stage_OnWaveFinished;
        OnMagnet -= Item_OnMagnet;

        ObjectPoolManager.Instance.Release(gameObject, EPool.Item);
    }
}
