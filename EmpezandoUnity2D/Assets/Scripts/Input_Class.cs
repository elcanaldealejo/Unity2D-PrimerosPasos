using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Input_Class : MonoBehaviour
{
    public static Input_Class instance;
    public bool PC_Controller;
    public bool Mobile_Controller;
    public GameObject cerraPausa;

    public GameObject Panel_MobilePad = null;

    void Start(){
        instance = this;
        if(Mobile_Controller){
            Panel_MobilePad.SetActive(true);
            
        }   
    }
    void Update(){
        if(Admin_Level.instance.panel_pausa.activeSelf && Mobile_Controller)
            cerraPausa.SetActive(true);
    }

}
