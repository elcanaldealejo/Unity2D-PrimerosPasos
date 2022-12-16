using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Admin_Movimientos : MonoBehaviour
{
    public static Admin_Movimientos instance;    
    public bool Horizontal_Mov;
    public bool Salto_Mov;
    public bool Mirar_Arriba_Mov;
    public bool invencible_Time;

    void Start()
    {
        instance = this;
    }
    public bool INVENCIBLE_TIME{ 
            get{
                return invencible_Time;
            }
            set{
                invencible_Time = value;              
            }
    }
    public bool HORIZONTAL_MOV{
            get{
                return Horizontal_Mov;
            }
            set{
                Horizontal_Mov = value;              
            }
    }
    public bool SALTO_MOV{
            get{
                return Salto_Mov;
            }
            set{
                Salto_Mov = value;              
            }
    }
    public bool MIRAR_ARRIBA_MOV{
            get{
                return Mirar_Arriba_Mov;
            }
            set{
                Mirar_Arriba_Mov = value;              
            }
    }

    public void Detener_MOVs(){
        Horizontal_Mov = false;
        Salto_Mov = false;
        Mirar_Arriba_Mov=false;
        invencible_Time = false;
    }

    public void Iniciar_MOVs(){
        Horizontal_Mov = true;
        Salto_Mov = true;
        Mirar_Arriba_Mov= true;
        invencible_Time = false;
    }
}
