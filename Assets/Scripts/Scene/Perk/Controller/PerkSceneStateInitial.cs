using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene.Controller
{
    public class PerkSceneStateInitial : ISceneState
    {
        private readonly PerkSceneStateMachine sM;

        public PerkSceneStateInitial(PerkSceneStateMachine sM)
        {
            this.sM = sM;
        }

        public void OnEnter()
        {
            sM.PerkView.OpenInitial();
        }

        public void HandleInput()
        {
            if (sM.IsToNext)
                sM.ChangeState(new PerkSceneStatePerk(sM, 1));
        }

        public void OnExit()
        {

        }
    }
}
