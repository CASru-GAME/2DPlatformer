using System.Collections.Generic;
using Perk.Model;
using Scene.Model;
using Scene.View;
using UnityEngine;

namespace Scene.Controller
{
    public class SelectStageSystem : MonoBehaviour
    {
        [SerializeField] private PerkView perkView;
        private readonly SelectedStageStorage selectedStageStorage = new();
        private int currentPickCount = 0;

        public void SetRandomIDs(int currentPickCount)
        {
            this.currentPickCount = currentPickCount;
            selectedStageStorage.SetRandomIDs(currentPickCount);
            for (int i = 0; i < selectedStageStorage.CurrentPerkIDs.Length; i++)
                perkView.SetPerkText(i, selectedStageStorage.CurrentPerkIDs[i], selectedStageStorage.CurrentStageIDs[i], selectedStageStorage.CurrentGimmickIDs[i], selectedStageStorage.CurrentDirectionIDs[i]);
            perkView.OpenPerk(currentPickCount);
        }

        public void SelectStage(int boxNumber)
        {
            selectedStageStorage.SelectStage(boxNumber, currentPickCount);
        }
    }
}