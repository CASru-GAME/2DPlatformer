using Scene.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene.Controller
{
    public class GameSceneStateToTitle : ISceneState
    {
        private readonly GameSceneStateMachine sM;
        
        public GameSceneStateToTitle(GameSceneStateMachine sM)
        {
            this.sM = sM;
        }

        public void OnEnter()
        {
            Time.timeScale = 1f;
            TransitionView.Instance.PlayAnim("Close_1");
            sM.LoadTitleSceneInvoke();
        }

        public void HandleInput()
        {

        }

        public void OnExit()
        {

        }
    }
}
