using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

[Header("Grupos de Enemigos - Coleccionables y más...")]
    public GameObject[] monedasObject;
    private bool[] monedasActivas;
    private int totalMonedas;

[Header("Prefab Jugador")]
    public GameObject PersonajePref;
    private GameObject ObjetoPrefab;
    

[Header("Puntos de Reaparición")]
    public int check=0;
    public Transform[] CheckPoints;
    
    void Awake(){
        ObjetoPrefab = Instantiate(PersonajePref);
        ObjetoPrefab.transform.position = puntoAparicion.position;
        if(PlayerPrefs.GetString("mundoCoins"+NumLevel,"")=="")//Sí nunca se creo el prefs se inicializa
            crearPrefsCoinsMundo();
    }
    void Start()
    {      
        instance = this;
        totalMonedas = PlayerPrefs.GetInt("monedasPrefs",0);
        monedasActivas = new bool[monedasObject.Length];
        inicializarMonedas(PlayerPrefs.GetString("mundoCoins"+NumLevel,""));
        StartCoroutine("abreTelon");
        tiempoActual = tiempoMax;      
        AdminAnimations.instance._animador = ObjetoPrefab.GetComponent<Animator>();
    }    
    public GameObject prefbLevel(){
        return ObjetoPrefab;
    }
    void Update()
    {        
        ControlTiempo();
        monedasPantalla.text = (totalMonedas + monedasCapturadas).ToString("D8");
        if(mundo_passed && !telon.enabled)
            TerminaEscena();
        if(UI_Vida.instance.sliderVida.value<=0)
            PierdeEscena();

        if(Input.GetKey("r") && (!ObjetoPrefab.activeSelf || UI_Vida.instance.sliderVida.value<=0)){//Si el objetivo esta inactivo y se preciona "r" se llamará reaparición en punto
                
                AdminCamera2D.instance.reIniciaSeguimiento(puntoAparicion);
                UI_Vida.instance.Revivir();
                Admin_Movimientos.instance.Iniciar_MOVs();
                ObjetoPrefab.SetActive(true);//el Objetivo se activa, pero primero se le da la posición de donde debe reaparecer
                StartCoroutine("abreTelon");
        }
    }
    private void TerminaEscena(){
        StartCoroutine("cerrarTelon");
        AudioManager.instance.StopSounds(0);
        Admin_Movimientos.instance.Detener_MOVs();
        AdminAnimations.instance._animador.enabled = false;
        crearPrefsCoinsMundo();
        PlayerPrefs.SetInt("monedasPrefs",totalMonedas + monedasCapturadas);  
    }
    private void PierdeEscena(){
        StartCoroutine("cerrarTelon");
        AudioManager.instance.StopSounds(0);
        Admin_Movimientos.instance.Detener_MOVs();
        ObjetoPrefab.GetComponent<Rigidbody2D>().Sleep();
    }
    public void ActualizarPrefsCoinsMundo(){
        string constructo="";
         for (int i=0;i<monedasObject.Length;i++){            
                constructo += monedasActivas[i] ? "1-" : "0-";
         }
         PlayerPrefs.SetString("mundoCoins"+NumLevel,constructo);
    }
    public void crearPrefsCoinsMundo(){
        string constructo="";
         for (int i=0;i<monedasObject.Length;i++){            
                constructo += "0-";
         }
         PlayerPrefs.SetString("mundoCoins"+NumLevel,constructo);
    }
    private void inicializarMonedas(string mundo_coins){
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
    public void controlMonedas(GameObject coin){
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
            telon.color = new Color(0,0,0,telon.color.a+0.01f);
            yield return new WaitForSeconds(0.01f);        
        }
        telon.color = new Color(0f,0f,0f,1f);
        
    }
}
