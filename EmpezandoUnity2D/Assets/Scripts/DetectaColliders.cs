using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectaColliders : MonoBehaviour
{
   private void OnCollisionEnter2D(Collision2D collision){
     print("Un objeto con Nombre: "+collision.gameObject.name+" ENTRA en colisión con "+gameObject.name);
   }
    private void OnCollisionStay2D(Collision2D collision){
        print("Un objeto con Nombre: "+collision.gameObject.name+" CONTINÚA en colisión con "+gameObject.name);
   }
   private void OnCollisionExit2D(Collision2D collision){
        print("Un objeto con Nombre: "+collision.gameObject.name+" TERMINO la colisión con "+gameObject.name);
   } 
}
