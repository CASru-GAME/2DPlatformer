using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene.Controller
{
    public class TitleSceneStateDefault : ISceneState
    {
        private readonly TitleSceneStateMachine sM;

        public TitleSceneStateDefault(TitleSceneStateMachine sM)
        {
            this.sM = sM;
        }

        public void OnEnter()
        {
            sM.TitleView.OpenDefault();
        }

        public void HandleInput()
        {
            if (sM.IsToPerk)
                sM.ChangeState(new TitleSceneStateToPerk(sM));
            else if (sM.IsToSetting)
                sM.ChangeState(new TitleSceneStateSetting(sM));
            else if (sM.IsToExit)
                sM.ChangeState(new TitleSceneStateExit(sM));
        }

        public void OnExit()
        {

        }
    }
}
