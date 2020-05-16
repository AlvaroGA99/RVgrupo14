using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : MonoBehaviour
{
   
    int contador = 3;
    public void inp()
    {
        StartCoroutine("Countdown", contador);
    }
    public void outp() {
        contador = 3;
        StopCoroutine("Countdown");
    }

    private IEnumerator Countdown(int contador)
    {
        while (contador > 0)
        {
            Debug.Log(contador--);
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Countdown Complete!");
    }
}
