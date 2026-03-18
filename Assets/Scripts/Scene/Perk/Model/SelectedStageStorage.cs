using Perk.Model;
using UnityEngine;

namespace Scene.Model
{
    public class SelectedStageStorage
    {
        private readonly static int[] selectedStageIDs = new int[8];
        //ステージID(1~18)参照用
        public int[] SelectedStageIDs => selectedStageIDs;
        private readonly static int[] selectedGimmickIDs = new int[8];
        //ギミックID(1~5)参照用
        public int[] SelectedGimmickIDs => selectedGimmickIDs;
        private readonly static int[] selectedDirectionIDs = new int[8];
        //向きID(1~4)参照用
        public int[] SelectedDirectionIDs => selectedDirectionIDs;

        private readonly int[] currentPerkIDs = new int[3];
        public int[] CurrentPerkIDs => currentPerkIDs;
        private readonly int[] currentStageIDs = new int[3];
        public int[] CurrentStageIDs => currentStageIDs;
        private readonly int[] currentGimmickIDs = new  int[3];
        public int[] CurrentGimmickIDs => currentGimmickIDs;
        private readonly int[] currentDirectionIDs = new int[3];
        public int[] CurrentDirectionIDs => currentDirectionIDs;
        private readonly int MaxStageID = 18;
        private readonly int MaxGimmickID = 5;

        public void SetRandomIDs()
        {
            SetRandomPerkID();
            SetRandomStageID();
            SetRandomGimmickID();
            SetRandomDirectionID();
        }
        
        private void SetRandomPerkID()
        {
            for (int i = 0; i < currentPerkIDs.Length; i++)
            {
                int randomPerkID = PerkEffectStorage.GetPerkIDAtRandom();
                currentPerkIDs[i] = randomPerkID;
            }            
        }

        private void SetRandomStageID()
        {
            for (int i = 0; i < currentStageIDs.Length; i++)
                currentStageIDs[i] = Random.Range(1, MaxStageID + 1);
        }

        private void SetRandomGimmickID()
        {
            for (int i = 0; i < currentGimmickIDs.Length; i++)
                currentGimmickIDs[i] = Random.Range(1, MaxGimmickID + 1);
        }

        private void SetRandomDirectionID()
        {
            for (int i = 0; i < currentDirectionIDs.Length; i++)
                currentDirectionIDs[i] = Random.Range(1, 5);
        }

        public void SelectStage(int boxNumber, int currentPickCount)
        {
            PerkEffectStorage.AddPerk(currentPerkIDs[boxNumber]);
            selectedStageIDs[currentPickCount - 1] = currentStageIDs[boxNumber];
            selectedGimmickIDs[currentPickCount - 1] = currentGimmickIDs[boxNumber];
            selectedDirectionIDs[currentPickCount - 1] = currentDirectionIDs[boxNumber];
        }

        public static void PrintSelectedStage()
        {
            string debugText = "選択されたステージ構成\n";
            for (int i = 0; i < selectedStageIDs.Length; i++)
            {
                debugText += "ステージ" + selectedStageIDs[i] + " ギミック" + selectedGimmickIDs[i] + " 向き" + selectedDirectionIDs[i] + "\n";
            }
            Debug.Log(debugText);
        }
    }
}
