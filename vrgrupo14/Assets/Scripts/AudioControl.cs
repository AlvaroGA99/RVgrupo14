using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleVR.HelloVR;

[ExecuteAlways]
public class AudioControl : MonoBehaviour
{
    private GameObject padre;
    [SerializeField]private GameObject menu;
    Component[] audioComponents;
    float volumeIncrement = 0.05f;
    bool value = true;
    public bool changeVolume = false;
    int direction;
    int instruction;    
    private IEnumerator countdown(int s){
        yield return new WaitForSeconds(s);
        switch(instruction){
            case 0:
                menu.SetActive(!menu.activeSelf);
                break;
            case 1:
                this.gameObject.GetComponent<AudioSource>().mute = !this.gameObject.GetComponent<AudioSource>().mute;
                break;
            case 2:                
                audioComponents = padre.GetComponentsInChildren<AudioSource>();
                foreach (AudioSource audio in audioComponents){
                    audio.mute=value;            
                }
                value = !value;
                this.gameObject.GetComponent<AudioSource>().mute = false;
                break;
            default: break;
        }
    }
    void Start()
    {
        GameObject padre = GameObject.FindGameObjectWithTag("sonidospadre");        
        Selector.flag_ambiente = true;
        this.gameObject.GetComponent<AudioSource>().enabled = false;
        value = true;
        changeVolume = false;
    }
    void FixedUpdate()
    {

        #if PLATFORM_ANDROID
        //menu.SetActive(menu.GetComponent<ObjectController>().IsGazed);
        #endif

        if (Selector.tempo_control <= 0) {
            //como sincronizar las canciones en fase: mago.mago.mago(mago);            
            Selector.flag_ambiente = false;
            this.gameObject.GetComponent<AudioSource>().enabled = true;            
            Selector.flag_sound = false;
        }
        if(changeVolume){
            this.gameObject.GetComponent<ResonanceAudioSource>().gainDb += direction * volumeIncrement;
        }
    }

    public void ShowMenu(){
        instruction = 0;
        StartCoroutine("countdown",1);        
    }

    public void HideMenu(){
        Intruction_OUT();
        menu.SetActive(false);
    }
    
    public void Mute(){
        instruction = 1;
        StartCoroutine("countdown",1);        
    }

    public void Solo(){
        instruction = 2;
        StartCoroutine("countdown",1); 
    }
    
    public void Intruction_OUT(){
        instruction = -1;
        StopCoroutine("countdown");
    }
    
    public void SetVolume (int value){        
        changeVolume = true;
        direction = value;
    }

    public void StopVolumeChange(){
        changeVolume = false;
    }
}
