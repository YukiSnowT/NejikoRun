using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    public Text highScoreText;
    public static int modeSelect = 0;
    public Dropdown dropdown;

    public void Start()
    {
        //ハイスコア表示
        highScoreText.text = "High Score\nNormal : " + PlayerPrefs.GetInt("HighScoreN") + "m\nHard     : " + PlayerPrefs.GetInt("HighScoreH") + "m\nExpart  : " + PlayerPrefs.GetInt("HighScoreE") + "m"; 
    }

    public void Update()
    {
        modeSelect = dropdown.value;
    }

    public void OnStartButtonClicked()
    {
        //Mainシーンに現在の難易度設定の値を渡す・次回やる！
        Debug.Log(modeSelect);
        //シーン遷移
        SceneManager.LoadScene("Main");
    }
}
