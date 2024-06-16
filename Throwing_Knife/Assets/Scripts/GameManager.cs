using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[Serializable]
public class Bloklar
{
    public GameObject blok;
    public GameObject bicakizi;
}
public class GameManager : MonoBehaviour
{
    public static GameManager instance;


    [SerializeField]
    GameObject[] bicaklar;

    [SerializeField]
    List<Bloklar> bloklar;


    int bicakHavuzuIndex;
    float mevcutYPozisyonu = 0.75f;

    public bool oyunBittiMi;
    bool oyunBasladiMi;

    [SerializeField]
    CameraController cameraController;

    [SerializeField]
    Top topAnaObje;


    //---------------------------------------------------------
    TimeSpan zaman;

    [SerializeField]
    TextMeshProUGUI geriSayimSuresi;

    public int sure;

    //---------------------------------------------------------
    int seriAtisSayisi;

    [SerializeField]
    TextMeshProUGUI seriAtisText;

    int yapilanSeriSayisi;
    public int bolumPuani;
    //---------------------------------------------------------
    int sahneIndex;
   

    [Header("---UI ISLEMLERI---")]
    [SerializeField]
    GameObject[] paneller;

    [Header("---SES ISLEMLERI---")]
    [SerializeField]
    AudioSource[] sesler;

    [SerializeField]
    Image[] butonGorselleri;
    [SerializeField]
    Sprite[] spriteObjeleri;


    private void Awake()
    {
        IlkSahneIslemleri();
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }


