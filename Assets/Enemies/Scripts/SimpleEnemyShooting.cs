using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemyShooting : MonoBehaviour
{
    public float minDelay;
    public float maxDelay;
    [SerializeField] GameObject laser;
    [SerializeField] Transform[] guns;
    [SerializeField] AudioClip pew;
    AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(StartShooting());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartShooting()
    {
        yield return new WaitForSeconds(3);
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay,maxDelay));
            if (GameManager.canInput && !GetComponent<EnemyScript>().isDead)
            {
                if(pew && audioSource) audioSource.PlayOneShot(pew);
                    foreach (var gun in guns)
                {
                    Instantiate(laser, gun.position, gun.rotation);
                }
            }

        }
        
    }
}
