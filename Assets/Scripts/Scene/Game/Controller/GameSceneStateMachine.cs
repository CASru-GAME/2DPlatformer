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
        }

        public void ToNext()
        {
            IsToNext = true;
        }
    }
}
