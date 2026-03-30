using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene.Controller
{
    public class GameSceneStateClear : ISceneState
    {
        private readonly GameSceneStateMachine sM;
        
        public GameSceneStateClear(GameSceneStateMachine sM)
        {
            this.sM = sM;
        }

        public void OnEnter()
        {
            sM.GameView.OpenClear();
        }

        public void HandleInput()
        {
            if (sM.IsToNext)
                sM.ChangeState(new GameSceneStateToTitle(sM));
        }

        public void OnExit()
        {

        }
    }
}
