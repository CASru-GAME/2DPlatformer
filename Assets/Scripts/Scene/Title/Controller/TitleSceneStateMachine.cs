using System;
using NUnit.Framework;
using Scene.Data;
using Scene.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene.Controller
{
    public class TitleSceneStateMachine : MonoBehaviour
    {
        public bool IsToPerk { get; private set; }
        public bool IsToSetting { get; private set; }
        public bool IsToDefault { get; private set; }
        public bool IsToExit { get; private set; }        
        private ISceneState currentState;
        [SerializeField] private SceneNameData sceneNameData;
        public SceneNameData SceneNameData => sceneNameData;
        [SerializeField] private TitleView titleView;
        public TitleView TitleView => titleView;

        private void Start()
        {
            currentState = new TitleSceneStateInitial(this);
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
            IsToPerk = false;
            IsToSetting = false;
            IsToDefault = false;
            IsToExit = false;
        }

        public void ToPerk()
        {
            IsToPerk = true;
        }

        public void ToSetting()
        {
            IsToSetting = true;
        }

        public void ToDefault()
        {
            IsToDefault = true;
        }

        public void ToExit()
        {
            IsToExit = true;
        }

        public void LoadPerkSceneInvoke()
        {
            Invoke(nameof(LoadPerkScene), 0.5f);
        }

        private void LoadPerkScene()
        {
            SceneManager.UnloadSceneAsync(sceneNameData.TitleSceneName);
            SceneManager.LoadScene(sceneNameData.PerkSceneName, LoadSceneMode.Additive);
        }
    }
}
