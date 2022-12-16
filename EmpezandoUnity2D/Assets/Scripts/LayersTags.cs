using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayersTags : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision){
        // Recuerda para que exista colisión solo deben tener el collider activo SIN Istrigger
        // Aqui los objetos podrán hacer parte de toda la física
        if(collision.gameObject.tag == "Enemigo"){
            print("Un objeto con Nombre: "+gameObject.name+" ENTRO en colisión con "+ collision.gameObject.name +" -> Tag " + collision.gameObject.tag);
            print("El "+gameObject.name+" recibe DAÑO");
        }
        if(collision.gameObject.tag=="Coleccionable" || collision.gameObject.tag=="Poder" || collision.gameObject.tag=="Arma"){
            Debug.Log("Un objeto con Nombre: "+gameObject.name+" Capturo un "+collision.gameObject.tag +" : "+ collision.gameObject.name);
            collision.gameObject.SetActive(false);
        }
   }
    private void OnTriggerEnter2D(Collider2D collider){
        // Recuerda los objetos tienen que tener el collider2D activo pero IsTrigger con check(true)
        // Si los objetos tienen un rigidbody2D activo y dinamico, no hay collisiones seguirian de largo por la gravedad
        if(collider.gameObject.tag=="Coleccionable" || collider.gameObject.tag=="Poder" || collider.gameObject.tag=="Arma"){
            Debug.Log("Un objeto con Nombre: "+gameObject.name+" Capturo un "+collider.gameObject.tag+" : "+ collider.gameObject.name);
            collider.gameObject.SetActive(false);
        }
        if(collider.gameObject.layer == 9){//Peligros
            //print("Un objeto con Nombre: "+gameObject.name);
            print("El "+gameObject.name+" recibe DAÑO... ENTRO en colisión con Layer "+ LayerMask.LayerToName(collider.gameObject.layer)+" : "+ collider.gameObject.name);
        }
        if(collider.gameObject.layer == 11){//Muerte
            Debug.Log("Un objeto con Nombre: "+gameObject.name+ " Muere");
        }
    }
}
