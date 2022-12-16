using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovFondos : MonoBehaviour
{
    //Podemos crear el numero que se desee de fondos y ajustarles una velocidad movimiento
    //al tiempo que la cámara se desplaza
    public Transform FondoLejos, FondoMedio, FondoCerca;
    private float ultimaXPos;
    [Range( 0 , 1 )] public float RangoMov_Atras = 1f;
    [Range( 0 , 1 )] public float RangoMov_Medio = 1f;
    [Range( 0 , 1 )] public float RangoMov_Cerca = 1f;
    
    void Start()
    {
        ultimaXPos = AdminCamera2D.instance.transform.position.x;
    }    
    void Update()
    {
        float amountToMoveX = AdminCamera2D.instance.transform.position.x - ultimaXPos;
        //El fondo lejos va casi al ritmo de  la cámara. Se puede fijar para que no se mueva o aplicar un leve movimiento
        FondoLejos.position = FondoLejos.position + new Vector3(amountToMoveX*RangoMov_Atras,0f,0f);
        //El fondo medio se desplazará mientras la cámara siga avanzando, pero lento.
        FondoMedio.position += new Vector3(amountToMoveX * RangoMov_Medio, 0f,0f);
        //Los fondo cerca deben desplazarse más rapido pero no tanto como la cámara.
        FondoCerca.position += new Vector3(amountToMoveX * RangoMov_Cerca, 0f,0f);
    
        //Otros fondos adicionales que esten en el entorno del jugador(Muy cerca) durante la escena 
        //...se desplazarán a la misma velocidad que se mueva la cámara.
    
        ultimaXPos = AdminCamera2D.instance.transform.position.x;
    }
}
