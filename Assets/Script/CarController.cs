using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public GameObject[] bodyObject;
    public Color32[] colors;
    public float rotSpeed = 0.1f;
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
        if(Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);

            //����, ��ġ ���°� �����̰� �ִ� ���̶��
            if(touch.phase == TouchPhase.Moved)
            {
                //����, ī�޶� ��ġ���� ���� �������� ���̸� �߻��� �ε��� �����
                //8�� ���̾��� ��ġ �̵����� ����
                Ray ray = new Ray(Camera.main.transform.position,
                    Camera.main.transform.forward);
                RaycastHit hitInfo;

                if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1<<8))
                {
                    Vector3 deltaPos = touch.deltaPosition;

                    //���� �����ӿ��� ���� �����ӱ����� X�� ��ġ �̵����� �����
                    //���� Y�� �������� ȸ��
                    transform.Rotate(transform.up, deltaPos.x * -1.0f *rotSpeed);
                }
            }
        }
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
