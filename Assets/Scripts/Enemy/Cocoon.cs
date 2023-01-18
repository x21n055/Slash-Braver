using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cocoon : MonoBehaviour
{
    //�f�[�^
    public int maxHealth;
    public int currentHealth;
    public int speed;
    public float attack1Distance;
    public float stopTimeCounter;
    public float distance;              //�v���C���[�Ƃ̍��W�I����
    public float range;                 //�v���C���[�Ƃ̋���
    private bool playerOnTheRight;
    public bool attacking = false;
    [SerializeField] GameObject target;

    //�R���|�[�l���g
    private Rigidbody2D rb2d = null;
    private SpriteRenderer sr = null;
    private Animator anime = null;
    //�R���o�b�gAI
    private bool inCombat = false;  //�퓬��Ԃ𔻒�
    private bool looming = false;   //�v���C���[�ɐڋ�
    public bool cantMove = false;  //�d�����
    public Transform setPosition;   //�G�̏����z�u�ꏊ
    public UnityEvent shock;        //��_���Ռ�
    private bool isDead = false;    //���S

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
        Move();
        Combat();
    }

    void Move()
    {
        if (!cantMove && looming)
        {
            anime.SetBool("HeavyArmor_Walk", true);
            if (playerOnTheRight)
            {
                rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            }
            else if(!playerOnTheRight)
            {
                rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
            }
        }
        else if(cantMove)
        {
            rb2d.velocity = new Vector2(0, rb2d.velocity.y);
        }
    }

    void Combat()
    {
        range = Vector2.Distance(transform.position, target.transform.position);
        if (range <= attack1Distance && !attacking)
        {
            anime.SetTrigger("HeavyArmor_Attack1");
        }

    }

    //�v���C���[���� �퓬State�ֈڍs
    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            Debug.Log("�퓬���");
            inCombat = true;
            looming = true;
        }
    }
    
    public void TakeDamage(int damage)  //��_��
    {
        if (!isDead)
        {

            shock.Invoke();
            currentHealth -= damage;
            stopTimeCounter = 0.4f;
            anime.SetTrigger("enemy_damaged");
            if (currentHealth <= 0)
            {
                isDead = true;
                Die();
            }
        }
    }

    void PlayerWhichSide() //�v���C���[���E�ɂ��邩���ɂ��邩������s���AFlip�Ɉ�����n��
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
                transform.localScale = new Vector3(1,1,1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    void Die()  //���S
    {
        anime.SetBool("enemy_dead", true);
    }

    
}
