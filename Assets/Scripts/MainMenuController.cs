using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public static bool FirstLaunch;
  
    private void Start()
    {
        Debug.Log(FirstLaunch);
        if (!FirstLaunch)
        {
            FirstLaunch = true;
            return;
        }
        PlayBtn();
    }


    public GameManager GM;
    public void PlayBtn()
    {
        gameObject.SetActive(false);
        GM.StartGame();
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
    }

    public void QuitBtn()
    {
        Application.Quit();
    }
}
