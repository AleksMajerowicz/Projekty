using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FunctionsOfTheUI : MonoBehaviour
{
    [SerializeField] GameObject loss = null;
    [SerializeField] PlayerMovement PM = null;
    // Start is called before the first frame update

    public void Off()
    {
        loss.gameObject.SetActive(true);
        PM.enabled = false;
        Time.timeScale = 0;
    }

    public void On()
    {
        SceneManager.LoadScene(0);
        loss.gameObject.SetActive(false);
        //PM.enabled = true;
        //PM.gameObject.transform.position = new Vector3(0, 1, -499.64f);
        Time.timeScale = 1; 
    }

    void Start()
    {
        loss.SetActive(false);
    }

}
