using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    public GameObject menu;
    private bool nowactive = false;
    private SpriteRenderer sr = null;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            menu.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.L))
        {
            menu.SetActive(false);
        }
    }
}
