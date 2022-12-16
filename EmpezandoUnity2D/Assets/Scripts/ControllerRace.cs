using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerRace : MonoBehaviour
{
    public static ControllerRace instance;
    public GameObject[] corredores;
    public bool iniciar;
    public Text InfoRace;
    private float guardaTiempo;
    public float tiempoConteo = 5f;
    public float posSalida;
    public int[] pos;
    // Start is called before the first frame update
    void Start()
    {
        instance =this;
        pos = new int[corredores.Length];
        guardaTiempo = tiempoConteo;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(iniciar && corredores.Length>1){
            StartCoroutine("conteoInicial");
            iniciar=false;
        }
        if(tiempoConteo==0f && corredores.Length>1){
            tablaPosiciones();
        }
    }

IEnumerator conteoInicial(){
    float cuentaAtras = tiempoConteo;
    while(cuentaAtras>0f){
        InfoRace.text = ""+cuentaAtras;
        yield return new WaitForSeconds(1f);
        cuentaAtras-=1f;
    }
    InfoRace.text = "Go!";
    tiempoConteo=0f;
    yield return new WaitForSeconds(1f);

}

public void tablaPosiciones(){
        for(int i=0; i<corredores.Length;i++)
            pos[i]=0;
    
        pos = new int[corredores.Length];
        for(int i=0; i<corredores.Length;i++){
            int cont=0;
                for(int u=0; u<corredores.Length;u++){
                    if(corredores[i].transform.position.x > corredores[u].transform.position.x)
                       cont++;
                }
                  pos[i] = corredores.Length - cont;  
        }
    
}
private void OnTriggerEnter2D(Collider2D collider){
       
        if(collider.gameObject.tag=="Jugador" || collider.gameObject.tag=="Enemigo"){
            Debug.Log("Un objeto con Nombre: "+collider.gameObject.name+" Capturo "+gameObject.name);
            tiempoConteo = guardaTiempo;
            InfoRace.text = "Finish!!!";
        }
}

}
