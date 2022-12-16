using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Luminosity.IO;
using System;

public class UI_Controller : MonoBehaviour
{
    private string NombreControlTema;//Nombre del tema de control a utilizar
    public static UI_Controller instance;
    public GameObject panelController=null;
    public RectTransform posIni;
    public RectTransform posFin;
    public float VelMov=1.5f;
    public bool InitController_default;// si se activa obtendra valores por defecto para los Prefs
    
    //KeyCodes que manejan los inputs
    public KeyCode Horizontal {get; set;}
    public KeyCode Vertical {get; set;}
    public KeyCode Jump {get; set;}
    public KeyCode Cancel {get; set;}
    public KeyCode Inventory {get; set;}
    public KeyCode Action {get; set;}

    //UI los componentes Text de cada botón
    public Text Horizontal_Neg;
    public Text Horizontal_Pos;
    public Text Vertical_Neg;
    public Text Vertical_Pos;
    public Text Saltar;
    public Text Accion;
    public Text Cancelar;
    public Text Inventario;

    void Awake(){
        //los PlayerPrefs deberian de venir creado y cargados con valores desde el menu principal del juego
        //para terminos de estudio creamos un metodo para crearlos o dar valores por defecto si se activa el booleano
        if(InitController_default)
            InitControllerPrefs();
    }
    // Start is called before the first frame update
    void Start()
    {
        /* ATENCION !!!  Aqui vamos por el valor del tema o configuración controlladora de inputs para el control 1,
        Dentro de dicha configuración deben existir los keyCodes creados o nombrados en este Script,
        para evitar errores creamos los prefs con los mismos nombres pero los KeyCode que posean
        valores positivos y Negativos a su vez, se crearon prefs para cada uno
        agregando _Neg o _Pos a su nombre. */
        NombreControlTema = InputManager.m_instance.m_playerOneScheme.Name;
        
        LoadPrefsController();
        instance= this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input_Class.instance.PC_Controller)
        {
            if(InputManager.GetButtonDown("Cancel")){
            if(!panelController.activeSelf && Time.timeScale==1){//Sí el timeScale es (0) es porque otro panel esta activo
                    panelController.SetActive(true);
                    PrintInputMapController();
                    StartCoroutine("Movimiento1");
            }
            if(panelController.activeSelf && Time.timeScale==0){                
                Time.timeScale=1;
                StartCoroutine("Movimiento2");                
            }
            }
        }
    }

#region Movimiento del Panel       
    IEnumerator Movimiento1(){//Derecha
        while(distance(1) > 0f){  
            //Sumamos cuantos frames queremos que se desplace en X          
            panelController.GetComponent<RectTransform>().position = new Vector3(panelController.GetComponent<RectTransform>().position.x + VelMov , panelController.GetComponent<RectTransform>().position.y,panelController.GetComponent<RectTransform>().position.z);
            yield return new WaitForSeconds(0.05f);            
        }
        Time.timeScale=0;          
    }
    IEnumerator Movimiento2(){//Izquierda
        while(distance(0) > 0f){ 
            //Restamos cuantos frames queremos que se desplace en X           
            panelController.GetComponent<RectTransform>().position = new Vector3(panelController.GetComponent<RectTransform>().position.x - VelMov , panelController.GetComponent<RectTransform>().position.y,panelController.GetComponent<RectTransform>().position.z);
            yield return new WaitForSeconds(0.05f);            
        }   
        panelController.SetActive(false); //Despues de retornar a la posicion inicial de inactiva el panel
    }
    public float distance(int dir){    
        float resta=0;
        if(dir==1)//Derecha
                resta = posFin.position.x - panelController.GetComponent<RectTransform>().position.x;
            else//Izquierda
                resta =  panelController.GetComponent<RectTransform>().position.x - posIni.position.x;
            
        return resta;//retorna un valor positivo siempre
     }
#endregion

