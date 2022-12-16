using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsTrigger_ON : MonoBehaviour
{
     private void OnTriggerEnter2D(Collider2D collider){

        if(collider.gameObject.layer == 11){//muerte
            AdminCamera2D.instance.DetenerSeguimiento();
        } 
        if(collider.gameObject.layer == 9){//Peligro
            UI_Vida.instance.Damage(collider.gameObject.GetComponent<Enemy_basic>().ataque);
        }      
        if(collider.gameObject.layer == 13){//Layer Restablecer vida
            UI_Vida.instance.Salud(30);// se envia al instance del UI vida el valor a incrementar
            collider.gameObject.SetActive(false);
        }
        if(collider.gameObject.layer == 14){//Coins        
            Admin_Level.instance.controlMonedas(collider.gameObject);
        }
        if(collider.gameObject.layer == 17){//Enemy_basic
            UI_Vida.instance.Damage(collider.gameObject.GetComponent<Enemy_basic>().ataque);
        }
        
        if(collider.gameObject.tag == "Coleccionable"){//solo si tienen el tag colecionable se agregaran al inventario
            UI_Inventario.instance.UpdateInventario(collider.gameObject);
        }

   }
}
