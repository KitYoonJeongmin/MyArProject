using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class CarManager : MonoBehaviour
{
    public GameObject indicator;
    public GameObject MyCar;

    ARRaycastManager arManager;
    GameObject placedObject;

    // Start is called before the first frame update
    void Start()
    {
        //�ε������� ��Ȱ��ȭ ����
        indicator.SetActive(false);
        //AR Raycast Manager�� ������
        arManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectGround();
        //�ε������Ͱ� Ȱ��ȭ ���� �� ȭ���� ��ġ�ϸ� �ڵ��� �𵨸��� �����ǰ� �Ұ���
        if(indicator.activeInHierarchy && Input.touchCount > 0)
        {
            //ù ��° ��ġ ���¸� ������
            Touch touch = Input.GetTouch(0);

            //���� ��ġ�� ���۵� ���¶�� �ڵ����� �ε������Ϳ� ������ ���� ����
            if (touch.phase == TouchPhase.Began)
            {
                //���� ������ ������Ʈ�� ���ٸ� �������� ���� �����ϰ�
                //placedObject ������ �Ҵ���
                if(placedObject == null)
                {
                    Instantiate(MyCar, indicator.transform.position, indicator.transform.rotation);
                }
                //������ ������Ʈ�� �ִٸ� �� ������Ʈ�� ��ġ�� ȸ������ ������
                else
                {
                    placedObject.transform.SetPositionAndRotation(indicator.transform.position, indicator.transform.rotation);
                }
            }
                
        }
    }

    void DetectGround()
    {
        //��ũ���� �߾� ������ ã��
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        //���̿� �ε��� ������ ������ ������ ����Ʈ ������ ����
        List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();

        //���� ��ũ���� �߾� �������� ���̸� �߻����� �� Plane Ÿ�� ���� ����� �ִٸ�
        if(arManager.Raycast(screenSize, hitInfos, TrackableType.Planes))
        {
            //ǥ�� ������Ʈ�� Ȱ��ȭ ��
            indicator.SetActive(true);

            //ǥ�� ������Ʈ�� ��ġ�� ȸ������ ���̰� ���� ������ ��ġ��Ŵ
            indicator.transform.position = hitInfos[0].pose.position;
            indicator.transform.rotation = hitInfos[0].pose.rotation;

            //��ġ�� ���� �������� 0.1m�ø���.
            //transform up�� ����ϴ� ������ ������ ����� �ִ� ��츦 �����ϱ� ����
            indicator.transform.position += indicator.transform.up * 0.1f;
        }
        else
        {
            //��Ȱ��ȭ
            indicator.SetActive(false);
        }
    }
}
