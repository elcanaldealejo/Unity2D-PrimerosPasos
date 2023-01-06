using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdminVideo : MonoBehaviour
{
    public VideoPlayer[] videosIntro; //En este arreglo almacenamos los videos pero ya agregados a un VideoPlayer
    public Button Omitir; // botón que nos permite pasar el video en curso
    private VideoPlayer videoPlayer; 
    private int duracion;// contendrá la duración de cada video y disminuirá el valor durante reproducción o se volverá cero si se pulsa en la pantalla
    void Start()
    {
        StartCoroutine("reproducirIntro");
    }
    public void OmitirVideo(){
        duracion = 0;
    }
   IEnumerator reproducirIntro(){
        //Primer Video
        duracion=5;//almacenamos la duración en seg de este video
        videoPlayer=videosIntro[0];
        while(duracion>0){
            videoPlayer.Play();
            yield return new WaitForSeconds(1f);
            duracion--;
        }
        //Segundo Video
        videosIntro[0].enabled=false; //inhabilitamos el video ya pasado
        //Omitir.interactable=true;
        duracion=8;//almacenamos la duración en seg de este video
        videoPlayer=videosIntro[1];
        while(duracion>0){
            videoPlayer.Play();
            yield return new WaitForSeconds(1f);
            duracion--;
        }
        videosIntro[1].enabled=false;
        //Despues de que todos los videos fueron reproducidos ir al menú principal
        SceneManager.LoadScene("MenuPpal");
   }
}
