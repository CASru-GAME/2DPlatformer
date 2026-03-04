using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene.Controller
{
    public class GameSceneStateToGame : ISceneState
    {
        private readonly GameSceneStateMachine sM;
        
        public GameSceneStateToGame(GameSceneStateMachine sM)
        {
            this.sM = sM;
        }

        public void OnEnter()
        {
            SceneManager.LoadScene(sM.SceneNameData.TitleSceneName);
        }

        public void HandleInput()
        {

        }

        public void OnExit()
        {

        }
    }
}
