using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Reaper : MonoBehaviour, IDamageable, Area
{
    public int maxHealth;
    public int currentHealth;
    public int thisArea;

    public float distance;              //�v���C���[�Ƃ̍��W�I����
    public float teleportInterval;

    public Vector3 offset;
    public Vector3 minusoffset;

    public bool attacking = false;

    private Rigidbody2D rb2d = null;
    private SpriteRenderer sr = null;
    private Animator anime = null;
    private Color defaultColor;
    [SerializeField] GameObject target; //�v���C���[�I�u�W�F�N�g���A�^�b�`

    private bool playerOnTheRight;      //�v���C���[�͉E�ɂ��邩
    private bool inCombat;
    private bool isDead = false;    //���S

    public UnityEvent shock;        //��_���Ռ�
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        rb2d = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anime = GetComponent<Animator>();
        defaultColor = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerWhichSide();
        TimeOrdhin();
        Combat();
    }
    void Combat()
    {
        if (inCombat)
        {
            if (teleportInterval <= 0)
            {
                anime.SetTrigger("Teleport_Attack");
                teleportInterval = 5f;
            }
        }
        
        
    }

    void TeleportToPlayer()
    {
        if (target.transform.localScale.x == 1)
        {
            this.transform.position = target.transform.position + offset;
        }
        else if (target.transform.localScale.x == -1)
        {
            this.transform.position = target.transform.position + minusoffset;
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
        if (!isDead && !attacking)
        {
            if (playerOnTheRight)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
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
    public void IEngage(int area)
    {
        if (area == thisArea)
        {
            inCombat = true;
        }

    }

    void Die()  //���S
    {
        anime.SetBool("Dead", true);
    }
    void TimeOrdhin()
    {
        teleportInterval -= Time.deltaTime;
    }
}
