using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAni : MonoBehaviour
{
    private Animator ani;
    bool rised = false;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        GameManager_Ka.BabyAnimation += PlayAnimation;
        GameManager_Ka.GameEnd += PlaySunRiseAnimation;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayAnimation()
    {
        ani.Play("DirLight_Ani_01", 0, 0.0f);
    }

    void PlaySunRiseAnimation()
    {
        if (!rised)
        {
            ani.Play("SunRise_01_Ani", 0, 0.0f);
            rised = true;
        }
    }
}
