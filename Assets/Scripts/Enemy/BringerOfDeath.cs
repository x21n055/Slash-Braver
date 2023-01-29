using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BringerOfDeath : MonoBehaviour, IDamageable, Area
{
    public int maxHealth;
    public int currentHealth;
    public int thisArea;
    public int inPosition = 1;          //初期位置
    public float distance;              //プレイヤーとの座標的距離
    public float range;                 //プレイヤーとの距離
    public float meleeDistance;         //プレイヤーが一定範囲内に入ったとき、メレーを行う。
    public float spellInterval;         //呪文攻撃のクールダウンタイム
    public Vector3 offset;
    

    private Rigidbody2D rb2d = null;
    private SpriteRenderer sr = null;
    private Animator anime = null;
    private Color defaultColor;
    [SerializeField] GameObject target; //プレイヤーオブジェクトをアタッチ
    [SerializeField] GameObject demonicGrasp; //デモニックグラスプオブジェクトをアタッチ
    [SerializeField] Transform position1;
    [SerializeField] Transform position2;

    public bool attacking = false;
    public bool cantMove = false;  //硬直状態
    public bool attackOnce = false;

    private bool playerOnTheRight;      //プレイヤーは右にいるか
    private bool inCombat = false;
    private bool isDead = false;    //死亡
    private bool meleeToken = false;

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
                transform.localScale = new Vector3(-1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
        }
    }

    void Die()  //死亡
    {
            anime.SetBool("Dead", true);
        
    }

    void TimeOrdhin()
    {
        spellInterval -= Time.deltaTime;
    }
}
