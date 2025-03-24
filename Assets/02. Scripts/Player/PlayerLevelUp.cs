using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using GooglePlayGames.BasicApi;
using R3;

public class PlayerLevelUp : MonoBehaviour
{
    private PlayerLevelUpDatabase playerDB;
    private WeaponLevelUpDatabase weaponDB;
    private Database weaponDetailsDB;

    private Player player;

    private Dictionary<int, List<int>> validChoice = new();



    private void Awake()
    {
        player = GetComponent<Player>();
    }

    private void Start()
    {
        weaponDetailsDB = AddressableManager.Instance.GetResource<Database>("DB_Weapon");
        playerDB = AddressableManager.Instance.GetResource<PlayerLevelUpDatabase>("PlayerLevelUpDatabase");
        weaponDB = AddressableManager.Instance.GetResource<WeaponLevelUpDatabase>("WeaponLevelUpDatabase");
    }


    public void PlayerStat_OnLevelChanged(int level)
    {
        GameManager.Instance.UIController.LevelUpController.InitializeLevelUpController();

        int options = 4; // ������ 4��

        // 0. �÷��̾� ���Ⱑ 4�� �̸��̶�� ù �������� ������ ��������
        if (player.WeaponList.Count < 4)
        {
            options--; // ���Ⱑ �����Ƿ� ������ �ϳ� ����

            WeaponDetailsSO weaponData = GetRandomWeapon();

            GameManager.Instance.UIController.LevelUpController.InitializeLevelUpUI(
                weaponData,
                options // ������ ��ġ
            );
        }

        /// 1. �÷��̾�,���� ���� ��� ���������� ���� (int ����)
        /// 2. if������ �˻� �� �ش� Ÿ�Կ� �´� ����so ��������
        /// 3. ��ųʸ��� 1������ �� ������ �ش� so�� type �˻��ϰ� ������ ����
        /// 4. ��� ����ؼ� ������ so�� i�� �Բ� gm-uc-lc ���� i��° ��ư �ʱ�ȭ
        /// 5. ��ư�� ������ �̺�Ʈ�� ��Ʈ�ѷ� Ŭ�������� ����
        ///

        int levelUpIndex = player.WeaponList.Count + 1; // ���� ����+�÷��̾� ���� ������ ����

        // ��������ŭ �ݺ�
        for (int i = 0; i < options;)
        {
            // �÷��̾�,���� �߿��� ��� ���׷��̵����� ��������
            int chose = Random.Range(0, levelUpIndex);

            if (chose == 0) // �÷��̾� ������
            {
                PlayerLevelUpData data = GetRandomData(playerDB.database);

                if (IsValidChoice(chose, data) == false)
                    continue;

                Color color = UtilitieHelper.GetGradeColor(data.ratio);

                // �÷��̾��� ������ ������ ������
                GameManager.Instance.UIController.LevelUpController.InitializeLevelUpUI(
                    color,
                    data,
                    player.SpriteRenderer.sprite,
                    i // ������ ��ġ
                );
            }
            else // ���� ������
            {
                Weapon weapon = player.WeaponList[chose - 1];

                if (weapon.WeaponLevel == Settings.weaponUpgradeLevel) // ���� ������ ���׷��̵� Ÿ�̹��̶��
                {
                    // ���׷��̵� �ؾ��ϴ� ������ �������� �ϳ��� ������
                    if (validChoice.TryGetValue(chose, out _) == false)
                    {
                        // ������ ���׷��̵� ������ ������
                        GameManager.Instance.UIController.LevelUpController.InitializeLevelUpUI(
                            weapon,
                            i
                        );

                        validChoice[chose] = new List<int>();
                    }
                    else continue;
                }
                else // ���� ������
                {
                    WeaponLevelUpData data = GetRandomData(weaponDB.database);

                    if (IsValidChoice(chose, data) == false)
                        continue;

                    Color color = UtilitieHelper.GetGradeColor(data.ratio);

                    // ������ ������ ������ ������
                    GameManager.Instance.UIController.LevelUpController.InitializeLevelUpUI(
                        color,
                        weapon, // �÷��̾� 
                        data,
                        i
                    );
                }
            }

            ++i;
        }

        validChoice.Clear();
    }

    private WeaponDetailsSO GetRandomWeapon()
    {
        // �̹� ������ ������ ID ����Ʈ �޾ƿ���
        List<int> hasWeapon = player.WeaponList.Select((weapon) => weapon.WeaponDetails.ID).ToList();
        int weaponID;

        do
        {
            weaponID = Random.Range(0, weaponDetailsDB.Count);

        } while (hasWeapon.Contains(weaponID));

        return weaponDetailsDB.GetDataByID<WeaponDetailsSO>(weaponID);
    }

    private bool IsValidChoice(int chose, ILevelUpData data)
    {
        // �̹� ���� ����Ÿ���� ���õǾ����� �˻�
        // ������ ������ �ߺ����� �ߴ� ���� ����

        if (validChoice.TryGetValue(chose, out var prevDatas))
        {
            if (prevDatas.Contains(data.GetStatType()))
                return false; 
        }
        else
            validChoice[chose] = new List<int>();

        validChoice[chose].Add(data.GetStatType());

        return true;
    }

    private T GetRandomData<T>(List<T> database) where T : ILevelUpData
    {
        // ILevelUpData�� ��ӹ��� �÷��̾�/���� ������ �����Ϳ��� �������� ��������
        // �� ������ �����ʹ� ������ Ȯ���� �ְ� Ȯ����� ����

        int totalRatio = database.Sum(x => x.GetRatio());
        int randomNumber = Random.Range(0, totalRatio);

        int ratioSum = 0;

        foreach (var data in database)
        {
            ratioSum += data.GetRatio();
            if (randomNumber < ratioSum)
                return data;
        }

        return default;
    }
}
