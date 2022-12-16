using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneDestroy : MonoBehaviour
{
    public bool stopThis;// Sí es true dejan de salir objetos
    public bool ilimitThis;//Sí es true continuan saliendo objetos ilimitadamente
    public GameObject prefabToClone=null;//Prefab a clonar
    public float tiempoDestroy;//Tiempo para la destruccion de los clones

    
    private Color c = new Color(255,255,255,255);
    private GameObject ObjetoPrefab;
    private float timeControl;//Tiempo para la intermitencia
    
    private void OnTriggerEnter2D(Collider2D collider){            
            if(collider.gameObject.tag=="Jugador" && (!stopThis || ilimitThis) ){               
                StartCoroutine("Intermitencia"); 
                stopThis=true;                                
            } 
    }
   IEnumerator Intermitencia(){

        float ValorIntermitencia = 0.2f;//Tiempo entre Opaco/transparencia
        CloneDestroy newObjetoClass = new CloneDestroy();
        newObjetoClass.timeControl = tiempoDestroy/2;// La intermitencia va a durar la mitad del valor de destrucción                                
        newObjetoClass.ObjetoPrefab = Instantiate(prefabToClone);
        
        while((newObjetoClass.timeControl)>0f){
            
            if(newObjetoClass.c.a == 0f)
                newObjetoClass.c.a = 255;// Quitar transparencia
            else
                newObjetoClass.c.a = 0;// Volver transparente

            newObjetoClass.ObjetoPrefab.GetComponent<SpriteRenderer>().color = newObjetoClass.c;
            yield return new WaitForSeconds(ValorIntermitencia);
            newObjetoClass.timeControl-=ValorIntermitencia;
            
        }
        newObjetoClass.c.a = 255;//Garantizamos que despues de la intermitencia se observe el objeto
        Destroy(newObjetoClass.ObjetoPrefab,tiempoDestroy); 
   
    }
}
