using UnityEngine;
using System.Collections.Generic;
using Firebase.Database;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Extensions;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Runtime.Remoting.Messaging;
using System;


public class SaveManager : Singleton<SaveManager>
{
    public Action OnLoadFinished; // 로드가 끝나고 UI 업데이트해야 반영됨

    private DatabaseReference databaseReference;
    private FirebaseUser user;
    private List<ISaveData> saveDatas = new List<ISaveData>();

    [HideInInspector] public SaveData SaveData;


    private void Start()
    {
        databaseReference = FirebaseDatabase.DefaultInstance.RootReference;
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        LoadGame();
    }

    public void Register(ISaveData saveData) // 저장해야하는 데이터를 해당 클래스 내에서 등록
    {
        saveDatas.Add(saveData);
    }

    public void SaveGame()
    {
        // 구조체를 Json 형태로 변환 (Newtonsoft 사용)
        string saveData = JsonConvert.SerializeObject(SaveData);

        // SaveData 노드 아래에 user.UserId 자식을 생성해서 SetRawJsonValueAsync으로 데이터 저장
        databaseReference.Child("SaveData").Child(user.UserId).SetRawJsonValueAsync(saveData);
    }

    public void LoadGame()
    {
        /// 파이어베이스 인스턴스에서 현재 로그인한 UserId를 가져오기
        /// 해당 UserId에 있는 saveData 가져와서 유니티 구조체로 역직렬화
        /// 
        DatabaseReference saveDB = FirebaseDatabase.DefaultInstance.GetReference("SaveData");
        saveDB.OrderByKey().EqualTo(user.UserId).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
                Debug.LogError("Task Exception: " + task.Exception);
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    // 데이터를 찾아서 json 문자열로 변환
                    string json = snapshot.GetRawJsonValue();

                    // JSON을 JObject로 파싱
                    JObject jsonObject = JObject.Parse(json);

                    // 사용자 ID에 해당하는 내부 객체를 추출 (데이터에서 ID는 제외시키는 작업)
                    JToken userDataToken = jsonObject[user.UserId];

                    if (userDataToken != null)
                    {
                        // 내부 객체만을 다시 JSON 문자열로 변환
                        string userDataJson = userDataToken.ToString();

                        try
                        {
                            // JSON 데이터 역직렬화 (Newtonsoft 사용)
                            var saveData = JsonConvert.DeserializeObject<SaveData>(userDataJson);
                            FromSaveData(saveData);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("역직렬화 중 오류 발생: " + ex.Message);
                        }

                        OnLoadFinished?.Invoke(); // 역직렬화 이후 로드 종료
                    }
                    else Debug.Log("User ID no found");
                }
                else
                {
                    OnLoadFinished?.Invoke(); // 로드할 데이터가 없을 경우
                    Debug.Log("no save data");
                }
            }
        });
    }

    private void FromSaveData(SaveData saveData)
    {
        // 저장된 데이터들 로드한 다음 각자의 클래스 내에서 사용
        foreach (var data in saveDatas)
        {
            data.FromSaveData(saveData);
        }
    }

    private void OnApplicationQuit()
    {
        foreach (var saveData in saveDatas)
        {
            saveData.ToSaveData();
        }

        SaveGame();
    }

    private void OnApplicationPause(bool pause)
    {
        // 모바일 게임은 일시정지(앱을 내릴때) 세이브
        if (pause)
        {
            foreach (var saveData in saveDatas)
            {
                saveData.ToSaveData();
            }

            SaveGame();
        }
    }
}
