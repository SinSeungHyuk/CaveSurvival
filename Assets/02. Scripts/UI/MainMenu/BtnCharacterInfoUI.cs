using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnCharacterInfoUI : MonoBehaviour
{
    private Button btnCharacterInfoUI;

    [SerializeField] private CharacterDescView characterDescView;
    [SerializeField] private StageDescView stageDescView;


    private void Awake()
    {
        btnCharacterInfoUI = GetComponent<Button>();
    }

    public void InitializeBtnCharacterInfoUI(PlayerDetailsSO playerDetailsSO, CurrencySystem currencySystem, UnlockSystem unlockSystem)
    {
        var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");

        stageCharacterData.playerDetails = playerDetailsSO;

        btnCharacterInfoUI.onClick.RemoveAllListeners();
        btnCharacterInfoUI.onClick.AddListener(()
            => InitializeCharacterDescView(playerDetailsSO, currencySystem, unlockSystem));
    }

    private void InitializeCharacterDescView(PlayerDetailsSO playerDetailsSO, CurrencySystem currencySystem, UnlockSystem unlockSystem)
    {
        if (unlockSystem.CharacterUnlockList[playerDetailsSO.ID] == true)
        {
            var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");
            stageCharacterData.playerDetails = playerDetailsSO;
        }

        characterDescView.InitializeCharacterDescView(playerDetailsSO, currencySystem, unlockSystem);
        stageDescView.gameObject.SetActive(false);
        characterDescView.gameObject.SetActive(true);
    }
}
