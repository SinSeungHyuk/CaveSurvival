using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Synergy_", menuName = "Scriptable Objects/Player/Synergy")]
public class SynergyDetailsSO : IdentifiedObject
{
    public ESynergyType synergyType;
    [TextArea] public string synergyDesc;
    public int synergyCount;
    public Sprite synergyGrade;
    public Sprite synergyIcon;
    public EWeaponStatType synergyStatType;
    public int synergyValue;
}
