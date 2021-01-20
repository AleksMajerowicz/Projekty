using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoMangament : MonoBehaviour
{
    Animator animations;
    Vector3 startPosition;
    IEnumerator timeDuration;
    private void Start()
    {
        animations = GetComponent<Animator>();
        startPosition = transform.position;
    }

    void Update()
    {
        if (gameObject.activeSelf == true)
        {
            transform.position = startPosition;
            timeDuration = activationTornadoAnimations(3.5f);
            StartCoroutine(timeDuration);
        }
    }

    IEnumerator activationTornadoAnimations(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        animations.SetBool("timeDuration",true);
    }
}
