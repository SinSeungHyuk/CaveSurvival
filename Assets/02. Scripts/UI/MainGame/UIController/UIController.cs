using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Buttons")] 
    [SerializeField] private Button btnPause;

    [Header("UI Controller")]
    [SerializeField] private LevelUpController levelUpController;
    [SerializeField] private ExpView expView;
    [SerializeField] private PlayerLevelView playerLevelView;

    //private ExpController expController;
    //private PlayerLevelUIController levelController;
    private StageInfoController stageInfoController;
    private WaveFinishController waveFinishController;
    private PauseController pauseController;
    private StageFinishController stageFinishController;

    public LevelUpController LevelUpController => levelUpController;
    public WaveFinishController WaveFinishController => waveFinishController;
    public StageFinishController StageFinishController => stageFinishController;


    private void Awake()
    {
        //levelController = GetComponent<PlayerLevelUIController>();
        stageInfoController = GetComponent<StageInfoController>();
        waveFinishController = GetComponent<WaveFinishController>();
        pauseController = GetComponent<PauseController>();
        stageFinishController = GetComponent<StageFinishController>();
    }

    public void InitializeUIController()
    {
        //expController.InitializeExpController();
        expView.InitializeExpView();
        playerLevelView.InitializePlayerLevelView();

        levelUpController.gameObject.SetActive(false);
        stageInfoController.InitializeStageInfoController();
        //levelController.InitializePlayerLevelController();

        btnPause.onClick.AddListener(OnBtnPause);
    }

    private void OnBtnPause()
    {
        pauseController.InitializePauseController();
    }
}
