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

        // ���� ���õ� ĳ���Ͱ� �������� ���� ĳ���Ϳ� �����Ҷ��� �÷��̹�ư Ȱ��ȭ
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
        // ��� ��ư�� �ش� ĳ���Ͱ� ����� ������ ��Ȳ�϶��� ���� �� ���� (Achive ������)
        // ��� ��ư�� Ŭ���ϸ� ����ڽ�Ʈ��ŭ ��ȭ�� �Ҹ�ǰ� ĳ���Ͱ� �����Ǹ鼭 ���õ�
        btnUnlock.enabled = false;
        if (unlockSystem.CharacterUnlockList[playerDetailsSO.ID] == false)
        {
            // ����� ������ ���� (��ȭ)
            if (unlockSystem.CanUnlockChracter(playerDetailsSO.ID, currencySystem, playerDetailsSO.unlockCost))
                btnUnlock.enabled = true;

            txtUnlockCost.text = playerDetailsSO.unlockCost.ToString();

            btnUnlock.onClick.RemoveAllListeners();
            btnUnlock.onClick.AddListener(() =>
            {
                UnlockCharacter();
            });
        }
        else // ����� �Ǿ��ִ� ����
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
