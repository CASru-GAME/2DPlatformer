using Scene.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene.Controller
{
    public class PerkSceneStateToGame : ISceneState
    {
        private readonly PerkSceneStateMachine sM;
        
        public PerkSceneStateToGame(PerkSceneStateMachine sM)
        {
            this.sM = sM;
        }

        public void OnEnter()
        {
            TransitionView.Instance.PlayAnim("Close_1");
            sM.LoadGameSceneInvoke();
        }

        public void HandleInput()
        {

        }

        public void OnExit()
        {

        }
    }
}
