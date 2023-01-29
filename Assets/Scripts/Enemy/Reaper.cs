using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Reaper : MonoBehaviour, IDamageable, Area
{
    public int maxHealth;
    public int currentHealth;
    public int thisArea;

    public float distance;              //プレイヤーとの座標的距離
    public float teleportInterval;

    public Vector3 offset;
    public Vector3 minusoffset;

    public bool attacking = false;

    private Rigidbody2D rb2d = null;
    private SpriteRenderer sr = null;
    private Animator anime = null;
    private Color defaultColor;
    [SerializeField] GameObject target; //プレイヤーオブジェクトをアタッチ

    private bool playerOnTheRight;      //プレイヤーは右にいるか
    private bool inCombat;
    private bool isDead = false;    //死亡

    public UnityEvent shock;        //被ダメ衝撃
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
    

    void PlayerWhichSide() //プレイヤーが右にいるか左にいるか判定を行い、Flip()を呼び出す
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

    void Flip()    //PlayerWhichSide()から引数を受け取り、それに応じてフリップする
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

    public void TakeDamage(int damage)  //被ダメ
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

    void Die()  //死亡
    {
        anime.SetBool("Dead", true);
    }
    void TimeOrdhin()
    {
        teleportInterval -= Time.deltaTime;
    }
}
