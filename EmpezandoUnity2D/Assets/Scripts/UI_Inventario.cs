using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Luminosity.IO;

public class UI_Inventario : MonoBehaviour
{
    public static UI_Inventario instance;
    public GameObject panelInventario=null;
    private string[] imageItem;
    private int[] cantItem;

    public Image[] ImagenEspacio;
    public Text[] CantItemText;

    public RectTransform posIni;
    public RectTransform posFin;
    public float VelMov=0.5f;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        imageItem = new string[3] {"","",""};
        cantItem = new int[3] {0,0,0};        
    }

    // Update is called once per frame
    void Update()
    {
        
        if(InputManager.GetButtonUp("Inventory") && !UI_Controller.instance.panelController.activeSelf && !UI_Dialogo.instance.PanelDialogo.activeSelf){
            if(!panelInventario.activeSelf){
                    panelInventario.SetActive(true);
                    StartCoroutine("Movimiento1");
            }
            else{                
                Time.timeScale=1;
                StartCoroutine("Movimiento2");                
            }
        }  

    }

    IEnumerator Movimiento1(){//Derecha
        while(distance(1) > 0f){  
            //Sumamos cuantos frames queremos que se desplace en X          
            panelInventario.GetComponent<RectTransform>().position = new Vector3(panelInventario.GetComponent<RectTransform>().position.x + VelMov , panelInventario.GetComponent<RectTransform>().position.y,panelInventario.GetComponent<RectTransform>().position.z);
            yield return new WaitForSeconds(0.05f);            
        }
        Time.timeScale=0;          
    }
    IEnumerator Movimiento2(){//Izquierda
        while(distance(0) > 0f){ 
            //Restamos cuantos frames queremos que se desplace en X           
            panelInventario.GetComponent<RectTransform>().position = new Vector3(panelInventario.GetComponent<RectTransform>().position.x - VelMov , panelInventario.GetComponent<RectTransform>().position.y,panelInventario.GetComponent<RectTransform>().position.z);
            yield return new WaitForSeconds(0.05f);            
        }   
        panelInventario.SetActive(false); //Despues de retornar a la posicion inicial de inactiva el panel
    }
   
     public float distance(int dir){    
        float resta=0;
        if(dir==1)//Derecha
                resta = posFin.position.x - panelInventario.GetComponent<RectTransform>().position.x;
            else//Izquierda
                resta =  panelInventario.GetComponent<RectTransform>().position.x - posIni.position.x;
            
        return resta;//retorna un valor positivo siempre
     }

    private void OnTriggerEnter2D(Collider2D collider){

        if(collider.gameObject.layer == 11){//muerte
            AdminCamera2D.instance.DetenerSeguimiento();
        }
      
        if(collider.gameObject.layer == 13){//Layer Restablecer vida
            UI_Vida.instance.Salud(30);// se envia al instance del UI vida el valor a incrementar
            collider.gameObject.SetActive(false);
        }
        

   }

   public void UpdateInventario(GameObject item){
    int pos_disponible=-1;
            int cont=0;

            for(int i=0;i<3;i++){//Si ya existe un objeto con ese nombre se suma en cantidad
                if(item.GetComponent<SpriteRenderer>().sprite.name == imageItem[i])
                    pos_disponible=i;//guardamos en que posicion del arreglo vamos hacer cambios
            }

            while(pos_disponible<0){// Si aun no existe el item en el inventario le otorga un espacio
                if(cont>2)//si el contador supera el tamaño del arreglo, salga del ciclo
                    break;
                if(imageItem[cont]==""){//Donde encuentre espacio vacio...
                    imageItem[cont] = item.GetComponent<SpriteRenderer>().sprite.name;//almacene el nombre del sprite en el arreglo
                    ImagenEspacio[cont].sprite = item.GetComponent<SpriteRenderer>().sprite;//cargue en el item del inventario la misma imagen que tenia el item recogido
                    ImagenEspacio[cont].enabled = true;//habilitamos el item para que se muestre 
                    pos_disponible=cont; //guardamos en que posicion del arreglo vamos hacer cambios
                }                
                cont++;
                
            }

            if(pos_disponible>=0){//mientras el valos sea mayo o igual que cero, significa que trajo un espacio donde hacer cambios
                cantItem[pos_disponible]++;// se aumentan cantidates
                CantItemText[pos_disponible].text="x"+cantItem[pos_disponible];//la cantidad se imprime en el texto
                item.SetActive(false);//el item se desactiva
            }
   }
}
