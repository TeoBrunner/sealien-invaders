using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    [SerializeField] int scoresReward;
    public int health = 1;
    [SerializeField] float dyingTime = 0.8f;
    [SerializeField] GameObject explosion;
    [SerializeField] AudioClip explosionSound;
    //public int listIndex;
    //public int rowNumber;
    [SerializeField] GameObject[] frames;
    private int activeFrame = 0;
    AudioSource audioSource;
    GameManager gameManager;
    public bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(DemoAnim());
        if(GameObject.FindGameObjectWithTag("Player"))gameManager = GameObject.FindGameObjectWithTag("Player").GetComponent<GameManager>();
        GameManager.UpdateTimeScale();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerLaser"))
        {
            GameManager.UpdateTimeScale();
            Destroy(other.gameObject);            
            Damage(1);

        }

        if (other.CompareTag("DeathZone"))
        {
            GameObject.Find("Game Manager").GetComponent<GameManager>().GameOver();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameObject.Find("Game Manager").GetComponent<GameManager>().GameOver();
        }
    }

    void Die()
    {
        isDead = true;
        if (explosionSound && audioSource) audioSource.PlayOneShot(explosionSound);
       // GameManager.UpdateScore(scoresReward);
       if (explosion) Instantiate(explosion,transform.position,Quaternion.identity);
        //transform.position += Vector3.back * 100;
        //gameManager.DecreaseEnemiesCount(1);
        GetComponent<Collider>().enabled = false;
        StopCoroutine(DemoAnim());
        foreach (var frame in frames)
        {
            Destroy(frame);
        }
        Destroy(gameObject,dyingTime);

    }

    public void NextFrame()
    {
        if (frames[activeFrame]) frames[activeFrame].SetActive(false);
        activeFrame++;
        if (activeFrame == frames.Length) activeFrame = 0;
        if(frames[activeFrame]) frames[activeFrame].SetActive(true);
    }
    void Damage(int _dmg)
    {
        health -= _dmg;
        if (health < 1) Die();
    }
     IEnumerator DemoAnim()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);
            NextFrame();
        }

    }
}
