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
        // �÷��̾��� ų �� ����
        GameStatsManager.Instance.AddStats(EStatsType.PlayerTotalKills);

        // ���Ͱ� �ı��� ������ ������ ����
        var item = ObjectPoolManager.Instance.Get(EPool.Item, args.point, Quaternion.identity);
        item.GetComponent<Item>().InitializeItem(monster.DropItem);

        MonsterRelease();
    }

    private void MonsterRelease()
    {
        // �÷��̾� �ݴ����� ���ϴ� ���⺤��
        dir = (transform.position - monster.Player.transform.position).normalized;

        // ���ο� ������ ����
        moveSequence = DOTween.Sequence();

        // ���Ͱ� ȭ�� ���̶�� Ʈ�� ������ ��� X => ���귮 ���̱�
        if (monster.isOutScreen)
        {
            PostMonsterRelease();
            return;
        }

        // �̵�,ȸ��,ũ�� ������ ���ÿ� ����
        monster.Rigid.freezeRotation = false;
        moveSequence.Join(transform.DOScale(0.2f,1f))
                    .Join(transform.DORotate(new Vector3(0f, 0f, 360f), 1f, RotateMode.FastBeyond360)) // 360�� ȸ���� ���� ȸ�����
                    .Join(transform.DOMove(dir * 3, 1f).SetRelative()) // DOMove�� �ش� '����'���� �̵��ϱ� ���� ��밪 ó��
                    .OnComplete(() => {
                        PostMonsterRelease();
                    });
    }

    private void PostMonsterRelease()
    {
        moveSequence.Kill();
        // ��Ȱ��ȭ �Ǹ鼭 ���� ���������� ���̺� ���� �̺�Ʈ ��������
        StageManager.Instance.CurrentStage.MonsterSpawnEvent.OnWaveFinish -= monster.Stage_OnWaveFinished;
        ObjectPoolManager.Instance.Release(gameObject, poolNalme);
    }
}
