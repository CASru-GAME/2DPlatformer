using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene.Controller
{
    public class TitleSceneStateToPerk : ISceneState
    {
        private readonly TitleSceneStateMachine sM;
        
        public TitleSceneStateToPerk(TitleSceneStateMachine sM)
        {
            this.sM = sM;
        }

        public void OnEnter()
        {
            SceneManager.LoadScene(sM.SceneNameData.PerkSceneName);
        }

        public void HandleInput()
        {

        }

        public void OnExit()
        {

        }
    }
}
