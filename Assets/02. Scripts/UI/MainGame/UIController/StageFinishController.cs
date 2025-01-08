using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageFinishController : MonoBehaviour
{
    [SerializeField] private StageFinishView stageFinishView;


    public void InitializeStageFinishController()
    {
        Player player = GameManager.Instance.Player;
        var stat = player.Stat;
        var weaponList = player.WeaponList;

        stageFinishView.InitializeStageFinishView(stat, weaponList);
    }
}
