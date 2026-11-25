using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLine : MonoBehaviour
{
    public float delayFireUp;
    public float delayHoldFireUp;
    public List<GameObject> fireObject;

    void OnEnable()
    {
        
    }

    void OnDisable()
    {
        StopAllCoroutines();
        foreach (GameObject g in fireObject)
        {
            g.SetActive(false);
        }
    }

    public void OpenWaveFireLine()
    {
        StartCoroutine(OnFire());
    }
    IEnumerator OnFire()
    {
        for (int i = 0; i < fireObject.Count; i++)
        {
            fireObject[i].SetActive(true);
            yield return new WaitForSeconds(delayFireUp);
        }
        yield return new WaitForSeconds(delayHoldFireUp);
        for (int i = 0; i < fireObject.Count; i++)
        {
            fireObject[i].SetActive(false);
            yield return new WaitForSeconds(delayFireUp);
        }
        gameObject.SetActive(false);
    }

    // [ContextMenu("GetFire")]
    // void GetFire()
    // {
    //     for (int i = 0; i < transform.childCount; i++)
    //     {
    //         fireObject.Add(transform.GetChild(i).gameObject);
    //     }
    // }

    // [ContextMenu("TestOnFire")]
    // void TestOnFire()
    // {
    //     OpenWaveFireLine();
    // }
}
