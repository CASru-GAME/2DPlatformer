using System.Collections.Generic;
using Perk.Model;
using Scene.View;
using UnityEngine;

namespace Scene.Controller
{
    public class SelectPerkSystem : MonoBehaviour
    {
        private readonly int[] currentPerkIDList = new int[3];
        [SerializeField] private PerkView perkView;

        public void SetRandomPerkID(int currentPerkCount)
        {
            for (int i = 0; i < currentPerkIDList.Length; i++)
            {
                int randomPerkID = PerkEffectStorage.GetPerkIDAtRandom();
                perkView.SetPerkText(i, randomPerkID);
                currentPerkIDList[i] = randomPerkID;
            }
            perkView.OpenPerk(currentPerkCount);
        }

        public void SelectPerk(int boxNumber)
        {
            int selectedPerkID = currentPerkIDList[boxNumber];
            PerkEffectStorage.AddPerk(selectedPerkID);
        }
    }
}