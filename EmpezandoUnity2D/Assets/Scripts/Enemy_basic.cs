using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_basic : MonoBehaviour
{
    public int nivel;
    public float salud;
    private float ori_salud;
    public float ataque;
    private float ori_ataque;
    public float defensa;
    private float ori_defensa;
    public bool invulnerable;
    public bool loop;
    public GameObject[] premios;
    public int exp;
    public Transform posPremios;
    void Awake(){
        ocultarPremios();
        ori_ataque = ataque;
        ori_defensa = defensa;
        ori_salud = salud;
    }
    void Update()
    {
        if(!invulnerable && salud<=0){
            if(premios.Length>0){
                posPremios.position = new Vector3(transform.position.x,transform.position.y + 3f,transform.position.z);
                darPremios();
                StartCoroutine("ObjetoVencido");
                premios = new GameObject[0];
            }           
        }
    }
    public void RevivirEnemy(){
        salud = ori_salud;
        defensa = ori_defensa;
        ataque = ori_ataque;
    }
    IEnumerator ObjetoVencido(){

        var cont=10f;
        while(cont>0f){
            gameObject.GetComponent<SpriteRenderer>().enabled = !gameObject.GetComponent<SpriteRenderer>().enabled;
            yield return new WaitForSeconds(0.02f);
            cont-=0.2f;
            gameObject.GetComponent<EdgeCollider2D>().enabled=false;
        }
        if(!loop)
            gameObject.SetActive(false);
           
    }
    public void reducirSalud(float value){
        salud = salud-value<0 ? 0 : salud - value;        
    }
    private void darPremios(){
        for(int i=0;i<premios.Length;i++){
            premios[i].SetActive(true);
            premios[i].transform.position = posPremios.position;
        }
    }
    private void ocultarPremios(){
        for(int i=0;i<premios.Length;i++){
            premios[i].SetActive(false);
        }
    }
}
