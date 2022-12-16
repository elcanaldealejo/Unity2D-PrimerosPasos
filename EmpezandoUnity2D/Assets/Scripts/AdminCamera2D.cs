using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminCamera2D : MonoBehaviour
{
    public static AdminCamera2D instance;
    public GameObject Objetivo= null;

    //////////////////// CONFIGURACIONES SEGUIMIENTO HORIZONTAL
    public bool seguir_horizontal = true;
    public float minPosx=0f;
    public float maxPosx=0f;
   [Range( 0 , 1 )] public float RSeguimiento_horizontal = 1f;
    public float DistaciaX_horizontal = 0f; 
    public float Suavizado_horizontal= 3f;
    private float _objetivoHorizontalSuavizaSeguimiento;

    /////////////////  CONFIGURACIONES SEGUIMIENTO VERTICAL
    public bool seguir_vertical = true;
    public float minPosy=0f;
    public float maxPosy=0f;
    [Range( 0 , 1 )] public float RSeguimiento_vertical = 1f;
    public float DistaciaY_vertical = 0f; 
    public float Suavizado_vertical= 3f;
    private float _objetivoVerticalSuavizaSeguimiento;
    
    //////////////  VECTORES PARA ALMACENAR POCICIONES TEMPORALES
    public Vector3 ObjetivoPosicion {get; set;}
    public Vector3 ObjetivoCamaraPosicion {get; set;}
    ///////////////////
    
    void Awake()
    {
        instance = this;        
    }

    void Start(){
        Objetivo = Admin_Level.instance.prefbLevel();
    }
    
    void Update()
    {
        if(Objetivo.activeSelf)//mientras el objetivo este activo la cámara lo seguirá
            MoverCamara();
            
        
    }

    private void MoverCamara(){
    if(Objetivo == null){
        return;
    }
    ObjetivoPosicion = GetObjetivoPosicion(Objetivo); // NECESITAMOS SABER A CADA INSTANTE EN QUE POSICION ESTA EL OBJETIVO   
    ObjetivoCamaraPosicion = new Vector3(ObjetivoPosicion.x, ObjetivoPosicion.y, 0f); 
    // SEGÚN SEA LA ORIENTACION O VISTA DEL SPRITE SE ACTUALIZA EL RANGO Y DISTANCIA EN LA HORIZONTAL
    float vistaObjetivo = Objetivo.GetComponent<SpriteRenderer>().flipX ? (DistaciaX_horizontal*-2f) : DistaciaX_horizontal;
    // LA CAMARA SIEMPRE SE MOSTRARA SI ES EL CASO CON LA SUMA DE LA DISTANCIA AGREGADA EN X
    ObjetivoCamaraPosicion += new Vector3(seguir_horizontal ? vistaObjetivo : 0f, seguir_vertical ? DistaciaY_vertical : 0f, 0f );

    // Establecer valor del suavizado
    _objetivoHorizontalSuavizaSeguimiento = Mathf.Lerp(_objetivoHorizontalSuavizaSeguimiento, ObjetivoCamaraPosicion.x, Suavizado_horizontal * Time.deltaTime);
    _objetivoVerticalSuavizaSeguimiento = Mathf.Lerp(_objetivoVerticalSuavizaSeguimiento, ObjetivoCamaraPosicion.y, Suavizado_vertical * Time.deltaTime);
 
    //Obtener dirección hacia la posición del objetivo
    float xDirection = _objetivoHorizontalSuavizaSeguimiento - transform.localPosition.x;
    float yDirection = _objetivoVerticalSuavizaSeguimiento - transform.localPosition.y;
    Vector3 deltaDireccion = new Vector3(xDirection, yDirection, 0f);
    // Nueva posicion
    Vector3 newCameraPosition = transform.localPosition + deltaDireccion;
    //Evalua los limites de pantalla
    if(newCameraPosition.y>maxPosy)
        newCameraPosition = new Vector3(newCameraPosition.x,maxPosy,newCameraPosition.z);
    if(newCameraPosition.y<minPosy)
        newCameraPosition = new Vector3(newCameraPosition.x,minPosy,newCameraPosition.z);
    if(newCameraPosition.x<minPosx)
        newCameraPosition = new Vector3(minPosx,newCameraPosition.y,newCameraPosition.z);
    if(newCameraPosition.x>maxPosx)
        newCameraPosition = new Vector3(maxPosx,newCameraPosition.y,newCameraPosition.z);    
   
    //Aplicar nueva posicion
    transform.localPosition = new Vector3(newCameraPosition.x, newCameraPosition.y, transform.localPosition.z);

}

    private Vector3 GetObjetivoPosicion(GameObject player){
    float xPos = 0f;
    float yPos = 0f;
    if(seguir_horizontal)
        xPos = (player.transform.position.x +  DistaciaX_horizontal) * RSeguimiento_horizontal;
    if(seguir_vertical)
        yPos = (player.transform.position.y + DistaciaY_vertical) * RSeguimiento_vertical;
    Vector3 posicionObjetivo = new Vector3(xPos, yPos, transform.position.z);
    return posicionObjetivo;
}

    public void DetenerSeguimiento(){
        Objetivo.SetActive(false);//el Objetivo se inactiva        
    }
    public void reIniciaSeguimiento(Transform location){
        if(Admin_Level.instance.check>0)
            Objetivo.transform.position = Admin_Level.instance.CheckPoints[Admin_Level.instance.check-1].position; //reaparecePunto.position; 
        else
            Objetivo.transform.position = Admin_Level.instance.puntoAparicion.position;
            
        Objetivo.SetActive(true);//el Objetivo se activa, pero primero se le da la posición de donde debe reaparecer
    }
    
}
