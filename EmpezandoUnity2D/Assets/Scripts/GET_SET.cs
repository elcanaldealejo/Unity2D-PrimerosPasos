using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GET_SET : MonoBehaviour
{
    public TipoPersonaje Tipo = new TipoPersonaje();  
    private string tipoSelec;  
    
    [Header("Atributos")]  
    [SerializeField] private int atk;
    [SerializeField] private int def;
    [SerializeField] private float lp;
    [SerializeField] private int manzanas;

    
    // Start is called before the first frame update
    void Start()
    {        
        tipoSelec = Tipo.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("i")){
            iniciaAtributos();
        }
        if(Input.GetKeyDown("p")){
            imprimeAtributos();
        }

    }
public int ATK{
        get{
            return atk;
        }
        set{
            if(tipoSelec!="none"){
                if( value>=40 && tipoSelec=="Guerrero")
                    atk=value;
                    else if(tipoSelec!="Guerrero")
                        atk=value;
                        else{
                         atk=40;
                            print("El ataque de un Guerrero no puede ser menor de 40 Puntos");
                        }
            }
        }
    }
    public int DEF{
        get{
            return def;
        }
        set{
            if(tipoSelec!="none"){
            if( value <= 20 && tipoSelec=="Mago")
                def=value;
                else if(tipoSelec!="Mago" )
                    def=value;
                    else{
                        def=20;
                        print("La Defensa de un Mago no puede ser mayor a 20 Puntos");                        
                    }
            }
        }
    }
    public float LP{
        get{
            return lp;
        }
        set{
            if(tipoSelec!="none"){
                lp=value;
            }
        }
    }
    public int MANZANAS{
        get{
            return manzanas;
        }
        set{
            if(tipoSelec!="none"){
                if(value < 5-manzanas)
                     manzanas+=value;
                        else if(manzanas<5){
                            var a = 5 - manzanas;
                            print("Se recogieron "+(a)+" manzana(s)");
                            manzanas = manzanas + a;
                            if(5+a>0)
                                print("EL INVENTARIO ESTA LLENO!!!, quedan "+(value-(a))+" Manzanas sin recoger");                     
                        }
            }
        }
    }
    private void iniciaAtributos(){
        
        ATK = Random.Range(10,50);
        DEF = Random.Range(30,50);
        LP = Random.Range(500,1000);
        MANZANAS = Random.Range(1,10);
    }
    private void imprimeAtributos(){
        if(MANZANAS>0){
            print("Ataque: "+ATK);
            print("Defensa: "+DEF);
            print("Vida: "+LP );
            manzanas-=1;
            print("Manzanas: "+MANZANAS+" valor por Imprimir = 1 Manzana");
            
        }else
            print("SIN SALDO PARA IMPRIMIR");
    }

}
public enum TipoPersonaje
     {
         none,
         Mago, 
         Guerrero, 
         Necromante,
         Druida
     };