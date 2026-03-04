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

        }

        public void HandleInput()
        {
            if (sM.IsToNext)
                sM.ChangeState(new GameSceneStateToGame(sM));
        }

        public void OnExit()
        {

        }
    }
}
