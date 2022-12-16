using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corutinas : MonoBehaviour
{
    
    public float altura;//Variable para la tarea
    void Update()
    {
        if (Input.GetKeyDown("b"))
            Movimiento2();
        

        if (Input.GetButtonDown("accion")){
            StartCoroutine("Movimiento");
            StartCoroutine("Transparencia");
        }

        if (Input.GetKeyDown("n"))
            StartCoroutine("Reaparece");
       

        if (Input.GetKeyDown("c"))
            StartCoroutine("MovimientoFlor");
        
    }
#region ClaseCorutinas
    IEnumerator Movimiento(){
        for(var i=0;i<7;i++){
            gameObject.transform.position = new Vector2(transform.position.x - 1f,transform.position.y);
            Debug.Log("Movimiento : "+(i+1));
            yield return new WaitForSeconds(2f);
        }      
    }
    void Movimiento2(){
        for(var i=0;i<7;i++){
            gameObject.transform.position = new Vector2(transform.position.x - 1f,transform.position.y);
            Debug.Log("Movimimiento : "+(i+1));            
        }      
    }

    IEnumerator Transparencia(){
    
        for (float f = 1f; f >= 0.0f; f -= 0.15f) {
            Color c = GetComponent<SpriteRenderer>().color;
            c.a = f;
            GetComponent<SpriteRenderer>().color = c;
            yield return new WaitForSeconds(2f);
        }
   
    }
    IEnumerator Reaparece(){
        for (float f = 0.0f; f <= 1f; f += 0.15f) {
        Color c = GetComponent<SpriteRenderer>().color;
        c.a = f;
        GetComponent<SpriteRenderer>().color = c;
        yield return new WaitForSeconds(2f);
    }
    
}
#endregion

#region Tarea
IEnumerator MovimientoFlor(){
    GetComponent<Rigidbody2D>().gravityScale = 0;
        while(transform.position.y <= altura){
            gameObject.transform.position = new Vector2(transform.position.x ,transform.position.y + 0.3f);
            yield return new WaitForSeconds(0.02f);
        } 
        GetComponent<Rigidbody2D>().gravityScale = 1; 
                      
    }
#endregion
}
