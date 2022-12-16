using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SaludEnemy : MonoBehaviour
{
    
    public Slider sliderVida;
    

    //se almacenará el valor para aplicar reduccion de vida
    private float resVida;

    //Variables para el texto flotante
    public GameObject textoDamage;//lo necesitamos como gameObject por su RectTransform y Text
    public RectTransform posTexto;
    private Color c;
    private Color tempC;//almacenará el color real inicial
   
    
    void Start()
    {
        //Extraemos el componente Text del GameObject para obtener su color
        c = textoDamage.GetComponent<Text>().color;//propiedad de color del texto 
        tempC = c;
    }
    void Update()
    {
        if(sliderVida.value>0f){
            if(Input.GetKeyDown("e"))
                   DamageEnemy(Random.Range(40,10));      
        }
    }
    public void DamageEnemy(float value){
            resVida = value;
            StartCoroutine("AnimaText");
            StartCoroutine("reducirVida");        
    }

    IEnumerator reducirVida(){
    //mientras el valor a restar vida sea mayor de cero, haga que el nivel de la vida vaya disminuyendo con el tiempo
        while(resVida>0){        
            resVida--;
            sliderVida.value-=1f;        
            yield return new WaitForSeconds(0.03f);
        }    
    }

    IEnumerator AnimaText(){
            //otrogamos el valor a reducir en el texto a mostrar
            textoDamage.GetComponent<Text>().text = ""+resVida;
            //Antes de cualquier animacion inicializar valores para posición y opacidad
            textoDamage.GetComponent<RectTransform>().position = posTexto.position;
            c = tempC;            
            float vel = 10f;//vamos hace 10 veces el ciclo            
            while (vel>0f)
            {
                c.a-=0.08f;//Reduciremos la opacidad
                textoDamage.GetComponent<Text>().color = c;//Igualamos la reduccion de color
                yield return new WaitForSeconds(0.01f);//tiempo de espera entre ciclos
                vel-=1f;//reducimos la salida del ciclo
                //Trasladamos el texto en Y un poco cada ciclo
                textoDamage.GetComponent<RectTransform>().position = new Vector2(textoDamage.GetComponent<RectTransform>().position.x,textoDamage.GetComponent<RectTransform>().position.y+0.1f);
            }
            textoDamage.GetComponent<Text>().text = "";//borramos el valor a mostrar
    }


}
