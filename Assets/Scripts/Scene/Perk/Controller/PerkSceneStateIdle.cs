using Scene.Model;
using UnityEngine;

namespace Scene.Controller
{
    public class PerkSceneStateIdle : ISceneState
    {
        private readonly PerkSceneStateMachine sM;
        
        public PerkSceneStateIdle(PerkSceneStateMachine sM)
        {
            this.sM = sM;
        }

        public void OnEnter()
        {
            sM.PerkView.OpenIdle();
            //デバッグ用：ステージ構築の結果を表示
            SelectedStageStorage.PrintSelectedStage();
        }

        public void HandleInput()
        {
            if (sM.IsToNext)
                sM.ChangeState(new PerkSceneStateToGame(sM));
        }

        public void OnExit()
        {
            SoundSourceObject.Instance.PlayButtonSE();
        }
    }
}
