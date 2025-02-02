using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDescView : MonoBehaviour
{
    [SerializeField] private Image imgCharacter;
    [SerializeField] private Image imgStartingWeapon;

    [SerializeField] private TextMeshProUGUI txtCharacterName;
    [SerializeField] private TextMeshProUGUI txtCharacterStrength;
    [SerializeField] private TextMeshProUGUI txtCharacterWeakness;

    [SerializeField] private Button btnUnlock;
    [SerializeField] private Button btnPlay;

    private CurrencySystem currencySystem;


    public void InitializeCharacterDescView(PlayerDetailsSO playerDetailsSO, CurrencySystem currencySystem)
    {
        this.currencySystem = currencySystem;

        imgCharacter.sprite = playerDetailsSO.playerSprite;
        imgStartingWeapon.sprite = playerDetailsSO.playerStartingWeapon.weaponSprite;

        txtCharacterName.text = playerDetailsSO.characterName;
        txtCharacterStrength.text = playerDetailsSO.characterStrength;
        txtCharacterWeakness.text = playerDetailsSO.characterWeakness;

        var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");

        if (stageCharacterData.stageDetails != null && stageCharacterData.playerDetails != null)
        {
            btnPlay.onClick.RemoveAllListeners();
            btnPlay.onClick.AddListener(()
                => LoadingSceneManager.LoadScene("CombatScene", "Stage1", ESceneType.MainGame));
        }
        else 
            btnPlay.enabled = false;
    }
}
