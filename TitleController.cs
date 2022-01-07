using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public Text highScoreText;
    public static int modeSelect;

    public void Start()
    {
        //ハイスコア表示
        highScoreText.text = "High Score : " + PlayerPrefs.GetInt("HighScore") + "m"; 
    }

    public void OnStartButtonClicked()
    {
        SceneManager.LoadScene("Main");
    }
}
