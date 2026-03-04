using UnityEngine;

namespace Scene.Controller
{
    public class TitleSceneStateExit : ISceneState
    {
        private readonly TitleSceneStateMachine sM;
        
        public TitleSceneStateExit(TitleSceneStateMachine sM)
        {
            this.sM = sM;
        }

        public void OnEnter()
        {
            Application.Quit();
        }

        public void HandleInput()
        {
            
        }

        public void OnExit()
        {

        }
    }
}
