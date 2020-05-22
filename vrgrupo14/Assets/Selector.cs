using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;


public class Selector : MonoBehaviour
{

    [SerializeField] int contador = 3;
    [SerializeField] int contador2 = 1;
    public static double tempo = 9;
    public static double tempo_control = tempo;
    public static bool flag_exit = false;
    public static bool flag_enter = false;
    public static bool flag_remove = false;
    public static bool flag_ambiente = false;
    public static bool flag_sound = false;
    public bool mov = false;
    
    Scene scene;

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

    public void borrarInp()
    {
        StartCoroutine("Countdown2", contador2);
    }
    public void borrarOutp()
    {
        contador2 = 1;
        StopCoroutine("Countdown2");
    }

    private IEnumerator Countdown(int contador)
    {
        while (contador > 0)
        {
            contador--;
            yield return new WaitForSeconds(1);
        }
        //Debug.Log("Countdown Complete!");

        flag_exit = true;
        flag_enter = true;
    }
    private IEnumerator Countdown2(int contador2)
    {
        while (contador2 > 0)
        {
            contador2--;
            yield return new WaitForSeconds(1);
        }
        //Debug.Log("Countdown Complete!");

        flag_remove = true;
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
        flag_exit = false; flag_enter = false; flag_remove = false;
        scene = SceneManager.GetActiveScene();
        sincronizar();
        flag_ambiente = true;

        tempo_control = 0;
    }

    public void INmovement()
    {
        mov = true;
    }
    public void OUTmovement()
    {
        mov = false;
    }


    private void FixedUpdate()
    {
        if (mov)
        {
            //this.transform.localPosition = new Vector3(this.transform.localPosition.x + 0.015f, this.transform.localPosition.y, this.transform.localPosition.z);
            this.transform.localPosition += this.transform.forward*0.015f;
        }
        if (flag_sound)
        {
            tempo_control -= Time.deltaTime;
            //Debug.Log(tempo_control);
        }
        else sincronizar();
        

        if (flag_exit && scene.name == "GameScene")
        {
            flag_exit = false;
            SceneManager.LoadScene("StartMenu");

        }
        if (flag_enter && scene.name == "StartMenu")
        {
            flag_enter = false;
            SceneManager.LoadScene("GameScene");

        }
        if (scene.name == "GameScene")
        {
            if (!flag_ambiente)
            {
                GameObject.Find("Ambiental").GetComponent<AudioSource>().mute = true;

            }
            else GameObject.Find("Ambiental").GetComponent<AudioSource>().mute = false;
        }
    }

}
