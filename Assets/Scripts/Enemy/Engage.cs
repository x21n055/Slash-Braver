using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface Area
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
            //Debug.Log("ÉvÉåÉCÉÑÅ[î≠å©");
            engage = true;
        }

    }

    private void OnTriggerStay2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Enemy")
        {
            //Debug.Log("ìGÇ™ë∂ç›");
            if (engage)
            {
                Area engageScript = trig.gameObject.GetComponentInParent<Area>();
                
                if (engageScript != null)
                {
                    engageScript.IEngage(area);
                }
            }
            
            
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
