using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarsParallax : MonoBehaviour
{
    [SerializeField] int amountOfStars;
    [SerializeField] GameObject starPrefab;
    [SerializeField] float xRange = 10;
    [SerializeField] float yTop = 30;
    [SerializeField] float yBottom = -30;
    public float parallaxSpeed;
    [SerializeField] float maxStarSpeed;
    [SerializeField] float speedModifier=1;
    public bool allowStarRespawn = true;


    private GameObject[] stars;
    void Start()
    {
        for (int i = 0; i < amountOfStars; i++)
        {
            Instantiate(starPrefab, starStartPos(), Quaternion.identity, transform);
        }
        stars = GameObject.FindGameObjectsWithTag("Star");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        int _starCount = 0;
        foreach (var star in stars)
        {
            _starCount++;
            star.transform.position += new Vector3(0, -((maxStarSpeed/_starCount+1)*parallaxSpeed)*speedModifier, 0);
            if (star.transform.position.y < yBottom && allowStarRespawn) star.transform.position = starStartPos();
        }
    }

    Vector3 starStartPos()
    {
        float _x = Random.Range(-xRange, xRange);
        float _y = Random.Range(yTop, yTop*2f);
        float _z = 5;
        return new Vector3(_x, _y, _z);
    
    }

    public void Restart()
    {

    }
}
