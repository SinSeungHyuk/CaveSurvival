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

    public void InitializeBtnCharacterInfoUI(PlayerDetailsSO playerDetailsSO, CurrencySystem currencySystem)
    {
        var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");

        stageCharacterData.playerDetails = playerDetailsSO;

        btnCharacterInfoUI.onClick.RemoveAllListeners();
        btnCharacterInfoUI.onClick.AddListener(()
            => InitializeCharacterDescView(playerDetailsSO, currencySystem));
    }

    private void InitializeCharacterDescView(PlayerDetailsSO playerDetailsSO, CurrencySystem currencySystem)
    {
        var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");
        stageCharacterData.playerDetails = playerDetailsSO;

        characterDescView.InitializeCharacterDescView(playerDetailsSO, currencySystem);
        stageDescView.gameObject.SetActive(false);
        characterDescView.gameObject.SetActive(true);
    }
}
