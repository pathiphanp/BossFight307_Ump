using UnityEngine;
using System.Collections;
using Unity.Android.Gradle;
using System;


public enum StateMovement
{
    Idle, Move,
    Attack1, Attack2, Attack3, Attack3Idle,
    Block
}
public class HeroKnight : MonoBehaviour
{
    [SerializeField] float m_speed = 4.0f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float m_rollForce = 6.0f;
    [SerializeField] GameObject m_slideDust;

    private Animator anim;
    private Rigidbody2D rb;
    private SpriteRenderer sp;

    [Header("Move")]
    [SerializeField] bool onMove;
    [SerializeField] bool canMove;
    public StateMovement movementState;

    [Header("Attack")]
    public PlayerHitBox playerHitBox;
    public int damage;
    public int bonusComboDamage;
    public int bonusHoldDamage = 1;
    public float intervalHoldBonusDamage = 1f;
    public float maxHoldBonusDamage = 4f;
    public int countAttack;
    [SerializeField] bool canAtk;
    // Use this for initialization
    Coroutine collHoldAttack;
    [SerializeField] GameObject holdAttackEffect;
    bool onHoldAttack;

    [SerializeField] bool canDoAction;

    [Header("Enemy")]
    public Boss boss;

    void Start()
    {
        canMove = true;
        canAtk = true;
        canDoAction = true;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Attack();
        Block();
        EditPlayerLayer();
    }

    #region Move
    void Move()
    {
        if (!canMove || !canDoAction)
        {
            rb.velocity = Vector2.zero;
            return;
        }

        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (inputX == 0 && inputY == 0)
        {
            onMove = false;
            ChangeStateAnimation(StateMovement.Idle);
        }
        else
        {
            ChangeStateAnimation(StateMovement.Move);
            onMove = true;
        }
        if (onMove == false)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            transform.localScale = new Vector3(1,1,1);
        }
        else if (inputX < 0)
        {
            transform.localScale = new Vector3(-1,1,1);
        }

        rb.velocity = new Vector2(inputX, inputY).normalized * m_speed;
    }
    #endregion

    #region Attack
    void Attack()
    {
        if (!canAtk && !canDoAction) return;
        if (Input.GetMouseButtonDown(0))
        {
            canMove = false;
            ChangeStateAnimation(StateMovement.Attack3Idle);
            collHoldAttack = StartCoroutine(HoldAttack());
        }
        if (Input.GetMouseButtonUp(0))
        {
            canAtk = false;
            canDoAction = false;
            if (!onHoldAttack)
            {
                if (collHoldAttack != null)
                {
                    StopCoroutine(collHoldAttack);
                    holdAttackEffect.SetActive(false);
                    collHoldAttack = null;
                }
            }
            else
            {
                onHoldAttack = false;
            }
            CallAttack();
        }
    }

    IEnumerator HoldAttack()
    {
        yield return new WaitForSeconds(0.5f);
        onHoldAttack = true;
        float _timeCount = 0f;
        int _countGetBonusHold = 0;
        while (onHoldAttack)
        {
            _timeCount += Time.deltaTime;
            if (_timeCount >= intervalHoldBonusDamage && _countGetBonusHold < maxHoldBonusDamage)
            {
                holdAttackEffect.SetActive(true);
                _timeCount -= intervalHoldBonusDamage;
                bonusHoldDamage += 1;
                _countGetBonusHold++;
                holdAttackEffect.transform.localScale += new Vector3(0.5f, 0.5f, 0.5f);
            }
            yield return null;
        }
        countAttack = 2;
        collHoldAttack = null;
        holdAttackEffect.SetActive(false);
        holdAttackEffect.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
        CallAttack();
    }

    void CallAttack()
    {
        switch (countAttack)
        {
            case 0:
                ChangeStateAnimation(StateMovement.Attack1);
                damage = 10 + bonusComboDamage;
                break;
            case 1:
                ChangeStateAnimation(StateMovement.Attack2);
                damage = 10 + bonusComboDamage;
                break;
            case 2:
                ChangeStateAnimation(StateMovement.Attack3);
                damage = (10 + bonusComboDamage) * bonusHoldDamage;
                countAttack = 0;
                bonusHoldDamage = 1;
                break;
        }
        countAttack = (countAttack + 1) % 2;
        StartCoroutine(playerHitBox.CanDodamage());
        Invoke(nameof(ResetAttackState), 0.5f);
    }

    void ResetAttackState()
    {
        canMove = true;
        canAtk = true;
        canDoAction = true;
    }
    #endregion

    #region Block
    private void Block()
    {
        if (Input.GetMouseButtonDown(1) && canDoAction)
        {
            canDoAction = false;
            ChangeStateAnimation(StateMovement.Block);
        }
        if (Input.GetMouseButtonUp(1))
        {
            canDoAction = true;
        }
    }
    #endregion

    void ChangeStateAnimation(StateMovement _input)
    {
        if (movementState == _input) return;
        movementState = _input;
        anim.SetTrigger(movementState.ToString());
    }

    void EditPlayerLayer()
    {
        if (boss.transform.position.y < this.transform.position.y)
        {
            sp.sortingLayerName = "Top";
        }
        else
        {
            sp.sortingLayerName = "Down";
        }
    }
}
