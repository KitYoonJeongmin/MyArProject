using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public ARFaceManager faceManager;
    public Material[] faceMats;
    public Text indexText;

    int vertNum = 0;
    int vertCount = 468;

    private void Start()
    {
        //������ �ε��� ���� 0���� �ʱ�ȭ ��
        indexText.text = vertNum.ToString();
    }
    public void IndexIncrease()
    {
        //vertNum�� ���� 1������Ű��, �ִ� �ε����� ���� �ʵ��� ��
        int number = Mathf.Min(++vertNum, vertCount - 1);
        indexText.text = number.ToString();
    }
    public void IndexDecrease()
    {
        //vertNum�� ���� 1���ҽ�Ű��, 0�� ���� �ʵ��� ��
        int number = Mathf.Max(--vertNum, 0);
        indexText.text = number.ToString();
    }
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
