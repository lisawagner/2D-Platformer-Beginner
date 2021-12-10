using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawController : MonoBehaviour
{
    [SerializeField] AudioSource sawSound;
    private Collider2D sawHitBox;
    [SerializeField] GameObject bloodSpray;

    void Start()
    {
        sawSound = GetComponent<AudioSource>();
        sawHitBox = GetComponent<Collider2D>();
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("Hit Player");
            HitYourHead();
        }
    }
    private void HitYourHead()
    {
        sawSound.Play();
        //GameObject.Instantiate(bloodSpray, Vector3.zero, Quaternion.identity);
        GameObject.Instantiate(bloodSpray, transform.position, Quaternion.identity);

        Debug.Log("play blood particle");
        //Instantiate(bloodSpray, transform.position, Quaternion.identity);
        //spray blood
        //Instantiate(bloodSpray);
    }

}
