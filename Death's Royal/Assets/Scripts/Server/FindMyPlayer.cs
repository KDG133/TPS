using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMyPlayer : MonoBehaviour
{
    [SerializeField] MyTPController _myPlayer;
    CinemachineVirtualCamera _followCam;
    // Start is called before the first frame update
    void Start()
    {
        _followCam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        FindmyPlayer();
        _followCam.Follow = _myPlayer.transform.GetChild(0).transform;
    }

    void FindmyPlayer()
    {
        while (_myPlayer == null)
        {
            _myPlayer = GameObject.Find("MyPlayer").GetComponent<MyTPController>();
        }
    }
}
