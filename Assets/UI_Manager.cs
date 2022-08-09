using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class UI_Manager : MonoBehaviour
{
    public ARFaceManager faceManager;
    public Material[] faceMats;
    //버튼 눌렀을 때 실행 함수
    public void ToggleMaskImage()
    {
        //faceManager 컴포넌트에서 현재 생성된 Face 오브젝트를 모두 순회함
        foreach(ARFace face in faceManager.trackables)
        {
            //만약 Face오브젝트가 얼굴을 인식하고 있는 상태라면
            if(face.trackingState == TrackingState.Tracking)
            {
                //Face오브젝트의 활성화 상태를 반대로 변경
                face.gameObject.SetActive(!face.gameObject.activeSelf);
            }
        }
    }
    
    //매터리얼 변경 버튼 함수
    public void SwitchFaceMaterial(int num)
    {
        foreach(ARFace face in faceManager.trackables)
        {
            //만일 Face 오브젝트가 얼굴을 인식하고 있는 상태라면
            if(face.trackingState == TrackingState.Tracking)
            {
                //Face 오브젝트의 MeshRenderer 컴포넌트에 접근함
                MeshRenderer mr = face.GetComponent<MeshRenderer>();

                //버튼에 설정된 번호(이미지: 0, 영상: 1)에 해당하는 매터리얼로
                //변경함
                mr.material = faceMats[num];
            }
        }
    }
   
}
