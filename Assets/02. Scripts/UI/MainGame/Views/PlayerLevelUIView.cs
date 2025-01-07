using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelUIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtLevel;


    public void SetLevel(int level)
        => txtLevel.text = $"Lv. {level}";
}
