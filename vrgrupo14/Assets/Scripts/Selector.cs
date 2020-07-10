using System.Collections;
using System.Collections.Generic;
using GoogleVR.HelloVR;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;


public class Selector : MonoBehaviour
{

    [SerializeField] int contador = 3; //interfaz general
    [SerializeField] int contador2 = 1; //funciones propias
    public static double tempo = 9; //tempo de los sonidos para sincronizar
    public static double tempo_control = tempo; 

    //flags de interfaz y control
    public static bool Selection_Flag = false; 
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
        contador = 3;
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


    public static bool delante(bool x)
    {
        return x;
    }
    public static bool detras(bool x) { return x; }
    public static bool derecha(bool x) { return x; }
    public static bool izquierda(bool x) { return x; }

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
        contador = 3;
        tempo_control = 0;
    }

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
        if (movW)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z + 0.015f);
            
        }
        if (movS)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z - 0.015f);
            
        }
        if (movA)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x - 0.015f, this.transform.localPosition.y, this.transform.localPosition.z);
            
        }
        if (movD)
        {
            this.transform.localPosition = new Vector3(this.transform.localPosition.x + 0.015f, this.transform.localPosition.y, this.transform.localPosition.z);
            
        }
        if (flag_sound)
        {
            Debug.Log("hola1234");            
            tempo_control -= Time.deltaTime;
            //Debug.Log(tempo_control);
        }
        else sincronizar();
        
        if (!flag_ambiente)
        {
            GameObject.Find("Ambiental").GetComponent<AudioSource>().mute = true;
            Debug.Log("hola");
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
        
    }

}
