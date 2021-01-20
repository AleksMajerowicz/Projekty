using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;

public class Generator : MonoBehaviour
{
    [SerializeField] GameObject hindrace = null;//Przeszkoda
    [SerializeField] GameObject playerPosition = null;//Pozycja Gracza
    Vector3 startingPositionOfThObstacle = new Vector3(0, 1f, -499.64f);//Startowa pozycja Przeszkodyy
    float curentPositionX = 0;//AktualnaPozycja w osi X
    float positionX = 1;//PozycjaX
    int counter = 1;//Licznik
    List<GameObject> obstlaceOnMap = new List<GameObject>();
    IEnumerator time;

    //Generator Przeszkód
    void ObstlaceGenerator(Vector3 curretPosition)
    {
        Vector3 curretPositionHindrace;
        curretPositionHindrace = new Vector3(curentPositionX = (int)Random.Range(-1.9f, 1.9f) * 3, curretPosition.y,
            curretPosition.z + 10 /*Random.Range(0.5f, 10)*/);
        if (curentPositionX != positionX || curentPositionX == positionX && counter < 3)
        {
            obstlaceOnMap.Add(Instantiate(hindrace, curretPositionHindrace, Quaternion.Euler(Vector3.zero)));//Pierw wykonuje się funckaj Instantine(co tworzy obiekt) a potem ten obiekt która ta funckja zwrociła jest dodoawany do listy
            positionX = obstlaceOnMap[obstlaceOnMap.Count - 1].transform.position.x;
        }
        if (curentPositionX == positionX && counter < 3 )
        {
            counter++;
        }
        else if (curentPositionX != positionX && counter > 1)
        {
            counter = 1;
        }
        else if (curentPositionX == positionX && counter == 3 )
        {
            positionX = curentPositionX;
            while (curentPositionX == positionX)
            {
                curentPositionX = (int)Random.Range(-1.9f, 1.9f) * 3;
            }
            curretPositionHindrace.x = curentPositionX;
            counter = 1;
        }
    }
    IEnumerator Series(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            ObstlaceGenerator(obstlaceOnMap[obstlaceOnMap.Count - 1].transform.position);
            if (obstlaceOnMap[0].transform.position.z < playerPosition.transform.position.z - 1)
            {
                Destroy(obstlaceOnMap[0]);
                obstlaceOnMap.Remove(obstlaceOnMap[0]);
            }
        }
    }

    void Start()
    {
        ObstlaceGenerator(startingPositionOfThObstacle);
        for (int i = 0; i < 20; i++)
        {
            ObstlaceGenerator(obstlaceOnMap[obstlaceOnMap.Count - 1].transform.position);
        }
        time = Series(0.07f);
        StartCoroutine(time);
    }
}
