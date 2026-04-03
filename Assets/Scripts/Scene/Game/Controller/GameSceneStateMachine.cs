using System;
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
        public static Action OnClear;
        public static Action OnOver;

        private void Start()
        {
            currentState = new GameSceneStateInitial(this);
            currentState.OnEnter();
            OnClear += ToClear;
            OnOver += ToOver;
            SoundSourceObject.Instance.PlayGameBGM();
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
            SoundSourceObject.Instance.PlayButtonSE();
        }

        public void ToClear()
        {
            IsToClear = true;
            OnClear -= ToClear;
        }

        public void ToOver()
        {
            IsToOver = true;
            OnOver -= ToOver;
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
