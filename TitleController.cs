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
        //Mainシーンに現在の難易度設定の値を渡す・次回やる！

        //シーン遷移
        SceneManager.LoadScene("Main");
    }
}
