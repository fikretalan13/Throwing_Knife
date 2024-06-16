using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Top : MonoBehaviour
{
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    GameObject[] topObjeleri;

    bool topPatladi;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BicakSapi") && !topPatladi)
        {
            rb.velocity = Vector3.zero;
            rb.AddForce(0, Random.Range(1,3), 0, ForceMode.Impulse);
            GameManager.instance.TopCarpti();
        }

        else if (other.CompareTag("BicakUcu") && !topPatladi)
        {
            topPatladi = true;
            GameManager.instance.Kaybettin();
            rb.isKinematic = true;
            rb.constraints = RigidbodyConstraints.FreezePositionZ;

            topObjeleri[0].SetActive(false);
            topObjeleri[1].SetActive(true);

            transform.position = new Vector3(.7f, other.gameObject.transform.position.y, 0);

        }


    }
}
