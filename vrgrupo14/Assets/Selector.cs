using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Selector : MonoBehaviour
{
    
    [SerializeField]int contador = 3;
    public static double tempo = 9;
    public static double tempo_control = tempo;
    public static bool flag_exit = false;
    public static bool flag_enter = false;
    public static bool flag_sound;
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

    private IEnumerator Countdown(int contador)
    {
        while (contador > 0)
        {
            Debug.Log(contador--);
            yield return new WaitForSeconds(1);
        }
        Debug.Log("Countdown Complete!");
        flag_exit = true; flag_enter = true;
    }

    private void Start()
    {
        flag_exit = false; flag_enter = false;
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


    }



}
