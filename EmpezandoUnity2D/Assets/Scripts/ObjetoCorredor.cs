using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjetoCorredor : MonoBehaviour
{   
    public bool CPU;
    public bool human;
    public GameObject personaje = null;    
    public string Select_Key="Solo si Human es true";
    public Text TextoPosicion; 
    private bool movCPU=true;
    

    void Update()
    {
        if(human)
            if(Input.GetKeyDown(Select_Key))
                gameObject.transform.position = new Vector2(gameObject.transform.position.x+0.05f,gameObject.transform.position.y);
            
        if(ControllerRace.instance.tiempoConteo==0f){
            mostrarPosiciones();
            if(CPU && movCPU)
                StartCoroutine("Run_CPU");
        }else
            gameObject.transform.position = new Vector2(ControllerRace.instance.posSalida,gameObject.transform.position.y);
        
        
        
    }
     public void mostrarPosiciones(){
        for(int i=0; i<ControllerRace.instance.corredores.Length;i++){
            if(ControllerRace.instance.corredores[i].gameObject.name == gameObject.name)
                TextoPosicion.text = ControllerRace.instance.pos[i]+"";

        }
    } 
    /*  public void mostrarPosiciones(){
            ControllerRace objetoController = new ControllerRace();
            for(int i=0; i < objetoController.corredores.Length;i++){
                if(objetoController.corredores[i].gameObject.name == gameObject.name)
                    TextoPosicion.text = objetoController.pos[i]+"";
        }
    } */

    IEnumerator Run_CPU(){        
           
            movCPU=false;//IMPORTANTE!!! se vuelve falso este valor para que no se ejecute el metodo en el Update mientras espera
            gameObject.transform.position = new Vector2(gameObject.transform.position.x+Random.Range(0.05f,0.15f),gameObject.transform.position.y);
            yield return new WaitForSeconds(0.5f);//Espera medio segundo y se mueve un valor random en x
            movCPU=true;

    }
}
