using UnityEngine;
using Scene.Model;

public class GimmickGenerator2 : MonoBehaviour
{
    public MonoBehaviour[] gimmickScripts;
    public GameObject[] gimmickPos;

    void Start()
    {
        GimmickGenerate();
    } 

    void GimmickGenerate()
    {
        int gimmick = 0;

        for (int i = 0; i < 8; i++)
        {
            int stage = SelectedStageStorage.SelectedStageIDs[i]; //ステージID取得
            gimmick = SelectedStageStorage.SelectedGimmickIDs[i]; //ギミックID取得

            GameObject target = gimmickPos[stage - 1];
            target.AddComponent(gimmickScripts[gimmick - 1].GetType());
        }
    }
}
