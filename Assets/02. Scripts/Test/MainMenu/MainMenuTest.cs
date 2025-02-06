using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTest : MonoBehaviour
{
    public int meleeRange;


    private void OnDrawGizmos()
    {
        // Gizmo 색상 설정 (옵션)
        Gizmos.color = Color.red;

        // 구체 그리기
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
