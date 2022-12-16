using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    
public static AudioManager instance;

    public AudioSource[] sonidoEscena;
    public bool ModSpeed;
    private void Awake(){
        instance = this;
    }
    
    void Start(){
        PlaySounds(0);
    }
    void Update(){

        if(ModSpeed){//Ejemplo cambio de velocidad
            FasterSounds(0);
            ModSpeed=false;
        }

    }

    public void PlaySounds(int sonido){
        sonidoEscena[sonido].Play();
    }
    public void PauseSounds(int sonido){
        sonidoEscena[sonido].Pause();
    }
    public void StopSounds(int sonido){
        sonidoEscena[sonido].Stop();

    }
    public void FasterSounds(int sonido){
        if(sonidoEscena[sonido].pitch<=1f)
            sonidoEscena[sonido].pitch = 1.5f;
            else
              sonidoEscena[sonido].pitch = 1f; 
    }


}
