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
    [SerializeField] private TextMeshProUGUI txtUnlockCost;

    [SerializeField] private Button btnUnlock;
    [SerializeField] private Button btnPlay;

    private CurrencySystem currencySystem;


    public void InitializeCharacterDescView(PlayerDetailsSO playerDetailsSO, CurrencySystem currencySystem, UnlockSystem unlockSystem)
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


        // ��� ��ư�� �ش� ĳ���Ͱ� ����� ������ ��Ȳ�϶��� ���� �� ���� (Achive ������)
        // ��� ��ư�� Ŭ���ϸ� ����ڽ�Ʈ��ŭ ��ȭ�� �Ҹ�ǰ� ĳ���Ͱ� �����Ǹ鼭 ���õ�
        btnUnlock.enabled = false;
        if (unlockSystem.CharacterUnlockList[playerDetailsSO.ID] == false)
        {
            if (unlockSystem.CanUnlockChracter(playerDetailsSO.ID, currencySystem, playerDetailsSO.unlockCost))
                btnUnlock.enabled = true;

            txtUnlockCost.text = playerDetailsSO.unlockCost.ToString();

            btnUnlock.onClick.RemoveAllListeners();
            btnUnlock.onClick.AddListener(() =>
            {
                unlockSystem.UnlockCharacter(playerDetailsSO.ID, currencySystem, playerDetailsSO.unlockCost);
                var stageCharacterData = AddressableManager.Instance.GetResource<StageCharacterDataSO>("StageCharacterData");
                stageCharacterData.playerDetails = playerDetailsSO;
                btnUnlock.enabled = false;
            });
        } 
        else
        {
            txtUnlockCost.text = "������";
        }
    }
}
