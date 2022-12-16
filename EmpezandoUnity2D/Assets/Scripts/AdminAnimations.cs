using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

public class AdminAnimations : MonoBehaviour
{
    public static AdminAnimations instance;
    public Animator _animador;
    
    void Awake(){
        instance = this;
    }
    void Start(){
        _animador = Admin_Level.instance.prefbLevel().GetComponent<Animator>();
    }
    void Update()
    {
        if(_animador.tag=="Jugador")
            SetAnimationPersonaje();
        
    }
    public void SetAnimationPersonaje(){            
            
            _animador.SetBool("caminar" , Personaje_Movimiento.instance.EnTierra  && (InputManager.GetAxisRaw("Horizontal")!=0 || InputManager.GetAxis("Horizontal")!=0.0f));
            _animador.SetBool("mirarArriba" , !Personaje_Movimiento.instance.EnAire && Admin_Movimientos.instance.MIRAR_ARRIBA_MOV  && (InputManager.GetAxisRaw("Vertical")>0 || InputManager.GetAxis("Vertical")>0.0f ));
            
            
            _animador.SetBool("saltar" , Personaje_Movimiento.instance.EnTierra  && InputManager.GetButton("Jump"));
            _animador.SetBool("cayendo" , Personaje_Movimiento.instance.EnAire );
           
    }
}
