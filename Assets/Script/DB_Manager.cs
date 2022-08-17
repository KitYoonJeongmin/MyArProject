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
        //DB�� URL�� ����
        FirebaseApp.DefaultInstance.Options.DatabaseUrl = new Uri(databaseUrl);

        //������ ���� �Լ��� ����
        //SaveData();
    }
    void SaveData()
    {
        //����� Ŭ���� ���� ����
        ImageGPSData data1 = new ImageGPSData("Cat", 36.14854f, 128.3934f, false);
        ImageGPSData data2 = new ImageGPSData("SCar", 36.14854f, 128.3934f, false);

        //Ŭ���� ������ Json �����ͷ� ����
        string jsonCat = JsonUtility.ToJson(data1);
        string jsonSCar = JsonUtility.ToJson(data2);

        //DB�� �ֻ�� ���͸��� ����
        DatabaseReference refData = FirebaseDatabase.DefaultInstance.RootReference;

        //�ֻ�� ���͸��� �������� ���� ���͸��� ������ json �����͸� DB�� ����
        refData.Child("Markers").Child("Data1").SetRawJsonValueAsync(jsonCat);
        refData.Child("Markers").Child("Data2").SetRawJsonValueAsync(jsonSCar);

        print("������ ���� �Ϸ�");
    }

    //�����ͺ��̽� �˻� �Լ�
    public IEnumerator LoadData(Vector2 myPos, Transform trackedImage)
    {
        //���� ���� ��ġ�� ����
        currentPos = myPos;

        //�����͸� �о���� ���� ���� ��带 ����
        DatabaseReference refData = FirebaseDatabase.DefaultInstance.GetReference("Markers");

        //DB�κ��� ������ �޾ƿ���
        isSearch = true;
        refData.GetValueAsync().ContinueWith(LoadFunc);

        //DB�κ��� �����͸� �޾ƿ��� ���ȿ��� �Լ� ������ �ߴ�
        while(isSearch)
        {
            yield return null;
        }
        //Resources �������� objectName�� �̸��� ������ �̸��� �������� ã��
        GameObject imagePrefab = Resources.Load<GameObject>(objectName);

        if(imagePrefab != null)
        {
            //�̹����� ��ϵ� �ڽ� ������Ʈ�� ���ٸ�...
            if(trackedImage.transform.childCount < 1)
            {
                //�̹��� ��ġ�� �������� �����ϰ� �̹����� �ڽ� ������Ʈ�� ���
                GameObject go = Instantiate(imagePrefab, trackedImage.position, trackedImage.rotation);
                go.transform.SetParent(trackedImage.transform);
            }
        }
    }

    //�˻��� ������ ó�� �Լ�
    void LoadFunc(Task<DataSnapshot> task)
    {
        if(task.IsFaulted)
        {
            Debug.LogError("DB���� �����͸� �������µ� �����߽��ϴ�.");
        }
        else if (task.IsCanceled)
        {
            Debug.Log("DB���� �����͸� �������� ���� ��ҵǾ����ϴ�.");
        }
        else if (task.IsCompleted)
        {
            //DB�κ��� �����͸� �����´�.
            DataSnapshot snapShot = task.Result;

            //��ü �����͸� ��ȸ
            foreach(DataSnapshot data in snapShot.Children)
            {
                //������ �����͸� json�����ͷ� ��ȯ
                string myData = data.GetRawJsonValue();

                //Json �����͸� imageGPSData ������ ����
                ImageGPSData myClassData = JsonUtility.FromJson<ImageGPSData>(myData);

                //���� ���������� ��ȹ���� �ʾҴٸ�...
                if(!myClassData.isCaptured)
                {
                    //DB �����Ϳ� ����� ��ġ�� ������� ���� ��ġ ���� �Ÿ��� ����
                    Vector2 dataPos = new Vector2(myClassData.latitude, myClassData.longitude);
                    float distance = Vector2.Distance(currentPos, dataPos);

                    //�Ÿ� ���̰� 0.001�̳���� ������ �������� �̸��� DBŰ ���� ����
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
    //��ȹ ���� �� DB ���� �Լ�
    public void UpdateCapture()
    {
        //Ű ���� ������ ��θ� ������ DB�� Ư�� ��带 ����
        string dataPath = "/Markers/" + currentKey + "/isCaptured";
        DatabaseReference refData = FirebaseDatabase.DefaultInstance.GetReference(dataPath);

        if(refData != null)
        {
            //���� ������ ����� ���� false���� true�� ����
            refData.SetValueAsync(true);
        }
    }
    //DB ����� ������ Ŭ���� 
    public class ImageGPSData
    {
        public string name;
        public float latitude;
        public float longitude;
        public bool isCaptured = false;

        //Ŭ���� ������
        public ImageGPSData(string objName, float lat, float lon, bool captured)
        {
            name = objName;
            latitude = lat;
            longitude = lon;
            isCaptured = captured;
        }
    }
}
