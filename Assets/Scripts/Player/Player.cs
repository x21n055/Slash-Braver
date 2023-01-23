using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

interface IDamageable   //�_���[�W��^���邱�Ƃ��ł�����̂ւ̋��ʃC���^�[�t�F�C�X
{
    void TakeDamage(int damage);
}
public class Player : MonoBehaviour
{
    public int attackDamage;
    public int fireAttackDamage;
    public int maxHealth;
    public int currentHealth;

    public bool attack = false;    //�v���C���[���U�����ɍU���{�^�������������ǂ���
    public bool attacking;  //�A�j���[�V�����ŊǗ��B�v���C���[���U�������ǂ���
    public bool nextAttack = true; //�v���C���[�����̃R���{�U���Ɉڍs�\���ǂ���
    public bool isEnemyLeft = false;       //�G���E�ɂ���O��
    public bool cantMove = false; //�v���C���[���ړ��s�\���ǂ��� �ꕔ�A�j���[�V�����Ǘ�
    public bool invincible = false;

    public Image HPBar;

    public float speed;
    public float jumpForce;
    public float jumpImpulse;   //��i�W�����v���̔�ы
    public float jumpTimer;
    public float knockBackForce;
    public float checkRadius;
    public float attackRange = 0.5f;
    public float horizontalkey;

    public Transform feetPos;
    public Transform attackPoint;
    public LayerMask whatIsGround;
    public LayerMask enemyLayers;

    private Animator anime = null;
    private Rigidbody2D rb2d = null;
    private SpriteRenderer sr = null;
    private bool isGround;
    private bool isJumping;

    private bool fired;
    private float jumpTimeCounter;
    private float freezeTime;


    
    // Start is called before the first frame update
    void Start()
    {
        anime = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(horizontalkey);
        //Debug.Log(invincible);
        freezeTime -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.G))
        {
            SceneManager.LoadScene("Title1");
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && isGround)
        {
            PlayerAttack();
        }
        ComboCheck();
        Jump();
        if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            horizontalkey = 0;
            anime.SetBool("Run", false);
        }
        else
        {
            Flip();
        }
        Move();
        if (freezeTime <= 0)
        {
            rb2d.velocity = new Vector2(horizontalkey * speed, rb2d.velocity.y);
        }
        
        //�ݒu����
        isGround = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);


    }

    private void FixedUpdate()
    {

        
        anime.SetBool("Jump", isJumping);
        anime.SetBool("Ground", isGround);
    }

    void Move()
    {
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            horizontalkey = 0;
        } 
        else if (Input.GetKey(KeyCode.D) && !(horizontalkey == 1))
        {
            horizontalkey += 1;
        }
        else if (Input.GetKey(KeyCode.A) && !(horizontalkey == -1))
        {
            horizontalkey -= 1;
        }
        else if (!(Input.GetKey(KeyCode.A)) && !(Input.GetKey(KeyCode.D)))
        {
            horizontalkey = 0;
        }
    }
    //���ړ�
    void Flip()
    {
        if (!cantMove)
        {
            
            //���ړ�
            if (horizontalkey > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
                anime.SetBool("Run", true);
            }
            else if (horizontalkey < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                anime.SetBool("Run", true);
            }
            else
            {
                anime.SetBool("Run", false);
            }
        }
    }

    //�W�����v
    void Jump()
    {
        if (!cantMove)
        {
            if (isGround == true && Input.GetKeyDown(KeyCode.Space))
            {
                isJumping = true;
                jumpTimeCounter = jumpTimer;
                rb2d.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
            }

            if (Input.GetKey(KeyCode.Space) && isJumping == true)
            {
                if (jumpTimeCounter > 0)
                {
                    rb2d.velocity = Vector2.up * jumpForce;
                    jumpTimeCounter -= Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                }
            }

            if (!isJumping && !isGround && Input.GetKeyDown(KeyCode.Space))
            {

                rb2d.AddForce(Vector2.up * jumpImpulse, ForceMode2D.Impulse);
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                isJumping = false;
            }
        }
    }


    public void PlayerAttack()
    {
        if (attacking)
        {
            attack = true;
        }
        else
        {
            anime.SetTrigger("Attack");
        }

    }

    public void ComboCheck()
    {
        if (attack && nextAttack)
        {
            anime.SetBool("ComboAttack", true);
            attack = false;
        }
        else
        {
            anime.SetBool("ComboAttack", false);
        }
    }

    //�����蔻��`�F�b�N�@�A�j���[�V��������Ăяo��
    void AttackCheck()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);

        foreach (Collider2D enemy in hitEnemies)
        {
            IDamageable damageable = enemy.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }
            
        }

    }



    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

    }

    public void GetFireItem()
    {
        fired = true;

    }

    public void TakeDamage(GameObject enemy,int damage)
    {
        if (!invincible)
        {
            invincible = true;
            currentHealth -= damage;
            HPBar.fillAmount = (float)currentHealth / (float)maxHealth;
            freezeTime = 0.1f;
            rb2d.velocity = new Vector2(0, 0);
            isEnemyLeft = this.transform.position.x > enemy.transform.position.x;
            Vector2 knockBackDirection = isEnemyLeft ? Vector2.right : Vector2.left;
            rb2d.AddForce(knockBackDirection * knockBackForce, ForceMode2D.Impulse);
            transform.localScale = new Vector3(isEnemyLeft ? -1 : 1, 1, 1);
            StartCoroutine(DamageEffect());
            IEnumerator DamageEffect()
            {
                for (int i = 0; i < 4; i++)
                {
                    sr.color = new Color(1, 1, 1, 0);
                    yield return new WaitForSeconds(0.1f);
                    sr.color = new Color(1, 1, 1, 1);
                    yield return new WaitForSeconds(0.1f);
                }
                invincible = false;
            }
            Debug.Log("�������");
        }
        

    }
}
