using UnityEngine;
using UnityEngine.SceneManagement;

public class AnaMenu : MonoBehaviour
{
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("SonBolum"))
        {
            PlayerPrefs.SetInt("SonBolum",1);
            PlayerPrefs.SetInt("Puan", 0);
            PlayerPrefs.SetInt("OyunSes", 1);
            PlayerPrefs.SetInt("EfektSes", 1);
        }

        SceneManager.LoadScene(PlayerPrefs.GetInt("SonBolum"));
    }

}
