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
            SceneManager.LoadScene(sM.SceneNameData.GameSceneName);
            SceneManager.LoadScene("CurrentPerkList", LoadSceneMode.Additive);
        }

        public void HandleInput()
        {

        }

        public void OnExit()
        {

        }
    }
}
