using UnityEngine;
using Scene.Model;

public class GimmickGenerator : MonoBehaviour
{
    public GameObject[] gimmicks;
    public Transform[] spawnPoints;
    public Grid grid;

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

            GameObject gimmickPrefab = gimmicks[gimmick - 1];
            Transform parent = spawnPoints[stage - 1];

            foreach (Transform child in parent)
            {
                Instantiate(gimmickPrefab, child.position, Quaternion.identity);
            }
        }
    }
}
