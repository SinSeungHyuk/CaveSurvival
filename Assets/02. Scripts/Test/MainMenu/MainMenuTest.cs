using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTest : MonoBehaviour
{
    public int meleeRange;


    private void OnDrawGizmos()
    {
        // Gizmo ���� ���� (�ɼ�)
        Gizmos.color = Color.red;

        // ��ü �׸���
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
