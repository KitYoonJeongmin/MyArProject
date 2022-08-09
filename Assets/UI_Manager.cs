using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class UI_Manager : MonoBehaviour
{
    public ARFaceManager faceManager;
    public Material[] faceMats;
    //��ư ������ �� ���� �Լ�
    public void ToggleMaskImage()
    {
        //faceManager ������Ʈ���� ���� ������ Face ������Ʈ�� ��� ��ȸ��
        foreach(ARFace face in faceManager.trackables)
        {
            //���� Face������Ʈ�� ���� �ν��ϰ� �ִ� ���¶��
            if(face.trackingState == TrackingState.Tracking)
            {
                //Face������Ʈ�� Ȱ��ȭ ���¸� �ݴ�� ����
                face.gameObject.SetActive(!face.gameObject.activeSelf);
            }
        }
    }
    
    //���͸��� ���� ��ư �Լ�
    public void SwitchFaceMaterial(int num)
    {
        foreach(ARFace face in faceManager.trackables)
        {
            //���� Face ������Ʈ�� ���� �ν��ϰ� �ִ� ���¶��
            if(face.trackingState == TrackingState.Tracking)
            {
                //Face ������Ʈ�� MeshRenderer ������Ʈ�� ������
                MeshRenderer mr = face.GetComponent<MeshRenderer>();

                //��ư�� ������ ��ȣ(�̹���: 0, ����: 1)�� �ش��ϴ� ���͸����
                //������
                mr.material = faceMats[num];
            }
        }
    }
   
}
