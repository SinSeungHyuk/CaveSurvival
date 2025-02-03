using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainOptionController : MonoBehaviour
{
    [SerializeField] private MainOptionView mainOptionView;


    public void InitializeMainOptionController()
    {
        mainOptionView.gameObject.SetActive(true);
    }
}
