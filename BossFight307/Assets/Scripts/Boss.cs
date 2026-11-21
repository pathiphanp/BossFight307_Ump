using TMPro;
using UnityEngine;

public class Boss : MonoBehaviour
{
    private Animator anim;


    [Header("Stat")]
    public int hp;
    public int maxHp;


    [Header("Effect")]
    public TMP_Text damagePopUp;
    public GameObject pointSpawnDamagePopup;


    [Header("Collider")]
    public GameObject legCollider;
    public GameObject boddyCollider;
    public GameObject headCollider;

    void Awake()
    {
        anim = GetComponent<Animator>();
        hp = maxHp;
    }

    void Start()
    {
    }

    public void TakeDamage(int _inputDamage)
    {
        hp -= _inputDamage;
        TMP_Text _damPop = Instantiate(damagePopUp, pointSpawnDamagePopup.transform.position, damagePopUp.transform.localRotation);
        _damPop.text = _inputDamage.ToString();
        _damPop.fontSize += _inputDamage / 5;
        if (hp <= 0)
        {
            anim.SetTrigger("Die");
        }
        else
        {

        }
    }
}
