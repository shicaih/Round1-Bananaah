using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    private StartMenu startMenu;
    // Start is called before the first frame update
    void Start()
    {
        startMenu = transform.parent.GetComponent<StartMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        startMenu.OnStartButtonClipped();
    }
}
