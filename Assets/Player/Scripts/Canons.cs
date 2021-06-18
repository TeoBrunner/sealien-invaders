using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canons : MonoBehaviour
{
    [SerializeField] GameObject[] guns;
    [SerializeField] GameObject laserPrefab;
    [SerializeField] AudioClip pew;
    [SerializeField] float cooldownTime;
    private AudioSource audioSource;
    public bool isReloaded = true;
    GameManager gameManager;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        gameManager = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        TryShoot();
    }

    void TryShoot()
    {

        if (Input.GetButton("Fire") && isReloaded && GameManager.canInput)
        {
            foreach (var gun in guns)
            {
                Instantiate(laserPrefab, gun.transform.position, Quaternion.identity);
            }
            audioSource.PlayOneShot(pew);
            StartCoroutine(Cooldown(cooldownTime));
        }

    }
    IEnumerator Cooldown(float timer)
    {
        isReloaded = false;
        yield return new WaitForSeconds(timer);
        isReloaded = true;
    }
    public void Restart()
    {

    }
}
