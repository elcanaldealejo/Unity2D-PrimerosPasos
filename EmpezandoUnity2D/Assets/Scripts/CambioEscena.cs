using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CambioEscena : MonoBehaviour
{
    private bool iniciarFin;
    public string nextScene;
    public Text textoFin;
    public Image panelOscuridad;
    private Color c;
    private float Transition;
    
    void Start()
    {
        c = new Color(panelOscuridad.color.r,panelOscuridad.color.g,panelOscuridad.color.b,0);
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag=="Jugador" && !iniciarFin){
            iniciarFin=true;
            StartCoroutine("preparaFin");
        }
   }

   IEnumerator preparaFin(){
        textoFin.text="Congratulations!!!";
        yield return new WaitForSeconds(2f);
        Transition=0;
        while(Transition<250){ 
            c.a+=0.2f;
            panelOscuridad.color = c;                       
            yield return new WaitForSeconds(0.1f);
            Transition+=20;            
        }
        textoFin.text="";
        SceneManager.LoadScene(nextScene);
   }
}
