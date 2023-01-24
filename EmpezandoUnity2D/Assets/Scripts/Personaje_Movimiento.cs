using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

public class Personaje_Movimiento : MonoBehaviour
{
    public static Personaje_Movimiento instance;
    
     [Header("Movimiento Settings")] 
            [SerializeField] private float velMov= 0f;
            [SerializeField] private PhysicsMaterial2D Inercia = null;
            [SerializeField] private PhysicsMaterial2D EnAireMaterial = null;
    [Header("Parametros Raycast")]         
            [SerializeField] private float Long_Ray=0.0f;
            [SerializeField] private LayerMask Mascara_Layer=0;

    public bool EnTierra;
    public bool EnAgua;
    public bool EnAire;
            
    //Otras variables y componentes
    private float Velocidad;
    private Rigidbody2D RB;
    
    void Start()
    {
        instance = this;
        Velocidad = velMov;
        /*Opción 1 */
         RB = this.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //La opción 1 de movimiento nos permite usar las físicas al tiempo de su rigidbody
        if(Admin_Movimientos.instance.HORIZONTAL_MOV){
            Valida_VelocidadMov();
            if((InputManager.GetAxis("Horizontal")<-0.15f || InputManager.GetAxis("Horizontal")>0.15f) && Input_Class.instance.PC_Controller){
                    
                   
                         RB.velocity = new Vector2(Valida_VelocidadMov() * InputManager.GetAxisRaw("Horizontal"),RB.velocity.y );
            }
            if(InputManager.GetAxisRaw("Vertical")<0 && EnAire)
                        RB.velocity = new Vector2( RB.velocity.x ,(Valida_VelocidadMov()*2) * InputManager.GetAxisRaw("Vertical"));
                    
            if(InputManager.GetAxisRaw("Horizontal")!=0)
                dirSprite(InputManager.GetAxisRaw("Horizontal"));
                
            if( InputManager.GetAxis("Horizontal")!=0.0f && Input_Class.instance.Mobile_Controller &&  InputManager.GetAxis("Vertical")<=0.0f){
                    var dirEntero=0;
                    if(InputManager.GetAxis("Horizontal")>0.0f)
                     dirEntero =1;
                    if(InputManager.GetAxis("Horizontal")<0.0f)
                     dirEntero =-1;
                    if(InputManager.GetAxis("Horizontal")*2 < 1f || InputManager.GetAxis("Horizontal")*2 > -1f) 
                        RB.velocity = new Vector2(Valida_VelocidadMov() * (dirEntero),RB.velocity.y);
                    else    
                       RB.velocity = new Vector2(Valida_VelocidadMov() * (dirEntero),RB.velocity.y); 
                    dirSprite(dirEntero);

                    /* if(InputManager.GetAxis("Vertical")*2 < 1f && EnAire)
                        RB.velocity = new Vector2( RB.velocity.x ,(Valida_VelocidadMov()*-2)); */
            }
            
                        
        }
      
       /*Opción 2 de movimiento siempre trasladamos al objeto por el escenario
       float dir.x = InputManager.GetAxisRaw("Horizontal");
       Vector2 movimiento = dir.normalized * velMov * Time.deltaTime;
       transform.Translate(movimiento); */

    }
    
    private void dirSprite(float value){//según sea el valor entrante -1 da girar al sprite, 1 lo retorna a su estado original
        if(value<0)
            this.GetComponent<SpriteRenderer>().flipX = true;
        else
            this.GetComponent<SpriteRenderer>().flipX = false;
    }

    private float Valida_VelocidadMov(){
        Velocidad=velMov;
        this.GetComponent<CapsuleCollider2D>().sharedMaterial = Inercia;//se carga el material por defecto
        if(CreateRaycast("Down", Long_Ray, this.transform, new Vector2(this.transform.position.x ,this.transform.position.y -0.13f), LayerMask.GetMask("Water")))
        {
            EnTierra = false;
            EnAgua = true;
            EnAire = false;
            return Velocidad/=2; //En el Agua la mitad de la velocidad          
          }          
        if(CreateRaycast("Down", Long_Ray+(Long_Ray/2), this.transform, new Vector2(this.transform.position.x ,this.transform.position.y -0.13f), LayerMask.GetMask("Superficies")))
        { 
            EnTierra = true;
            EnAgua = false;
            EnAire = false;
            return Velocidad=velMov;//En superficies normales la velocidad original   
        }        
                    
        if(CreateRaycast("Down", Long_Ray, this.transform, new Vector2(this.transform.position.x ,this.transform.position.y -0.13f), LayerMask.GetMask("SuperficiesDificiles")))
        {
            EnTierra = true;
            EnAgua = false;
            EnAire = false;
            return Velocidad/=3;//En superficies dificiles una tercera parte de la velocidad original           
        }
        //si llega a estar en el aire, adapte el material sin rozamiento para quye no quede pegado a superficies al caer
        this.GetComponent<CapsuleCollider2D>().sharedMaterial = EnAireMaterial;
        EnTierra = false;
        EnAgua = false;
        EnAire = true;
        return Velocidad= Velocidad/2 + Velocidad/4;//En el Aire la mitad de la velocidad original           
    }
    private bool CreateRaycast(string Orientacion, float Longitud,  Transform Dir_Posicion, Vector2 Altura_Vector ,  LayerMask mascara){

        Vector2 dir = Dir_Posicion.position;
        if(Orientacion=="Down")
            dir = Dir_Posicion.TransformDirection(Vector2.down) * Longitud;

            RaycastHit2D hit = Physics2D.Raycast(Altura_Vector, dir, Longitud, mascara);
            Debug.DrawRay(Altura_Vector, dir , hit ? Color.yellow : Color.blue); //Ya no es un Gizmo
    
        if(hit)
            return true;
        else
            return false;
    }
}
