using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mov_mundos : MonoBehaviour
{
    public static mov_mundos instance;
    public GameObject _personaje=null;
    public GameObject botonjugar=null;
    public int cantidadMundos=0;
    private int mundoActual=0;
    public bool[] Mundos;

    void Awake(){
        Mundos  = new bool[cantidadMundos];
        if(PlayerPrefs.GetString("info_mundos","")!=""){
            inicializarMundos(PlayerPrefs.GetString("info_mundos",""));
        }else   
            PlayerPrefs.SetString("info_mundos",inicializarMundos_NewGame());    
    }
    void Start(){
        instance = this;
        //Debug.Log(PlayerPrefs.GetString("info_mundos",""));
        habilitarMundos();
         if(PosUltimoMundo()>0)
            _personaje.transform.position = GameObject.Find("mundo"+(PosUltimoMundo()-1)).transform.position;
    }
    private void inicializarMundos(string mundos){
        /* 
        
         */
        int cont=0;
        for (int i=0;i<Mundos.Length;i++){
            if(mundos.Substring(i,1)!="-"){
                if( mundos.Substring(i,1)=="0"){
                    Mundos[cont]=false; 
                    cont++;                   
                }else{
                    Mundos[cont]=true;
                    cont++; 
                }
            }           
        }
    }
    private void habilitarMundos(){
        /* 
        
         */
        
        for (int i=0;i<Mundos.Length;i++){
           if(Mundos[i])
             GameObject.Find("mundo"+(i+1)).GetComponent<Button>().interactable=true;          
        }
    }
    public void ActualizarPrefsMundos(){
        string constructo="";
         for (int i=0;i<cantidadMundos;i++){            
                constructo += Mundos[i] ? "1-" : "0-";
         }
    }
    public int PosUltimoMundo(){
        int cont=0;
         for (int i=0;i<cantidadMundos;i++){ 
                if(Mundos[i])
                    GameObject.Find("mundo"+(i)).GetComponent<Image>().color = new Color(1f,0.7185f,0f,1f);//naranja
                else if(!Mundos[i] && GameObject.Find("mundo"+(i)).GetComponent<Button>().interactable)
                    GameObject.Find("mundo"+(i)).GetComponent<Image>().color = new Color(0.1562241f,1f,0f,1f);//verde 
                     
                cont += Mundos[i] ? 1 : 0;
         }
         return cont;
    }
    private string inicializarMundos_NewGame(){
        string constructo ="";
        for (int i = 0; i < cantidadMundos; i++)
        {
            constructo+="0-";
        }
        return constructo;
    }
    public void moverMundo(int nivel){           
        StartCoroutine(desplazar(GameObject.Find("mundo"+nivel).transform.position));
        mundoActual = nivel;         
    }
    public void cargarEscena(){
        SceneManager.LoadScene("Mundo"+(mundoActual+1));
    }
    public void MenuPpal(){
        SceneManager.LoadScene("MenuPpal");
    }
    IEnumerator desplazar(Vector3 posMundo){
        Vector3 resta = _personaje.transform.position - posMundo;
        float x=0f;
        float y=0f;
        float val=0.3f;
        _personaje.GetComponent<Animator>().SetBool("caminar" , true);
        botonjugar.SetActive(false);
        while (resta.magnitude >= 0.5f)
        {
            if(_personaje.transform.position.x < posMundo.x){//personaje es menor en x
                x=val; 
                _personaje.GetComponent<SpriteRenderer>().flipX=false;
                }
                else if(_personaje.transform.position.x > posMundo.x){
                x=val*-1;
                _personaje.GetComponent<SpriteRenderer>().flipX=true;
                }

            if(_personaje.transform.position.y < posMundo.y)//personaje es menor en y
                y=val;
                else if(_personaje.transform.position.y > posMundo.y)
                y=val*-1;

            _personaje.transform.position = new Vector3(_personaje.transform.position.x+x,_personaje.transform.position.y+y,_personaje.transform.position.z);
            yield return new WaitForSeconds(0.1f);
            x=0f;
            y=0f;
            resta = _personaje.transform.position - posMundo;
        }
        _personaje.GetComponent<Animator>().SetBool("caminar" , false);
        
    }

}
