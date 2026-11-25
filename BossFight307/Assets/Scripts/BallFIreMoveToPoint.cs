using System.Collections;
using UnityEngine;

public class BallFIreMoveToPoint : MonoBehaviour
{
    public Boss boss;
    public Animator anim;
    public float speedMove;
    public GameObject pointFireBall;
    public bool onPosition;
    public bool canMove;

    public Collider2D coll;

    int rnd;
    int countRnd;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, pointFireBall.transform.position, Time.deltaTime * speedMove);
        }
        if (transform.position == pointFireBall.transform.position && onPosition == false)
        {
            onPosition = true;
            canMove = false;
            anim.SetTrigger("Ex");
            StartCoroutine(OffAndOnFireBall());
        }
    }

    IEnumerator OffAndOnFireBall()
    {
        coll.enabled = true;
        yield return new WaitForFixedUpdate();
        coll.enabled = false;
        pointFireBall.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        CheckLoop();
    }

    void CheckLoop()
    {
        countRnd++;
        if (countRnd <= rnd)
        {
            StartCoroutine(boss.FireBall());
        }
        else
        {
            countRnd = 0;
            StartCoroutine(boss.RndAttackSet());
        }
    }

    public void LoopRnd(int _rnd)
    {
        rnd = _rnd;
        StartCoroutine(boss.FireBall());
    }
}
