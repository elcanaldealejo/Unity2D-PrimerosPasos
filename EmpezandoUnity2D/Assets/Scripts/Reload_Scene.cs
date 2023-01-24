using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Reload_Scene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("rutina");
    }


 IEnumerator rutina(){

    yield return new WaitForSeconds(0.5f);
    SceneManager.LoadScene(PlayerPrefs.GetString("escena_load",""));  }
    
}
