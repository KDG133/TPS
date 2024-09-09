using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.WebRequestMethods;

public class DisableParticle : MonoBehaviour
{
    private string eftName;
    void Start()
    {
        string Name = gameObject.name;
        eftName = Name.Replace("(Clone)", "");
    }

    void Update()
    {
        if (gameObject.activeSelf)
            StartCoroutine(disableEft());
    }

    IEnumerator disableEft()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);
        EffectManager.Instance.insertValue(eftName, gameObject);
    }
}
