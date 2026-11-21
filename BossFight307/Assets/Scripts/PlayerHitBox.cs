using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class PlayerHitBox : MonoBehaviour
{
    private Rigidbody2D rb;
    private Collider2D coll;

    public HeroKnight player;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        player.boss.TakeDamage(player.damage);
    }

    public IEnumerator CanDodamage()
    {
        coll.enabled = true;
        // รอให้ผ่านรอบการคำนวณฟิสิกส์ไป 1 รอบ
        yield return new WaitForFixedUpdate();
        coll.enabled = false;
    }
}
