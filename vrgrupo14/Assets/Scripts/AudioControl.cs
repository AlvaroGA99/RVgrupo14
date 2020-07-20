using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleVR.HelloVR;

[ExecuteAlways]
public class AudioControl : MonoBehaviour
{   
    private GameObject padre;
    [SerializeField]public GameObject menu;
    Component[] audioComponents;
    float volumeIncrement = 0.05f;
    public bool value = false;
    public bool changeVolume = false;
    int direction;
    int instruction;
    GameObject playerReference;
    private IEnumerator countdown(int s){
        yield return new WaitForSeconds(s);
        Selector.playSelectSound();
        switch(instruction){
            case 0:
                if (!Selector.flag_scan){
                    menu.SetActive(!menu.activeSelf);
                    menu.GetComponent<Transform>().rotation = new Quaternion(playerReference.GetComponent<Transform>().rotation.x,playerReference.GetComponent<Transform>().rotation.y,playerReference.GetComponent<Transform>().rotation.z,playerReference.GetComponent<Transform>().rotation.w);
                }
                break;
            case 1:
                this.gameObject.GetComponent<AudioSource>().mute = !this.gameObject.GetComponent<AudioSource>().mute;
                if(this.gameObject.GetComponent<AudioSource>().mute){
                    transform.parent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).GetComponentInChildren<Renderer>().material.color = Color.red;
                }else{
                    transform.parent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).GetComponentInChildren<Renderer>().material.color = Color.white;
                }
                audioComponents = padre.GetComponentsInChildren<AudioControl>();
                foreach (AudioControl audio in audioComponents){
                    if (audio.value){                        
                        audio.transform.parent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).GetComponentInChildren<Renderer>().material.color = Color.white;
                    }
                    audio.value = false;
                }
                break;
            case 2:
                value = !value;           
                audioComponents = padre.GetComponentsInChildren<AudioSource>();
                foreach (AudioSource audio in audioComponents){
                    audio.mute = value;
                    if(value){
                        audio.transform.parent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).GetComponentInChildren<Renderer>().material.color = Color.red;                    
                    }
                    else{
                        audio.transform.parent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).GetComponentInChildren<Renderer>().material.color = Color.white; 
                    }
                }
                
                this.gameObject.GetComponent<AudioSource>().mute = false;
                
                audioComponents = padre.GetComponentsInChildren<AudioControl>();
                
                if(value){
                    foreach (AudioControl audio in audioComponents){
                        if (audio.value){                        
                            audio.transform.parent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).GetComponentInChildren<Renderer>().material.color = Color.white;
                        }
                        audio.value = false;
                    }
                    value = true;
                }                

                if(value){
                   transform.parent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).GetComponentInChildren<Renderer>().material.color = Color.green;                   
                }
                else{
                    transform.parent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(3).GetComponentInChildren<Renderer>().material.color = Color.white;
                }
                transform.parent.transform.GetChild(0).transform.GetChild(0).transform.GetChild(2).GetComponentInChildren<Renderer>().material.color = Color.white;


                break;
            case 3:                
                transform.parent.gameObject.SetActive(false);
                break;
            default: break;
        }
    }
    void Start()
    {
        padre = GameObject.FindGameObjectWithTag("sonidospadre");
        playerReference = GameObject.FindGameObjectWithTag("MainCamera");
        menu.SetActive(false);
        Selector.flag_ambiente = false;
        this.gameObject.GetComponent<AudioSource>().enabled = false;
        value = false;
        changeVolume = false;
    }
    void FixedUpdate()
    {
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
        Selector.playInSound();
        instruction = 0;
        StartCoroutine("countdown",1);        
    }

    public void HideMenu(){        
        Intruction_OUT();
        menu.SetActive(false);
    }
    
    public void Mute(){
        Selector.playInSound();
        instruction = 1;
        StartCoroutine("countdown",1);        
    }

    public void Solo(){
        Selector.playInSound();
        instruction = 2;
        StartCoroutine("countdown",1); 
    }
    public void Borrar(){
        Selector.playInSound();
        instruction = 3;
        StartCoroutine("countdown",1);
    }
    
    public void Intruction_OUT(){
        Selector.playOutSound();
        instruction = -1;
        StopCoroutine("countdown");
    }
    
    public void SetVolume (int value){        
        Selector.playInSound();
        changeVolume = true;
        direction = value;
    }

    public void StopVolumeChange(){
        Selector.playOutSound();
        changeVolume = false;
    }
}
