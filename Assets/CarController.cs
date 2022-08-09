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
        //carMats 배열을 자동차 body 오브젝트의 수 만큼 초기화 함
        carMats = new Material[bodyObject.Length];

        //자동차 보디 오브젝트의 메터리얼 각각을 carMats 배열에 지정
        for(int i=0; i< carMats.Length; i++)
        {
            carMats[i] = bodyObject[i].GetComponent<MeshRenderer>().material;
        }

        //색상 배열 0번에는 매터리얼의 초기 색상을 지정
        colors[0] = carMats[0].color;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.touchCount>0)
        {
            Touch touch = Input.GetTouch(0);

            //만일, 터치 상태가 움직이고 있는 중이라면
            if(touch.phase == TouchPhase.Moved)
            {
                //만일, 카메라 위치에서 정면 방향으로 레이를 발사해 부딪힌 대상이
                //8번 레이어라면 터치 이동량을 구함
                Ray ray = new Ray(Camera.main.transform.position,
                    Camera.main.transform.forward);
                RaycastHit hitInfo;

                if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1<<8))
                {
                    Vector3 deltaPos = touch.deltaPosition;

                    //직전 프레임에서 현재 프레임까지의 X축 터치 이동량에 비례해
                    //로컬 Y축 방향으로 회전
                    transform.Rotate(transform.up, deltaPos.x * -1.0f *rotSpeed);
                }
            }
        }
    }
    public void ChangeColor(int num)
    {
        //각 LCD 메터리얼의 색상을 버튼에 지정된 색상으로 변경
        for(int i=0;i<carMats.Length;i++)
        {
            carMats[i].color = colors[num];
        }
    }
}
