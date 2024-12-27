using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginUIController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtLogin;
    [SerializeField] private TextMeshProUGUI txtNickname;
    [SerializeField] private TMP_InputField inpNickname;
    [SerializeField] private GameObject createNickname;
    [SerializeField] private GameObject dlgNickname;
    [SerializeField] private GameObject dlgLoginFailed;
    [SerializeField] private GameObject dlgInvalidNickname;
    [SerializeField] private Button btnCreateDlgNickname;
    [SerializeField] private Button btnCreateNickname;
    [SerializeField] private Button btnExitCreateNickname;
    [SerializeField] private Button btnExitInvalidNickname;
    [SerializeField] private Button btnExit;
    [SerializeField] private Button btnStart;

    public TMP_InputField InpNickname => inpNickname;


    private void Start()
    {
        btnCreateDlgNickname.onClick.AddListener(OnCreateDialogNickname);
        btnCreateNickname.onClick.AddListener(OnCreateNickname);
        btnExitCreateNickname.onClick.AddListener(OnExitCreateNickname);
        btnExitInvalidNickname.onClick.AddListener(OnExitInvalidNickname);
        btnStart.onClick.AddListener(LoadStartScene);
        btnExit.onClick.AddListener(OnExit);
    }


    public void SetCreateNicknameUI()
    {
        inpNickname.text = "";
        txtLogin.gameObject.SetActive(false);
        createNickname.SetActive(true);
    }

    public void StartGame()
    {
        btnStart.gameObject.SetActive(true);
        txtLogin.text = "Touch To Start";
    }

    public void LoginFailed()
        => dlgLoginFailed.gameObject.SetActive(true);


    private void OnCreateDialogNickname() // �г��� ���� ��ư�� ���
    {
        if (Regex.IsMatch(inpNickname.text, Settings.regex) == false)
        {
            dlgInvalidNickname.SetActive(true);
            return;
        }

        dlgNickname.SetActive(true);

        txtNickname.text = $"Use [{inpNickname.text}] ?";
    }

    private void OnCreateNickname() // �г��� Ȯ�� ��ư�� ���
    {
        dlgNickname.SetActive(false);
        createNickname.SetActive(false);
        txtLogin.gameObject.SetActive(true);

        StartGame();
    }

    private void OnExitInvalidNickname() // �г��� ����� ��ư�� ���
        => dlgInvalidNickname.SetActive(false);

    private void OnExitCreateNickname() // ���̾�α� ��� ��ư�� ���
        => dlgNickname.SetActive(false);

    private void OnExit()
        => Application.Quit();


    public void LoadStartScene() // ��ŸƮ ��ư�� ���
    {
        // ���� ���࿡ �ʿ��� ���ҽ� �ε� (��巹���� Ȱ��)
        //List<string> levelResources = new List<string> { "Database", "Sprites", "Prefabs" };
        List<string> levelResources = new List<string> {};
        LoadingSceneManager.LoadScene("TestMainMenuScene", "TestA", ESceneType.MainMenu);
    }
}
