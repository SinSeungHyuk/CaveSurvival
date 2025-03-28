using Cysharp.Threading.Tasks;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;


[RequireComponent(typeof(MonsterDestroyedEvent))]
[DisallowMultipleComponent]
public class MonsterDestroyed : MonoBehaviour
{
    private MonsterDestroyedEvent destroyedEvent;
    private Monster monster;
    private Sequence moveSequence;
    private EPool poolNalme;
    private Vector2 dir;


    private void Awake()
    {
        destroyedEvent = GetComponent<MonsterDestroyedEvent>();
        monster = GetComponent<Monster>();

        poolNalme = (monster.IsBoss == true) ? EPool.Boss : EPool.Monster;
    }
    private void OnEnable()
    {
        destroyedEvent.OnMonsterDestroyed += DestroyedEvent_OnDestroyed;
    }
    private void OnDisable()
    {
        destroyedEvent.OnMonsterDestroyed -= DestroyedEvent_OnDestroyed;
    }

    private void DestroyedEvent_OnDestroyed(MonsterDestroyedEvent obj, MonsterDestroyedEventArgs args)
    {
        // 플레이어의 킬 수 증가
        GameStatsManager.Instance.AddStats(EStatsType.PlayerTotalKills);

        // 몬스터가 파괴된 지점에 아이템 생성
        var item = ObjectPoolManager.Instance.Get(EPool.Item, args.point, Quaternion.identity);
        item.GetComponent<Item>().InitializeItem(monster.DropItem);

        MonsterRelease();
    }

    private void MonsterRelease()
    {
        // 플레이어 반대편을 향하는 방향벡터
        dir = (transform.position - monster.Player.transform.position).normalized;

        // 새로운 시퀀스 생성
        moveSequence = DOTween.Sequence();

        // 몬스터가 화면 밖이라면 트윈 시퀀스 재생 X => 연산량 줄이기
        if (monster.isOutScreen)
        {
            PostMonsterRelease();
            return;
        }

        // 이동,회전,크기 변경을 동시에 실행
        monster.Rigid.freezeRotation = false;
        moveSequence.Join(transform.DOScale(0.2f,1f))
                    .Join(transform.DORotate(new Vector3(0f, 0f, 360f), 1f, RotateMode.FastBeyond360)) // 360도 회전을 위한 회전모드
                    .Join(transform.DOMove(dir * 3, 1f).SetRelative()) // DOMove를 해당 '방향'으로 이동하기 위한 상대값 처리
                    .OnComplete(() => {
                        PostMonsterRelease();
                    });
    }

    private void PostMonsterRelease()
    {
        moveSequence.Kill();
        // 비활성화 되면서 현재 스테이지의 웨이브 종료 이벤트 구독해지
        StageManager.Instance.CurrentStage.MonsterSpawnEvent.OnWaveFinish -= monster.Stage_OnWaveFinished;
        ObjectPoolManager.Instance.Release(gameObject, poolNalme);
    }
}
