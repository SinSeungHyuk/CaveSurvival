using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopView : MonoBehaviour
{
    [SerializeField] private Button btnExit;


    private void OnEnable()
    {
        btnExit.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void OnDisable()
    {
        btnExit.onClick.RemoveAllListeners();
    }
}
