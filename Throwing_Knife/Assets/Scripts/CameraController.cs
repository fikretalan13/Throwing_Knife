using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public List<Transform> hedefler;

    public float OffSetY;

    Vector3 gecerliHiz;

    float hareketYumusakligi=.8f;

    [SerializeField]
    Camera camera;

    float minZoom = 85f;
    float maxZoom = 70f;
    float zoomLimit = 10f;

    private void Awake()
    {
        if(instance==null)
            instance = this;

        else
        {
            Destroy(instance);
        }
    }
    private void LateUpdate()
    {
        HareketEt();
        Zoom();
    }


    void Zoom()
    {
        if (MesafeHesapla() > 5)
        {
            float yeniZoom = Mathf.Lerp(maxZoom, minZoom, MesafeHesapla() / zoomLimit);

            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, yeniZoom, Time.deltaTime * 1.5f);
        }
        else
        {
            camera.fieldOfView = Mathf.Lerp(camera.fieldOfView, maxZoom, Time.deltaTime * 1.5f);
        }
    }

    float MesafeHesapla()
    {
        var Bounds = new Bounds(hedefler[0].position, Vector3.zero);
        for (int i = 0; i < hedefler.Count; i++)
        {
            Bounds.Encapsulate(hedefler[i].position);
        }

        return Bounds.size.y;
    }
    public void HareketEt()
    {
        Vector3 yeniPozisyon = new(transform.position.x, hedefler[0].position.y + OffSetY, transform.position.z);

        transform.position = Vector3.SmoothDamp(transform.position, yeniPozisyon, ref gecerliHiz, hareketYumusakligi);
    }

}
