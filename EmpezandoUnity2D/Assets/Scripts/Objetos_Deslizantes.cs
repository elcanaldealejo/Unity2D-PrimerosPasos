using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetos_Deslizantes : MonoBehaviour
{
    public bool Ini_derecha = false;
    public float sizeRayo=0.5f;
    public float iniRay=0f;
    public float VelMov=1f;
    public LayerMask reboteLayers ;
    public float sizeRayoMuerte=0.5f;
    public float iniRayMuerte=0f;    
    public LayerMask muerteLayers ;
    private bool dir_Ini_derecha;
    private Vector3 aparicionPos;
    private Rigidbody2D RB;
    private Collider2D _collider;
    void Start()
    {
        dir_Ini_derecha = Ini_derecha;
        RB = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();
        aparicionPos = transform.position;
    }

    
    void Update()
    {
    if(_collider.enabled){
        if(CreateRaycast("Left",sizeRayo,reboteLayers))
            Ini_derecha=true;
        if(CreateRaycast("Right",sizeRayo,reboteLayers))
            Ini_derecha=false; 

         RB.velocity =  Ini_derecha && _collider.enabled ? new Vector2(1f * (VelMov),RB.velocity.y) : new Vector2(-1f * (VelMov),RB.velocity.y) ;  

        if(CreateRaycast("Left",sizeRayo,muerteLayers) || CreateRaycast("Right",sizeRayo,muerteLayers) || CreateRaycast("Down",sizeRayo,muerteLayers)){
            UI_Vida.instance.Damage(GetComponent<Enemy_basic>().ataque);
           
        }
        if(CreateRaycast("Up",sizeRayoMuerte,muerteLayers) && !gameObject.GetComponent<Enemy_basic>().invulnerable ){
              _collider.enabled=false;
              gameObject.GetComponent<Enemy_basic>().reducirSalud(11);   
              if(gameObject.GetComponent<Enemy_basic>().loop)            
                StartCoroutine("reaparecerObjeto");        
       } 
       
    }
    }  
    IEnumerator reaparecerObjeto(){
        
        var cont=10f;
        while(cont>0f){
            gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(0.02f);
            cont-=0.2f;
           gameObject.GetComponent<EdgeCollider2D>().enabled=false;
        }
        gameObject.GetComponent<Enemy_basic>().RevivirEnemy();
        gameObject.GetComponent<SpriteRenderer>().enabled =true;
        Ini_derecha = dir_Ini_derecha;
        _collider.enabled=true;
        gameObject.GetComponent<EdgeCollider2D>().enabled=true;
        gameObject.transform.position = aparicionPos;
    }
   
    private bool CreateRaycast(string Orientacion, float Longitud, LayerMask mascara){
        RaycastHit2D hit = new RaycastHit2D();
        Vector2 dir = transform.position;
        if(Orientacion=="Up"){
            dir = transform.TransformDirection(Vector2.right) * Longitud;
            hit = Physics2D.Raycast(new Vector3(transform.position.x - Longitud/2 ,transform.position.y + iniRayMuerte,transform.position.z), dir, Longitud, mascara);
            Debug.DrawRay(new Vector3(transform.position.x - Longitud/2 ,transform.position.y + iniRayMuerte,transform.position.z), dir , hit ? Color.yellow : Color.blue); //Ya no es un Gizmo
        }
        if(Orientacion=="Down"){
            dir = transform.TransformDirection(Vector2.right) * Longitud;
            hit = Physics2D.Raycast(new Vector3(transform.position.x - Longitud/2 ,transform.position.y - iniRayMuerte,transform.position.z), dir, Longitud, mascara);
            Debug.DrawRay(new Vector3(transform.position.x - Longitud/2 ,transform.position.y - iniRayMuerte,transform.position.z), dir , hit ? Color.yellow : Color.blue); //Ya no es un Gizmo
        }
        if(Orientacion=="Left"){
            dir = transform.TransformDirection(Vector2.left) * Longitud;
            hit = Physics2D.Raycast(new Vector3(transform.position.x - iniRay,transform.position.y ,transform.position.z), dir, Longitud, mascara);
            Debug.DrawRay(new Vector3(transform.position.x - iniRay,transform.position.y ,transform.position.z), dir , hit ? Color.yellow : Color.blue); //Ya no es un Gizmo
        }
        if(Orientacion=="Right"){
            dir = transform.TransformDirection(Vector2.right) * Longitud;            
            hit = Physics2D.Raycast(new Vector3(transform.position.x + iniRay,transform.position.y ,transform.position.z), dir, Longitud, mascara);
            Debug.DrawRay(new Vector3(transform.position.x + iniRay,transform.position.y ,transform.position.z), dir , hit ? Color.yellow : Color.blue); //Ya no es un Gizmo
        }
            
    
        if(hit)
            return true;
        else
            return false;
    }
    private void OnTriggerEnter2D(Collider2D collider){            
       if(collider.gameObject.layer == 11){// muerte 11
            StartCoroutine("reaparecerObjeto"); 
       }
       
   }
   private void OnCollisionEnter2D(Collision2D collision){
        /* if(collision.gameObject.layer == 10 ){//personaje 10
            
        }  */

   }
}
