using UnityEngine;
using Scene.Model;

public class StageGenerator3 : MonoBehaviour
{
    public StagePrefabs prefabManager;
    public GameObject[] gimmicks;
    public Transform[] spawnPoints;
    public GameObject SideUpStage;
    public GameObject UpSideStage;
    public GameObject SideDownStage;
    public GameObject DownSideStage;
    public GameObject StartStage;
    public GameObject GoalStage;
    public GameObject FallDetect;

    void Start()
    {
        StageGenerate();
    } 

    void StageGenerate()
    {
        Vector3 position = Vector3.zero;
        int direction = 0;
        int previousDirection = 0;

        Instantiate (StartStage, new Vector3(-10, 0, 0), Quaternion.identity);

        for (int i = 0; i < 8; i++)
        {
            int stage = SelectedStageStorage.SelectedStageIDs[i]; //ステージID取得
            int gimmick = SelectedStageStorage.SelectedGimmickIDs[i]; //ギミックID取得
            direction = SelectedStageStorage.SelectedDirectionIDs[i]; //方向ID取得

            GameObject prefab = prefabManager.stagePrefabs[stage - 1];

            GameObject gimmickPrefab = gimmicks[gimmick - 1];
            Transform gimPos = spawnPoints[stage - 1];

            if (previousDirection == 1)
            {
                if (direction == 1) //横から横
                {
                    position = new Vector3(position.x + 10, position.y, position.z);
                }

                if (direction == 2) //横から上
                {
                    Instantiate (SideUpStage, new Vector3(position.x + 10, position.y, position.z), Quaternion.identity);
                    Instantiate(FallDetect, new Vector3(position.x + 10, position.y - 7, position.z), Quaternion.identity);
                    position = new Vector3(position.x + 10, position.y + 10, position.z);
                }

                if (direction == 3) //横から下
                {
                    Instantiate (SideDownStage, new Vector3(position.x + 10, position.y, position.z), Quaternion.identity);
                    position = new Vector3(position.x + 10, position.y - 10, position.z);
                }
            }

            if (previousDirection == 2)
            {
                if (direction == 1) //上から横
                {
                    Instantiate (UpSideStage, new Vector3(position.x, position.y + 10, position.z), Quaternion.identity);
                    position = new Vector3(position.x + 10, position.y + 10, position.z);
                }

                if (direction == 2) //上から上
                {
                    position = new Vector3(position.x, position.y + 10, position.z);
                }

            }

            if (previousDirection == 3)
            {
                if (direction == 1) //下から横
                    {
                        Instantiate(DownSideStage, new Vector3(position.x, position.y - 10, position.z), Quaternion.identity);
                        position = new Vector3(position.x + 10, position.y - 10, position.z);
                    }

                if (direction == 3) //下から下
                    {
                        position = new Vector3(position.x, position.y - 10, position.z);
                    }
            }
            
            if(direction == 1) //落下死検知オブジェクト設置
            {
                Instantiate(FallDetect, new Vector3(position.x, position.y - 7, position.z), Quaternion.identity);
            }

            Instantiate(prefab, position, Quaternion.identity);
            Instantiate(gimPos, position, Quaternion.identity);

            foreach (Transform child in gimPos)
            {
                Instantiate(gimmickPrefab, child.position + position, child.rotation);
            }

            previousDirection = direction;
        }

        if (direction == 1)
        {
            Instantiate(GoalStage, new Vector3(position.x + 10, position.y, position.z), Quaternion.identity);
        }

        if (direction == 2)
        {
            Instantiate(GoalStage, new Vector3(position.x, position.y + 10, position.z), Quaternion.identity);
        }

        if (direction == 3)
        {
            Instantiate(GoalStage, new Vector3(position.x, position.y - 10, position.z), Quaternion.identity);
        }
    }
}
