using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainOptionView : MonoBehaviour
{
    [SerializeField] private Button btnExit;
    [SerializeField] private Button btnBlog;


    private void OnEnable()
    {
        btnExit.onClick.AddListener(() => gameObject.SetActive(false));
        btnBlog.onClick.AddListener(() => Application.OpenURL("https://blog.naver.com/tmdgur0147"));
    }

    private void OnDisable()
    {
        btnExit.onClick.RemoveAllListeners();
        btnBlog.onClick.RemoveAllListeners();
    }
}
