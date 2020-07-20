using System.Collections;
using System.Collections.Generic;
using GoogleVR.HelloVR;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;


public class Selector : MonoBehaviour
{
    [SerializeField]private GameObject Movement;
    [SerializeField]private GameObject Room;
    [SerializeField]private GameObject ResonanceRoom;
    [SerializeField]private GameObject MenuControl;
    [SerializeField]private GameObject Canvas_Menu;
    [SerializeField]private GameObject Pivote;
    [SerializeField]public GameObject camara;
    GameObject padre;

    [SerializeField] int contador = 1; //interfaz general

    public static AudioSource uiSound;
    public static AudioSource uiSound2;

    Component[] audioComponents;
    [SerializeField] float velocity = 15f;    
    [SerializeField] float scaleValue = 0.5f;    
    float audioPitch;
    int instruccion;
    Vector3 scaleVec;
    int scaleAxis;
    bool passScale;
    public static double tempo = 9; //tempo de los sonidos para sincronizar
    public static double tempo_control = tempo;     

    //flags de interfaz y control

    public AudioMixer master;
    public static bool play = true;
    [SerializeField] public Material mat_Play;
    [SerializeField] public Material mat_Pause;
    
    public static bool Selection_Flag = false;
    public static bool flag_scale = false;
    public static bool flag_ambiente = false;
    public static bool flag_sound = false;
    public static bool flag_scan = false;    
    Color activo = Color.green;
    Color inactivo = Color.red;

    //movimiento
    public bool movW = false;
    public bool movD = false;
    public bool movS = false;
    public bool movA = false;

    Preset[] roomPresets = new Preset[6];
    Scene scene;

    //salas de musica
    public GameObject Sala0; //Menu principal
    public GameObject Sala1; //sala Electronica
    public GameObject Sala2; //sala Orquestal   
    GameObject Ambiente;
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
        playSelectSound();
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
        padre = GameObject.FindGameObjectWithTag("sonidospadre");
        Selection_Flag = false;
        storePresets();
        scene = SceneManager.GetActiveScene();
        sincronizar();
        
        if(SceneManager.GetActiveScene().name != "StartMenu")
        GetRoomInfo();
        
        audioPitch = 1f;
        instruccion = -1;
        flag_ambiente = true;
        contador = 1;
        tempo_control = 0;
        velocity *= 0.001f;
        uiSound = this.gameObject.GetComponents<AudioSource>()[0];
        uiSound2 = this.gameObject.GetComponents<AudioSource>()[1];

        Ambiente = GameObject.Find("Ambiental");
    }
//escalado de la sala
    public void ScalePositive(int axis){
        scaleValue = Mathf.Abs(scaleValue);        
        ScaleRoom(axis);
    }

    public void ScaleNegative(int axis){
        scaleValue = -Mathf.Abs(scaleValue);        
        ScaleRoom(axis);
    }
    public void ScaleRoom(int axis){
        playInSound();     
        scaleAxis = axis;
        flag_scale = true;
    }

    public void OUTscale()
    {
        playOutSound();
        flag_scale = false;
    }
//Control del menu

    public GameObject General_M;
    public GameObject Escalas_M;
    public GameObject Presets_M;
    public GameObject Config_M;
    public void Escalas_In(){
        playInSound();
        StartCoroutine("Escalas", 1);
    }
    public void Escalas_Out(){
        playOutSound();
        StopCoroutine("Escalas");
    }
    private IEnumerator Escalas(int time){
        yield return new WaitForSeconds(time);
        playSelectSound();

        General_M.SetActive(false);
        Escalas_M.SetActive(!Escalas_M.activeSelf);

    }
    public void Presets_In(){
        playInSound();
        StartCoroutine("Presets", 1);
    }
    public void Presets_Out(){
        playOutSound();
        StopCoroutine("Presets");
    }
    private IEnumerator Presets(int time){
        yield return new WaitForSeconds(time);
        playSelectSound();
        GetRoomInfo();
        General_M.SetActive(false);
        Presets_M.SetActive(!Presets_M.activeSelf);

    }
    public void Config_In(){
        playInSound();
        StartCoroutine("Config", 1);
    }
    public void Config_Out(){
        playOutSound();
        StopCoroutine("Config");
    }
    private IEnumerator Config(int time){
        yield return new WaitForSeconds(time);
        playSelectSound();

        General_M.SetActive(false);
        Config_M.SetActive(!Config_M.activeSelf);

    }    

    public void closeAll(){
        Escalas_M.SetActive(false);
        Presets_M.SetActive(false);
        Config_M.SetActive(false);
        General_M.SetActive(true);
    }
