using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibraCamera : MonoBehaviour
{
    public static VibraCamera instance;
[Header("Configuraciones - test L -")]
   [SerializeField] [Range( 0 , 10 )] private float Nivel_Agitar = 10f;//Cantidad de iteraciones para agitarse
   [SerializeField] [Range( 0 , 0.5f )] private float Random_Agitar = 0.2f;//Valor que hace incrementar una posicion aleatoria en el espacio
   private float Tiempo_Agitar = 0.01f;
void Start(){
    instance = this;
}
private void Update(){
    if(Input.GetKeyDown(KeyCode.L)){
        Vibrar();
    }
}

public void Vibrar(){
    StartCoroutine(IEAgitar());
}

private IEnumerator IEAgitar(){
    Vector3 ActualPos = transform.position;
    
    for(int i =0; i < Nivel_Agitar; i++){
        Vector3 randomVector = Random.onUnitSphere;
        randomVector = new Vector3(randomVector.x,randomVector.y,0f);
        Vector3 AgitarPos = ActualPos + randomVector * Random_Agitar;
        yield return new WaitForSeconds(Tiempo_Agitar);
        transform.position = AgitarPos;
    }
}
}
