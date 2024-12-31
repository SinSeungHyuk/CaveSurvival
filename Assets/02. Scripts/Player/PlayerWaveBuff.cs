using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerWaveBuff : MonoBehaviour
{
    /*
     * 매 웨이브를 클리어할때마다 누적되는 버프
     * 총 10라운드 -> 마지막 라운드를 제외한 9개의 버프
     * 리스트에 각 버프를 담아서 설정화면에서 한눈에 보여주기
     * 리스트 원소 { 이미지, 스탯타입, 수치 }
     * 최소,최대 적혀있는 리스트 미리 만들어놓고 거기서 하나 골라서 랜덤값으로 버프주기
     */

    private Player player;

    public struct WaveBuffData
    {
        public Sprite BuffSprite;
        public EStatType BuffType;
        public int Value;
    }

    [SerializeField] private List<BuffData> waveBuffList = new List<BuffData>(); // 버프 목록

    public List<WaveBuffData> WaveBuffList { get; private set; } = new List<WaveBuffData>(); // 현재 적용된 버프 리스트


    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void InitializePlayerWaveBuff()
    {
        BuffData buffData = waveBuffList[UnityEngine.Random.Range(0, waveBuffList.Count)];
        Debug.Log($"{buffData.BuffType.ToString()}");

        WaveBuffData newBuffData = new WaveBuffData()
        {
            BuffSprite = buffData.BuffSprite,
            BuffType = buffData.BuffType,
            Value = UnityEngine.Random.Range(buffData.MinValue, buffData.MaxValue)
        };

        WaveBuffList.Add(newBuffData);

        PlayerLevelUpData data = ScriptableObject.CreateInstance<PlayerLevelUpData>();
        data.statType = newBuffData.BuffType;
        data.value = newBuffData.Value;

        player.Stat.PlayerStatChanged(data);
    }
}


[Serializable]
public struct BuffData
{
    public Sprite BuffSprite;
    public EStatType BuffType;
    public int MinValue;
    public int MaxValue;
}