using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReflectionProbeControl : MonoBehaviour
{
    public float intervlaTime = 5f;
    float timer = .0f;
    ReflectionProbe reflectionProbe;
    // Start is called before the first frame update
    void Start()
    {
        reflectionProbe = GetComponent<ReflectionProbe>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager_Ka.gameStatus == GameManager_Ka.GameStatus.cutscene3)
        {
            reflectionProbe.RenderProbe();
        }
        else if (GameManager_Ka.gameStatus == GameManager_Ka.GameStatus.battle)
        {
            timer += Time.deltaTime;
            if(timer >= intervlaTime)
            {
                timer = .0f;
                reflectionProbe.RenderProbe();
            }
        }
    }
}
