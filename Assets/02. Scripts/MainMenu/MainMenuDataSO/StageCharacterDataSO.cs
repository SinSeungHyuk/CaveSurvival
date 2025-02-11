using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "StageCharacterData", menuName = "Scriptable Objects/MainMenu/StageCharacterData")]
public class StageCharacterDataSO : ScriptableObject
{
    public PlayerDetailsSO playerDetails;
    public StageDetailsSO stageDetails;
}
