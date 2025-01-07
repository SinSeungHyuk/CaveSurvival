using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private PauseView pauseView;


    public void InitializePauseView()
    {
        Player player = GameManager.Instance.Player;
        var stat = player.Stat;
        var weaponList = player.WeaponList;

        pauseView.InitializePauseView(stat,weaponList);
    }
}
