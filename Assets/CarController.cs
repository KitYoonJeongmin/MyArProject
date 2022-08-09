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
