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
    public float drainInterval;

    public Vector3 offset;
    public Vector3 upOffset;
    public Vector3 minusoffset;

    public bool attacking = false;
    public bool cantMove = false;
    public bool teleportAttack;

    private Rigidbody2D rb2d = null;
    private SpriteRenderer sr = null;
    private Animator anime = null;
    private Color defaultColor;
    [SerializeField] GameObject target; //�v���C���[�I�u�W�F�N�g���A�^�b�`
    [SerializeField] GameObject drain1;
    [SerializeField] GameObject drain2;
    [SerializeField] GameObject drain3;
    [SerializeField] GameObject drain4;
    [SerializeField] GameObject drain5;

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
        if (!isDead)
        {
            PlayerWhichSide();
            TimeOrdhin();
            Combat();
        }
        
    }
    void Combat()
    {
        if (inCombat)
        {
            if (teleportInterval <= 0 && !attacking)
            {
                anime.SetTrigger("Teleport_Attack");
                teleportInterval = 5f;
                teleportAttack = true;
            }
            else if (drainInterval <= 0 && !attacking)
            {
                anime.SetTrigger("Drain");
                drainInterval = 15f;
                teleportAttack = false;
            }
        }
        
        
    }

    void TeleportToPlayer()
    {
        if (teleportAttack)
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
        else if (!teleportAttack)
        {
            this.transform.position = target.transform.position + upOffset;
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

    public void CallDrain()
    {
        drain1.GetComponent<Drain>().CastCall();
        drain2.GetComponent<Drain>().CastCall();
        drain3.GetComponent<Drain>().CastCall();
        drain4.GetComponent<Drain>().CastCall();
        drain5.GetComponent<Drain>().CastCall();
    }

    void Die()  //���S
    {
        isDead = true;
        anime.Play("dead");
        StartCoroutine(WaitForAnimationEnd());
        IEnumerator WaitForAnimationEnd()
        {
            yield return new WaitForEndOfFrame();

            // �A�j���[�V�����Đ��I���܂őҋ@
            while (anime.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
            {
                yield return null;
            }
            Destroy(this.gameObject);
        }
    }
    void TimeOrdhin()
    {
        teleportInterval -= Time.deltaTime;
        drainInterval -= Time.deltaTime;
    }
}
