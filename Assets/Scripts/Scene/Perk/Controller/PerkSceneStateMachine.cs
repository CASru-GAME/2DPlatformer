using NUnit.Framework;
using Scene.Data;
using Scene.View;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scene.Controller
{
    public class PerkSceneStateMachine : MonoBehaviour
    {
        public bool IsToNext { get; private set; }    
        private ISceneState currentState;
        [SerializeField] private SceneNameData sceneNameData;
        public SceneNameData SceneNameData => sceneNameData;
        [SerializeField] private PerkView perkView;
        public PerkView PerkView => perkView;
        [SerializeField] private int maxPerkCount;
        public int MaxPerkCount => maxPerkCount;
        [SerializeField] private SelectPerkSystem selectPerkSystem;

        private void Start()
        {
            currentState = new PerkSceneStateInitial(this);
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

        public void SetSelectPerk(int currentPerkCount)
        {
            selectPerkSystem.SetRandomPerkID(currentPerkCount);
        }
    }
}
