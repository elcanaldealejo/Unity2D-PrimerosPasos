using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class mov_mundos : MonoBehaviour
{
    public static mov_mundos instance;
    public GameObject _personaje=null; //personaje que se desplaza por el mapa
    public GameObject botonjugar=null; //Boton que se habilita cuando estamos sobre un mundo
    public int cantidadMundos=0; //inicializamos con el valor de mundos creados en el mapa
    private int mundoActual=0;// cada vez que vamos a un mundo adopta el valor del nivel
    public bool[] Mundos; // Almacena con true cuando el mundo se completa o false si no

    void Awake(){
        Mundos  = new bool[cantidadMundos]; // otrogamos el tamaño al arreglo igual al numero de mundos
        if(PlayerPrefs.GetString("info_mundos","")!="") 
            inicializarMundos(PlayerPrefs.GetString("info_mundos",""));// Según la info de los mundos del mapa se gestiona el arreglo Mundos
        else   
            PlayerPrefs.SetString("info_mundos",inicializarMundos_NewGame());//Sí el prefs con la información de los mundos del mapa aun no existe se crea  
    }
    void Start(){
        instance = this;
        habilitarMundos();
        if(PosUltimoMundo()>0){// Aqui le decimos al personaje que se mueva al ultimo mundo que logro terminar
            _personaje.transform.position = GameObject.Find("mundo"+(PosUltimoMundo())).transform.position;
            mundoActual = PosUltimoMundo();
        }
    }
    private void inicializarMundos(string mundos){
        /* 
        Se recibe el prefs para crear el arreglo de mundos pasados y aun pendientes
         */
        int cont=0;
        for (int i=0;i<Mundos.Length;i++){
            if(mundos.Substring(i,1)!="-"){
                if( mundos.Substring(i,1)=="0"){
                    Mundos[cont]=false; 
                    cont++;                   
                }else{
                    Mundos[cont]=true;
                    cont++; 
                }
            }           
        }
    }
    private void habilitarMundos(){
        /* 
        Siempre y cuando el mundo anterior este completo permite habilitar el boton del siguiente
         */        
        for (int i=0;i<Mundos.Length;i++){
           if(Mundos[i])
             GameObject.Find("mundo"+(i+2)).GetComponent<Button>().interactable=true;          
        }
    }
    public int PosUltimoMundo(){
        int cont=0;
         for (int i=0;i<cantidadMundos;i++){ 
                if(Mundos[i])//un mundo completado se mostrara naranja
                    GameObject.Find("mundo"+(i+1)).GetComponent<Image>().color = new Color(1f,0.7185f,0f,0.5019608f);//naranja
                else if(!Mundos[i] && GameObject.Find("mundo"+(i+1)).GetComponent<Button>().interactable)//Pero un mundo no completado y disponible para jugar verde
                    GameObject.Find("mundo"+(i+1)).GetComponent<Image>().color = new Color(0.1562241f,1f,0f,0.5019608f);//verde 
                     
                cont += Mundos[i] ? 1 : 0; //contamos los mundos pasados para dejar el contador en el ultimo mundo completado
         }
         return cont;
    }
    private string inicializarMundos_NewGame(){
        /*    
        Se construye una info de mundos para el mapa vacia segun sea el numero de mundos descritos en el inspector
        */
        string constructo ="";
        for (int i = 0; i < cantidadMundos; i++)
        {
            constructo+="0-";
        }
        return constructo;
    }
    public void moverMundo(int nivel){// Cuando se da clic en el botón activo de un mundo, solicitamos desplazamiento hacia este            
        StartCoroutine(desplazar(GameObject.Find("mundo"+nivel).transform.position));
        mundoActual = nivel;         
    }
    public void cargarEscena(){//Si el boton de Jugar esta activo se carga la escena con el nombre y numero de nivel elejido
        SceneManager.LoadScene("Mundo"+(mundoActual));
        //Como creamos los mundos o escenarios desde Mundo1, Mundo2 etc. sumamos 1 al mundo seleccionado ya que estos empiezan desde cero(0)
    }
    public void MenuPpal(){//cargar menú principal
        SceneManager.LoadScene("MenuPpal");
    }
    IEnumerator desplazar(Vector3 posMundo){// Corrutina de movimiento a un punto a otro en el mapa
        Vector3 resta = _personaje.transform.position - posMundo; 
        //creamos un nuevo vector para conocer mas adelante la distancia entre los puntos, el donde se encuentra el personaje y a donde quiere ir 
        float x=0f;
        float y=0f;
        float val=0.3f;// representa el valor adicional que se va ir desplazando en el espacio
        _personaje.GetComponent<Animator>().SetBool("caminar" , true);//habilitamos la animación de caminar
        botonjugar.SetActive(false);// ya cuando va empezar a moverse inactiva el boton de jugar hasta que llegue a un nuevo mundo
        while (resta.magnitude >= 0.5f)//recordemos que la magnitud de un vector es la distancia
        {
            /* 
                El objetivo es aproximarse cada vez más al mundo elejido, asi la distancia se va reduciendo.
                El valor de movimiento adicional se sumará positivamente si el personaje es menor en x o y.
                El valor de movimiento adicional se sumará negativamente si el personaje es mayor en x o y.
            */
            if(_personaje.transform.position.x < posMundo.x){//personaje es menor en x
                x=val; 
                _personaje.GetComponent<SpriteRenderer>().flipX=false;
                }
                else if(_personaje.transform.position.x > posMundo.x){//personaje es mayor en x
                x=val*-1;
                _personaje.GetComponent<SpriteRenderer>().flipX=true;//Giramos el sprite para que mire hacia donde se dirige
                }

            if(_personaje.transform.position.y < posMundo.y)//personaje es menor en y
                y=val;
                else if(_personaje.transform.position.y > posMundo.y)//personaje es mayor en y
                y=val*-1;

            _personaje.transform.position = new Vector3(_personaje.transform.position.x+x,_personaje.transform.position.y+y,_personaje.transform.position.z);
            //Otrogamos al personaje la posición nueva en cada tiempo de la corutina
            yield return new WaitForSeconds(0.1f);
            x=0f;
            y=0f;
            //volvemos los valores cero (0) para una nueva validación despues del tiempo de espera de la corutina
            resta = _personaje.transform.position - posMundo;//actualizamos la salida del While reduciendo cada vez la distancia
        }
        _personaje.GetComponent<Animator>().SetBool("caminar" , false);// cuando sale del while es porque ya llego al mundo, deje de caminar
        botonjugar.SetActive(true);//Activamos el botón para poder jugar el mundo alcanzado
        
    }

}