#region Carga y Muestra de Botones
     public void InitControllerPrefs(){
        //Metodo para Crear o Modificar Prefs
        //Esté Script presentará errores si la aplicación no detecta los Prefs con los siguientes Nombres
            PlayerPrefs.SetString("Jump","J");
            PlayerPrefs.SetString("Action","X");
            PlayerPrefs.SetString("Cancel","C");
            PlayerPrefs.SetString("Inventory","I");
            PlayerPrefs.SetString("Horizontal_Neg","A");
            PlayerPrefs.SetString("Horizontal_Pos","D");
            PlayerPrefs.SetString("Vertical_Neg","S");
            PlayerPrefs.SetString("Vertical_Pos","W");
     }
     private void LoadPrefsController(){
        //Los Prefs con valores los cargamos a cada KeyCode o Botón       
        Horizontal = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Horizontal_Neg",""));
        InputManager.GetAction(NombreControlTema, "Horizontal").Bindings[0].Negative = Horizontal;
        Horizontal = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Horizontal_Pos",""));
        InputManager.GetAction(NombreControlTema, "Horizontal").Bindings[0].Positive = Horizontal;

        Vertical = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Vertical_Neg",""));
        InputManager.GetAction(NombreControlTema, "Vertical").Bindings[0].Negative = Vertical;
        Vertical = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Vertical_Pos",""));
        InputManager.GetAction(NombreControlTema, "Vertical").Bindings[0].Positive = Vertical;

        Jump = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Jump",""));
        InputManager.GetAction(NombreControlTema, "Jump").Bindings[0].Positive = Jump;

        Action = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Action",""));
        InputManager.GetAction(NombreControlTema, "Action").Bindings[0].Positive = Action;

        Inventory = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Inventory",""));
        InputManager.GetAction(NombreControlTema, "Inventory").Bindings[0].Positive = Inventory;

        Cancel = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Cancel",""));
        InputManager.GetAction(NombreControlTema, "Cancel").Bindings[0].Positive = Cancel;
     }
     public void PrintInputMapController(){
        //Actualizamos el mapa de botones del panel en la UI
        Horizontal_Neg.text=  PlayerPrefs.GetString("Horizontal_Neg","");
        Horizontal_Pos.text=  PlayerPrefs.GetString("Horizontal_Pos","");
        Vertical_Neg.text= PlayerPrefs.GetString("Vertical_Neg","");
        Vertical_Pos.text= PlayerPrefs.GetString("Vertical_Pos","");
        Saltar.text=PlayerPrefs.GetString("Jump","");
        Accion.text= PlayerPrefs.GetString("Action","");
        Cancelar.text= PlayerPrefs.GetString("Cancel","");
        Inventario.text= PlayerPrefs.GetString("Inventory","");
     }
#endregion
    
#region Reconfiguración de Control       
    public void OnClick_Botton_config(string name){        
        //A está función se llama desde cada Botón en el Panel para inicial el cambio de Key
        //Recuerda que esta funcion debe ser publica para poder acceder a ella desde el Inspector en Unity
        switch (name)
        {
           case "Horizontal_Pos" :
                Scan_Input("Horizontal", 1);
           break;
           case "Horizontal_Neg" :
                Scan_Input("Horizontal", -1);
           break;
           case "Vertical_Pos" :
                Scan_Input("Vertical", 1);
           break;
           case "Vertical_Neg" :
                Scan_Input("Vertical", -1);
           break;

           default:
                Scan_Input(name,1);
                break;
        }
              
    }

    public void Scan_Input(string nameButton, int axisValor){
       
        GameObject.Find("Text_Esperando").GetComponent<Text>().text= "Esperando...";
     
        ScanSettings settings = new ScanSettings{
	        ScanFlags = ScanFlags.Key,
	        CancelScanKey = KeyCode.Escape,	    
	        Timeout = 5f        
        };
    
        InputManager.StartInputScan(settings, result =>{
        if(result.Key.ToString()!="None"){
            if(nameButton=="Horizontal" || nameButton=="Vertical"){
                InputManager.GetAction(NombreControlTema, nameButton).Bindings[0].Type = InputType.DigitalAxis;
                if(axisValor<0){
                    InputManager.GetAction(NombreControlTema, nameButton).Bindings[0].Negative = result.Key;
                    PlayerPrefs.SetString(nameButton+"_Neg" ,InputManager.GetAction(NombreControlTema, nameButton).Bindings[0].Negative.ToString());
                }
                if(axisValor>0){       
                    InputManager.GetAction(NombreControlTema, nameButton).Bindings[0].Positive = result.Key;
                    PlayerPrefs.SetString(nameButton+"_Pos" ,InputManager.GetAction(NombreControlTema, nameButton).Bindings[0].Positive.ToString());
                }
            }else{
                InputManager.GetAction(NombreControlTema, nameButton).Bindings[0].Type = InputType.Button;
                InputManager.GetAction(NombreControlTema, nameButton).Bindings[0].Positive = result.Key;
                PlayerPrefs.SetString(nameButton ,InputManager.GetAction(NombreControlTema, nameButton).Bindings[0].Positive.ToString());
            }     
            PrintInputMapController();
        }
        GameObject.Find("Text_Esperando").GetComponent<Text>().text= "";
	    return true;
        });
    }
#endregion
}

