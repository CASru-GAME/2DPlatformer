using UnityEngine;

namespace Scene.Controller
{
    public interface ISceneState
    {
        void OnEnter();
        void HandleInput();
        void OnExit();
    }
}
