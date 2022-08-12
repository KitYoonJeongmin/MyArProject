using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float resetTime = 3.0f;

    Rigidbody rb;
    bool isReady = true;
    Vector2 startPos;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isReady)
        {
            return;
        }
        //���� ī�޶� ���� �ϴܿ� ��ġ��
        SetBallPosition(Camera.main.transform);

        if(Input.touchCount> 0 & isReady)
        {
            Touch touch = Input.GetTouch(0);

            //����, ��ġ�� �����ߴٸ�...
            if(touch.phase == TouchPhase.Began)
            {
                //��ġ�� ������ �ȼ��� ����
                startPos = touch.position;
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                //�հ����� �巡���� �ȼ��� Y�� �Ÿ��� ����
                float dragDistance = touch.position.y - startPos.y;

                //AR ī�޶� �������� ���� ����(���� 45�� ����)�� ����
                Vector3 throwAngle = (Camera.main.transform.forward
                    + Camera.main.transform.up).normalized;
                //���� �ɷ��� Ȱ��ȭ�ϰ� �غ� ���¸� false�� �ٲ� ����
                rb.isKinematic = false;
                isReady = false;

                //���� ����* �հ��� �巡�� �Ÿ���ŭ ���� ������ ���� ���Ѵ�.
                rb.AddForce(throwAngle * dragDistance * 0.005f, ForceMode.VelocityChange);

                //3�� �Ŀ� ���� ��ġ �� �ӵ��� �ʱ�ȭ ��
                Invoke("ResetBall", resetTime);
            }
        }

    }
    void SetBallPosition(Transform anchor)
    {
        //ī�޶��� ��ġ���� ���� �Ÿ���ŭ ������ Ư�� ��ġ�� ����
        Vector3 offset = anchor.forward * 0.5f + anchor.up * -0.2f;

        //���� ��ġ�� ī�޶� ��ġ���� Ư�� ��ġ��ŭ �̵��� �Ÿ��� ����
        transform.position = anchor.position + offset;
    }
    private void ResetBall()
    {
        //���� �ɷ��� ��Ȱ��ȭ�ϰ� �ӵ��� �ʱ�ȭ ��
        rb.isKinematic = true;
        rb.velocity = Vector3.zero;

        //�غ���·� ����
        isReady = true;
    }
}
