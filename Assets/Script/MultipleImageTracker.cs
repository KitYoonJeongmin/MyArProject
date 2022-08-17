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

        //�̹��� �ν� ��������Ʈ�� ����� �Լ��� ����
        //imageManager.trackedImagesChanged += OnTrackedImage;

        StartCoroutine(TurnOnImageTracking());
    }

    IEnumerator TurnOnImageTracking()
    {
        imageManager.enabled = false;

        //��ġ ������ ���ŵ� ������ ���
        while(!GPS_Manager.instance.receiveGPS)
        {
            yield return null;
        }
        imageManager.enabled = true;

        //�̹����� �ν� ��������Ʈ�� ����� �Լ��� ����
        imageManager.trackedImagesChanged += OnTrackedImage;
    }

    public void OnTrackedImage(ARTrackedImagesChangedEventArgs args)
    {
        //���� �ν��� �̹������� ��� ��ȸ��
        foreach(ARTrackedImage trackedImage in args.added)
        {
            // ���� �ڵ� ����
            //�ڽ��� ���� ��ġ ��ǥ�� ���� ���·� ����
            Vector2 myPos = new Vector2(GPS_Manager.instance.latitude,
                GPS_Manager.instance.longitude);
            // DB �˻� �� ������ ���� �ڷ�ƾ �Լ��� ����
            StartCoroutine(DB_Manager.instance.LoadData(myPos, trackedImage.transform));
        }
        //�ν� ���� �̹������� ��� ��ȸ
        foreach(ARTrackedImage trackedImage in args.updated)
        {
            //�̹����� ��ϵ� �ڽ� ������Ʈ�� �ִٸ�...
            if(trackedImage.transform.childCount > 0)
            {
                //�ڽ� ������Ʈ�� ��ġ�� �̹����� ��ġ�� ����ȭ��
                trackedImage.transform.GetChild(0).position = trackedImage.transform.position;
                trackedImage.transform.GetChild(0).rotation = trackedImage.transform.rotation;
            }
        }
    }
}
