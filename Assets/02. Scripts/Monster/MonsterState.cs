using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class MonsterState : MonoBehaviour
{
    /// ������ ������� �����ϴ� ����â Ŭ����
    /// -> ������ ����� ��ġ ������ �����ϸ鼭 ���ÿ� �ش� ������� �̹��� ����
    /// 

    private struct DebuffData // ����� ������
    {
        public EDebuffType debuffType;
        public Sprite debuffSprite;
        public int debuffValue; // ����� ��ġ
        public int debuffDuration;
    }

    private class ActiveDebuff // ���� Ȱ��ȭ���� ����� ������ (������ �Ѱܾ� �ϹǷ� Ŭ����)
    {
        public DebuffData debuffData;
        public GameObject debuffIconObject; // ���� �����ִ� ����� ����â �̹���
        public CancellationTokenSource cts; // ����� ������ ���� �����ū
        public float revertValue; // ����� ���� ���� ������
    }

    private Dictionary<EDebuffType, DebuffData> debuffDatas = new();
    private Dictionary<EDebuffType, ActiveDebuff> activeDebuffs = new();

    private Monster monster;
    private GameObject stateImagePrefab; // ����â �̹��� ������


    private void Awake()
    {
        InitializeDebuffDatas(); // ����� �����Ϳ� ��������Ʈ�� �̸� ĳ���Ͽ� �ʱ�ȭ

        stateImagePrefab = AddressableManager.Instance.GetResource<GameObject>("StateImage");
        monster = GetComponentInParent<Monster>();
    }

    private void InitializeDebuffDatas()
    {
        foreach (EDebuffType debuffType in Enum.GetValues(typeof(EDebuffType)))
        {
            Sprite sprite;
            switch (debuffType)
            {
                case EDebuffType.AttackDebuff:
                    sprite = CreateSprite(AddressableManager.Instance.GetResource<Texture2D>("StateAttack"));
                    break;
                case EDebuffType.Slow:
                    sprite = CreateSprite(AddressableManager.Instance.GetResource<Texture2D>("StateSpeed"));
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            debuffDatas.Add(debuffType, new DebuffData
            {
                debuffType = debuffType,
                debuffSprite = sprite,
                debuffValue = 0,
                debuffDuration = 0,
            });
        }
    }


    // �ܺο��� ���Ϳ��� ������� �ٶ� ȣ��Ǵ� �Լ�
    public void AddState(EDebuffType debuffType, int value, int duration)
    {
        if (CanApplyDebuff(debuffType, value)) // ����� ������ ��������
        {
            RemoveDebuff(debuffType); // ���� ������� �����
            ApplyDebuff(debuffType, value, duration); // �� ����� ����
        }
    }

    private bool CanApplyDebuff(EDebuffType debuffType, int value)
    {
        // �ش� ������� ��Ȱ��ȭ ���̰ų� ���� ��ġ �̻��� ��쿡 ���ο� ����� ����
        return activeDebuffs.TryGetValue(debuffType, out var activeData) == false ||
            value >= (activeData.debuffData.debuffValue);
    }

    #region APPLY DEBUFF
    private void ApplyDebuff(EDebuffType debuffType, int value, int duration)
    {
        DebuffData debuffData = new DebuffData
        {
            debuffType = debuffType,
            debuffSprite = debuffDatas[debuffType].debuffSprite, // �̸� ĳ���ص� ��������Ʈ ��������
            debuffDuration = duration,
            debuffValue = value
        };

        ActiveDebuff activeDebuff = new ActiveDebuff
        {
            debuffData = debuffData,
            debuffIconObject = null,
            cts = new CancellationTokenSource(), // �� ������� �����½�ũ �����ū ���� (�� ������� Ÿ�̸� ����)
            revertValue = GetStatValue(debuffType) // ������� ������ �ǵ����� ���� ���罺�� �����س���
        };

        DebuffStatValue(debuffType, value); // ����� ���� ����
        CreateDebuffIcon(debuffData, activeDebuff); // ����� ������ ����
        StartDebuffTimer(debuffData, activeDebuff).Forget(); // ����� Ÿ�̸� ����

        activeDebuffs[debuffType] = activeDebuff; // Ȱ��ȭ�� ����� ��Ͽ� �߰�
    }

    private void CreateDebuffIcon(DebuffData debuffData, ActiveDebuff activeDebuff)
    {
        var iconObject = Instantiate(stateImagePrefab, transform);
        iconObject.GetComponent<SpriteRenderer>().sprite = debuffData.debuffSprite;

        // ���� Ȱ��ȭ�� ������� ������ ������Ʈ ����
        // ����ü�� �ƴ� class Ÿ���̱� ������ �Ű������� �Ѿ�͵� ����Ÿ���̹Ƿ� ���������� �ݿ���
        activeDebuff.debuffIconObject = iconObject; 
    }

    private async UniTask StartDebuffTimer(DebuffData debuffData, ActiveDebuff activeDebuff)
    {
        await UniTask.Delay(debuffData.debuffDuration * 1000, cancellationToken: activeDebuff.cts.Token);
        RemoveDebuff(debuffData.debuffType);
    }
    #endregion


    #region REMOVE DEBUFF
    private void RemoveDebuff(EDebuffType debuffType)
    {
        // Ȱ��ȭ ���� ������� �ش� Ÿ���� ������� ������ ����
        if (!activeDebuffs.TryGetValue(debuffType, out var activeDebuff)) 
            return;

        RevertStatValue(debuffType, activeDebuff.revertValue); // ���� ������� �ǵ�����
        RemoveDebuffResources(activeDebuff); // ����� ������ ����, �����½�ũ ���
    }

    private void RemoveDebuffResources(ActiveDebuff activeDebuff)
    {
        Destroy(activeDebuff.debuffIconObject); // ����� ������ ����
        activeDebuffs.Remove(activeDebuff.debuffData.debuffType); // Ȱ��ȭ�� ����� ��Ͽ��� ����

        activeDebuff.cts?.Cancel(); // ����� Ÿ�̸� ���
        activeDebuff.cts?.Dispose();
    }

    public void ClearAllDebuff()
    {
        // ������ ��Ȱ��ȭ -> ��� ����� ����

        // for�� ����Ͽ� �� Ű���� ���� RemoveDebuff ȣ�� -> foreach�� �߰��� �����Ͱ� ����Ǹ� �ȵǱ� ����
        var activeDebuffsArray = activeDebuffs.Keys.ToArray();
        for (int i = 0; i < activeDebuffsArray.Length; i++)
        {
            RemoveDebuff(activeDebuffsArray[i]);
        }
        activeDebuffs.Clear();
    }
    #endregion


    #region STAT VALUE
    private float GetStatValue(EDebuffType debuffType)
    {
        switch (debuffType)
        {
            case EDebuffType.AttackDebuff:
                return monster.Stat.Atk;
            case EDebuffType.Slow:
                return monster.Stat.Speed;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void DebuffStatValue(EDebuffType debuffType, float value)
    {
        switch (debuffType)
        {
            case EDebuffType.AttackDebuff:
                monster.Stat.Atk = UtilitieHelper.DecreaseByPercent(monster.Stat.Atk, value);
                break;
            case EDebuffType.Slow:
                monster.Stat.Speed = UtilitieHelper.DecreaseByPercent(monster.Stat.Speed, value);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void RevertStatValue(EDebuffType debuffType, float revertValue)
    {
        switch (debuffType)
        {
            case EDebuffType.AttackDebuff:
                monster.Stat.Atk = revertValue;
                break;
            case EDebuffType.Slow:
                monster.Stat.Speed = revertValue;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    #endregion

    private Sprite CreateSprite(Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}