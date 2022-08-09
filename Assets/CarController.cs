using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject[] bodyObject;
    public Color32[] colors;

    Material[] carMats;
    // Start is called before the first frame update
    void Start()
    {
        //carMats �迭�� �ڵ��� body ������Ʈ�� �� ��ŭ �ʱ�ȭ ��
        carMats = new Material[bodyObject.Length];

        //�ڵ��� ���� ������Ʈ�� ���͸��� ������ carMats �迭�� ����
        for(int i=0; i< carMats.Length; i++)
        {
            carMats[i] = bodyObject[i].GetComponent<MeshRenderer>().material;
        }

        //���� �迭 0������ ���͸����� �ʱ� ������ ����
        colors[0] = carMats[0].color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeColor(int num)
    {
        //�� LCD ���͸����� ������ ��ư�� ������ �������� ����
        for(int i=0;i<carMats.Length;i++)
        {
            carMats[i].color = colors[num];
        }
    }
}
