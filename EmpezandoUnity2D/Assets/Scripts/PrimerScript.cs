using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class PrimerScript : MonoBehaviour
{
    
    public Button PrimerButton;
    public Text texto_Tiempo;
    public bool time_console;
    public bool unScaleTime_console;

    private int tiempo;

    [Header("Objetos a Desplazar")]
    [SerializeField] private float velocidad = 0;
    [SerializeField] private Transform ObjetoX=null;
    [SerializeField] private Transform ObjetoY=null;

   
    void Start(){
        PrimerButton.onClick.AddListener(TaskOnClick);
    }
    void Update(){
        tiempo = (int)(Time.time);
        if(time_console)            
            print("Tiempo: "+tiempo);
                           
        texto_Tiempo.text= "" + tiempo;
        float translation = Time.deltaTime * velocidad;       
        ObjetoX.Translate(translation,0,0);// Se modifica el valor de la posición para X frame por frame  

        float translation2 = Time.deltaTime * velocidad;
        ObjetoY.Translate(0,translation2,0);// Se modifica el valor de la posición para Y frame por frame    
        if(unScaleTime_console){
            tiempo = (int)(Time.unscaledTime);
            print("unScaled: "+tiempo); 
        }     
    }
        
    #region otrosMetodos

    public void Pausar() {  
        if(Time.timeScale == 1 ){    //si la velocidad es 1
			Time.timeScale = 0; 	//que la velocidad del juego sea 0
            Debug.Log("En Pausa...");
            PrimerButton.GetComponentInChildren<Text>().text="Continue";
		} else if(Time.timeScale == 0 ) {   // si la velocidad es 0
			Time.timeScale = 1;  	// que la velocidad del juego regrese a 1
            Debug.Log("Fin Pausa...");
            PrimerButton.GetComponentInChildren<Text>().text="Pausa";
		}
    }
    void TaskOnClick()
    {
       Pausar();
    }
    #endregion
}
