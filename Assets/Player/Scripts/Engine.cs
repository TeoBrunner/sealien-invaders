using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine : MonoBehaviour
{
    
    [SerializeField] float borderX;
    [SerializeField] float borderYmax;
    [SerializeField] float borderYmin;
    [SerializeField] float speed;
    [SerializeField] float tiltAngle;
    [SerializeField] ParticleSystem[] FTL;
    [SerializeField] ParticleSystem[] STL;
    private float shipX, shipY;
    private Rigidbody shipRb;


    void Start()
    {
        shipRb = GetComponent<Rigidbody>();
        SwitchToSTL();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1)) SwitchToFTL();
        //if (Input.GetKeyDown(KeyCode.Alpha2)) SwitchToSTL();
    }
    void FixedUpdate()
    {

        float speedHor = Input.GetAxis("Horizontal") * speed;
        //float speedVer = Input.GetAxis("Vertical") * speed;
        shipRb.velocity = new Vector3(speedHor, 0);
        shipX = Mathf.Clamp(transform.position.x, -borderX, borderX);
        //shipY = Mathf.Clamp(transform.position.y, borderYmin, borderYmax);
        //shipY = -8.6f;
        transform.position = new Vector3(shipX, transform.position.y);
        transform.rotation = Quaternion.AngleAxis(tiltAngle * Input.GetAxis("Horizontal"), Vector3.down);
    }


    public void SwitchToSTL()
    {
        foreach (var engine in FTL)
        {
            if (engine.isPlaying) engine.Stop();
        }
        foreach (var engine in STL)
        {
            if (engine.isStopped) engine.Play();
        }
    }
    public void SwitchToFTL()
    {
        foreach (var engine in STL)
        {
            if (engine.isPlaying) engine.Stop();
        }
        foreach (var engine in FTL)
        {
            if (engine.isStopped) engine.Play();
        }
    }
    public void Restart()
    {

    }
}
