using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    public GameObject babyIdling;
    private Animator ani;


    public static event GameStatusEvent FinishBabyAnimation;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        GameManager_Ka.BabyAnimation += PlayAnimation;
        foreach (var i in GetComponentsInChildren<Renderer>())
        {
            i.enabled = false;
        }
        //PlayAnimation();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlayAnimation()
    {
        //GetComponent<AudioSource>().Play();
        ani.Play("BabyAni_01", 0, 0.0f);
        foreach (var i in GetComponentsInChildren<Renderer>())
        {
            i.enabled = true;
        }
        GetComponent<AudioSource>().Play();

        StartCoroutine(WaitForAni());
    }

    IEnumerator WaitForAni()
    {
        yield return new WaitForSeconds(2.5f);
        FinishAnimation();
    }

    void FinishAnimation()
    {
        gameObject.SetActive(false);
        babyIdling.SetActive(true);
        FinishBabyAnimation.Invoke();
        //explosionParticle.SetActive(true);
        //fireParticle.SetActive(true);
        //crushedPlane.SetActive(true);
        //foreach (var i in GetComponentsInChildren<Renderer>())
        //{
        //    i.enabled = false;
        //}

        //crushedPlane.SetActive(true);
    }
}