using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    private Animator _animator;
    public float sizeRayo;
    public bool estado=true;
    public LayerMask seActivaCon;
    public bool FinEscena;
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    
    void Update()
    {
        if(CreateRaycast("Up",sizeRayo,seActivaCon) && estado){
            _animator.enabled = true;
            Admin_Level.instance.check++;
            Admin_Level.instance.ActualizarPrefsCoinsMundo();
            Admin_Level.instance.ActualizarPrefsEnemyMundo();
            estado=false;
            if(FinEscena)
                Admin_Level.instance.mundo_passed=true;
        }

    }

    private bool CreateRaycast(string Orientacion, float Longitud, LayerMask mascara){

        Vector2 dir = transform.position;
        if(Orientacion=="Up")
            dir = transform.TransformDirection(Vector2.up) * Longitud;
        if(Orientacion=="Down")
            dir = transform.TransformDirection(Vector2.down) * Longitud;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, Longitud, mascara);
            Debug.DrawRay(transform.position, dir , hit ? Color.yellow : Color.blue); //Ya no es un Gizmo
    
        if(hit)
            return true;
        else
            return false;
    }
}
