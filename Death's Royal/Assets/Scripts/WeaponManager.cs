using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] Firearms[] firearms;

    private Firearms currentFirearm = null;
    public Firearms CurrentFirearm
    {
        get { return currentFirearm; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            currentFirearm = firearms[0];
            firearms[0].gameObject.SetActive(true);
            firearms[1].gameObject.SetActive(false);
            firearms[2].gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentFirearm = firearms[1];
            firearms[1].gameObject.SetActive(true);
            firearms[0].gameObject.SetActive(false);
            firearms[2].gameObject.SetActive(false);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentFirearm = firearms[2];
            firearms[2].gameObject.SetActive(true);
            firearms[0].gameObject.SetActive(false);
            firearms[1].gameObject.SetActive(false);
        }
    }
}
