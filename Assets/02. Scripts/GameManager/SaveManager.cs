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
    public Action OnLoadFinished; // �ε尡 ������ UI ������Ʈ�ؾ� �ݿ���

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

    public void Register(ISaveData saveData) // �����ؾ��ϴ� �����͸� �ش� Ŭ���� ������ ���
    {
        saveDatas.Add(saveData);
    }

    public void SaveGame()
    {
        // ����ü�� Json ���·� ��ȯ (Newtonsoft ���)
        string saveData = JsonConvert.SerializeObject(SaveData);

        // SaveData ��� �Ʒ��� user.UserId �ڽ��� �����ؼ� SetRawJsonValueAsync���� ������ ����
        databaseReference.Child("SaveData").Child(user.UserId).SetRawJsonValueAsync(saveData);
    }

    public void LoadGame()
    {
        /// ���̾�̽� �ν��Ͻ����� ���� �α����� UserId�� ��������
        /// �ش� UserId�� �ִ� saveData �����ͼ� ����Ƽ ����ü�� ������ȭ
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
                    // �����͸� ã�Ƽ� json ���ڿ��� ��ȯ
                    string json = snapshot.GetRawJsonValue();

                    // JSON�� JObject�� �Ľ�
                    JObject jsonObject = JObject.Parse(json);

                    // ����� ID�� �ش��ϴ� ���� ��ü�� ���� (�����Ϳ��� ID�� ���ܽ�Ű�� �۾�)
                    JToken userDataToken = jsonObject[user.UserId];

                    if (userDataToken != null)
                    {
                        // ���� ��ü���� �ٽ� JSON ���ڿ��� ��ȯ
                        string userDataJson = userDataToken.ToString();

                        try
                        {
                            // JSON ������ ������ȭ (Newtonsoft ���)
                            var saveData = JsonConvert.DeserializeObject<SaveData>(userDataJson);
                            FromSaveData(saveData);
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError("������ȭ �� ���� �߻�: " + ex.Message);
                        }

                        OnLoadFinished?.Invoke(); // ������ȭ ���� �ε� ����
                    }
                    else Debug.Log("User ID no found");
                }
                else
                {
                    OnLoadFinished?.Invoke(); // �ε��� �����Ͱ� ���� ���
                    Debug.Log("no save data");
                }
            }
        });
    }

    private void FromSaveData(SaveData saveData)
    {
        // ����� �����͵� �ε��� ���� ������ Ŭ���� ������ ���
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
        // ����� ������ �Ͻ�����(���� ������) ���̺�
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
