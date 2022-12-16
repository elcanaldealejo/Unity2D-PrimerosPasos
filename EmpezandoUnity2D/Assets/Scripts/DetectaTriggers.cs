using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectaTriggers : MonoBehaviour
{

     

   private void OnTriggerEnter2D(Collider2D collider){
        Debug.Log("Un objeto con Nombre: "+collider.gameObject.name+" ENTRO en el Area de "+gameObject.name);
   }
   private void OnTriggerStay2D(Collider2D collider){
        Debug.Log("Un objeto con Nombre: "+collider.gameObject.name+" CONTINÚA en el Area de "+gameObject.name);
   }
   private void OnTriggerExit2D(Collider2D collider){
        Debug.Log("Un objeto con Nombre: "+collider.gameObject.name+" SALIO del Area de "+gameObject.name);
   }



}
