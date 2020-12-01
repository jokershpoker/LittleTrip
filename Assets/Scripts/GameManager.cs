using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public RestartLevel_1 Res;

    public GameObject ResultObj;
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
        PM.StartGame();
        StartCoroutine(Play());
        PM.SkinAnimator.SetTrigger("respawn");
        RestartLevel1();

        Points = 0;
    }


    IEnumerator Play()
    {
        yield return new WaitForSeconds(0.1f);
        CanPlay = true;
    }

    private void Update()
    {
        if (CanPlay)
            Points += Time.deltaTime * 3;

        PointsTxt.text = ((int)Points).ToString();
    }

    public void ShowResult()
    {
        ResultObj.SetActive(true);
        SaveManager.Instance.SaveGame();
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
