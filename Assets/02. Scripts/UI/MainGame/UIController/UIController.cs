using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Button")] 
    [SerializeField] private Button btnPause;

    [Header("UI Controller")]
    [SerializeField] private LevelUpController levelUpController;

    private ExpController expController;
    private PlayerLevelUIController levelController;
    private StageInfoController stageInfoController;
    private WaveFinishController waveFinishController;
    private PauseController pauseController;

    public LevelUpController LevelUpController => levelUpController;
    public WaveFinishController WaveFinishController => waveFinishController;


    private void Awake()
    {
        levelController = GetComponent<PlayerLevelUIController>();
        expController = GetComponent<ExpController>();
        stageInfoController = GetComponent<StageInfoController>();
        waveFinishController = GetComponent<WaveFinishController>();
        pauseController = GetComponent<PauseController>();
    }

    public void InitializeUIController()
    {
        expController.InitializeExpController();
        levelUpController.gameObject.SetActive(false);
        stageInfoController.InitializeStageInfoController();
        levelController.InitializePlayerLevelController();

        btnPause.onClick.AddListener(OnBtnPause);
    }

    private void OnBtnPause()
    {
        pauseController.InitializePauseView();
    }
}
