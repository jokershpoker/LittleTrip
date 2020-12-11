using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public PauseMenuController PMC;
    public GameObject ResultObj;
    public GameObject FinishObj;
    public PlayerMovement PM;

    public Text PointsTxt,
                CoinsTxt;
    public float Points;

    public int Coins = -1;

    public bool CanPlay = true;

    public float MoveSpeed;

    public List<Skin> Skins;

    public void StartGame()
    {
        ResultObj.SetActive(false);
        FinishObj.SetActive(false);
        CanPlay = true;

        PM.SkinAnimator.SetTrigger("respawn");
        StartCoroutine(FixTrigger());

        

        Points = 0;
    }

    IEnumerator FixTrigger()
    {
        yield return null;
        PM.SkinAnimator.ResetTrigger("respawn");
    }



    private void Update()
    {
        if (CanPlay)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                PMC.Pause();

            Points += Time.deltaTime * 3;
        }

        PointsTxt.text = ((int)Points).ToString();
    }

    public void ShowResult()
    {
        ResultObj.SetActive(true);
        SaveManager.Instance.SaveGame();
    }

    public void ShowFinish()
    {
        FinishObj.SetActive(true);
        SaveManager.Instance.SaveGame();
    }    

    public void AddCoins (int number)
    {
        Coins += number;
        RefreshText();
    }    

    public void RefreshText()
    {
        CoinsTxt.text = Coins.ToString();
    }

    public void ActivateSkin(int skinIndex, bool setTrigger = false)
    {
        foreach (var skin in Skins)
            skin.HideSkin();

        Skins[skinIndex].ShowSkin();
        PM.SkinAnimator = Skins[skinIndex].AC;

        if (setTrigger)
            PM.SkinAnimator.SetTrigger("death");

    }

    public void RestartLevel1()
    {
        SceneManager.LoadScene("Level_1");
    }

}
