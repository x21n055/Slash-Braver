using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BringerOfDeath : MonoBehaviour, IDamageable, Area
{
    public int maxHealth;
    public int currentHealth;
    public int thisArea;
    public int inPosition = 1;          //�����ʒu
    public float distance;              //�v���C���[�Ƃ̍��W�I����
    public float range;                 //�v���C���[�Ƃ̋���
    public float meleeDistance;         //�v���C���[�����͈͓��ɓ������Ƃ��A�����[���s���B
    public float spellInterval;         //�����U���̃N�[���_�E���^�C��
    public Vector3 offset;
    

    private Rigidbody2D rb2d = null;
    private SpriteRenderer sr = null;
    private Animator anime = null;
    private Color defaultColor;
    [SerializeField] GameObject target; //�v���C���[�I�u�W�F�N�g���A�^�b�`
    [SerializeField] GameObject demonicGrasp; //�f���j�b�N�O���X�v�I�u�W�F�N�g���A�^�b�`
    [SerializeField] Transform position1;
    [SerializeField] Transform position2;

    public bool attacking = false;
    public bool cantMove = false;  //�d�����
    public bool attackOnce = false;

    private bool playerOnTheRight;      //�v���C���[�͉E�ɂ��邩
    private bool inCombat = false;
    private bool isDead = false;    //���S
    private bool meleeToken = false;

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
            Combat();
            TimeOrdhin();
            
        }
    }


    void Combat()
    {
        range = Vector2.Distance(transform.position, target.transform.position);
        if (inCombat)
        {
            
            if (range <= meleeDistance && !attacking)
            {
                if (!meleeToken && !attackOnce)
                {
                    attackOnce = true;
                    StartCoroutine(GiveMeleeToken());
                    IEnumerator GiveMeleeToken()
                    {
                        yield return new WaitForSeconds(1f);
                        meleeToken = true;
                    }
                }
                else if (meleeToken)
                {
                    anime.SetTrigger("BringerOfDeath_Melee");
                    meleeToken = false;
                }
                
            }
            else if(spellInterval <= 0 && !attacking && range > meleeDistance)
            {
                anime.SetTrigger("BringerOfDeath_DemonicGrasp");
                spellInterval = 3;
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

    void Teleport()
    {
        if (!isDead)
        {
            
            if (inPosition == 1)
            {
                this.transform.position = position2.position;
                attackOnce = false;
                inPosition = 2;
            }
            else if (inPosition == 2)
            {
                this.transform.position = position1.position;
                attackOnce = false;
                inPosition = 1;
            }
        }
        
    }

    void SpellFinish()
    {
        demonicGrasp.GetComponent<Animator>().SetTrigger("DemonicGrasp");
        demonicGrasp.transform.position = target.transform.position + offset;
    }
    public void IEngage(int area)
    {
        if (area == thisArea)
        {
            //Debug.Log("0");
            inCombat = true;
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
            anime.SetBool("Dead", true);
        
    }

    void TimeOrdhin()
    {
        spellInterval -= Time.deltaTime;
    }
}
