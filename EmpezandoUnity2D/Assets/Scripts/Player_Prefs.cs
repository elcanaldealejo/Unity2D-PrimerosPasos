using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Prefs : MonoBehaviour
{
    private int monedas,banco;
    private Button ButtonSave;
    public bool Reset;
    // Start is called before the first frame update
    void Start()
    {
        ButtonSave = GameObject.Find("Button_save").GetComponent<Button>();
        ButtonSave.onClick.AddListener(TaskOnClick);
        monedas = PlayerPrefs.GetInt("monedasPrefs",0);
        banco = PlayerPrefs.GetInt("bancoPrefs",0);
        printCoins();

          /* PlayerPrefs.GetString("idName","");
          PlayerPrefs.GetFloat("idName",0f); */          
    }
        void Update(){
            if(Reset)
                ResetValues();
        }

    public void printCoins(){//Muestra los valores actuales en pantalla para cada variable
        GameObject.Find("Monedas_GO").GetComponent<Text>().text = "Monedas: $" + monedas;
        GameObject.Find("Banco_GO").GetComponent<Text>().text = "Banco: $" + banco;
    }

    public void saveCoins(){
        PlayerPrefs.SetInt("monedasPrefs",monedas);

        /* PlayerPrefs.SetString("idName",variable);
          PlayerPrefs.SetFloat("idName",value); */
    }
    void TaskOnClick()
    {
        PlayerPrefs.SetInt("monedasPrefs",0);
        PlayerPrefs.SetInt("bancoPrefs",banco+monedas);
        banco+=monedas;
        monedas=0; 
        printCoins();      
    }
    private void OnCollisionEnter2D(Collision2D collision){
     if(collision.gameObject.layer == 14){//El id layer 14 es Coins
        monedas+=1;
        collision.gameObject.SetActive(false);
        AudioManager.instance.PlaySounds(1);
        printCoins();//Imprime valores de las variables
        //saveCoins();//Almacena datos para la aplicación
        /* NOTA: No es recomendable cada vez que cambie un valor almacenar al PlayerPrefs,
         en una base de datos o sistema de Archivos. Es mejor crear puntos de control,
         sitios de guardado, checkPoints, al finalizar un escenario, despues de cierto tiempo
         en zonas seguras o al momento de cumplir una misión.
         Así podemos reducir el uso de recursos y lograr que sea más fluido la ejecución de nuestro juego. */
     }
   }

   private void ResetValues(){//Retornar valores a cero(0)
        PlayerPrefs.SetInt("monedasPrefs",0);
        PlayerPrefs.SetInt("bancoPrefs",0);
        banco=0;
        monedas=0; 
        printCoins(); 
        Reset=false;
   }
}
