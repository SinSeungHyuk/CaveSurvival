using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainOptionView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtMusic;
    [SerializeField] private TextMeshProUGUI txtSound;
    [SerializeField] private Button btnExit;
    [SerializeField] private Button btnBlog;


    private void OnEnable()
    {
        btnExit.onClick.AddListener(() => gameObject.SetActive(false));
        btnBlog.onClick.AddListener(() => Application.OpenURL("https://blog.naver.com/tmdgur0147"));

        txtMusic.text = MusicManager.Instance.musicVolume.ToString();
        txtSound.text = SoundEffectManager.Instance.soundsVolume.ToString();
    }

    private void OnDisable()
    {
        btnExit.onClick.RemoveAllListeners();
        btnBlog.onClick.RemoveAllListeners();
    }

    public void IncreaseMusicVolume()
    {
        MusicManager.Instance.IncreaseVolume();
        txtMusic.SetText(MusicManager.Instance.musicVolume.ToString());
    }
    public void DecreaseMusicVolume()
    {
        MusicManager.Instance.DecreaseVolume();
        txtMusic.SetText(MusicManager.Instance.musicVolume.ToString());
    }
    public void IncreaseSoundsVolume()
    {
        SoundEffectManager.Instance.IncreaseVolume();
        txtSound.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
    }
    public void DecreaseSoundsVolume()
    {
        SoundEffectManager.Instance.DecreaseVolume();
        txtSound.SetText(SoundEffectManager.Instance.soundsVolume.ToString());
    }
}
