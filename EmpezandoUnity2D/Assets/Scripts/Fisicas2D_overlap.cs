using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


public class Fisicas2D_overlap : MonoBehaviour
{
    public bool point;
    public bool box;
    public bool circle;
    
    
        [Header("Parametros Circle")]     
            [SerializeField] private float Move_circle=0.0f;        
            [SerializeField] private float Radio=0.0f;

        [Header("Parametros Box")] 
            [SerializeField] private float Angle=0.0f;
                   
        [Header("Parametros Point")]
            [SerializeField] private float MovePoint=0.0f;
                    
        [Header("Parametros Función")] 
            [SerializeField] private LayerMask Mascara_Layer=0;
            [SerializeField] private Transform objeto=null;

        private String Respuesta=null;
        
    
    void Update()
    {
         Respuesta=""; 
         Muestra_OverLap();     
         if(Respuesta!="")
            Debug.Log(Respuesta);
    }
#region Ovelap
private void Muestra_OverLap(){
    
        //En este Ejemplo Evaluaremos el contacto con un layer especifico

        //Circle
        bool Contacto1 = Physics2D.OverlapCircle(new Vector2(objeto.position.x, objeto.position.y + Move_circle), Radio ,Mascara_Layer);
        if(Contacto1)
            Respuesta = Respuesta + " Overlap -------------- > Circle ";

        //Box   
        bool Contacto2 = Physics2D.OverlapBox(objeto.position, new Vector2(Angle,Angle), Angle/2, Mascara_Layer);
        if(Contacto2)
            Respuesta = Respuesta +" Overlap -------------- > Box " ;
        
        //En este Ejemplo Evaluaremos el contacto entre un punto y un collider
        //Point
        bool Contacto3 = Physics2D.OverlapPoint(new Vector2(objeto.position.x, objeto.position.y+(MovePoint*-1)));    
        if(Contacto3)
            Respuesta = Respuesta +" Overlap -------------- > Point " ;  

        
}

#endregion
void OnDrawGizmos(){        
        
        if(box){
             Gizmos.color = Color.blue;
             Gizmos.DrawWireCube(objeto.position, new Vector2(Angle,Angle));
        }
        if(point){
             Gizmos.color = Color.green;
             Gizmos.DrawLine(objeto.position,new Vector2(objeto.position.x, (objeto.position.y+(MovePoint*-1))));
             Gizmos.DrawWireSphere(new Vector2(objeto.position.x, objeto.position.y+(MovePoint*-1)), 0.012f);             
        }
        if(circle){
             Gizmos.color = Color.yellow;
             Gizmos.DrawWireSphere(new Vector2(objeto.position.x, objeto.position.y + Move_circle), Radio);
        }
       
    }

}
