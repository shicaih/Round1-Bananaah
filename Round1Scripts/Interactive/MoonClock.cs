using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MoonClock : MonoBehaviour
{
    public GameObject pointer;
    public float gameTime = 20.0f;
    private bool activated = false;
    private float clockTime = .0f;
    // Start is called before the first frame update
    void Start()
    {
        pointer.transform.localRotation = Quaternion.Euler(0f, -90, 0f);
        foreach(var i in GetComponentsInChildren<Renderer>())
        {
            i.enabled = false;
        }

        GameManager_Ka.BattleBegin += ActivateClock;
        GameManager_Ka.SetMoonClock += SetClock;
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            //pointer.transform.localRotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0f, -90f, 0f), new Vector3(0f, 90f, 0f), GameManager_Ka.curTime / gameTime));
            pointer.transform.localRotation = Quaternion.Euler(Vector3.Lerp(new Vector3(0f, -90f, 0f), new Vector3(0f, 90f, 0f), clockTime));
        }
    }

    void ActivateClock(float time)
    {
        gameTime = time;
        foreach (var i in GetComponentsInChildren<Renderer>())
        {
            i.enabled = true;
        }
        activated = true;
    }

    void SetClock(float t)
    {
        clockTime = t;
    }
}
