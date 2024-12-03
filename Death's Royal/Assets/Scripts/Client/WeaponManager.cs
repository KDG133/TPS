using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }
    [SerializeField] private MyFirearms[] firearms;
    [SerializeField] private MyFirearms currentFirearm = null;
    public bool isAiming {  get; set; }
    public MyFirearms CurrentFirearm
    {
        get { return currentFirearm; }
    }

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    private void Start()
    {
        currentFirearm = firearms[0];
    }

    // Update is called once per frame
    private void Update()
    {
        if(!currentFirearm.Reloading && !isAiming)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
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
}
