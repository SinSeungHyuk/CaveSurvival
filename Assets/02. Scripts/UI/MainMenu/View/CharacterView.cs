using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        for (int i = 0; i < btnCharacterInfoUIs.Count; i++)
        {
            // 언락 반영 구현


            btnCharacterInfoUIs[i].InitializeBtnCharacterInfoUI(playerDB.GetDataByID<PlayerDetailsSO>(i), currencySystem);
        }
    }
}