using UnityEngine;

namespace Scene.Controller
{
    public class PerkSceneStatePerk : ISceneState
    {
        private readonly PerkSceneStateMachine sM;
        private readonly int currentPerkCount;
        
        public PerkSceneStatePerk(PerkSceneStateMachine sM, int currentPerkCount)
        {
            this.sM = sM;
            this.currentPerkCount = currentPerkCount;
        }

        public void OnEnter()
        {
            sM.PerkView.OpenPerk(currentPerkCount);
        }

        public void HandleInput()
        {
            if (sM.IsToNext)
                if (currentPerkCount < sM.MaxPerkCount)
                    sM.ChangeState(new PerkSceneStatePerk(sM, currentPerkCount + 1));
                else
                sM.ChangeState(new PerkSceneStateIdle(sM));
        }

        public void OnExit()
        {

        }
    }
}
