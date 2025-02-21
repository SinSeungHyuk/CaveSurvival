using R3;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnUltimate : MonoBehaviour
{
    [SerializeField] private Image characterImage;
    [SerializeField] private Image blindImage;

    private Button btnUltimate;


    private void Awake()
    {
        btnUltimate = GetComponent<Button>();
        btnUltimate.interactable = false;
    }

    public void InitializeBtnUltimate()
    {
        Player player = GameManager.Instance.Player;

        btnUltimate.onClick.AddListener(() => player.UseUltimateSkill());

        characterImage.sprite = player.UltimateSkillBehaviour.SkillSO.UltimateSkillData.skillIcon;

        player.UltimateSkillBehaviour.GaugeRatio.Subscribe(gauge 
            => SetBtnUltimate(gauge));
    }

    private void SetBtnUltimate(float gauge)
    {
        blindImage.fillAmount = 1 - gauge;

        if (gauge >= 1)
            btnUltimate.interactable = true;
        else
            btnUltimate.interactable = false;
    }
}
