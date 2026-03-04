using UnityEngine;

namespace Scene.Controller
{
    public class TitleSceneStateSetting : ISceneState
    {
        private readonly TitleSceneStateMachine sM;
        
        public TitleSceneStateSetting(TitleSceneStateMachine sM)
        {
            this.sM = sM;
        }

        public void OnEnter()
        {
            sM.TitleView.OpenSetting();
        }

        public void HandleInput()
        {
            if (sM.IsToDefault)
                sM.ChangeState(new TitleSceneStateDefault(sM));
        }

        public void OnExit()
        {

        }
    }
}
