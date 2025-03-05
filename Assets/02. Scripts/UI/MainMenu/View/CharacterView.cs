using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    private List<BtnCharacterInfoUI> btnCharacterInfoUIs;
    private Database playerDB;


    private void Awake()
    {
        btnCharacterInfoUIs = GetComponentsInChildren<BtnCharacterInfoUI>().ToList();
        playerDB = AddressableManager.Instance.GetResource<Database>("DB_Player");
    }

    public void InitializeCharacterView(CurrencySystem currencySystem, UnlockSystem unlockSystem)
    {
        // �÷��̾� DB���� ID�� �ϳ��� ������ ��ư�� �־��ֱ�
        for (int i = 0; i < btnCharacterInfoUIs.Count; i++)
        {
            btnCharacterInfoUIs[i].InitializeBtnCharacterInfoUI(playerDB.GetDataByID<PlayerDetailsSO>(i), currencySystem, unlockSystem);
        }
    }
}