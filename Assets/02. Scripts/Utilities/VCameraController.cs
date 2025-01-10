using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class VCameraController : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;


    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    public void InitializeVCam()
    {
        vcam.Follow = GameManager.Instance.Player.transform;
    }

    public void VCamStageFinishEffect()
    {
        vcam.m_Lens.FieldOfView = 45f;

        int mode = Random.Range(0, 2);
        int z = (mode == 0) ? -25 : 25;

        vcam.transform.DORotate(new Vector3(0, 0, z), 2.5f).SetEase(Ease.OutCubic);
    }
}
