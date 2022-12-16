using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Fisicas2D_raycast : MonoBehaviour
{
        
        [Header("Parametros Lines")]         
            [SerializeField] private float LongLine=0.0f;
            [SerializeField] private float Long_LineaPiernas=0.0f;
        
            [SerializeField] private LayerMask Mascara_Layer=0;
            [SerializeField] private Transform objeto=null;
       
        public bool line;
        private string Respuesta;
        
    void Update()
    {
         Respuesta ="";         
         Muestra_RayCast();
         if(Respuesta!="")
            Debug.Log(Respuesta);      
    
    }

void OnDrawGizmosSelected(){
        
        if(line){
            Gizmos.color = Color.red;
            //Derechos
            Vector2 direction = objeto.TransformDirection(Vector2.right) * LongLine;  //1      
            Gizmos.DrawRay(objeto.position, direction);
            
            Vector2 upPos = new Vector2(objeto.position.x , objeto.position.y + 0.3f); //2    
            Gizmos.DrawRay(upPos, direction);
            
            Vector2 downPos = new Vector2(objeto.position.x , objeto.position.y - 0.3f); //3        
            Gizmos.DrawRay(downPos, direction);
            //Izquierdos
            direction = objeto.TransformDirection(Vector2.right) * -LongLine;   // 1     
            Gizmos.DrawRay(objeto.position, direction);

            upPos = new Vector2(objeto.position.x , objeto.position.y + 0.3f); //2    
            Gizmos.DrawRay(upPos, direction);

            downPos = new Vector2(objeto.position.x , objeto.position.y - 0.3f);  //3       
            Gizmos.DrawRay(downPos, direction);

            //Gizmo para la altura de las piernas del personaje
            Gizmos.color = Color.white;
            direction = objeto.TransformDirection(Vector2.down) * Long_LineaPiernas; 
            Gizmos.DrawRay(new Vector2(objeto.position.x+ 0.1f,objeto.position.y -0.13f), direction);//adelante
            Gizmos.DrawRay(new Vector2(objeto.position.x- 0.1f,objeto.position.y -0.13f), direction);//atras

        }
        
    }


#region RayCast
private void Muestra_RayCast(){

     if(CreateRaycast("Right", LongLine,  objeto, objeto.position , Mascara_Layer))
            Respuesta =Respuesta +  "(Ray_Derecho_Medio) " ;  

        if(CreateRaycast("Left", LongLine, objeto,  objeto.position , Mascara_Layer))
            Respuesta =Respuesta +  "(Ray_Izquierdo_Medio)" ;      
                   
        if(CreateRaycast("Left", LongLine, objeto, new Vector2(objeto.position.x , objeto.position.y + 0.3f), Mascara_Layer))
            Respuesta =Respuesta +  "(Ray_Izquierdo_Superior) " ; 

        if(CreateRaycast("Right", LongLine, objeto, new Vector2(objeto.position.x , objeto.position.y + 0.3f), Mascara_Layer))
            Respuesta =Respuesta +  "(Ray_Derecho_Superior) " ;   

        if(CreateRaycast("Left", LongLine, objeto, new Vector2(objeto.position.x , objeto.position.y - 0.3f), Mascara_Layer))
            Respuesta =Respuesta +  "(Ray_Izquierdo_Inferior) " ; 

        if(CreateRaycast("Right", LongLine, objeto, new Vector2(objeto.position.x , objeto.position.y - 0.3f), Mascara_Layer))
            Respuesta =Respuesta + "(Ray_Derecho_Inferior) " ; 

        if(CreateRaycast("Down", Long_LineaPiernas, objeto, new Vector2(objeto.position.x+ 0.1f,objeto.position.y -0.13f), LayerMask.GetMask("Water")))
            Respuesta = Respuesta+"Sobre Agua" ;

        if(CreateRaycast("Down", Long_LineaPiernas, objeto, new Vector2(objeto.position.x- 0.1f,objeto.position.y -0.13f), LayerMask.GetMask("Suelos")))
            Respuesta =Respuesta +"Sobre Suelo Firme" ;

           
}

private bool CreateRaycast(String Orientacion, float Longitud,  Transform Dir_Posicion, Vector2 Altura_Vector ,  LayerMask mascara){

    Vector2 dir = Dir_Posicion.position;
    if(Orientacion=="Right")
        dir = Dir_Posicion.TransformDirection(Vector2.right) * Longitud;
        
    if(Orientacion=="Left")
        dir = Dir_Posicion.TransformDirection(Vector2.left) * Longitud;

    if(Orientacion=="Down")
        dir = Dir_Posicion.TransformDirection(Vector2.down) * Longitud;

    if(Orientacion=="Up")
        dir = Dir_Posicion.TransformDirection(Vector2.up) * Longitud;

    RaycastHit2D hit = Physics2D.Raycast(Altura_Vector, dir, Longitud, mascara);
    Debug.DrawRay(Altura_Vector, dir , hit ? Color.yellow : Color.blue); //Ya no es un Gizmo
    
    if(hit)
        return true;
    else
        return false;
}

#endregion

    
}
