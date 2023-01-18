using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Cocoon : MonoBehaviour
{
    //�f�[�^
    public int maxHealth;
    public int currentHealth;
    public float stopTimeCounter;

    //�R���|�[�l���g
    private Rigidbody2D rb2d = null;
    private SpriteRenderer sr = null;
    private Animator anime = null;
    //�R���o�b�gAI
    private bool inCombat = false;  //�퓬��Ԃ𔻒�
    private bool looming = false;   //�v���C���[�ɐڋ�
    private bool cantMove = false;  //�d�����
    public Transform setPosition;   //�G�̏����z�u�ꏊ
    private Transform target;       //�v���C���[�̈ʒu
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

    //�v���C���[���� �퓬State�ֈڍs
    void OnTriggerEnter2D(Collider2D trig)
    {
        if (trig.gameObject.tag == "Player")
        {
            target = trig.transform;
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

    }

    void Flip(bool playerOnTheRight)    //PlayerWhichSide()����������󂯎��A����ɉ����ăt���b�v����
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
