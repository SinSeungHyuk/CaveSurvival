using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTest : MonoBehaviour
{
    [SerializeField] private Material materialTest;


    private void Update()
    {
        materialTest.SetFloat("_SplitValue", Mathf.Sin(Time.time));
    }
}
