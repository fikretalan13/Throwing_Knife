using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bicak : MonoBehaviour
{
    [SerializeField]
    float donusHizi;

    [SerializeField]
    bool baslangicBicagiMi;

    //[HideInInspector]
    public bool ileri;
    bool hedefeUlasti;

    private void Update()
    {
        if (baslangicBicagiMi)
            return;


        if (!ileri)
        {
            transform.Rotate(donusHizi * Time.deltaTime * 90, 0, 0, Space.World);
            
        }
        else
        {
            if (!hedefeUlasti)
            {
                transform.Translate(30 * Time.deltaTime * Vector3.left, Space.World);
               
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (baslangicBicagiMi)
            return;

        if (other.CompareTag("Varis"))
        {
            transform.SetPositionAndRotation(other.transform.position,other.transform.rotation);
            hedefeUlasti = true;
            GameManager.instance.BicakSaplandi();
            
        }


        else if (other.CompareTag("Final"))
        {
            transform.SetPositionAndRotation(other.transform.position, other.transform.rotation);
            hedefeUlasti = true;
            GameManager.instance.BicakSaplandi();
            GameManager.instance.Kazandin();
        }
    }
}
