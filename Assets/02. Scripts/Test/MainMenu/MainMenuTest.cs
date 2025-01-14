using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTest : MonoBehaviour
{

    private void Start()
    {
        var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");
        var playerData = AddressableManager.Instance.GetResource<Database>("DB_Player");
        var stageData = AddressableManager.Instance.GetResource<Database>("DB_Stage");

        Debug.Log($"데이터 개수 : {AddressableManager.Instance.Resources.Count}");
        Debug.Log($"playerData 1번 이름 : {playerData.GetDataByID<PlayerDetailsSO>(1).characterName}");
        //Debug.Log($"stageData 개수 : {AddressableManager.Instance.Resources.Count}");
    }

    public void BtnTest()
    {
        //LoadingSceneManager.LoadScene("CombatTestScene", "NULL", ESceneType.MainGame);

        var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");
        var playerData = AddressableManager.Instance.GetResource<Database>("DB_Player");
        var stageData = AddressableManager.Instance.GetResource<Database>("DB_Stage");

        stageCharacterData.playerDetails = playerData.GetDataByID<PlayerDetailsSO>(1);
        stageCharacterData.stageDetails = stageData.GetDataByID<StageDetailsSO>(0);


        LoadingSceneManager.LoadScene("CombatScene", "Stage1", ESceneType.MainGame);
    }
}
