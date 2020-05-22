using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTime : MonoBehaviour
{
    
    void Start()
    {
        //Selector.flag_ambiente = true;
        this.gameObject.GetComponent<AudioSource>().enabled = false;
    }
    void FixedUpdate()
    {
        if (Selector.tempo_control <= 0) {
            //como sincronizar las canciones en fase: mago.mago.mago(mago);
            Selector.flag_ambiente = false;
            this.gameObject.GetComponent<AudioSource>().enabled = false;
            this.gameObject.GetComponent<AudioSource>().enabled = true;
            Selector.flag_sound = false;
                        
        }

    }
}
