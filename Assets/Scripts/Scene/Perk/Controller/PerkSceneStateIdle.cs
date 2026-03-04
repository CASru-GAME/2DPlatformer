using UnityEngine;

namespace Scene.Controller
{
    public class PerkSceneStateIdle : ISceneState
    {
        private readonly PerkSceneStateMachine sM;
        
        public PerkSceneStateIdle(PerkSceneStateMachine sM)
        {
            this.sM = sM;
        }

        public void OnEnter()
        {
            sM.PerkView.OpenIdle();
        }

        public void HandleInput()
        {
            if (sM.IsToNext)
                sM.ChangeState(new PerkSceneStateToGame(sM));
        }

        public void OnExit()
        {

        }
    }
}
