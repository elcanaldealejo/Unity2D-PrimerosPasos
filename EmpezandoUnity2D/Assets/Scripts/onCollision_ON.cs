using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class onCollision_ON : MonoBehaviour
{
   
    private void OnCollisionEnter2D(Collision2D collision){
     
      
      if(collision.gameObject.layer == 13){//salud 13
         UI_Vida.instance.Salud(30);// se envia al instance del UI vida el valor a incrementar
         collision.gameObject.SetActive(false);
      }
   }
}
