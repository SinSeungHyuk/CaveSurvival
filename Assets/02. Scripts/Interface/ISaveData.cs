using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���̺��ؾ��ϴ� Ŭ������ ���. ����ü Ÿ�����θ� ���̺�
public interface ISaveData
{
    void Register();
    void ToSaveData();
    void FromSaveData(SaveData saveData);
}
