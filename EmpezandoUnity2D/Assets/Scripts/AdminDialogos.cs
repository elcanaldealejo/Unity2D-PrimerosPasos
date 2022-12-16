using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminDialogos : MonoBehaviour
{
    public bool cargaTextos;//coloco este activador para cargar los textos o no; mientras se este en desarrollo
    public static AdminDialogos instance;//Creamos una instancia de la clase para poder acceder desde todos los NPC con quien se converse
    public int NumDialogos;//Este valor lo administra quien carga los dialogos al arreglo el sabe cuantas conversaciones graba

    //Contenedores Multi-Lenguaje
    public string[] ES , EN;// segun el numero de Lenguajes se crea un arreglo

    public string[] turnoDialogo;//Almacena quien habla en cada conversacion que se carga desde los NPC
    
    void Awake()
    {
        //Se supone que el numero de conversaciones en un idioma debe ser igual en todos los demas idiomas
        ES = new string[NumDialogos];
        EN = new string[NumDialogos];

        if(cargaTextos){
            //Español
            ES[0] = ">Hola...>Quien Eres?<Hola, mi nombre es Lilo>Que haces aquí?<Estoy estudiando Unity2D>En serio... Yo también>Está bien te dejo entonces. Chao<Adiós. Hasta pronto";
            ES[1] = "<Estoy muy ocupado ahora<vuelve más tarde>OK!";
    
    
            //Inglés
            EN[0] = ">Hello...>Who are you?<Hello, my name is Lilo>What are you doing here?<I'm studying Unity2D>Seriously... Me too>Okay I'll leave you then. Bye <bye. See you soon";
            EN[1] = "<I'm really busy right now...<Come back later>OK!";
    //
        }
        instance = this;
    }
    void Start(){
        
    }

    public string[] OrdenaDialogo(string value){//letra por letra hasta completar una oración, almacena en arreglo y prosigue a la siguiente oración
        string[] dialogo;
        string D="";//Almacena cada Oración
        int next=1;//Se inicia en 1 para que empiece a crear la primera oración apenas entra al ciclo
        int numC = numDialogos(value)+1;//Sumamos 1 más para que salga de la conversacion en el ultimo vacio
        dialogo = new string[numC];//almacenará las oraciones
        turnoDialogo = new string[numC];//Almacenará quien habla >(representará al Jugador) y <(Representa al NPC)        
        turnoDialogo[0] = value.Substring(0,1);//Quien inicia la conversacion (Guarda el Caracter(> ó <) para distinguir despues)
        for(int u=0;u<numC-1;u++){//for para guardar Oraciones
            for (int i=next;i<value.Length;i++){//for para construir cada Oración
                if(value.Substring(i,1)=="<"|| value.Substring(i,1)==">"){
                    turnoDialogo[u+1] = value.Substring(i,1);//almacena caracter de que personaje continuará la proxima oración
                    next=i+1; //se almacena en que caracter debe de continuar para almacenar la siguiente oración
                    break;// Se hacer break para salir del for inmediatamente
                }
                D+=value.Substring(i,1);//Construye la oración caracter por caracter
            }
            dialogo[u]=D;//Guarda la oracion creada y pasa a gestionar la siguiente
            D="";
        }
        return dialogo;//Al final retorna todos las oraciones en un arreglo ordenadas que corresponde a toda la conversación
    }
    public int numDialogos(string value){//Devuelve el numero de dialogos para la conversación
        int numC =0;
        for (int i=0;i<value.Length;i++){
            if(value.Substring(i,1)=="<"|| value.Substring(i,1)==">"){
                numC++;
            }
        }
        return numC;
    }
}
