using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadosMovement : MonoBehaviour
{
    [SerializeField]GameObject[] numberTornados;
    IEnumerator timeDuration;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(gameObject.activeSelf == true )
        {
            timeDuration = TornadosDuration(0.3f);
            StartCoroutine(timeDuration);

            timeDuration = TornadosDurationToDeactivation(5);
            StartCoroutine(timeDuration);
        }
    }

    IEnumerator TornadosDuration(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
    }

    IEnumerator TornadosDurationToDeactivation(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        for (int i = 0; i < numberTornados.Length; i++)
        {
            numberTornados[i].transform.position = transform.position;
        }
    }
}
