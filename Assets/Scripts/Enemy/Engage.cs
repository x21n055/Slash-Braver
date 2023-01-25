using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Area4
{
    void IEngage(int area);
}
public class Engage : MonoBehaviour
{
    public int area;
    private bool engage = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnTriggerEnter2D(Collider2D trig)
    {
        
        if (trig.gameObject.tag == "Player")
        {
            engage = true;
        }

    }

    private void OnTriggerStay2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Enemy" && engage)
        {
            Area4 engageScript = trig.gameObject.GetComponentInParent<Area4>();
            if (engageScript != null)
            {
                engageScript.IEngage(area);
            }
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
