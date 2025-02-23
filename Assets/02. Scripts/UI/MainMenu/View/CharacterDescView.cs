using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDescView : MonoBehaviour
{
    [SerializeField] private GameStartUI gameStartUI;

    [SerializeField] private Image imgCharacter;
    [SerializeField] private Image imgStartingWeapon;

    [SerializeField] private TextMeshProUGUI txtCharacterName;
    [SerializeField] private TextMeshProUGUI txtCharacterStrength;
    [SerializeField] private TextMeshProUGUI txtCharacterWeakness;
    [SerializeField] private TextMeshProUGUI txtUnlockCost;

    [SerializeField] private Button btnUnlock;
    [SerializeField] private Button btnPlay;

    private CurrencySystem currencySystem;
    private PlayerDetailsSO playerDetailsSO;
    private UnlockSystem unlockSystem;


    public void InitializeCharacterDescView(PlayerDetailsSO playerDetailsSO, CurrencySystem currencySystem, UnlockSystem unlockSystem)
    {
        this.currencySystem = currencySystem;
        this.playerDetailsSO = playerDetailsSO;
        this.unlockSystem = unlockSystem;

        SetCharacterDescUI();
        SetBtnPlay();
        SetBtnUnlock();
    }

    private void SetCharacterDescUI()
    {
        imgCharacter.sprite = playerDetailsSO.playerSprite;
        imgStartingWeapon.sprite = playerDetailsSO.playerStartingWeapon.weaponSprite;

        txtCharacterName.text = playerDetailsSO.characterName;
        txtCharacterStrength.text = playerDetailsSO.characterStrength;
        txtCharacterWeakness.text = playerDetailsSO.characterWeakness;
    }

    private void SetBtnPlay()
    {
        var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");

        // 현재 선택된 캐릭터가 스테이지 진행 캐릭터와 동일할때만 플레이버튼 활성화
        if (stageCharacterData.playerDetails == playerDetailsSO)
        {
            btnPlay.onClick.RemoveAllListeners();
            btnPlay.onClick.AddListener(()
                => gameStartUI.InitializeGameStartUI(stageCharacterData));
        }
        else
            btnPlay.enabled = false;
    }

    private void SetBtnUnlock()
    {
        // 언락 버튼은 해당 캐릭터가 언락이 가능한 상황일때만 누를 수 있음 (Achive 보유량)
        // 언락 버튼을 클릭하면 언락코스트만큼 재화가 소모되고 캐릭터가 해제되면서 선택됨
        btnUnlock.enabled = false;
        if (unlockSystem.CharacterUnlockList[playerDetailsSO.ID] == false)
        {
            // 언락이 가능한 상태 (재화)
            if (unlockSystem.CanUnlockChracter(playerDetailsSO.ID, currencySystem, playerDetailsSO.unlockCost))
                btnUnlock.enabled = true;

            txtUnlockCost.text = playerDetailsSO.unlockCost.ToString();

            btnUnlock.onClick.RemoveAllListeners();
            btnUnlock.onClick.AddListener(() =>
            {
                UnlockCharacter();
            });
        }
        else // 언락이 되어있는 상태
        {
            txtUnlockCost.text = Settings.unlockText;
        }
    }

    private void UnlockCharacter()
    {
        unlockSystem.UnlockCharacter(playerDetailsSO.ID, currencySystem, playerDetailsSO.unlockCost);
        txtUnlockCost.text = Settings.unlockText;
        var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");
        stageCharacterData.playerDetails = playerDetailsSO;
        btnUnlock.enabled = false;
    }
}
