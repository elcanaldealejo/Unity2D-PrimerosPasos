using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Vida : MonoBehaviour
{
    public static UI_Vida instance;
    //Creamos una Instancia de la UI correspondiente a la Vida
    //para que otros Scripts puedan acceder a usarlo
    public Slider sliderVida;

    //Los siguientes almacenarán los valores para aplicar según sea el caso
    private float sumVida;
    private float resVida;

    //Colores para las tres fases de nivel el UI
    public Color vidaMedia;
    public Color vidaPoca;
    public Color vidaNormal;    
    private GameObject prefbVida = null;
    
    void Start()
    {
        instance = this;
        prefbVida = Admin_Level.instance.prefbLevel();
    }

    void Update()
    {
        StartCoroutine("invulnerableTime");
       
    }

    public void Damage(float value){
        if(!Admin_Movimientos.instance.INVENCIBLE_TIME){
            resVida = value;            
            StartCoroutine("reducirVida");            
        }
    }
    public void Salud(float value){
            sumVida = value;
            StartCoroutine("restaurarVida");

    }
    public void Revivir(){
            Admin_Movimientos.instance.Iniciar_MOVs();
            sumVida = sliderVida.maxValue;
            StartCoroutine("restaurarVida");
            AudioManager.instance.PlaySounds(0);
    }

    IEnumerator invulnerableTime(){
        
        while(Admin_Movimientos.instance.INVENCIBLE_TIME){
            
            prefbVida.GetComponent<SpriteRenderer>().enabled= !prefbVida.GetComponent<SpriteRenderer>().enabled;
            
            yield return new WaitForSeconds(0.005f);     
        }
    }

IEnumerator reducirVida(){
    //mientras el valor a restar vida sea mayor de cero, haga que el nivel de la vida vaya disminuyendo con el tiempo
   Admin_Movimientos.instance.INVENCIBLE_TIME=true;
   while(resVida>0 && sliderVida.value>0){
        resVida--;
        sliderVida.value-=1f;        
        //El nivel de vida va de 0 a 100, se evaluan rangos para cambio de colores
        if(sliderVida.value<=sliderVida.maxValue/2 && sliderVida.value>25)
            GameObject.Find("Fill_vida").GetComponent<Image>().color = vidaMedia;//Encuentre el gameObject con nombre Fill_vida y a su componente imagen el color cambielo
            else if(sliderVida.value<=25)
                GameObject.Find("Fill_vida").GetComponent<Image>().color = vidaPoca;
                else
                    GameObject.Find("Fill_vida").GetComponent<Image>().color = vidaNormal;
        yield return new WaitForSeconds(0.03f);        
    }
    Admin_Movimientos.instance.INVENCIBLE_TIME=false;
    prefbVida.GetComponent<SpriteRenderer>().enabled=true;
}
IEnumerator restaurarVida(){
    Admin_Movimientos.instance.INVENCIBLE_TIME=true;
    //mientras el valor a sumar vida sea mayor de cero, haga que el nivel de la vida vaya incrementando con el tiempo
    while(sumVida>0){
        sumVida--;
        sliderVida.value+=1f;
        if(sliderVida.value<=sliderVida.maxValue/2 && sliderVida.value>25)
            GameObject.Find("Fill_vida").GetComponent<Image>().color = vidaMedia;
            else if(sliderVida.value<=25)
                GameObject.Find("Fill_vida").GetComponent<Image>().color = vidaPoca;
                else
                    GameObject.Find("Fill_vida").GetComponent<Image>().color = vidaNormal;
        yield return new WaitForSeconds(0.03f);
    }
    Admin_Movimientos.instance.INVENCIBLE_TIME=false;
    prefbVida.GetComponent<SpriteRenderer>().enabled=true;
}

}
