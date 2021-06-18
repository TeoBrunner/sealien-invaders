using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayedSelfDestroy : MonoBehaviour
{
    [SerializeField] float timer;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, timer);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
