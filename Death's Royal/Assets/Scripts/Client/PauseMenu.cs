using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuBG;
    [SerializeField] private GameObject pauseMenus;
    [SerializeField] private GameObject ShopMenus;
    private bool isShop = false;

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
                ActivePause();
            else if (ThirdPersonController._isPause && !isShop)
                DeActivePause();
            else if (ThirdPersonController._isPause && isShop)
                DeActiveShop();
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
        isShop = true;
        pauseMenus.SetActive(false);
        ShopMenus.SetActive(true);
    }

    public void DeActiveShop()
    {
        isShop = false;
        pauseMenus.SetActive(true);
        ShopMenus.SetActive(false);
    }

    public void ActivePause()
    {
        Time.timeScale = 0f;
        ThirdPersonController._isPause = true;
        pauseMenuBG.SetActive(true);
        pauseMenus.SetActive(true);
        CursorActive();
    }

    public void DeActivePause()
    {
        Time.timeScale = 1.0f;
        ThirdPersonController._isPause = false;
        pauseMenuBG.SetActive(false);
        pauseMenus.SetActive(false);
        CursorLock();
    }
}
