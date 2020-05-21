using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTime : MonoBehaviour
{
    void Start()
    {
        this.gameObject.GetComponent<AudioSource>().enabled = false;
    }
    void FixedUpdate()
    {
        if (Selector.tempo_control <= Mathf.Epsilon) {
            Selector.flag_sound = false;

            //como sincronizar las canciones en fase: mago.mago.mago(mago);
            this.gameObject.GetComponent<AudioSource>().enabled = false;
            this.gameObject.GetComponent<AudioSource>().enabled = true;
            
        }

    }
}
