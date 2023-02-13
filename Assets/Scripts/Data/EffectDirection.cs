using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDirection : MonoBehaviour
{
    public GameObject player;
    public GameObject parent;
    public float width;

    public float distance;              //�v���C���[�Ƃ̍��W�I����

    public Vector3 offset;
    public bool cantMove = false;

    private bool playerOnTheRight;      //�v���C���[�͉E�ɂ��邩
    
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

    void PlayerWhichSide() //�v���C���[���E�ɂ��邩���ɂ��邩������s���AFlip�Ɉ�����n��
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

    void Flip()    //PlayerWhichSide()����������󂯎��A����ɉ����ăt���b�v����
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
