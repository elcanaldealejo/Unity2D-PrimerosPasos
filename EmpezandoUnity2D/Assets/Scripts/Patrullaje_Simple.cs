using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrullaje_Simple : MonoBehaviour
{
    public float distancia;
    [Range (0.1f,0.01f)] public  float  velocidad;
    public float tiempoEspera=0f;
    private Vector2 posIni;
    public bool miraDerUP;
    public SentidoPatrulla Tipo = new SentidoPatrulla();
    private string patrullaje; 
    private bool destino;

    // Start is called before the first frame update
    void Start()
    {
        posIni = transform.position;
        patrullaje = Tipo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(!destino && patrullaje!="none")
            StartCoroutine("patrullar");
    }
    IEnumerator patrullar(){
        destino=true;
        Vector2 posFin = patrullaje=="Horizontal" ?  new Vector2(transform.position.x + distancia,transform.position.y) : new Vector2(transform.position.x,transform.position.y + distancia);
        
// iniciar patrulla

        Vector2 diferencia =  
        patrullaje=="Horizontal" ?  new Vector2(transform.position.x ,transform.position.y) - 
        new Vector2(transform.position.x + distancia,transform.position.y) 
        :
         new Vector2(transform.position.x ,transform.position.y) - 
         new Vector2(transform.position.x,transform.position.y + distancia);
        if(patrullaje=="Horizontal" )
            gameObject.GetComponent<SpriteRenderer>().flipX = miraDerUP ?  gameObject.GetComponent<SpriteRenderer>().flipX=false : gameObject.GetComponent<SpriteRenderer>().flipX=true;
        else
            gameObject.GetComponent<SpriteRenderer>().flipY = miraDerUP ?  gameObject.GetComponent<SpriteRenderer>().flipY=false : gameObject.GetComponent<SpriteRenderer>().flipY=true;
        float distanciaReal = diferencia.magnitude;
        while(distanciaReal>1f){
            transform.position =  patrullaje=="Horizontal" ? new Vector2(transform.position.x + 0.2f ,transform.position.y) : new Vector2(transform.position.x,transform.position.y + 0.2f) ;
            yield return new WaitForSeconds(velocidad);
            distanciaReal = (new Vector2(transform.position.x ,transform.position.y)- posFin).magnitude;
            
        }

//retornar a posición inicial

        diferencia =  posIni - new Vector2(transform.position.x ,transform.position.y);
        if(patrullaje=="Horizontal" )
            gameObject.GetComponent<SpriteRenderer>().flipX = miraDerUP ?  gameObject.GetComponent<SpriteRenderer>().flipX=true : gameObject.GetComponent<SpriteRenderer>().flipX=false;
        else
            gameObject.GetComponent<SpriteRenderer>().flipY = miraDerUP ?  gameObject.GetComponent<SpriteRenderer>().flipY=true : gameObject.GetComponent<SpriteRenderer>().flipY=false;
        
        distanciaReal = diferencia.magnitude;
        while(distanciaReal>1f){
            transform.position =  patrullaje=="Horizontal" ? new Vector2(transform.position.x - 0.2f ,transform.position.y) : new Vector2(transform.position.x,transform.position.y - 0.2f) ;
            yield return new WaitForSeconds(velocidad);   
             distanciaReal = (posIni - new Vector2(transform.position.x ,transform.position.y)).magnitude;
        }
        yield return new WaitForSeconds(tiempoEspera);
        destino=false; 

        
    }
    
}
public enum SentidoPatrulla
     {
         none,
         Horizontal, 
         Vertical
     };
