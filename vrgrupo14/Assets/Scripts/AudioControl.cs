using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AudioTime : MonoBehaviour
{
    
    void Start()
    {
        //Selector.flag_ambiente = true;
        this.gameObject.GetComponent<AudioSource>().enabled = false;
    }
    void FixedUpdate()
    {
        UnityEngine.Debug.Log("asfasf: " + Selector.tempo_control);
        if (Selector.tempo_control <= 0) {
            //como sincronizar las canciones en fase: mago.mago.mago(mago);
            UnityEngine.Debug.Log("buoeaunoausfa");         
            Selector.flag_ambiente = false;
            //this.gameObject.GetComponent<AudioSource>().enabled = false;
            this.gameObject.GetComponent<AudioSource>().enabled = true;
            Selector.flag_sound = false;
                        
        }

    }
    
}
