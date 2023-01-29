using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour, IDamageable, Area
{
    public int maxHealth;
    public int currentHealth;
    public int speed;
    public int thisArea;
    public float attack1Distance;       //�ʏ�ߐڍU��
    public float attack1Cool;           //�ߐڍU���N�[���^�C��
    [SerializeField] private float coolTime;              //�s���N�[���^�C��
    public float distance;              //�v���C���[�Ƃ̍��W�I����
    public float range;                 //�v���C���[�Ƃ̋���
    private bool playerOnTheRight;      //�v���C���[�͉E�ɂ��邩
    public bool attacking = false;
    [SerializeField] GameObject target;
    private Color defaultColor;

    //�R���|�[�l���g
    private Rigidbody2D rb2d = null;
    private SpriteRenderer sr = null;
    private Animator anime = null;

    //�R���o�b�gAI
    private bool inCombat = false;  //�퓬��Ԃ𔻒�
    private bool approachingPlayer = false;   //�v���C���[�ɐڋ�
    public bool cantMove = false;  //�d�����
    public Transform setPosition;   //�G�̏����z�u�ꏊ
    public UnityEvent shock;        //��_���Ռ�
    private bool isDead = false;    //���S
    void Start()
    {
        currentHealth = maxHealth;
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
        anime = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDead)
        {
            PlayerWhichSide();
            Move();
            Combat();
            TimeManage();
        }

    }
    void Move()
    {
        if (!cantMove && approachingPlayer && range >= attack1Distance)
        {
            anime.SetBool("enemy_run", true);
            if (playerOnTheRight)
            {
                rb2d.velocity = new Vector2(0, 0);
                rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            }
            else if (!playerOnTheRight)
            {
                rb2d.velocity = new Vector2(0, 0);
                rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
            }
        }
        else if (!cantMove && approachingPlayer && !(range >= attack1Distance))
        {
            anime.SetBool("enemy_run", false);
            rb2d.velocity = new Vector2(0, 0);
        }
    }

    void Combat()
    {
        range = Vector2.Distance(transform.position, target.transform.position);
        if (inCombat)
        {
            if (range <= attack1Distance && !attacking && coolTime <= 0 && attack1Cool <= 0)
            {
                rb2d.velocity = new Vector2(0, 0);
                anime.SetBool("enemy_run", false);
                anime.SetTrigger("attack");
                coolTime = 3;
                attack1Cool = 3;
            }
        }


    }

    //�v���C���[���� �퓬State�ֈڍs
    public void IEngage(int area)
    {
        if (area == thisArea)
        {
            inCombat = true;
            approachingPlayer = true;
        }

    }

    public void TakeDamage(int damage)  //��_��
    {
        if (!isDead)
        {

            shock.Invoke();
            currentHealth -= damage;
            StartCoroutine(DamageEffect());
            IEnumerator DamageEffect()
            {
                anime.SetTrigger("enemy_damaged");
                for (int i = 0; i < 2; i++)
                {
                    sr.color = sr.color == defaultColor ? Color.red : defaultColor;
                    yield return new WaitForSeconds(0.1f);
                }
                if (currentHealth <= 0)
                {
                    isDead = true;
                    Die();
                }
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
        anime.SetBool("enemy_dead", true);
    }

    void TimeManage() //�N�[���^�C���Ȃǂ��ꌳ��
    {
        coolTime -= Time.deltaTime;
        attack1Cool -= Time.deltaTime;
    }
}