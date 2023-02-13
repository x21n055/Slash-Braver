using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int damage;
    private void OnTriggerEnter2D(Collider2D player)
    {
        if (player.gameObject.tag == "Player")
        {
            Player playerScript = player.GetComponent<Player>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(this.gameObject, damage);
            }
            
        }
    }
}
