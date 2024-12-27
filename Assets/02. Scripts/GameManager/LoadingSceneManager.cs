using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using Photon.Pun;
using Photon.Realtime;


public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] private Image imgLoadingBar;

    private static string nextSceneName;
    private static string groupToLoad;
    private static ESceneType sceneTypeToLoad;

    private float resourceProgress;


    [PunRPC]
    // static �����Լ� : �ν��Ͻ�ȭ���� �ʰ� �ƹ������� ȣ�Ⱑ���� �ε��Լ�
    public static void LoadScene(string sceneName, string labelsToLoad, ESceneType sceneType)
    {
        nextSceneName = sceneName;
        groupToLoad = labelsToLoad;
        sceneTypeToLoad = sceneType;

        PhotonNetwork.LoadLevel("LoadingScene");
        //SceneManager.LoadScene("LoadingScene");
    }

    private async void Awake()
    {
        // �ε����� �����ϸ� �ε� ����
        await LoadSceneAsync();

        /*
        if (sceneTypeToLoad == ESceneType.MainGame)
            GameManager.Instance.CreateMainGameScene();
        */
    }

    private async UniTask LoadSceneAsync()
    {
        imgLoadingBar.fillAmount = 0;

        // ���ҽ� �ε�
        // AddressableManager�� LoadResources �Լ��� UniTask�� ȣ��
        /*
        await AddressableManager.Instance.LoadResourcesAsync(
            groupToLoad,
            (progress) =>
            {
                UpdateLoadingProgress(progress);
            }
        );
        */
        

        // �� �ε� �񵿱� �޼ҵ�
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;

        // �ε��� �Ϸ�� ������ ���
        while (!asyncLoad.isDone)
        {
            float sceneProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // �� �ε��� 90% �̻��̸� allowSceneActivation�� true�� �����Ͽ� �� �����ϱ�
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            // ������ ��� (UniTask.DelayFrame()�� �̿��Ͽ� �� ������ ���)
            await UniTask.Yield();
        }
    }


    private void UpdateLoadingProgress(float progress)
        => imgLoadingBar.fillAmount = progress;
}
