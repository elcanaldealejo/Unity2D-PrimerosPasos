using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Luminosity.IO;


public class UI_Dialogo : MonoBehaviour
{
    public static UI_Dialogo instance;
    public int Id_Dialogo;//Representa al dialogo que se debe cargar para este NPC
    public string language = "ES";// esta variable deberia capturar un Prefs configurado desde las opciones del juego en MenuPPAL para Idioma
    public string nameNPC;//Todos los Enemigos, Personajes, Objetos etc... deberian poseer un objeto con informacion propia de su clase. aqui se tendria que traer el nombre de ese personaje

    [Header("Elementos UI ")] 
    [SerializeField] public GameObject PanelDialogo = null;
    [SerializeField] private GameObject Botonaction = null;
    [SerializeField] private Text nameHabla = null;
    [SerializeField] private Text dialogo = null; 
    [SerializeField] private Image imageTalk = null;
    private int recorreTextos=0; //Cada vez que damos botón continuar aumenta su valor para mostrar la oración siguiente de la conversación
    private string[] Conversacion;//Almacena en cada espacio una oración
     
    [Header("Parametros Area Overlap")] 
            [SerializeField] private float Angle=0.0f;
            private Transform objeto =null;//para crear el ovelap tiene como referencia la posicion del gameObject NPC

    [Header("Parametros Detección")] 
        [SerializeField] private LayerMask Mascara_Layer=0;// seleccionamos que layer podra interactuar y conversar con es NPC
        
    void Start()
    {   
        instance = this;     
        objeto = this.transform;        
    }

    void Update()
    {
        if(!PanelDialogo.activeSelf)//mientras el panel este INACTIVO puede evaluar el Overlap
            Muestra_OverLap();
        else{
            //si el panel esta ACTIVO el botón para iniciar la conversación desaparece
            Botonaction.SetActive(false);                
            if(InputManager.GetButtonUp("Action") && recorreTextos<Conversacion.Length){//Para continuar conversando, recorra el dialogo
                imageTalk.sprite = personajeHabla(recorreTextos);                
                dialogo.text = Conversacion[recorreTextos];
                recorreTextos++;
                if(recorreTextos>=Conversacion.Length){//Al finalizar la conversación... Haga
                    Time.timeScale= 1;
                    PanelDialogo.SetActive(false); 
                    recorreTextos=0;               
                }    
            }
        }
        if(InputManager.GetButtonUp("Action") && !PanelDialogo.activeSelf && Botonaction.activeSelf){//Abrir panel e iniciar la conversación
                                    
            Conversacion = CargaConversacion();//Cargue la conversación para este NPC apenas vaya abrir el panel
        
            Time.timeScale= 0;
            PanelDialogo.SetActive(true);
            if(Input_Class.instance.PC_Controller){
                GameObject.Find("Text_cancelB").GetComponent<Text>().text = InputManager.GetAction("PC_control 1", "Exit").Bindings[0].Positive+"";
                GameObject.Find("Text_nextB").GetComponent<Text>().text = InputManager.GetAction("PC_control 1", "Action").Bindings[0].Positive+"";
            }
            imageTalk.sprite = personajeHabla(recorreTextos);            
            dialogo.text = Conversacion[recorreTextos];
            recorreTextos++;       
        }
        if(InputManager.GetButton("Exit") && PanelDialogo.activeSelf){//Salir de la conversación            
            Time.timeScale= 1;
            PanelDialogo.SetActive(false);
            recorreTextos=0;
        }    
    }
private string[] CargaConversacion(){    
    switch (language){//Según sea el Idioma se cargará la conversación con identificador definido para este NPC
        case "ES":
            return AdminDialogos.instance.OrdenaDialogo(AdminDialogos.instance.ES[Id_Dialogo-1]);        
        case "EN":
            return AdminDialogos.instance.OrdenaDialogo(AdminDialogos.instance.EN[Id_Dialogo-1]);       
       
    }
    //Si no hay un lenguaje definido se retornará este dialogo como por defecto
    return AdminDialogos.instance.OrdenaDialogo(AdminDialogos.instance.EN[Id_Dialogo-1]);
}
    private Sprite personajeHabla(int value){//Carga el nombre e imagen de quien habla en el momento
            if(AdminDialogos.instance.turnoDialogo[value]==">"){// el caracter > para personaje y < para NPC
                nameHabla.text = PlayerPrefs.GetString("namePrefs","");
                return GameObject.FindGameObjectWithTag("Jugador").GetComponent<SpriteRenderer>().sprite;
            }
            if(AdminDialogos.instance.turnoDialogo[value]=="<"){// el caracter > para personaje y < para NPC
                nameHabla.text = nameNPC;
                return this.GetComponent<SpriteRenderer>().sprite;
            }
        return null;
    }
    private void Muestra_OverLap(){    
        //En este Ejemplo Evaluaremos el ingreso con un layer especifico "Personaje"
        //Box   
        bool Contacto2 = Physics2D.OverlapBox(objeto.position, new Vector2(Angle*3,Angle/2), Angle/2, Mascara_Layer);
        if(Contacto2){
            Botonaction.SetActive(true);
            if(Input_Class.instance.PC_Controller)
                GameObject.Find("Text_actionB").GetComponent<Text>().text = InputManager.GetAction("PC_control 1", "Action").Bindings[0].Positive+"";
        }else
            Botonaction.SetActive(false);
}
    void OnDrawGizmos(){        
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position, new Vector2(Angle*3,Angle/2));        
    }
}
