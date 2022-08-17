using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using System;
using System.Threading.Tasks;

public class DB_Manager : MonoBehaviour
{
    public string databaseUrl =
        "https://myarproject-3d6d5-default-rtdb.asia-southeast1.firebasedatabase.app/";
    public static DB_Manager instance;

    Vector2 currentPos;
    string objectName = "";
    string currentKey = "";
    bool isSearch = false;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        //DB의 URL을 설정
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri(databaseUrl);

        //데이터 저장 함수를 실행
        //SaveData();
    }
    void SaveData()
    {
        //저장용 클래스 변수 생성
        ImageGPSData data1 = new ImageGPSData("Cat", 36.14854f, 128.3934f, false);
        ImageGPSData data2 = new ImageGPSData("SCar", 36.14854f, 128.3934f, false);

        //클래스 변수를 Json 데이터로 변경
        string jsonCat = JsonUtility.ToJson(data1);
        string jsonSCar = JsonUtility.ToJson(data2);

        //DB의 최상단 디렉터리를 참조
        DatabaseReference refData = FirebaseDatabase.DefaultInstance.RootReference;

        //최상단 디렉터리를 기준으로 하위 디렉터리를 지정해 json 데이터를 DB에 저장
        refData.Child("Markers").Child("Data1").SetRawJsonValueAsync(jsonCat);
        refData.Child("Markers").Child("Data2").SetRawJsonValueAsync(jsonSCar);

        print("데이터 저장 완료");
    }

    //데이터베이스 검색 함수
    public IEnumerator LoadData(Vector2 myPos, Transform trackedImage)
    {
        //나의 현재 위치를 저장
        currentPos = myPos;

        //데이터를 읽어오기 위한 기준 노드를 설정
        DatabaseReference refData = FirebaseDatabase.DefaultInstance.GetReference("Markers");

        //DB로부터 데이터 받아오기
        isSearch = true;
        refData.GetValueAsync().ContinueWith(LoadFunc);

        //DB로부터 데이터를 받아오는 동안에는 함수 실행을 중단
        while(isSearch)
        {
            yield return null;
        }
        //Resources 폴더에서 objectName의 이름과 동일한 이름의 프리팹을 찾음
        GameObject imagePrefab = Resources.Load<GameObject>(objectName);

        if(imagePrefab != null)
        {
            //이미지에 등록된 자식 오브젝트가 없다면...
            if(trackedImage.transform.childCount < 1)
            {
                //이미지 위치에 프리팹을 생성하고 이미지의 자식 오브젝트로 등록
                GameObject go = Instantiate(imagePrefab, trackedImage.position, trackedImage.rotation);
                go.transform.SetParent(trackedImage.transform);
            }
        }
    }

    //검색된 데이터 처리 함수
    void LoadFunc(Task<DataSnapshot> task)
    {
        if(task.IsFaulted)
        {
            Debug.LogError("DB에서 데이터를 가져오는데 실패했습니다.");
        }
        else if (task.IsCanceled)
        {
            Debug.Log("DB에서 데이터를 가져오는 것이 취소되었습니다.");
        }
        else if (task.IsCompleted)
        {
            //DB로부터 데이터를 가져온다.
            DataSnapshot snapShot = task.Result;

            //전체 데이터를 순회
            foreach(DataSnapshot data in snapShot.Children)
            {
                //스냅샷 데이터를 json데이터로 변환
                string myData = data.GetRawJsonValue();

                //Json 데이터를 imageGPSData 변수로 저장
                ImageGPSData myClassData = JsonUtility.FromJson<ImageGPSData>(myData);

                //만일 누군가에게 포획되지 않았다면...
                if(!myClassData.isCaptured)
                {
                    //DB 데이터에 저장된 위치와 사용자의 현재 위치 간의 거리를 측정
                    Vector2 dataPos = new Vector2(myClassData.latitude, myClassData.longitude);
                    float distance = Vector2.Distance(currentPos, dataPos);

                    //거리 차이가 0.001이내라면 생성할 프리팹의 이름과 DB키 값을 저장
                    if(distance < 0.001f)
                    {
                        objectName = myClassData.name;
                        currentKey = data.Key;
                    }
                }
            }
        }
        isSearch = false;
    }
    //포획 성공 시 DB 갱신 함수
    public void UpdateCapture()
    {
        //키 값을 조합한 경로를 설정해 DB의 특정 노드를 선택
        string dataPath = "/Markers/" + currentKey + "/isCaptured";
        DatabaseReference refData = FirebaseDatabase.DefaultInstance.GetReference(dataPath);

        if(refData != null)
        {
            //현재 지정된 노드의 값을 false에서 true로 변경
            refData.SetValueAsync(true);
        }
    }
    //DB 저장용 데이터 클래스 
    public class ImageGPSData
    {
        public string name;
        public float latitude;
        public float longitude;
        public bool isCaptured = false;

        //클래스 생성자
        public ImageGPSData(string objName, float lat, float lon, bool captured)
        {
            name = objName;
            latitude = lat;
            longitude = lon;
            isCaptured = captured;
        }
    }
}
