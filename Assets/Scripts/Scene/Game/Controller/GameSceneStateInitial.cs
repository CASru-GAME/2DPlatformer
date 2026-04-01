using Scene.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene.Controller
{
    public class GameSceneStateInitial : ISceneState
    {
        private readonly GameSceneStateMachine sM;

        public GameSceneStateInitial(GameSceneStateMachine sM)
        {
            this.sM = sM;
        }

        public void OnEnter()
        {
            TransitionView.Instance.PlayAnim("Open_1");
        }

        public void HandleInput()
        {
            if (sM.IsToClear)
                sM.ChangeState(new GameSceneStateClear(sM));
            else if (sM.IsToOver)
                sM.ChangeState(new GameSceneStateOver(sM));
        }

        public void OnExit()
        {

        }
    }
}
