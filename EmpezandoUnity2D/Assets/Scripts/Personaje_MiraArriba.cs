using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Luminosity.IO;

public class Personaje_MiraArriba : MonoBehaviour
{
    public float AumentoVista;
    private float tempDistanciaYCam;
    // Start is called before the first frame update
    void Start()
    {
        tempDistanciaYCam = AdminCamera2D.instance.DistaciaY_vertical;
        AumentoVista+=tempDistanciaYCam;
    }

    // Update is called once per frame
    void Update()
    {
        if(Admin_Movimientos.instance.MIRAR_ARRIBA_MOV && Input_Class.instance.PC_Controller)
            AdminCamera2D.instance.DistaciaY_vertical = 
            (InputManager.GetAxisRaw("Vertical")>0 && !Personaje_Movimiento.instance.EnAire && InputManager.GetAxisRaw("Horizontal")==0) 
            ? AumentoVista : tempDistanciaYCam;
        if(Admin_Movimientos.instance.MIRAR_ARRIBA_MOV && Input_Class.instance.Mobile_Controller)
            AdminCamera2D.instance.DistaciaY_vertical = 
            (InputManager.GetAxis("Vertical")>0.0f && !Personaje_Movimiento.instance.EnAire && InputManager.GetAxis("Horizontal")==0.0f) 
            ? AumentoVista : tempDistanciaYCam;
    }
}
