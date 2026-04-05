using Perk.Model;
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
            PerkEffectStorage.ResetAllPerks();
            if(!SceneManager.GetSceneByName(sM.SceneNameData.TransitionSceneName).isLoaded)
                SceneManager.LoadScene(sM.SceneNameData.TransitionSceneName, LoadSceneMode.Additive);
            sM.PlayOpenInvoke();
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
