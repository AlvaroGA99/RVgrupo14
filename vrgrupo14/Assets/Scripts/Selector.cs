using System.Collections;
using System.Collections.Generic;
using GoogleVR.HelloVR;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;


public class Selector : MonoBehaviour
{
    [SerializeField]private GameObject Movement;
    [SerializeField]private GameObject Room;
    [SerializeField]private GameObject MenuControl;
    [SerializeField]private GameObject General_Menu;

    [SerializeField] int contador = 1; //interfaz general

    [SerializeField] float velocity = 15f;
    [SerializeField] float scaleValue = 0.5f;
    Vector3 scaleVec;
    int scaleAxis;
    bool passScale;
    public static double tempo = 9; //tempo de los sonidos para sincronizar
    public static double tempo_control = tempo; 

    //flags de interfaz y control
    public static bool Selection_Flag = false;
    public static bool flag_scale = false;
    public static bool flag_remove = false;
    public static bool flag_ambiente = false;
    public static bool flag_sound = false;

    //movimiento
    public bool movW = false;
    public bool movD = false;
    public bool movS = false;
    public bool movA = false;

    Scene scene;

    //salas de musica
    public GameObject Sala0; //Menu principal
    public GameObject Sala1; //sala Electronica
    public GameObject Sala2; //sala Orquestal   

    public void sincronizar()
    {
        tempo_control = 9;
        flag_sound = true;
    }
    public void inp()
    {
        StartCoroutine("Countdown", contador);
    }
    public void outp() {
        contador = 1;
        StopCoroutine("Countdown");
    }

    private IEnumerator Countdown(int contador) //contador general para la interfaz
    {
        while (contador > 0)
        {
            contador--;
            yield return new WaitForSeconds(1);
        }
        //Debug.Log("Countdown Complete!");
        Selection_Flag = true;
    } 

    private void Awake()
    {
#if PLATFORM_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera))
        {
            Permission.RequestUserPermission(Permission.Camera);
        }
#endif

    }

    private void Start()
    {
        Selection_Flag = false; flag_remove = false;
        scene = SceneManager.GetActiveScene();
        sincronizar();
        flag_ambiente = true;
        contador = 1;
        tempo_control = 0;
        velocity *= 0.001f;
    }
//escalado de la sala
    public void ScalePositive(int axis){
        scaleValue = Mathf.Abs(scaleValue);        
        ScaleRoom(axis);
    }

    public void ScaleNegative(int axis){
        scaleValue = Mathf.Abs(scaleValue);        
        ScaleRoom(axis);
    }
    public void ScaleRoom(int axis){        
        scaleAxis = axis;
        flag_scale = true;
    }

    public void OUTscale()
    {
        flag_scale = false;
    }

//movimiento del jugador
    public void movement_W()
    {
        movW = true;
    }
    public void movement_A()
    {
        movA = true;
    }
    public void movement_S()
    {
        movS = true;
    }
    public void movement_D()
    {
        movD = true;
    }
    public void OUTmovement()
    {
        movA = false; movS = false; movD = false; movW = false;
    }


    public void FixedUpdate()
    {
        if(flag_scale){
            switch (scaleAxis){                
            case 0:
                if ((Room.transform.localScale.x > 0.5f && scaleValue < 0) || (Room.transform.localScale.x < 10 && scaleValue > 0)){
                    scaleVec = new Vector3(Room.transform.localScale.x + scaleValue, Room.transform.localScale.y, Room.transform.localScale.z);
                }
                break;
            case 1:
                if ((Room.transform.localScale.y > 0.5f && scaleValue < 0) || (Room.transform.localScale.y < 20 && scaleValue > 0)){
                    scaleVec = new Vector3(Room.transform.localScale.x, Room.transform.localScale.y + scaleValue, Room.transform.localScale.z);
                }
                break;
            case 2:
                if ((Room.transform.localScale.z > 0.5f && scaleValue < 0) || (Room.transform.localScale.z < 10 && scaleValue > 0)){
                    scaleVec = new Vector3(Room.transform.localScale.x, Room.transform.localScale.y, Room.transform.localScale.z + scaleValue);
                }
                break;
            }
            Room.transform.localScale = scaleVec;
        }
        if (movW)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z + velocity);
            Movement.transform.localPosition = new Vector3(Movement.transform.localPosition.x, Movement.transform.localPosition.y, Movement.transform.localPosition.z + velocity);            
        }
        else if (movS)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z - velocity);
            Movement.transform.localPosition = new Vector3(Movement.transform.localPosition.x, Movement.transform.localPosition.y, Movement.transform.localPosition.z - velocity);
        }
        else if (movA)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x - velocity, this.transform.localPosition.y, this.transform.localPosition.z);
            Movement.transform.localPosition = new Vector3(Movement.transform.localPosition.x - velocity, Movement.transform.localPosition.y, Movement.transform.localPosition.z);
        }
        else if (movD)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x + velocity, this.transform.localPosition.y, this.transform.localPosition.z);
            Movement.transform.localPosition = new Vector3(Movement.transform.localPosition.x + velocity, Movement.transform.localPosition.y, Movement.transform.localPosition.z);
        }

        if (flag_sound)
        {                       
            tempo_control -= Time.deltaTime;
            //Debug.Log(tempo_control);
        }
        else sincronizar();
        
        if (!flag_ambiente)
        {
            GameObject.Find("Ambiental").GetComponent<AudioSource>().mute = true;            
        }

        //else GameObject.Find("Ambiental").GetComponent<AudioSource>().mute = false;

        if (Selection_Flag && Sala0.GetComponent<ObjectController>().IsGazed)
        {
            Selection_Flag = false;
            
            SceneManager.LoadScene("StartMenu");
        }

        if (Selection_Flag && Sala1.GetComponent<ObjectController>().IsGazed) 
        {
            Selection_Flag = false;
            
            SceneManager.LoadScene("Electronica");

        }
        
        if (Selection_Flag && Sala2.GetComponent<ObjectController>().IsGazed)
        {
            Selection_Flag = false;
            
            SceneManager.LoadScene("Orquestal");

        }

        if(Selection_Flag && MenuControl.GetComponent<ObjectController>().IsGazed){            
            Selection_Flag = false;
            if(General_Menu.activeSelf){
                MenuControl.GetComponent<AudioSource>().Play();
                General_Menu.SetActive(false);
            }else{
                MenuControl.GetComponent<AudioSource>().Play();
                General_Menu.SetActive(true);
            }
        }
        
    }

}
