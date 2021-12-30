using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plane : MonoBehaviour
{
    private Animator ani;
    public GameObject explosionParticle;
    public GameObject fireParticle;
    public GameObject crushedPlane;

    public static event GameStatusEvent FinishPlaneAnimation;
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
        GameManager_Ka.PlaneAnimation += PlayAnimation;
        //PlayAnimation();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void PlayAnimation()
    {
        GetComponent<AudioSource>().Play();
        ani.Play("PlaneCrush_Animation_Final", 0, 0.0f);
    }

    void FinishAnimation()
    {
        FinishPlaneAnimation.Invoke();
        explosionParticle.SetActive(true);
        fireParticle.SetActive(true);
        crushedPlane.SetActive(true);
        foreach(var i in GetComponentsInChildren<Renderer>())
        {
            i.enabled = false;
        }

        crushedPlane.SetActive(true);
    }
}