//movimiento del jugador
    public void movement_W()
    {
        playInSound();
        movW = true;
    }
    public void movement_A()
    {
        playInSound();
        movA = true;
    }
    public void movement_S()
    {
        playInSound();
        movS = true;
    }
    public void movement_D()
    {
        playInSound();
        movD = true;
    }
    public void OUTmovement()
    {
        playOutSound();
        movA = false; movS = false; movD = false; movW = false;
    }
    //presets
    public void setPreset(int index){
        roomPresets[index].setPreset(ResonanceRoom);
    }

    //Métodos 
    public static void playInSound(){        
        uiSound.pitch = 1f;            
        uiSound.volume = 0.75f;
        if(!uiSound.isPlaying){            
            uiSound.Play();                       
        }
    }

    public static void playOutSound(){        
        uiSound.volume = 1f;
        uiSound.pitch = 0.5f;
        if(!uiSound2.isPlaying){
            uiSound.Play();
        }    
    }

    public static void playSelectSound(){
        uiSound2.Play();
    }


    private IEnumerator controlador(int time){
        yield return new WaitForSeconds(time);
        playSelectSound();
        switch(instruccion){
            case 0:
                playPauseStop(false);
                break;
            case 1:
                stop();
                break;            
            default: break;
        }

    }
    
    public void controladorInstruccion(int n){
        playInSound();
        instruccion = n;
        StartCoroutine("controlador",1);
    }
    public void playPauseStop(bool stop){        
        play = !play && !stop;

        if(play){
            Config_M.transform.GetChild(2).transform.GetChild(4).GetComponent<Renderer>().material = mat_Pause;
            Ambiente.GetComponent<AudioSource>().Play();                
        }
        else if(stop){
            Config_M.transform.GetChild(2).transform.GetChild(4).GetComponent<Renderer>().material = mat_Play;
            Ambiente.GetComponent<AudioSource>().Stop();                
        }
        else{
            Config_M.transform.GetChild(2).transform.GetChild(4).GetComponent<Renderer>().material = mat_Play;
            Ambiente.GetComponent<AudioSource>().Pause();                
        }

        audioComponents = padre.GetComponentsInChildren<AudioSource>();

        foreach (AudioSource audio in audioComponents){
            if(play){
                audio.Play();
            }
            else if(stop){
                audio.Stop();
            }
            else{
                audio.Pause();
            }
        }

        
    }
    
    public void stop(){
        audioPitch = 1f;
        master.SetFloat("pitch", audioPitch);
        playPauseStop(true);
    }

    public void Exit_Instruction(){
        playOutSound();
        StopCoroutine("controlador");
    }

    int pitch_flag = 0;
    public void Pitch_In(int n){
        playInSound();
        pitch_flag = n;
    }
    public void Pitch_Out(){
        playOutSound();
        pitch_flag = 0;
    }
    public void PitchMas(){
        audioPitch += 0.001f;
        master.SetFloat("pitch", audioPitch);
    }
    public void PitchMenos(){
        audioPitch -= 0.001f;
        master.SetFloat("pitch", audioPitch);
    }

    public GameObject presetName;
    public GameObject roomInfo;    
    public void setP_In(int index){
        playInSound();
        StartCoroutine(PressetSettings(1,index));
    }
    public void setP_Out(){
        playOutSound();
        StopAllCoroutines();
    }
    public void GetRoomInfo(){ //magia amigos, magia
            roomInfo.GetComponent<TextMeshProUGUI>().text = 
            Room.GetComponent<Transform>().localScale+"\n"+
            "Frente: "+ResonanceRoom.GetComponent<ResonanceAudioRoom>().frontWall+"\n"+
            "Atrás: "+ResonanceRoom.GetComponent<ResonanceAudioRoom>().backWall+"\n"+
            "Izquierda: "+ResonanceRoom.GetComponent<ResonanceAudioRoom>().leftWall+"\n"+
            "Derecha: "+ResonanceRoom.GetComponent<ResonanceAudioRoom>().rightWall+"\n"+
            "Techo: "+ResonanceRoom.GetComponent<ResonanceAudioRoom>().ceiling+"\n"+
            "Suelo: "+ResonanceRoom.GetComponent<ResonanceAudioRoom>().floor+"\n";
    }
    
    private IEnumerator PressetSettings(int time, int index){
        yield return new WaitForSeconds(time);
        playSelectSound();
        
            setPreset(index);
            presetName.GetComponent<TextMeshProUGUI>().text = roomPresets[index].presetName;

            GetRoomInfo();

    }

    public void storePresets(){
        float[] auxRevProperties;
        
        roomPresets[0] = new Preset("Default");
        auxRevProperties = new float[] {22,22,14,9,22,22,1.11f,-0.03f,0.09f,1.23f};
        roomPresets[0].setSize(1f,1f,1f);
        roomPresets[0].setMaterials(auxRevProperties);
        
        roomPresets[1] = new Preset("Sala de cine");
        auxRevProperties = new float[] {19,19,10,0,6,7,1.11f,-0.03f,0.09f,1.23f};
        roomPresets[1].setSize(3f,1.5f,1.7f);
        roomPresets[1].setMaterials(auxRevProperties);

        roomPresets[2] = new Preset("Baño");
        auxRevProperties = new float[] {1,1,20,15,1,1,1.11f,-0.03f,0.09f,1.23f};
        roomPresets[2].setSize(0.5f,1f,0.7f);
        roomPresets[2].setMaterials(auxRevProperties);
        
        roomPresets[3] = new Preset("Bunker");
        auxRevProperties = new float[] {13,13,4,4,13,13,1.11f,-0.03f,0.09f,1.23f};
        roomPresets[3].setSize(1.2f,0.5f,2f);
        roomPresets[3].setMaterials(auxRevProperties);

        roomPresets[4] = new Preset("Salón");
        auxRevProperties = new float[] {9,15,11,15,5,6,1.11f,-0.03f,0.09f,1.23f};
        roomPresets[4].setSize(2f,2f,2f);
        roomPresets[4].setMaterials(auxRevProperties);

        roomPresets[5] = new Preset("Catedral");
        auxRevProperties = new float[] {9,9,12,12,9,12,1.11f,-0.03f,0.09f,1.23f};
        roomPresets[5].setSize(6f,13f,6f);
        roomPresets[5].setMaterials(auxRevProperties);
        
    }

    public bool CheckAmbiente(){
        
        
        for(int i = 0; i < padre.transform.childCount; i++){
            if (padre.transform.GetChild(i).gameObject.activeSelf){
                return false;
            }
        }

        return true;
    }
    
    public void FixedUpdate()
    {   
        switch(pitch_flag){
            case 1:
                PitchMas();
                break;
            case 2:
                PitchMenos();
                break;
            default: break;            
        } 
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
        
        flag_ambiente = CheckAmbiente();

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
        if (flag_sound && play)
        {                       
            tempo_control -= Time.deltaTime;
        }
        else sincronizar();
        
        if (!flag_ambiente)
        {
            Ambiente.GetComponent<AudioSource>().mute = true;
        }else Ambiente.GetComponent<AudioSource>().mute = false;

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
        if(!Canvas_Menu.activeSelf){
            Pivote.GetComponent<Transform>().rotation = new Quaternion(0,camara.GetComponent<Transform>().rotation.y,0,camara.GetComponent<Transform>().rotation.w);
        }
        if(Selection_Flag && MenuControl.GetComponent<ObjectController>().IsGazed){         
            Selection_Flag = false;
            if(Canvas_Menu.activeSelf){
                MenuControl.GetComponent<Renderer>().material.color = inactivo;
                closeAll();
                Canvas_Menu.SetActive(false);
            }else{
                MenuControl.GetComponent<Renderer>().material.color = activo;
                closeAll();
                Canvas_Menu.SetActive(true);
            }
        }
        
    }
}
