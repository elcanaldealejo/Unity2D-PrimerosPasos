using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class UI_MenuPpal : MonoBehaviour
{
    public bool continuar;
    [Header ("Botones Menu")]
    [SerializeField] private Button continueGameB=null;  

    [Header ("Paneles Menu")]
      [SerializeField] private GameObject optionPanel=null;
      [SerializeField] private Image brilloPanel=null; 

     [Header ("Opciones Elementos")]
      [SerializeField]  private Dropdown dificult=null;
      [SerializeField]  private InputField namePlayer=null;
      [SerializeField]  private Scrollbar brilloLevel=null;
      [SerializeField]  private Slider soundLevel=null;
      [SerializeField]  private Toggle mute=null;
      [SerializeField]  private AudioSource Music=null; 

      [Header ("Variables")]
      [SerializeField] private int dificultValue;
      [SerializeField] private string nameValue;
      [SerializeField] private float brilloSelect;
      [SerializeField] private float volumeValue;
      [SerializeField] private bool muteValue;

      private Color c;
      

    // Start is called before the first frame update
    void Start()
    {   
        if(PlayerPrefs.GetString("info_mundos","")!=""){
            continueGameB.interactable=true;
        }else{
            InitControllerPrefs();
        }     
        dificultValue = PlayerPrefs.GetInt("dificultPrefs",0);
        nameValue = PlayerPrefs.GetString("namePrefs","");
        brilloSelect = PlayerPrefs.GetFloat("brilloPrefs",0);
        volumeValue = PlayerPrefs.GetFloat("volumePrefs",1f);
        Music.volume = volumeValue;
        c = new Color(brilloPanel.color.r,brilloPanel.color.g,brilloPanel.color.b,brilloSelect);
        brilloPanel.color= c;
        if(PlayerPrefs.GetInt("mutePrefs",0)!=0)
            muteValue=true;

            UpdatePrefs();
    }

    // Update is called once per frame
    void Update()
    {
        if(optionPanel.activeSelf){
            dificultValue = dificult.value;
            dificult.captionText.text = ""+dificult.options[dificultValue].text;
            volumeValue = soundLevel.value;
            Music.volume = volumeValue;
            muteValue = mute.isOn;
            Music.mute = muteValue;
            brilloSelect = brilloLevelStepValue(brilloLevel.value);
            c = new Color(brilloPanel.color.r,brilloPanel.color.g,brilloPanel.color.b,brilloSelect);
            brilloPanel.color= c;
            nameValue = namePlayer.text;
        }
        if(continuar)
            continueGameB.interactable=true;
    }
    public void InitControllerPrefs(){
        //Metodo para Crear o Modificar Prefs
        //Esté Script presentará errores si la aplicación no detecta los Prefs con los siguientes Nombres
            PlayerPrefs.SetString("Jump","J");
            PlayerPrefs.SetString("Action","X");
            PlayerPrefs.SetString("Cancel","C");
            PlayerPrefs.SetString("Inventory","I");
            PlayerPrefs.SetString("Horizontal_Neg","A");
            PlayerPrefs.SetString("Horizontal_Pos","D");
            PlayerPrefs.SetString("Vertical_Neg","S");
            PlayerPrefs.SetString("Vertical_Pos","W");
     }
    public void UpdatePrefs(){//Actualiza toda la UI activa en la escena con los valores que tengan las variables
            dificult.value = dificultValue;
            dificult.captionText.text = ""+dificult.options[dificultValue].text;
            soundLevel.value = volumeValue;
            Music.volume = volumeValue;
            mute.isOn = muteValue;
            Music.mute = muteValue;
            brilloLevel.value = brilloLevelStepScroll(brilloSelect);
            namePlayer.text = nameValue;

    }

    public float brilloLevelStepValue(float value){// Según la posición del Scroll devuelve valor para dar opacidad/transparencia
        if(value>0.2f && value<0.4f)
           return 0.6f;
        if(value>=0.4f && value<0.6f)
           return 0.4f;
        if(value>=0.6f && value<0.8f)
            return 0.2f;
        if(value>=0.8f)
            return 0f;
       
        return 0.8f;
    }
    public float brilloLevelStepScroll(float value){//Devuelve el valor para posicionar el Scroll
        if(value>=0.2f && value<0.4f)
           return 0.75f;
        if(value>=0.4f && value<0.6f)
           return 0.55f;
        if(value>=0.6f && value<0.8f)
            return 0.25f;
        if(value>=0.8f)
            return 0f;
       
        return 1f;
    }

    public void optionsPanel_view(){//Muestra u oculta el panel Opciones
        if(!optionPanel.activeSelf)
            optionPanel.SetActive(true);
            else
                optionPanel.SetActive(false);
    }
    public void saveOptions(){// Guarda las preferencias del usuario para la opciones
        PlayerPrefs.SetInt("dificultPrefs",dificultValue);
        PlayerPrefs.SetString("namePrefs",nameValue);
        PlayerPrefs.SetFloat("brilloPrefs",brilloSelect);
        PlayerPrefs.SetFloat("volumePrefs",volumeValue);
        if(muteValue)
            PlayerPrefs.SetInt("mutePrefs",1);
            else
            PlayerPrefs.SetInt("mutePrefs",0);
        optionsPanel_view(); 
    }
    public void restoreOptions(){//Restaura los valores por defecto para las opciones
        PlayerPrefs.SetInt("dificultPrefs",0);
        //PlayerPrefs.SetString("namePrefs","");
        PlayerPrefs.SetFloat("brilloPrefs",0);
        PlayerPrefs.SetFloat("volumePrefs",1f);
        PlayerPrefs.SetInt("mutePrefs",0);            
        optionsPanel_view();
        dificultValue = 0;
        //nameValue = "";
        brilloSelect = 0f;
        volumeValue = 1f;
        Music.volume = volumeValue;
        c = new Color(brilloPanel.color.r,brilloPanel.color.g,brilloPanel.color.b,0f);
        brilloPanel.color= c;
        muteValue=false; 
        UpdatePrefs();
    }
    public void Load_newGame(){//Inicia al usuario a la primer escena por ver
        PlayerPrefs.SetString("info_mundos","");
        PlayerPrefs.SetInt("monedasPrefs",0);
        SceneManager.LoadScene("City_1");
    }
    public void Load_continueGame(){//Envia al Usuario a la ultima escena Jugada
        SceneManager.LoadScene("City_1");
    }
    public void ExitGame(){
         #if UNITY_EDITOR
         UnityEditor.EditorApplication.isPlaying = false;
         #else
         Application.Quit();
         #endif        
    }

}
