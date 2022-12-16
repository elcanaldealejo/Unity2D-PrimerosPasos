using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

public class Personaje_Salto : MonoBehaviour
{
    
    [Header("Parametros Raycast")]         
            [SerializeField] private float Long_Ray=0.0f;
            [SerializeField] private LayerMask Mascara_Layer=0;
    [Header("Jump Settings")] 
            [SerializeField] private float FuerzaSalto=0;
            [SerializeField] private int NumSaltos=0;
            [SerializeField] private int MaxSaltos=1;

            //Componentes
            private Rigidbody2D RB;
            
    void Start()
    {
        RB = this.GetComponent<Rigidbody2D>();
    }

    
    void Update()
    {
        if(Admin_Movimientos.instance.SALTO_MOV){
            Valida_Salto();
            if(InputManager.GetButtonDown("Jump") && NumSaltos<MaxSaltos){
                    VerticalMovimiento();
            }
        }
    }
    private void VerticalMovimiento(){ 
            RB.velocity = new Vector2(RB.velocity.x, FuerzaSalto);
            NumSaltos++;            
    }
    void OnDrawGizmosSelected(){        
            Gizmos.color = Color.red;
            Vector2 direction = this.transform.TransformDirection(Vector2.down) * Long_Ray; 
            Gizmos.DrawRay(new Vector2(this.transform.position.x+ 0.1f,this.transform.position.y -0.13f), direction);//adelante
            Gizmos.DrawRay(new Vector2(this.transform.position.x ,this.transform.position.y -0.13f), direction);//centro
            Gizmos.DrawRay(new Vector2(this.transform.position.x- 0.1f,this.transform.position.y -0.13f), direction);//atras
    }

    private void Valida_Salto(){

        if(CreateRaycast("Down", Long_Ray, this.transform, new Vector2(this.transform.position.x+ 0.1f,this.transform.position.y -0.13f), Mascara_Layer)){
            NumSaltos=0;            
        }            
        if(CreateRaycast("Down", Long_Ray, this.transform, new Vector2(this.transform.position.x,this.transform.position.y -0.13f), Mascara_Layer)){
            NumSaltos=0;            
        }            
        if(CreateRaycast("Down", Long_Ray, this.transform, new Vector2(this.transform.position.x- 0.1f,this.transform.position.y -0.13f), Mascara_Layer)){
            NumSaltos=0;            
        }
                    
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
