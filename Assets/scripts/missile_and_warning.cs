using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile_and_warning : MonoBehaviour
{
    private GameObject warning;
    private GameObject missile;
    public float warningtime = 2.0f;
    public float missiletime = 3.0f;
    float warningtimer;
    float missiletimer;

    private int missileout;
    private int warningout;

    //delayshenanigans
    public float delaytime = 2.0f;
    float delaytimer;
    private int delaying;

    // Start is called before the first frame update
    void Start()
    {
        warning = GameObject.FindGameObjectWithTag("warning");
        missile = GameObject.FindGameObjectWithTag("missile");

        warningtimer = warningtime;
        missiletimer = missiletime;
        delaytimer = delaytime;

        missile.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (missileout == 0 && delaying == 0)
        {
            warning.SetActive(true);
            missile.SetActive(false);
            warningtimer -= Time.deltaTime;
            missiletimer = missiletime;
        }

        if (warningtimer < 0)
        {
            missileout = 1;
            warningtimer = warningtime;
        }

        if (missileout == 1)
        {
            missile.SetActive(true);
            warning.SetActive(false);
            missiletimer -= Time.deltaTime;
        }

        if (missiletimer < 0)
        {
            delaytimer -= Time.deltaTime;
            delaying = 1;
            missile.SetActive(false);
        }

        if (delaytimer < 0 && missileout == 1)
        {
            missileout = 0;
            delaytimer = delaytime;
            delaying = 0;
        }

    }
}
