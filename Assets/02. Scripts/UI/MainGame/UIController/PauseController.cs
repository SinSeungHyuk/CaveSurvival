using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [SerializeField] private PauseView pauseView;


    public void InitializePauseController()
    {
        Player player = GameManager.Instance.Player;

        pauseView.InitializePauseView(player);
    }
}
