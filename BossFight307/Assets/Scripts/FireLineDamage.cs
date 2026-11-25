using UnityEngine;

public class FireLineDamage : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision + " : AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        collision.GetComponent<HeroKnight>().TakeDamage();
    }
}
