using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cocoon : MonoBehaviour, IDamageable
{
    //�f�[�^
    public int maxHealth;
    public int currentHealth;
    public int speed;
    public float attack1Distance;       //�ʏ�ߐڍU��
    public float attack2Distance;       //�������ːi�U��
    public float attackAcceleration;    //�ːi���̉����x
    public float attack1Cool;           //�ߐڍU���N�[���^�C��
    public float attack2Cool;           //�������ːi�N�[���^�C��
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

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        defaultColor = sr.color;
        anime = GetComponent<Animator>();
    }

    // Update is called once per frame
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
            anime.SetBool("HeavyArmor_Walk", true);
            if (playerOnTheRight)
            {
                rb2d.velocity = new Vector2(0,0);
                rb2d.velocity = new Vector2(speed, rb2d.velocity.y);
            }
            else if(!playerOnTheRight)
            {
                rb2d.velocity = new Vector2(0, 0);
                rb2d.velocity = new Vector2(-speed, rb2d.velocity.y);
            }
        }
        else if (!cantMove && approachingPlayer && !(range >= attack1Distance))
        {
            anime.SetBool("HeavyArmor_Walk", false);
            rb2d.velocity = new Vector2(0, 0);
        }
    }

    void Combat()
    {
        range = Vector2.Distance(transform.position, target.transform.position);
        if (inCombat)
        {
            if (range <= attack2Distance && !attacking && coolTime <= 0 && attack2Cool <= 0)
            {
                Flip();
                rb2d.velocity = new Vector2(0, 0);
                anime.SetBool("HeavyArmor_Walk", false);
                anime.SetTrigger("HeavyArmor_Attack3");
                coolTime = 3;
                attack2Cool = 7;
            }
            else if (range <= attack1Distance && !attacking && coolTime <= 0 && attack1Cool <= 0)
            {
                rb2d.velocity = new Vector2(0, 0);
                anime.SetBool("HeavyArmor_Walk", false);
                anime.SetTrigger("HeavyArmor_Attack1");
                coolTime = 3;
                attack1Cool = 3;
            }
        }
       

    }

    //�v���C���[���� �퓬State�ֈڍs
    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            inCombat = true;
            approachingPlayer = true;
        }
    }
    
    public void TakeDamage(int damage)  //��_��
    {
        if (!isDead && !attacking)
        {

            shock.Invoke();
            currentHealth -= damage;
            StartCoroutine(DamageEffect());
            IEnumerator DamageEffect()
            {

                for (int i = 0; i < 1; i++)
                {
                    sr.color = sr.color == defaultColor ? Color.white : defaultColor;
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
                transform.localScale = new Vector3(1,1,1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
    }

    public void Accelerate()
    {
        if (this.transform.localScale.x == -1)
        {
            rb2d.velocity = new Vector2(-attackAcceleration, rb2d.velocity.y);
        }
        else if (this.transform.localScale.x == 1)
        {
            rb2d.velocity = new Vector2(attackAcceleration, rb2d.velocity.y);
        }
    }
    void Die()  //���S
    {
        anime.SetBool("HeavyArmor_Dead", true);
    }

    void TimeManage() //�N�[���^�C���Ȃǂ��ꌳ��
    {
        coolTime -= Time.deltaTime;
        attack1Cool -= Time.deltaTime;
        attack2Cool -= Time.deltaTime;
    }

    
}
