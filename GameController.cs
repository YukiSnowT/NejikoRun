using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //UI使用に必要
using UnityEngine.SceneManagement; //シーン移動に必要

public class GameController : MonoBehaviour
{
    public NejikoController nejiko;
    public Text scoreText; //ScoreTextの参照
    public LifePanel lifePanel;

    void Update()
    {
        //スコア更新
        int score = CalcScore();
        scoreText.text = "Score : " + score + "m"; //テキスト更新

        //ライフパネル更新
        lifePanel.UpdateLife(nejiko.Life());

        //ライフ０でゲームオーバー
        if(nejiko.Life() <= 0)
        {
            //これ以降のUpdateを停止
            enabled = false;

            //ハイスコア更新
            if(PlayerPrefs.GetInt("HighScoreN") < score & TitleController.modeSelect == 0)
            {
                PlayerPrefs.SetInt("HighScoreN",score);
            }
            if(PlayerPrefs.GetInt("HighScoreH") < score & TitleController.modeSelect == 1)
            {
                PlayerPrefs.SetInt("HighScoreH",score);
            }
            if(PlayerPrefs.GetInt("HighScoreE") < score & TitleController.modeSelect == 2)
            {
                PlayerPrefs.SetInt("HighScoreE",score);
            }
        
            //2秒後にReturntoTitleを呼び出す
            Invoke("ReturnToTitle",2.0f);
        }
    }
    int CalcScore()
    {
        //走行教理をスコアとする
        return (int)nejiko.transform.position.z;
    }

    void ReturnToTitle()
    {
        //タイトルシーンに移動
        SceneManager.LoadScene("Title");
    }
}
