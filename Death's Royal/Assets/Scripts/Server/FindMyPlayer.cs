using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindMyPlayer : MonoBehaviour
{
    MyTPController _myPlayer;
    CinemachineVirtualCamera _followCam;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine("FindmyPlayer");
        _followCam = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        _followCam.Follow = _myPlayer.transform.GetChild(0).transform;
    }

    IEnumerator FindmyPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.05f);

            _myPlayer = GameObject.Find("MyPlayer").GetComponent<MyTPController>();
        }
    }
}
