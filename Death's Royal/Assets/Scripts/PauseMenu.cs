using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuBG;
    [SerializeField] private GameObject pauseMenus;
    [SerializeField] private GameObject ShopMenus;

    void Start()
    {
        CursorLock();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            if(!ThirdPersonController._isPause) 
            {
                Time.timeScale = 0f;
                pauseMenuBG.SetActive(true);
                pauseMenus.SetActive(true);
                ThirdPersonController._isPause = true;
                CursorActive();
            }
            else if (ThirdPersonController._isPause)
            {
                Time.timeScale = 1.0f;
                pauseMenuBG.SetActive(false);
                pauseMenus.SetActive(false);
                ThirdPersonController._isPause = false;
                CursorLock();
            }
        }
    }

    void CursorActive()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void ActiveShopMenu()
    {
        pauseMenus.SetActive(false);
        ShopMenus.SetActive(true);
    }

    public void DeActiveShop()
    {
        pauseMenus.SetActive(true);
        ShopMenus.SetActive(false);
    }
}
