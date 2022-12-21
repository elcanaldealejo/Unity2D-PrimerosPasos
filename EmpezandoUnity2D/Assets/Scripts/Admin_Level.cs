using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Luminosity.IO;

public class Admin_Level : MonoBehaviour
{
    public static Admin_Level instance;
[Header("Configuración de Nivel")]
    public int NumLevel;
    public bool mundo_passed;
    public Transform puntoAparicion;    
    public int tiempoMax;
    private int tiempoActual;

[Header("UI en Pantalla")]
    public Image telon;
    public Text tiempoPantalla;
    public Text monedasPantalla;
    private int monedasCapturadas;
    public GameObject panel_pausa = null;
    public Text texto_pausa;
    public Button continuarCheck=null;

[Header("Grupos de Enemigos - Coleccionables y más...")]
    //Monedas
    public GameObject[] monedasObject;
    private bool[] monedasActivas;
    private int totalMonedas;
    //Enemigos
    public GameObject[] enemyObject;
    private bool[] enemyActivos;

[Header("Prefab Jugador")]
    public GameObject PersonajePref;
    private GameObject ObjetoPrefab;
    

[Header("Puntos de Reaparición")]
    public int check=0;
    public Transform[] CheckPoints;
    
    void Awake(){
        instance = this;
        ObjetoPrefab = Instantiate(PersonajePref);
        ObjetoPrefab.transform.position = puntoAparicion.position;
        if(PlayerPrefs.GetString("mundoCoins"+NumLevel,"")=="")//Sí nunca se creo el prefs se inicializa
            crearPrefsCoinsMundo();
        if(PlayerPrefs.GetString("mundoEnemy"+NumLevel,"")=="")//Sí nunca se creo el prefs se inicializa
            crearPrefsEnemyMundo();
    }
    void Start()
    {              
        totalMonedas = PlayerPrefs.GetInt("monedasPrefs",0);// Traemos el valor de las monedas totales ya ganadas
        //Iniciamos los arreglos de las collecciones con el tamaño ofrecido en el insperctor
        monedasActivas = new bool[monedasObject.Length];
        enemyActivos = new bool[enemyObject.Length];
        //Leemos los prefs con la información de las colleciones (Monedas ya capturados o no? y enemigos ya vencidos o no?)
        inicializarMonedas(PlayerPrefs.GetString("mundoCoins"+NumLevel,""));
        inicializarEnemigos(PlayerPrefs.GetString("mundoEnemy"+NumLevel,""));
        StartCoroutine("abreTelon");
        tiempoActual = tiempoMax;  // el tiempo total de la escena se transfiere a una variable temporal que va ir disminuyendo  
        //para que se ejecuten las animaciones del prefab clonado (personaje) le ofrecemos el animator de este al administrador de animaciones  
        AdminAnimations.instance._animador = ObjetoPrefab.GetComponent<Animator>();
    }    
    public GameObject prefbLevel(){
        /* 
        El clon del personaje es privado asi que si queremos compartirlo o realizar cambios a este
        es necesario siempre pedirlo por este metodo publico.
        */
        return ObjetoPrefab;
    }
    void Update()
    {        
        ControlTiempo();//el tiempo de juego se disminuye y se imprime a la vez
        monedasPantalla.text = (totalMonedas + monedasCapturadas).ToString("D8");//imprimimos monedas en pantalla
        if(mundo_passed && !telon.enabled)//cuando se alcance el ultimo checkpoint se termina la escena
            TerminaEscena();

        if(UI_Vida.instance.sliderVida.value<=0 && !panel_pausa.activeSelf)//Evaluamos constantemente la vida del personaje
            PierdeEscena();

        if(!ObjetoPrefab.activeSelf && !panel_pausa.activeSelf){
                StartCoroutine("cerrarTelon");
                panel_pausa.SetActive(true);
                if(panel_pausa.activeSelf && check>0)
                    continuarCheck.interactable=true;
            
        }
        if(InputManager.GetButtonDown("Exit")){
            pausaNivel();
            if(panel_pausa.activeSelf && check>0)
                continuarCheck.interactable=true;
            
        }
       
    }
    public void puntocontrol(){
        
            AdminCamera2D.instance.reIniciaSeguimiento(puntoAparicion);
            Time.timeScale = 1;
            StartCoroutine("abreTelon");
            if(UI_Vida.instance.sliderVida.value<=0){                
                UI_Vida.instance.Revivir();
                Admin_Movimientos.instance.Iniciar_MOVs();
                //El Objetivo se activa, pero primero se le da la posición de donde debe reaparecer
                               
            }  
            ObjetoPrefab.SetActive(true);           
            pausaNivel();
       
    }
    public void ReiniciarEscena(){
        SceneManager.LoadScene("Mundo"+(NumLevel+1));
        Time.timeScale = 1;
    }
    public void DejarEscena(){
        SceneManager.LoadScene("City_1");
        Time.timeScale = 1;
    }
    private void pausaNivel(){
        if(!panel_pausa.activeSelf){
            panel_pausa.SetActive(true);
            texto_pausa.text = "Pausa";
            Time.timeScale = 0;
        }else{
            panel_pausa.SetActive(false);
            Time.timeScale = 1;
            }
    }
    private void TerminaEscena(){
        StartCoroutine("cerrarTelon");
        //se terminan e inician nuevos sonidos
        AudioManager.instance.StopSounds(0);
        //detenemos la interacción usuario personaje e animaciones
        Admin_Movimientos.instance.Detener_MOVs();
        AdminAnimations.instance._animador.enabled = false;
        // Si alcanzo el fin se vuelve a iniciar los prefs para que aparezcan todos 
        // los objetos de nuevo en la escena la proxima vez que se inicie el mundo.
        crearPrefsCoinsMundo();
        crearPrefsEnemyMundo();
        ActualizarPrefsMundos(NumLevel);
        PlayerPrefs.SetInt("monedasPrefs",totalMonedas + monedasCapturadas);  
    }
    public void ActualizarPrefsMundos(int nivel){       
        int cont=0;
        string constructo="";        
        for (int i = 0; i < PlayerPrefs.GetString("info_mundos","").Length; i++)
        {
            if(PlayerPrefs.GetString("info_mundos","").Substring(i,1)!="-"){
                constructo += nivel==cont ? "1" : PlayerPrefs.GetString("info_mundos","").Substring(i,1);
                cont++;
            }
            else{
                constructo += "-";
            }
        }
        PlayerPrefs.SetString("info_mundos",constructo);
    }
    private void PierdeEscena(){
        StartCoroutine("cerrarTelon");
        //se terminan e inician nuevos sonidos
        AudioManager.instance.StopSounds(0);
        //detenemos la interacción usuario personaje e animaciones
        Admin_Movimientos.instance.Detener_MOVs();
        ObjetoPrefab.GetComponent<Rigidbody2D>().Sleep();//detenemos la fisicas temporalmente
        panel_pausa.SetActive(true);
        if(panel_pausa.activeSelf && check>0)
                continuarCheck.interactable=true;
        texto_pausa.text = "Game Over";
    }
    public void ActualizarPrefsCoinsMundo(){//En cada punto de salvado de escena se actualiza el prefs
        string constructo="";
         for (int i=0;i<monedasObject.Length;i++){            
                constructo += monedasActivas[i] ? "1-" : "0-";
         }
         PlayerPrefs.SetString("mundoCoins"+NumLevel,constructo);
    }
    public void ActualizarPrefsEnemyMundo(){//En cada punto de salvado de escena se actualiza el prefs
        string constructo="";
         for (int i=0;i<enemyObject.Length;i++){            
                constructo += enemyActivos[i] ? "1-" : "0-";
         }
         PlayerPrefs.SetString("mundoEnemy"+NumLevel,constructo);
    }
    public void crearPrefsCoinsMundo(){//crear la memoria del coleccionable vacia con todos los objetos disponibles en escena
        string constructo="";
         for (int i=0;i<monedasObject.Length;i++){            
                constructo += "0-";
         }
         PlayerPrefs.SetString("mundoCoins"+NumLevel,constructo);
    }
    public void crearPrefsEnemyMundo(){//crear la memoria del coleccionable vacia con todos los objetos disponibles en escena
        string constructo="";
         for (int i=0;i<enemyObject.Length;i++){            
                constructo += "0-";
         }
         PlayerPrefs.SetString("mundoEnemy"+NumLevel,constructo);
    }
    private void inicializarMonedas(string mundo_coins){
        /* 
        Según lo que traiga el prefs del mundo(Monedas), el controlará el estado de cada GameObject_moneda
        entrado en el Arreglo. El lo identifica:
        Ejemplo 1
        0-0-0-0-0-0-    Aqui inicializa 6 monedas todas activas en la escena 
        Ejemplo 2
        1-1-1-0-1-0-    Aqui inicializa 6 monedas pero ya capturadas 4 y aún activas en escena 2
         */
        int cont=0;
        for (int i=0;i<mundo_coins.Length;i++){
            if(mundo_coins.Substring(i,1)!="-"){
                if( mundo_coins.Substring(i,1)=="0"){
                    monedasActivas[cont]=false; 
                    cont++;                   
                }else{
                    monedasActivas[cont]=true;
                    monedasObject[cont].SetActive(false);
                    cont++;
                    monedasCapturadas++; 
                }
            }           
        }
    }
    private void inicializarEnemigos(string mundo_enemy){
        /* 
        Según lo que traiga el prefs del mundo(enemigos), el controlará el estado de cada GameObject_enemigo
        entrado en el Arreglo. El lo identifica:
        Ejemplo 1
        0-0-0-0-0-0-    Aqui inicializa 6 enemigos todos activos en la escena 
        Ejemplo 2
        1-1-1-0-1-0-    Aqui inicializa 6 enemigos pero ya derrotados 4 y aún activos en escena 2
         */
        int cont=0;
        for (int i=0;i<mundo_enemy.Length;i++){
            if(mundo_enemy.Substring(i,1)!="-"){
                if( mundo_enemy.Substring(i,1)=="0"){
                    enemyActivos[cont]=false; 
                    cont++;                   
                }else{
                    enemyActivos[cont]=true;
                    enemyObject[cont].SetActive(false);
                    cont++; 
                }
            }            
        }
    }
    public void controlMonedas(GameObject coin){// este metodo se llamará donde se capture la moneda
        monedasCapturadas++;
        AudioManager.instance.PlaySounds(1);
        for(int i=0;i<monedasObject.Length;i++){
            if(monedasObject[i]==coin){
                monedasObject[i].SetActive(false);
                monedasActivas[i]=true;
                return;
            }
        }      
    }
    public void controlEnemigos(GameObject enemy){//este metodo se llamará donde se derrote al enemigo
        //AudioManager.instance.PlaySounds(1);
        for(int i=0;i<enemyObject.Length;i++){
            if(enemyObject[i]==enemy){
                enemyObject[i].SetActive(false);
                enemyActivos[i]=true;
                return;
            }
        }   
    }
    private void ControlTiempo(){
        if(Time.timeScale>0 && tiempoActual>0){
            tiempoActual = tiempoMax - (int)Time.time;
            tiempoPantalla.text =(tiempoMax - (int)Time.time).ToString("D3");
        }
        if(tiempoActual==0 && telon.enabled==false){
            StartCoroutine("cerrarTelon");
            Admin_Movimientos.instance.Detener_MOVs();
        }
    }
    IEnumerator abreTelon(){
        Color c = new Color(0f,0f,0f,0f);
        while(telon.color.a>c.a){
            telon.color = new Color(0f,0f,0f,telon.color.a-0.01f);
            yield return new WaitForSeconds(0.01f);   
        }
        telon.color = new Color(0f,0f,0f,0f);
        telon.enabled = false;
    }
    IEnumerator cerrarTelon(){
        telon.enabled = true;
        Color c = new Color(0f,0f,0f,1f);
        while(telon.color.a<c.a){
            telon.color = new Color(0,0,0,telon.color.a+0.2f);
            yield return new WaitForSeconds(0.01f);        
        }
        telon.color = new Color(0f,0f,0f,1f);
        if(mundo_passed){
            SceneManager.LoadScene("City_1");
            Time.timeScale = 1;
        }else
            Time.timeScale = 0;
    }
}
