using Scene.View;
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
            TransitionView.Instance.PlayAnim("Close_1");
            sM.LoadPerkSceneInvoke();
        }

        public void HandleInput()
        {

        }

        public void OnExit()
        {

        }
    }
}
