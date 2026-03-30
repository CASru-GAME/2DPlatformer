using NUnit.Framework;
using Scene.Data;
using Scene.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene.Controller
{
    public class GameSceneStateMachine : MonoBehaviour
    {
        public bool IsToNext { get; private set; }    
        public bool IsToClear { get; private set; }
        public bool IsToOver { get; private set; }
        private ISceneState currentState;
        [SerializeField] private SceneNameData sceneNameData;
        public SceneNameData SceneNameData => sceneNameData;
        [SerializeField] private GameView gameView;
        public GameView GameView => gameView;

        private void Start()
        {
            currentState = new GameSceneStateInitial(this);
            currentState.OnEnter();
        }

        private void Update()
        {
            currentState.HandleInput();
        }

        public void ChangeState(ISceneState newState)
        {
            currentState.OnExit();
            ResetFlags();
            currentState = newState;
            currentState.OnEnter();
        }

        private void ResetFlags()
        {
            IsToNext = false;
            IsToClear = false;
            IsToOver = false;
        }

        public void ToNext()
        {
            IsToNext = true;
        }

        //以下が必要：
        //using Scene.Controller;
        //[SerializeField] private GameSceneStateMachine gameSceneStateMachine;

        //ゲームクリア時：gameSceneStateMachine.ToClear();
        //ゲームオーバー時：gameSceneStateMachine.ToOver();
        //を呼び出す

        public void ToClear()
        {
            IsToClear = true;
        }

        public void ToOver()
        {
            IsToOver = true;
        }

        public void LoadTitleSceneInvoke()
        {
            Invoke(nameof(LoadTitleScene), TransitionView.Instance.TransitionHalfDuration);
        }

        private void LoadTitleScene()
        {
            SceneManager.UnloadSceneAsync(sceneNameData.CurrentPerkSceneName);
            SceneManager.UnloadSceneAsync(sceneNameData.GameSceneName);
            SceneManager.LoadScene(sceneNameData.TitleSceneName, LoadSceneMode.Additive);
        }
    }
}
