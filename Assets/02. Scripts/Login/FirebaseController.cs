using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Extensions;
using Firebase.Auth;
using System.Threading.Tasks;
using System;
using TMPro;
using GooglePlayGames.BasicApi;
using GooglePlayGames;
using Google;
using Firebase.Database;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class FirebaseController : MonoBehaviour
{
    [SerializeField] private LoginUIController loginUIController;

    private FirebaseAuth auth; // 인증에 관한 정보 저장할 객체
    private FirebaseUser user; // 파이어베이스 유저의 정보를 담을 객체

    private string authCode; // 로그인을 위한 유저코드


    void Start()
    {
        // 구글플레이 로그인
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.Instance.Authenticate(success =>
        {
            Debug.Log($" @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ {success}");

            if (success == SignInStatus.Success)
            {
                // RequestServerSideAccess : ServerAuthCode(= code) 를 반환해주는 함수 
                PlayGamesPlatform.Instance.RequestServerSideAccess(true, code =>
                {
                    authCode = code;

                    // 위에서 받은 코드를 바탕으로 로그인 인증서를 발급받기 (GetCredential)
                    auth = FirebaseAuth.DefaultInstance;
                    Credential credential = PlayGamesAuthProvider.GetCredential(authCode);

                    auth.SignInAndRetrieveDataWithCredentialAsync(credential)
                        .ContinueWithOnMainThread(task =>
                        {
                            if (task.IsCompleted)
                            {
                                Debug.Log($" @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ task.IsCompleted");
                                HasNicknameByID(); // 로그인에 성공하면 계정 있는지 검사
                            }

                            Firebase.Auth.AuthResult result = task.Result;
                        });
                });
            }
            else // 로그인 성공 이외의 모든 상황
            {
                loginUIController.LoginFailed();
            }
        });
    }

    private void HasNicknameByID()
    {
        Debug.Log($" @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@ HasNicknameByID");

        // 현재 로그인한 파이어베이스 계정의 UserId를 가져와서 Nickname 데이터가 있는지 검사
        user = auth.CurrentUser;
        DatabaseReference nameDB = FirebaseDatabase.DefaultInstance.GetReference("Nickname");

        nameDB.OrderByKey().EqualTo(user.UserId).GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // 에러 처리
                Debug.Assert(task.Exception != null, "Task Exception!!" + task.Exception);
            }
            else if (task.IsCompleted)
            {
                DataSnapshot snapshot = task.Result;
                if (snapshot.Exists) // 데이터가 존재하는 경우 (Exists = 존재하다)
                {
                    foreach (var childSnapshot in snapshot.Children)
                    {
                        // 닉네임 로컬에 저장 (다른 씬에서도 빠르게 사용)
                        string nickname = childSnapshot.Value.ToString();
                        PlayerPrefs.SetString("Nickname", nickname);
                        PlayerPrefs.Save();

                        loginUIController.StartGame();
                        break; // 첫 번째 (그리고 유일한) 자식만 처리합니다.
                    }
                }
                else
                {
                    // 데이터가 존재하지 않는 경우
                    loginUIController.SetCreateNicknameUI();
                }
            }
            else
            {
                Debug.Log("HasNicknameByID - " + task.Status);
            }
        });
    }

    public void CreateNickname() // 닉네임 확정 버튼에 등록
    {
        DatabaseReference nameDB = FirebaseDatabase.DefaultInstance.GetReference("Nickname");

        // <유저아이디, 유저닉네임> 딕셔너리 : 유저의 ID마다 고유한 닉네임 밸류값으로 저장
        Dictionary<string, object> nicknameDic = new Dictionary<string, object>();

        string nickname = loginUIController.InpNickname.text;
        nicknameDic.Add(user.UserId, nickname);

        // nameDB에 nicknameDic를 추가해서 데이터 업데이트
        nameDB.UpdateChildrenAsync(nicknameDic).ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                PlayerPrefs.SetString("Nickname", nickname);
                PlayerPrefs.Save();
            }
        });
    }
}