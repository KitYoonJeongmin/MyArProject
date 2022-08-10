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
        //최초의 인덱스 값을 0으로 초기화 함
        indexText.text = vertNum.ToString();
    }
    public void IndexIncrease()
    {
        //vertNum의 값을 1증가시키되, 최대 인덱스를 넘지 않도록 함
        int number = Mathf.Min(++vertNum, vertCount - 1);
        indexText.text = number.ToString();
    }
    public void IndexDecrease()
    {
        //vertNum의 값을 1감소시키되, 0을 넘지 않도록 함
        int number = Mathf.Max(--vertNum, 0);
        indexText.text = number.ToString();
    }
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
