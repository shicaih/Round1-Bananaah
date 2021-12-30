using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ImageSlider : MonoBehaviour
{
    public String[] texts;
    public Sprite[] images;
    [Range(0.0f, 10.0f)]
    public float transitionTime = 1.0f;
    public bool autoPlay = false;
    [Range(0.0f, 10.0f)]
    public float autoplayIntervalTime = 1.0f;

    public UnityEngine.UI.Image PingImageObj;
    public UnityEngine.UI.Image PongImageObj;
    public TMPro.TextMeshProUGUI textObj;

    // add by shicai
    public InputActionReference throwAxe, returnAxe, controllerVeloctiy, spaceDown;
    public bool guestCommand = false;

    private int numOfImages;
    private int curImageIndex = 0;
    private bool pingTex = true;                //Current texture is PingTex
    private float counter = .0f;                //Timer for autoplay

    public static event GameStatusEvent EndCutscene1;

    // add by caicai
    private void Awake()
    {
        throwAxe.action.canceled +=
            context =>
            {
                if (autoPlay) guestCommand = true;
            };
        returnAxe.action.canceled +=
            context =>
            {
                if (autoPlay) guestCommand = true;
            };
        spaceDown.action.canceled += context =>
        {

            if (autoPlay) {
                Debug.Log("SpacePressed");
                guestCommand = true;
            }
            
        };
    }

    // Start is called before the first frame update
    void Start()
    {
        numOfImages = images.Length;
        curImageIndex = 0;
        //MoveToNextImage();
        textObj.text = texts[0];
        GameManager_Ka.StartCutscene1 += StartPlay;

        PongImageObj.color = new Color(1, 1, 1, 0);
        PingImageObj.color = new Color(1, 1, 1, 0);
        textObj.alpha = .0f;
        GameManager_Ka.StartCutscene += StartCutscene;

        curEndIndex = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (controllerVeloctiy.action.ReadValue<Vector3>().magnitude > 50) guestCommand = true;
        
        if (autoPlay)
            {
                if (counter > autoplayIntervalTime)
                {
                    MoveToNextImage();
                    counter = .0f;
                }
                counter += Time.deltaTime;
                // add by caicai
                
            }
    }

    public int curStartIndex = 0;
    // decide when to stop
    public int curEndIndex = 1;

    public void StartCutscene(int begin, int end)
    {
        curStartIndex = begin;
        curEndIndex = end;
        NextImage();
        StartCoroutine(Reappear(0.5f));
    }

    // the main process of cutscene playing
    public void MoveToNextImage()
    {
        if ((curImageIndex < numOfImages - 1) && (curImageIndex < curEndIndex))
        {
            if (pingTex)
            {
                PongImageObj.sprite = images[++curImageIndex];
                pingTex = !pingTex;
                StartCoroutine(transferImage(transitionTime));
            }
            else
            {
                PingImageObj.sprite = images[++curImageIndex];
                pingTex = !pingTex;
                StartCoroutine(transferImage(transitionTime));
            }
            guestCommand = false;
            textObj.text = texts[curImageIndex];
        }
        else
        {
            autoPlay = false;
            guestCommand = false;
            StartCoroutine(FadeAway(transitionTime));
        }
    }

    // change the sprite image of the UI
    public void NextImage()
    {
        if ((curImageIndex < numOfImages - 1) && (curImageIndex < curEndIndex))
        {
            if (pingTex)
            {
                PongImageObj.sprite = images[++curImageIndex];
                PingImageObj.sprite = images[curImageIndex];
            }
            else
            {
                PingImageObj.sprite = images[++curImageIndex];
                PongImageObj.sprite = images[curImageIndex];
            }

            textObj.text = texts[curImageIndex];
        }
    }

    // start playing the first of the slides
    void StartPlay()
    {
        autoPlay = true;
        textObj.alpha = 1.0f;
        PongImageObj.color = new Color(1, 1, 1, 1);
        PingImageObj.color = new Color(1, 1, 1, 1);
    }

    // transision between 2 ping texture and pong texture
    IEnumerator transferImage(float time)
    {
        if ((curImageIndex >= numOfImages - 1) || (curImageIndex >= curEndIndex))
        {
            autoPlay = false;

            UnityEngine.UI.Image previousTex = pingTex ? PongImageObj : PingImageObj;
            UnityEngine.UI.Image currentTex = pingTex ? PingImageObj : PongImageObj;

            for (float i = .0f; i < time;)
            {
                float previousAlpha = 1.0f - (i / time);
                float currentAlpha = (i / time);
                previousTex.color = new Color(previousTex.color.r, previousTex.color.g, previousTex.color.b, previousAlpha);
                currentTex.color = new Color(previousTex.color.r, previousTex.color.g, previousTex.color.b, 1);
                i += Time.deltaTime;

                if (pingTex)
                {
                    PingImageObj = currentTex;
                    PongImageObj = previousTex;
                }
                else
                {
                    PingImageObj = previousTex;
                    PongImageObj = currentTex;
                }

                yield return null;
            }

            StartCoroutine(FadeAway(time));
        }
        else
        {
            UnityEngine.UI.Image previousTex = pingTex ? PongImageObj : PingImageObj;
            UnityEngine.UI.Image currentTex = pingTex ? PingImageObj : PongImageObj;

            for (float i = .0f; i < time;)
            {
                float previousAlpha = 1.0f - (i / time);
                float currentAlpha = (i / time);
                previousTex.color = new Color(previousTex.color.r, previousTex.color.g, previousTex.color.b, previousAlpha);
                currentTex.color = new Color(currentTex.color.r, currentTex.color.g, currentTex.color.b, 1);
                i += Time.deltaTime;

                if (pingTex)
                {
                    PingImageObj = currentTex;
                    PongImageObj = previousTex;
                }
                else
                {
                    PingImageObj = previousTex;
                    PongImageObj = currentTex;
                }

                yield return null;
            }

        }
    }

    // All cutsceneUI fade away and start monster spwaning
    IEnumerator FadeAway(float time)
    {
        for (float alpha = 1.0f; alpha > 0;)
        {
            //time = 5.0f;
            alpha -= Time.deltaTime / time;
            foreach (var i in GetComponentsInChildren<CanvasRenderer>())
            {
                i.SetAlpha(alpha);
            }
            yield return null;
        }

        EndCutscene1.Invoke();
    }

    // change all the ping pong texture from alpha 0 - alpha 1
    IEnumerator Reappear(float time)
    {
        for (float alpha = 1.0f; alpha > 0;)
        {
            alpha -= Time.deltaTime / time;
            foreach (var i in GetComponentsInChildren<CanvasRenderer>())
            {
                i.SetAlpha(1.0f - alpha);
            }
            yield return null;
        }
        autoPlay = true;
    }

}
