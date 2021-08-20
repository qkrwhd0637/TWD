using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{
    readonly WaitForSeconds wait = new WaitForSeconds(3f);
    void OnEnable()
    {
        StartCoroutine(AutoClose());
    }

    IEnumerator AutoClose()
    {
        yield return wait;
        gameObject.SetActive(false);
    }

    public void CloseButton()
    {
        gameObject.SetActive(false);
    }
}
