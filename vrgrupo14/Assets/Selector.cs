using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;


public class Selector : MonoBehaviour
{
    
    [SerializeField]int contador = 3;
    public static double tempo = 9;
    public static double tempo_control = tempo;
    public static bool flag_exit = false;
    public static bool flag_enter = false;
    public static bool flag_sound;
    public static bool flag_remove = false;
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
        StartCoroutine("Countdown2", contador);
    }
    public void borrarOutp()
    {
        contador = 3;
        StopCoroutine("Countdown2");
    }

    private IEnumerator Countdown(int contador)
    {
        while (contador > 0)
        {
            Debug.Log(contador--);
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Countdown Complete!");

        flag_exit = true;
        flag_enter = true;
    }
    private IEnumerator Countdown2(int contador)
    {
        while (contador > 0)
        {
            Debug.Log(contador--);
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Countdown Complete!");

        flag_remove = true;
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
        flag_exit = false; flag_enter = false; flag_remove = false;
        scene = SceneManager.GetActiveScene();
        sincronizar();
    }
    private void FixedUpdate()
    {
        if (flag_sound)
        {
            tempo_control -= Time.deltaTime;
           // Debug.Log(tempo_control);
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
        if (flag_remove)
        {
            flag_remove = false;
        }
        if (scene.name == "GameScene")
        {
            if (!AudioTime.flag_ambiente)
            {
                GameObject.Find("Ambiental").GetComponent<AudioSource>().mute = true;

            }
            else GameObject.Find("Ambiental").GetComponent<AudioSource>().mute = false;
        }
    }



}
