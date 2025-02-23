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
    /// 몬스터의 디버프를 관리하는 상태창 클래스
    /// -> 몬스터의 디버프 수치 적용을 관리하면서 동시에 해당 디버프의 이미지 관리
    /// 

    private struct DebuffData // 디버프 데이터
    {
        public EDebuffType debuffType;
        public Sprite debuffSprite;
        public int debuffValue; // 디버프 수치
        public int debuffDuration;
    }

    private class ActiveDebuff // 현재 활성화중인 디버프 데이터 (참조로 넘겨야 하므로 클래스)
    {
        public DebuffData debuffData;
        public GameObject debuffIconObject; // 몬스터 위에있는 디버프 상태창 이미지
        public CancellationTokenSource cts; // 디버프 해제를 위한 취소토큰
        public float revertValue; // 디버프 적용 이전 원래값
    }

    private Dictionary<EDebuffType, DebuffData> debuffDatas = new();
    private Dictionary<EDebuffType, ActiveDebuff> activeDebuffs = new();

    private Monster monster;
    private GameObject stateImagePrefab; // 상태창 이미지 프리팹


    private void Awake()
    {
        InitializeDebuffDatas(); // 디버프 데이터에 스프라이트를 미리 캐싱하여 초기화

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


    // 외부에서 몬스터에게 디버프를 줄때 호출되는 함수
    public void AddState(EDebuffType debuffType, int value, int duration)
    {
        if (CanApplyDebuff(debuffType, value)) // 디버프 적용이 가능한지
        {
            RemoveDebuff(debuffType); // 기존 디버프를 지우고
            ApplyDebuff(debuffType, value, duration); // 새 디버프 적용
        }
    }

    private bool CanApplyDebuff(EDebuffType debuffType, int value)
    {
        // 해당 디버프가 비활성화 중이거나 기존 수치 이상일 경우에 새로운 디버프 적용
        return activeDebuffs.TryGetValue(debuffType, out var activeData) == false ||
            value >= (activeData.debuffData.debuffValue);
    }

    #region APPLY DEBUFF
    private void ApplyDebuff(EDebuffType debuffType, int value, int duration)
    {
        DebuffData debuffData = new DebuffData
        {
            debuffType = debuffType,
            debuffSprite = debuffDatas[debuffType].debuffSprite, // 미리 캐싱해둔 스프라이트 가져오기
            debuffDuration = duration,
            debuffValue = value
        };

        ActiveDebuff activeDebuff = new ActiveDebuff
        {
            debuffData = debuffData,
            debuffIconObject = null,
            cts = new CancellationTokenSource(), // 이 디버프의 유니태스크 취소토큰 생성 (각 디버프별 타이머 따로)
            revertValue = GetStatValue(debuffType) // 디버프가 끝나면 되돌리기 위해 현재스탯 저장해놓기
        };

        DebuffStatValue(debuffType, value); // 디버프 스탯 적용
        CreateDebuffIcon(debuffData, activeDebuff); // 디버프 아이콘 생성
        StartDebuffTimer(debuffData, activeDebuff).Forget(); // 디버프 타이머 시작

        activeDebuffs[debuffType] = activeDebuff; // 활성화된 디버프 목록에 추가
    }

    private void CreateDebuffIcon(DebuffData debuffData, ActiveDebuff activeDebuff)
    {
        var iconObject = Instantiate(stateImagePrefab, transform);
        iconObject.GetComponent<SpriteRenderer>().sprite = debuffData.debuffSprite;

        // 현재 활성화된 디버프에 아이콘 오브젝트 대입
        // 구조체가 아닌 class 타입이기 때문에 매개변수로 넘어와도 참조타입이므로 영구적으로 반영됨
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
        // 활성화 중인 디버프에 해당 타입의 디버프가 없으면 리턴
        if (!activeDebuffs.TryGetValue(debuffType, out var activeDebuff)) 
            return;

        RevertStatValue(debuffType, activeDebuff.revertValue); // 스탯 원래대로 되돌리기
        RemoveDebuffResources(activeDebuff); // 디버프 아이콘 제거, 유니태스크 취소
    }

    private void RemoveDebuffResources(ActiveDebuff activeDebuff)
    {
        Destroy(activeDebuff.debuffIconObject); // 디버프 아이콘 제거
        activeDebuffs.Remove(activeDebuff.debuffData.debuffType); // 활성화된 디버프 목록에서 제거

        activeDebuff.cts?.Cancel(); // 디버프 타이머 취소
        activeDebuff.cts?.Dispose();
    }

    public void ClearAllDebuff()
    {
        // 몬스터의 비활성화 -> 모든 디버프 해제

        // for를 사용하여 각 키값에 대해 RemoveDebuff 호출 -> foreach는 중간에 데이터가 변경되면 안되기 때문
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