using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopController : MonoBehaviour
{
    [SerializeField] private ShopView shopView;


    public void InitializeShopController()
    {
        shopView.gameObject.SetActive(true);
    }
}
