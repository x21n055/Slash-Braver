using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackTitle : MonoBehaviour
{
    public void BackToTitle()
    {
        SceneManager.LoadScene("Title1");
    }
    public void Return()
    {
        this.gameObject.SetActive(false);
    }
}
