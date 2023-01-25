using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BringerOfDeath : MonoBehaviour
{
    public int maxHealth;
    public int currentHealth;
    public float distance;              //�v���C���[�Ƃ̍��W�I����
    public float range;                 //�v���C���[�Ƃ̋���
    private bool playerOnTheRight;      //�v���C���[�͉E�ɂ��邩

    private Rigidbody2D rb2d = null;
    private SpriteRenderer sr = null;
    private Animator anime = null;
    [SerializeField] GameObject target; //�v���C���[�I�u�W�F�N�g���A�^�b�`

    public bool attacking = false;
    public bool cantMove = false;  //�d�����

    private bool isDead = false;    //���S

    public UnityEvent shock;        //��_���Ռ�
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerWhichSide();
    }

    public void TakeDamage(int damage)  //��_��
    {
        if (!isDead && !attacking)
        {
            shock.Invoke();
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                isDead = true;
                Die();
            }
        }
    }

    void PlayerWhichSide() //�v���C���[���E�ɂ��邩���ɂ��邩������s���AFlip()���Ăяo��
    {
        distance = this.gameObject.transform.position.x - target.transform.position.x;
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
        if (!isDead && !cantMove)
        {
            if (playerOnTheRight)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    void Die()  //���S
    {
        anime.SetBool("HeavyArmor_Dead", true);
    }
}
