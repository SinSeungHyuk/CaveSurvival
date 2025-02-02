using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;



public class LoadingSceneManager : MonoBehaviour
{
    [SerializeField] private Slider loadingBar;

    private static string nextSceneName; // �ε��� �� �̸�
    private static string groupToLoad; // �ε��� ���ҽ� �׷��
    private static ESceneType sceneTypeToLoad; // �ε��� ���� Ÿ��

    private float resourceProgress;


    // static �����Լ� : �ν��Ͻ�ȭ���� �ʰ� �ƹ������� ȣ�Ⱑ���� �ε��Լ�
    public static void LoadScene(string sceneName, string labelsToLoad, ESceneType sceneType)
    {
        nextSceneName = sceneName;
        groupToLoad = labelsToLoad;
        sceneTypeToLoad = sceneType;

        SceneManager.LoadScene("LoadingScene");
    }

    private async void Start()
    {
        // �ε����� �����ϸ� �ε� ����, await���� �ε��� ���������� ����
        await LoadSceneAsync();
        
        if (sceneTypeToLoad == ESceneType.MainGame)
            GameManager.Instance.CreateMainGameScene();
    }

    private async UniTask LoadSceneAsync()
    {
        loadingBar.value = 0;

        // ���ҽ� �ε�
        //AddressableManager�� LoadResources �Լ��� UniTask�� ȣ��
        await AddressableManager.Instance.LoadResourcesAsync(
            groupToLoad,
            (progress) =>
            {
                UpdateLoadingProgress(progress);
            }
        );

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
        => loadingBar.value = progress;
}
