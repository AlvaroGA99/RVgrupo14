using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTime : MonoBehaviour
{
    public static bool flag_ambiente = true;
    void Start()
    {
        flag_ambiente = true;
        this.gameObject.GetComponent<AudioSource>().enabled = false;
    }
    void FixedUpdate()
    {
        if (Selector.tempo_control <= Mathf.Epsilon) {
            Selector.flag_sound = false;
            flag_ambiente = false;
            //como sincronizar las canciones en fase: mago.mago.mago(mago);
            this.gameObject.GetComponent<AudioSource>().enabled = false;
            this.gameObject.GetComponent<AudioSource>().enabled = true;
            
            
        }

    }
}
