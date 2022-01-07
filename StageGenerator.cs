using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    const int StageChipSize = 30;

    int currentChipIndex;

    GameObject[] stageChips;

    public Transform character; //ターゲットキャラクター
    public GameObject[] stageChipsNormal; //ステージ配列 normal
    public GameObject[] stageChipsHard; //ステージ配列 hard
    public GameObject[] stageChipsExpart; //ステージ配列 expart
    public int startChipIndex; //自動生成開始インデックス
    public int preInstantiate; //生成先読み個数
    public List<GameObject> generatedStageList = new List<GameObject>(); //生成済みステージのリスト。Listは動的に要素の追加や削除ができる機能付きの配列

    // Start is called before the first frame update
    void Start()
    {
        //stageChipsに難易度に対応したstageChipsNormal等をコピーする
        //現在ノーマルのみ。次回追加！！
        stageChipsNormal.CopyTo(stageChipsNormal,0);

        currentChipIndex = startChipIndex -1;
        UpdateStage(preInstantiate);
    }

    // Update is called once per frame
    void Update()
    {
        //ターゲットの位置から現在のインデックス位置を計算
        int charaPositionIndex = (int)(character.position.z / StageChipSize);

        //次のステージに入ったら更新
        if(charaPositionIndex + preInstantiate > currentChipIndex)
        {
            UpdateStage(charaPositionIndex + preInstantiate);
        }
    }

    //指定のインデックス位置までステージを生成する
    void UpdateStage(int toChipIndex)
    {
        if(toChipIndex <= currentChipIndex) return;

        //指定のステージまでを生成
        for(int i = currentChipIndex+1; i<=toChipIndex; i++)
        {
            GameObject stageObject = GenerateStage(i);

            //生成したステージを管理リストに追加
            generatedStageList.Add(stageObject);
        }

        //ステージ保持上限内になるまで古いステージを削除
        while(generatedStageList.Count > preInstantiate + 2) DestroyOldestStage();

        currentChipIndex = toChipIndex;
    }

    //指定のインデックス位置にStageオブジェクトをランダム生成
    GameObject GenerateStage(int chipIndex)
    {
        GameObject[] stageChips = stageChipsNormal;
        int nextStageChip = Random.Range(0,stageChips.Length);

        GameObject stageObject = (GameObject)Instantiate(
            stageChips[nextStageChip],
            new Vector3(0,0,chipIndex * StageChipSize),
            Quaternion.identity
        );
        return stageObject;
    }

    //一番古いステージを削除
    void DestroyOldestStage()
        {
            GameObject oldStage = generatedStageList[0];
            generatedStageList.RemoveAt(0);
            Destroy(oldStage);
        }
}
