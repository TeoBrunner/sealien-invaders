using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] GameObject explosion;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.localPosition += transform.up * speed;
        if (Mathf.Abs(transform.position.y) > 16) Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.CompareTag("EnemyLaser") && other.CompareTag("Player"))
        {
            GameObject.Find("Game Manager").GetComponent<GameManager>().DamagePlayer(1);
        }
        if (CompareTag("PlayerLaser") && other.CompareTag("Enemy"))
        {
            if(other.GetComponent<EnemyScript>().health > 1) Instantiate(explosion, transform.position, Quaternion.identity);
        }
    }

}
