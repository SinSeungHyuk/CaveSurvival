using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using R3;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtLevel;

    private PlayerStat playerStat;
    private PlayerLevelViewModel playerLevelViewModel;


    public void InitializePlayerLevelView()
    {
        playerStat = GameManager.Instance.Player.Stat;
        playerLevelViewModel = new PlayerLevelViewModel(playerStat);

        playerLevelViewModel.PlayerLevel.Subscribe(level
            => SetPlayerLevel(level));
    }

    private void SetPlayerLevel(string level)
        => txtLevel.text = level;
}