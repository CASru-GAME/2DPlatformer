using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace Scene.Common
{
    public class CustomEventSystem : MonoBehaviour
    {
        private void Start()
        {
            SceneManager.sceneLoaded += DestroyOthers;
            DestroyOthers(default, default);
        }

        //シーンロード時にイベントシステムが複数存在するのを防ぐため、EventSystemが存在するか確認し、存在する場合はそれを破壊する
        private void DestroyOthers(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
        {
            var eventSystems = FindObjectsByType<EventSystem>(FindObjectsSortMode.None);
            foreach (var es in eventSystems)
            {
                if (es.gameObject != gameObject)
                {
                    Destroy(es.gameObject);
                }
            }
        }
        
    }
}