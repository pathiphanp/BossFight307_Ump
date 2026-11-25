using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Boss : MonoBehaviour
{
    private Animator anim;
    public HeroKnight player;

    [Header("Stat")]
    public int hp;
    public int maxHp;


    [Header("Effect")]
    public TMP_Text damagePopUp;
    public GameObject pointSpawnDamagePopup;


    [Header("AttackFireBall")]
    public GameObject pointStartFireBall;
    public BallFIreMoveToPoint fireMoveToPoint;
    public GameObject pointFireMove;

    [Header("AttackFireWave")]
    public FireLine[] fireLine;

    [Header("UiHp")]
    public Image hpFillImage;

    public GameObject startText;

    void Awake()
    {
        anim = GetComponent<Animator>();
        hp = maxHp;
    }

    void Start()
    {
        Invoke(nameof(OffText), 2);
        startText.SetActive(true);
        anim.SetTrigger("Talk");

    }

    void OffText()
    {
        startText.SetActive(false);
        StartCoroutine(RndAttackSet());
    }

    public void TakeDamage(int _inputDamage)
    {
        hp -= _inputDamage;
        float ratio = (float)hp / (float)maxHp;
        hpFillImage.fillAmount = Mathf.Clamp01(ratio);
        TMP_Text _damPop = Instantiate(damagePopUp, pointSpawnDamagePopup.transform.position, damagePopUp.transform.localRotation);
        _damPop.text = _inputDamage.ToString();
        _damPop.fontSize += _inputDamage / 5;
        if (hp <= 0)
        {
            anim.SetTrigger("Die");
            player.textEndGame.text = "You Win";
            player.endGamePanal.SetActive(true);
            StopAllCoroutines();
            enabled = false;
            player.enabled = false;
        }
        else
        {

        }
    }


    public IEnumerator RndAttackSet()
    {
        yield return new WaitForSeconds(10);
        int rnd = Random.Range(0, 2);
        switch (rnd)
        {
            case 0:
                fireMoveToPoint.LoopRnd(Random.Range(1, 7));
                StartCoroutine(FireBall());
                break;
            case 1:
                // StartCoroutine(FireBall());
                StartCoroutine(FireLine());
                break;
        }
    }

    public IEnumerator FireBall()
    {
        fireMoveToPoint.boss = this;
        pointFireMove.transform.position = player.transform.position;
        pointFireMove.SetActive(true);
        fireMoveToPoint.transform.position = pointStartFireBall.transform.position;
        anim.SetTrigger("Special");
        yield return new WaitForSeconds(2.20f);
        fireMoveToPoint.gameObject.SetActive(true);
        fireMoveToPoint.anim.SetTrigger("Idle");
        fireMoveToPoint.canMove = true;
        fireMoveToPoint.onPosition = false;
    }

    IEnumerator FireLine()
    {
        for (int i = 0; i < 3; i++)
        {
            int _rndFire = Random.Range(0, fireLine.Length);
            fireLine[_rndFire].gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            anim.SetTrigger("Talk");
            fireLine[_rndFire].OpenWaveFireLine();
            yield return new WaitForSeconds(2);
        }
        StartCoroutine(RndAttackSet());
    }
}
