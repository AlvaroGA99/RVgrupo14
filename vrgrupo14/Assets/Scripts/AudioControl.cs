using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class AudioControl : MonoBehaviour
{
    private GameObject padre;
    Component[] audioComponents;
    float volumeIncrement = 0.01f;
    void Start()
    {
        GameObject padre = GameObject.FindGameObjectWithTag("sonidospadre");
        Selector.flag_ambiente = true;
        this.gameObject.GetComponent<AudioSource>().enabled = false;
    }
    void FixedUpdate()
    {        
        if (Selector.tempo_control <= 0) {
            //como sincronizar las canciones en fase: mago.mago.mago(mago);            
            Selector.flag_ambiente = false;
            this.gameObject.GetComponent<AudioSource>().enabled = true;            
            Selector.flag_sound = false;
        }
    }

    void Mute(bool value){
        this.gameObject.GetComponent<AudioSource>().mute = value;
    }

    void Solo (bool value){
        audioComponents = padre.GetComponentsInChildren<AudioSource>();        
        foreach (AudioSource audio in audioComponents){
            audio.mute=value;
        }

        this.gameObject.GetComponent<AudioSource>().mute = !value;
    }
    
    void SetVolume (int value){
        this.gameObject.GetComponent<ResonanceAudioSource>().gainDb += value * volumeIncrement;
    }
}
