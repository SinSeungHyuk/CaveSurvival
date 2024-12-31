using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class PlayerWaveBuff : MonoBehaviour
{
    /*
     * �� ���̺긦 Ŭ�����Ҷ����� �����Ǵ� ����
     * �� 10���� -> ������ ���带 ������ 9���� ����
     * ����Ʈ�� �� ������ ��Ƽ� ����ȭ�鿡�� �Ѵ��� �����ֱ�
     * ����Ʈ ���� { �̹���, ����Ÿ��, ��ġ }
     * �ּ�,�ִ� �����ִ� ����Ʈ �̸� �������� �ű⼭ �ϳ� ��� ���������� �����ֱ�
     */

    private Player player;

    public struct WaveBuffData
    {
        public Sprite BuffSprite;
        public EStatType BuffType;
        public int Value;
    }

    [SerializeField] private List<BuffData> waveBuffList = new List<BuffData>(); // ���� ���

    public List<WaveBuffData> WaveBuffList { get; private set; } = new List<WaveBuffData>(); // ���� ����� ���� ����Ʈ


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