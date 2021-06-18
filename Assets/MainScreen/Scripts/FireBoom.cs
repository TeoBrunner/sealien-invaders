using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBoom : MonoBehaviour
{
    [SerializeField] GameObject body;
    [SerializeField] ParticleSystem explosion;
    [SerializeField] AudioClip explosionSound;
    AudioSource audioSource;
    bool activated;
    MainScreenManager mainScreenManager;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        mainScreenManager = GameObject.Find("Manager").GetComponent<MainScreenManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !activated && mainScreenManager.canExplodeButtons)
        {
            audioSource.PlayOneShot(explosionSound);
            body.SetActive(false);
            Instantiate(explosion, transform.position, Quaternion.identity);
            //explosion.Play();
            activated = true;
            mainScreenManager.ButtonBoomCount();
        }   
    }
}
