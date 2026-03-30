using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene.Controller
{
    public class GameSceneStateOver : ISceneState
    {
        private readonly GameSceneStateMachine sM;
        
        public GameSceneStateOver(GameSceneStateMachine sM)
        {
            this.sM = sM;
        }

        public void OnEnter()
        {
            sM.GameView.OpenOver();
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
