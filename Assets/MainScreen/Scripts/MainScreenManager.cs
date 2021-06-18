using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScreenManager : MonoBehaviour
{
    [SerializeField] StarsParallax parallax;
    public bool canExplodeButtons = false;
    int buttonsToActivate = 4;
    GameObject buttons;
    // Start is called before the first frame update
    void Start()
    {
        buttons = GameObject.Find("Buttons");
        StartCoroutine(LiftButtons());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void  ButtonBoomCount()
    {
        buttonsToActivate--;
        if (buttonsToActivate == 0) StartCoroutine(LoadGameScene());
    }
    IEnumerator LiftButtons()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(0.04f);
            buttons.transform.position += Vector3.up * 0.03f;
        }
        canExplodeButtons = true;
    }
     IEnumerator LoadGameScene()
    {

        parallax.allowStarRespawn = false;


        yield return new WaitForSeconds(3.5f);
        SceneManager.LoadScene("GameItself");
    }
}
