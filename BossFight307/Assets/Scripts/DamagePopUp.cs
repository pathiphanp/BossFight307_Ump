using System.Collections;
using UnityEngine;

public class DamagePopUp : MonoBehaviour
{
    public float delayDestoyHold;
    public float timeMove;
    public float moveSpeed;
    void Start()
    {
        StartCoroutine(DalayDamagePopup());
    }

    IEnumerator DalayDamagePopup()
    {
        yield return new WaitForSeconds(delayDestoyHold);
        float _count = 0;
        while (_count < timeMove)
        {
            _count += Time.deltaTime;
            Vector2 dir = new Vector2(1f, 1f).normalized;
            transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(this.gameObject);
    }
}
