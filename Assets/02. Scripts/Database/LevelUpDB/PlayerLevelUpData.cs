using UnityEngine;



public class PlayerLevelUpData : ScriptableObject, ILevelUpData
{
    public int id;
    public string description;
    public EStatType statType;  // enum
    public int value;
    public ELevelUpGrade ratio;

    public int GetRatio()
        => (int)ratio;

    public int GetStatType()
        => (int)statType;
}


