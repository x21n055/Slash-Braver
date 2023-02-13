using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDirection : MonoBehaviour
{
    public GameObject player;
    public GameObject parent;
    public float width;

    public float distance;              //プレイヤーとの座標的距離

    public Vector3 offset;
    public bool cantMove = false;

    private bool playerOnTheRight;      //プレイヤーは右にいるか
    
    // Start is called before the first frame update
    void Start()
    {
        width = this.transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = parent.transform.position + offset;
        Debug.Log(player.transform.position.x);
        PlayerWhichSide();
    }

    void PlayerWhichSide() //プレイヤーが右にいるか左にいるか判定を行い、Flipに引数を渡す
    {
        distance = this.gameObject.transform.position.x - player.transform.position.x;
        if (distance < 0)
        {
            playerOnTheRight = true;
        }
        else if (distance > 0)
        {
            playerOnTheRight = false;
        }
        Flip();
    }

    void Flip()    //PlayerWhichSide()から引数を受け取り、それに応じてフリップする
    {
        if (!cantMove)
        {
            if (playerOnTheRight)
            {
                transform.localScale = new Vector3(this.transform.localScale.x, width, 1);
            }
            else
            {
                transform.localScale = new Vector3(this.transform.localScale.x, -width, 1);
            }
        }
    }
}
