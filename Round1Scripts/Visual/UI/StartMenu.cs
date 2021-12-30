using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

//public UnityEvent StartButtonClipped;

public class StartMenu : MonoBehaviour
{
    public GameObject[] uiElements;
    AudioSource audioManager;

    //public static UnityEvent StartButtonClipped;
    public static event GameStatusEvent StartButtonClipped;

    [Range(0.0f, 5.0f)]
    public float fadeTime = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartButtonClipped()
    {
        audioManager.Play();
        StartCoroutine(FadeAway(fadeTime));
    }

    IEnumerator FadeAway(float time)
    {
        for (float alpha = 1.0f; alpha > 0;)
        {
            alpha -= Time.deltaTime / fadeTime;
            foreach (var i in GetComponentsInChildren<CanvasRenderer>())
            {
                i.SetAlpha(alpha);
            }
            yield return null;
        }

        StartButtonClipped.Invoke();

        gameObject.SetActive(false);
    }

}
