using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vectores : MonoBehaviour
{
    private Vector2 vector_A;
    public bool mostrarSuma;
    public bool mostrarResta;
    public bool mostrarMag;
    [Header("Configuración")]        
    [SerializeField] private bool asignarValores=false;
    [SerializeField] private float valorA_x = 0f;
    [SerializeField] private float valorA_y = 0f;
    [SerializeField] private Transform Objeto1 = null;
    [SerializeField] private float valorObjeto1_x = 0f;
    [SerializeField] private float valorObjeto1_y = 0f;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if(asignarValores){
            vector_A = new Vector2(valorA_x,valorA_y);
            transform.position = vector_A;
        
            Vector2 vector_B= new Vector2(valorObjeto1_x,valorObjeto1_y);
            Objeto1.position = vector_B;
        }else{
            transform.position = transform.position;
            Objeto1.position = Objeto1.position;
        }
        

        //Distancia entre el vertice y el objeto
        if(mostrarMag)
            Debug.Log("Magnitud desde el vertice : "+ transform.position.magnitude);

        
        Vector2 SumVectores = transform.position + Objeto1.position;
        if(mostrarSuma)
            Debug.Log("Suma de los dos Vectores : "+ SumVectores.magnitude);


        //Diferencia es la distancia que existe entre ellos
        Vector2 difVectores = Objeto1.position - transform.position;
        if(mostrarResta)
            Debug.Log("Magnitud (Distancia) entre los objetos : "+ difVectores.magnitude);


    }

    void OnDrawGizmos(){
    
        Gizmos.color = Color.white;
        if(mostrarResta)
            Gizmos.DrawLine(transform.position, Objeto1.position);
        if(mostrarSuma)
            Gizmos.DrawLine(new Vector2(0,0), transform.position + Objeto1.position);

    if(asignarValores){
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector2(0,0), transform.position);
        Gizmos.DrawLine(new Vector2(0,0), Objeto1.position);
    }
    
    }
}
