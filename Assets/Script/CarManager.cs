using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;

public class CarManager : MonoBehaviour
{
    public GameObject indicator;
    public GameObject MyCar;
    public float relocationDistance = 1.0f;

    ARRaycastManager arManager;
    GameObject placedObject;

    // Start is called before the first frame update
    void Start()
    {
        //인디케이터 비활성화 해줌
        indicator.SetActive(false);
        //AR Raycast Manager를 가져옴
        arManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        DetectGround();
        //인디케이터가 활성화 중일 때 화면을 터치하면 자동차 모델링이 생성되게 할것임
        //만약, 현재 클릭 or 터치한 오브젝트가 UI 오브젝트라면 Update함수를 종료
        if(EventSystem.current.currentSelectedGameObject)
        {
            return;
        }
        //인디케이터가 활성화 중이면서 화면 터치가 있는 경우
        if(indicator.activeInHierarchy && Input.touchCount > 0)
        {
            //첫 번째 터치 상태를 가져옴
            Touch touch = Input.GetTouch(0);

            //만일 터치가 시작된 상태라면 자동차를 인디케이터와 동일한 곳에 생성
            if (touch.phase == TouchPhase.Began)
            {
                //만일 생성된 오브젝트가 없다면 프리팹을 씬에 생성하고
                //placedObject 변수에 할당함
                if(placedObject == null)
                {
                    placedObject = Instantiate(MyCar, indicator.transform.position, 
                        indicator.transform.rotation);
                }
                //생성된 오브젝트가 있다면 그 오브젝트의 위치와 회전값을 변경함
                else
                {
                    if(Vector3.Distance(placedObject.transform.position,
                        indicator.transform.position)>relocationDistance)
                    {
                        placedObject.transform.SetPositionAndRotation
                            (indicator.transform.position, indicator.transform.rotation);
                    }
                        
                }
            }
                
        }
    }

    void DetectGround()
    {
        //스크린의 중앙 지점을 찾음
        Vector2 screenSize = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        //레이에 부딪힌 대상들의 정보를 저장할 리스트 변수를 만듦
        List<ARRaycastHit> hitInfos = new List<ARRaycastHit>();

        //만약 스크린의 중앙 지점에서 레이를 발사했을 때 Plane 타입 추적 대상이 있다면
        if(arManager.Raycast(screenSize, hitInfos, TrackableType.Planes))
        {
            //표식 오브젝트를 활성화 함
            indicator.SetActive(true);

            //표식 오브젝트의 위치및 회전값을 레이가 닿은 지점에 일치시킴
            indicator.transform.position = hitInfos[0].pose.position;
            indicator.transform.rotation = hitInfos[0].pose.rotation;

            //위치를 위쪽 방향으로 0.1m올린다.
            //transform up을 사용하는 이유는 기울어진 평면이 있는 경우를 생각하기 때문
            indicator.transform.position += indicator.transform.up * 0.1f;
        }
        else
        {
            //비활성화
            indicator.SetActive(false);
        }
    }
}
