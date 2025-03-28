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

    private static string nextSceneName; // 로드할 씬 이름
    private static string groupToLoad; // 로드할 리소스 그룹명
    private static ESceneType sceneTypeToLoad; // 로드할 씬의 타입


    // static 정적함수 : 인스턴스화하지 않고도 아무데서나 호출가능한 로딩함수
    public static void LoadScene(string sceneName, string labelsToLoad, ESceneType sceneType)
    {
        nextSceneName = sceneName;
        groupToLoad = labelsToLoad;
        sceneTypeToLoad = sceneType;

        SceneManager.LoadScene("LoadingScene");
    }

    private async void Awake()
    {
        // 로딩씬에 진입하면 로딩 시작, await으로 로딩이 끝날때까지 진행
        await LoadSceneAsync();
        
        switch (sceneTypeToLoad)
        {
            case ESceneType.MainGame:
                GameManager.Instance.CreateMainGameScene();
                break;
        }
    }

    private async UniTask LoadSceneAsync()
    {
        loadingBar.value = 0;

        // 리소스 로딩
        //AddressableManager의 LoadResources 함수를 UniTask로 호출
        await AddressableManager.Instance.LoadResourcesAsync(
            groupToLoad,
            (progress) =>
            {
                UpdateLoadingProgress(progress);
            }
        );

        // 씬 로딩 비동기 메소드
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(nextSceneName);
        asyncLoad.allowSceneActivation = false;

        // 로딩이 완료될 때까지 대기
        while (!asyncLoad.isDone)
        {
            float sceneProgress = Mathf.Clamp01(asyncLoad.progress / 0.9f);

            // 씬 로딩이 90% 이상이면 allowSceneActivation을 true로 변경하여 씬 변경하기
            if (asyncLoad.progress >= 0.9f)
            {
                asyncLoad.allowSceneActivation = true;
            }
            // 프레임 대기 (UniTask.DelayFrame()을 이용하여 한 프레임 대기)
            await UniTask.Yield();
        }
    }


    private void UpdateLoadingProgress(float progress)
        => loadingBar.value = progress;
}
