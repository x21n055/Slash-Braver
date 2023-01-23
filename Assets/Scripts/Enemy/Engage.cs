using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Area4
{
    void IEngage();
}
public class Engage : MonoBehaviour
{
    public int area;
    private bool engage;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter2D(Collider2D trig)
    {
        
        if (trig.gameObject.tag == "Player")
        {
            engage = true;
            Debug.Log("プレイヤーを検知");
        }

    }

    private void OnTriggerStay2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Enemy")
        {
            Area4 engageScript = trig.gameObject.GetComponentInParent<Area4>();
            Debug.Log(engageScript);
            if (engageScript != null && engage)
            {
                engageScript.IEngage();
                Debug.Log("エンゲージ関数を呼び出す");
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