    private void Start()
    {
        bicaklar[bicakHavuzuIndex].transform.position = new Vector3(2.5f, mevcutYPozisyonu, 0f);
        bicaklar[bicakHavuzuIndex].SetActive(true);
        
        

    }

    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0) && !oyunBittiMi)
        {
            bicaklar[bicakHavuzuIndex].GetComponent<Bicak>().ileri = true;
            bicakHavuzuIndex++;
            mevcutYPozisyonu += 0.5f;

            if (bicakHavuzuIndex <= bicaklar.Length - 1)
            {
                bicaklar[bicakHavuzuIndex].transform.position = new Vector3(2.5f, mevcutYPozisyonu, 0f);
                bicaklar[bicakHavuzuIndex].SetActive(true);
            }
        }
        */

        if (Input.touchCount > 0 && Input.touchCount == 1 && !oyunBittiMi && oyunBasladiMi && !UIObjesineMi())
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                bicaklar[bicakHavuzuIndex].GetComponent<Bicak>().ileri = true;
                bicakHavuzuIndex++;
                mevcutYPozisyonu += 0.5f;
                SesCal(1);
                if (bicakHavuzuIndex <= bicaklar.Length - 1)
                {
                    bicaklar[bicakHavuzuIndex].transform.position = new Vector3(2.5f, mevcutYPozisyonu, 0f);
                    bicaklar[bicakHavuzuIndex].SetActive(true);
                }
            }

        }




    }
    public void BicakSaplandi()
    {
        SesCal(2);
        bloklar[bicakHavuzuIndex - 1].bicakizi.SetActive(true);
        cameraController.hedefler[0] = bloklar[bicakHavuzuIndex - 1].blok.transform;
        

        /* seriAtisSayisi++;
         seriAtisText.text = seriAtisSayisi.ToString();

         if (!seriAtisText.isActiveAndEnabled && !oyunBittiMi)
         {
             seriAtisText.gameObject.SetActive(true);
             yapilanSeriSayisi++;
         }

         */


        seriAtisSayisi++;
        seriAtisText.text = seriAtisSayisi.ToString();

        if (seriAtisSayisi >=3 && !seriAtisText.isActiveAndEnabled && !oyunBittiMi)
        {
            seriAtisText.gameObject.SetActive(true);
            yapilanSeriSayisi++;
        }


    }

    public void Kazandin()
    {
        SesCal(5);
        PanelAc(3);
        topAnaObje.gameObject.SetActive(false);
        StopAllCoroutines();
        geriSayimSuresi.gameObject.SetActive(false);
        seriAtisText.gameObject.SetActive(false);

        PlayerPrefs.SetInt("SonBolum", sahneIndex +1);

        if(yapilanSeriSayisi >= 3 && yapilanSeriSayisi <=7)
        {
            bolumPuani += 100;
        }

        else if (yapilanSeriSayisi >= 8)
        {
            bolumPuani += 200;
        }

        PlayerPrefs.SetInt("Puan",PlayerPrefs.GetInt("Puan") + bolumPuani);
        print("Kazanýlan Puan :" + bolumPuani);
        print("Toplam Puan :" + PlayerPrefs.GetInt("Puan"));
   
        yapilanSeriSayisi = 0;
    }

    public void Kaybettin(string durum = "TopPatladi")
    {

        if (durum == "TopPatladi")
        {
            SesCal(6);
        }
        PanelAc(4);
        oyunBittiMi = true;
        bicaklar[bicakHavuzuIndex].SetActive(false);
        geriSayimSuresi.gameObject.SetActive(false);


        seriAtisSayisi = 0;
        seriAtisText.gameObject.SetActive(false);

        StopAllCoroutines();
        if (durum == "SureBitti")
        {
            topAnaObje.gameObject.SetActive(false);
            SesCal(4);
        }

    }

    IEnumerator GeriSayim()
    {
        //   zaman = TimeSpan.FromSeconds(sure);
        geriSayimSuresi.text = sure.ToString();

        while (true)
        {
            yield return new WaitForSeconds(1f);

            if (sure != 0)
            {
                sure--;

                geriSayimSuresi.text = sure.ToString();

                if (sure == 0)
                {

                    Kaybettin("SureBitti");
                    yield break;
                }
            }
        }
    }

    public void TopCarpti()
    {
        SesCal(7);
        seriAtisSayisi = 0;
        seriAtisText.gameObject.SetActive(false);
    }

    // UI objelerine týklama

    public void ButonIslemleri(string ButonDeger)
    {
        switch (ButonDeger)
        {
            case "Durdur":
                SesCal(3);
                PanelAc(2);
                Time.timeScale = 0;
                break;


            case "DevamEt":
                SesCal(3);
                PanelKapat(2);
                Time.timeScale = 1;
                break;


            case "OyunaBasla":
                SesCal(3);
                PanelKapat(1);
                PanelAc(0);
                StartCoroutine(GeriSayim());
                oyunBasladiMi = true;
                Time.timeScale = 1;
                break;

            case "Tekrar":
                SesCal(3);
                SceneManager.LoadScene(sahneIndex);
                Time.timeScale = 1;
                break;


            case "SonrakiLevel":
                SesCal(3);
                SceneManager.LoadScene(sahneIndex+1);
                Time.timeScale = 1;
                break;


            case "Cikis":
                SesCal(3);
                PanelAc(5);
                break;



            case "Evet":
                SesCal(3);
                Application.Quit();
                break;

            case "Hayir":
                SesCal(3);
                PanelKapat(5);
                break;


            case "OyunSes":
                SesCal(3);

                if (PlayerPrefs.GetInt("OyunSes") == 0)
                {
                    PlayerPrefs.SetInt("OyunSes", 1);
                    butonGorselleri[0].sprite = spriteObjeleri[0];
                    sesler[0].mute = false;
                }

                else
                {
                    PlayerPrefs.SetInt("OyunSes", 0);
                    butonGorselleri[0].sprite = spriteObjeleri[1];
                    sesler[0].mute = true;
                }
                break;



            case "EfektSes":
                SesCal(3);

                if (PlayerPrefs.GetInt("EfektSes") == 0)
                {
                    PlayerPrefs.SetInt("EfektSes", 1);
                    butonGorselleri[1].sprite = spriteObjeleri[2];

                    for (int i = 1; i < sesler.Length; i++)
                    {
                        sesler[i].mute = false;
                    }
                }

                else
                {
                    PlayerPrefs.SetInt("EfektSes", 0);
                    butonGorselleri[1].sprite = spriteObjeleri[3];
                    for (int i = 1; i < sesler.Length; i++)
                    {
                        sesler[i].mute = true;
                    }
                }
                break;







        }
    }

    void PanelAc(int index)
    {
        paneller[index].SetActive(true);
    }

    void PanelKapat(int index)
    {
        paneller[index].SetActive(false);
    }


    //oyunu durdurunca extra olarak týklama yapmamasý için gereken kod
    bool UIObjesineMi()
    {
        if(Input.touchCount>0 && Input.touches[0].phase == TouchPhase.Began)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                return true;
        }
        return false;
    }


    public void SesCal(int index)
    {
        sesler[index].Play();
    }

    void IlkSahneIslemleri()
    {
        if (PlayerPrefs.GetInt("OyunSes") == 0)
        {
            
            butonGorselleri[0].sprite = spriteObjeleri[1];
            sesler[0].mute = true;
        }

        else
        {
           
            butonGorselleri[0].sprite = spriteObjeleri[0];
            sesler[0].mute = false;
        }

        //--------------------------------------------------------------


        if (PlayerPrefs.GetInt("EfektSes") == 0)
        {
            
            butonGorselleri[1].sprite = spriteObjeleri[3];

            for (int i = 1; i < sesler.Length; i++)
            {
                sesler[i].mute = true;
            }
        }

        else
        {
            
            butonGorselleri[1].sprite = spriteObjeleri[2];
            for (int i = 1; i < sesler.Length; i++)
            {
                sesler[i].mute = false;
            }
        }
    }
}
