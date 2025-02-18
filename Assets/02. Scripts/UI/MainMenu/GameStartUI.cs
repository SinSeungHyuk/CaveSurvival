using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStartUI : MonoBehaviour
{
    [SerializeField] private Image stageImage;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI txtStage;
    [SerializeField] private TextMeshProUGUI txtCharacter;
    [SerializeField] private Button btnConfirm;
    [SerializeField] private Button btnExit;


    public void InitializeGameStartUI(StageCharacterDataSO stageCharacterData)
    {
        stageImage.sprite = stageCharacterData.stageDetails.bossSprite;
        characterImage.sprite = stageCharacterData.playerDetails.playerSprite;

        txtStage.text = stageCharacterData.stageDetails.roomName;
        txtCharacter.text = stageCharacterData.playerDetails.characterName;

        SetButton();

        gameObject.SetActive(true);
    }

    private void SetButton()
    {
        btnConfirm.onClick.RemoveAllListeners();
        btnExit.onClick.RemoveAllListeners();

        btnConfirm.onClick.AddListener(()
            => LoadingSceneManager.LoadScene("CombatScene", "Stage1", ESceneType.MainGame));

        btnExit.onClick.AddListener(() => gameObject.SetActive(false));
    }
}
