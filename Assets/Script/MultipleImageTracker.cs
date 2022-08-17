using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class MultipleImageTracker : MonoBehaviour
{
    ARTrackedImageManager imageManager;

    // Start is called before the first frame update
    void Start()
    {
        imageManager = GetComponent<ARTrackedImageManager>();

        //이미지 인식 델리게이트에 실행될 함수를 연결
        //imageManager.trackedImagesChanged += OnTrackedImage;

        StartCoroutine(TurnOnImageTracking());
    }

    IEnumerator TurnOnImageTracking()
    {
        imageManager.enabled = false;

        //위치 정보가 수신될 때까지 대기
        while(!GPS_Manager.instance.receiveGPS)
        {
            yield return null;
        }
        imageManager.enabled = true;

        //이미지가 인식 델리게이트에 실행될 함수를 연결
        imageManager.trackedImagesChanged += OnTrackedImage;
    }

    public void OnTrackedImage(ARTrackedImagesChangedEventArgs args)
    {
        //새로 인식한 이미지들을 모두 순회함
        foreach(ARTrackedImage trackedImage in args.added)
        {
            // 기존 코드 삭제
            //자신의 현재 위치 좌표를 백터 형태로 변경
            Vector2 myPos = new Vector2(GPS_Manager.instance.latitude,
                GPS_Manager.instance.longitude);
            // DB 검색 및 프리팹 생성 코루틴 함수를 실행
            StartCoroutine(DB_Manager.instance.LoadData(myPos, trackedImage.transform));
        }
        //인식 중인 이미지들을 모두 순회
        foreach(ARTrackedImage trackedImage in args.updated)
        {
            //이미지에 등록된 자식 오브젝트가 있다면...
            if(trackedImage.transform.childCount > 0)
            {
                //자식 오브젝트의 위치를 이미지의 위치와 동기화함
                trackedImage.transform.GetChild(0).position = trackedImage.transform.position;
                trackedImage.transform.GetChild(0).rotation = trackedImage.transform.rotation;
            }
        }
    }
}
