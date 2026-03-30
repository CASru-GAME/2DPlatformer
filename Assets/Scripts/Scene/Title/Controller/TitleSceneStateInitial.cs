using Scene.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene.Controller
{
    public class TitleSceneStateInitial : ISceneState
    {
        private readonly TitleSceneStateMachine sM;

        public TitleSceneStateInitial(TitleSceneStateMachine sM)
        {
            this.sM = sM;
        }

        public void OnEnter()
        {
            SceneManager.LoadScene(sM.SceneNameData.TransitionSceneName, LoadSceneMode.Additive);
            if (TransitionView.Instance != null)
                TransitionView.Instance.PlayAnim("Open_1");
            sM.ChangeState(new TitleSceneStateDefault(sM));
        }

        public void HandleInput()
        {
            
        }

        public void OnExit()
        {

        }
    }
}
