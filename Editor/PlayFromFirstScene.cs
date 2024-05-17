using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FoodlesUtilities.Editor
{
    // Adapted from https://discussions.unity.com/t/editor-script-to-make-play-button-always-jump-to-a-start-scene/68990/8
    public static class PlayFromFirstScene
    {
        private const string _playFromFirstMenuStr = "Edit/Always Start From Scene 0 &p";

        private static bool playFromFirstScene
        {
            get => EditorPrefs.HasKey(_playFromFirstMenuStr) && EditorPrefs.GetBool(_playFromFirstMenuStr);
            set => EditorPrefs.SetBool(_playFromFirstMenuStr, value);
        }

        [MenuItem(_playFromFirstMenuStr, false, 150)]
        private static void PlayFromFirstSceneCheckMenu() 
        {
            playFromFirstScene = !playFromFirstScene;
            Menu.SetChecked(_playFromFirstMenuStr, playFromFirstScene);

            ShowNotifyOrLog(playFromFirstScene ? "Play from scene 0" : "Play from current scene");
        }

        // The menu won't be gray out, we use this validate method for update check state
        [MenuItem(_playFromFirstMenuStr, true)]
        private static bool PlayFromFirstSceneCheckMenuValidate()
        {
            Menu.SetChecked(_playFromFirstMenuStr, playFromFirstScene);
            return true;
        }

        // This method is called before any Awake. It's the perfect callback for this feature
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void LoadFirstSceneAtGameBegins()
        {
            if(!playFromFirstScene)
                return;

            if(EditorBuildSettings.scenes.Length  == 0)
            {
                Debug.LogWarning("The scene build list is empty. Can't play from first scene.");
                return;
            }

            foreach(var gameObject in Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None))
            {
                gameObject.SetActive(false);
            }
        
            SceneManager.LoadScene(0);
        }

        private static void ShowNotifyOrLog(string msg)
        {
            if(Resources.FindObjectsOfTypeAll<SceneView>().Length > 0)
                EditorWindow.GetWindow<SceneView>().ShowNotification(new GUIContent(msg));
            else
                Debug.Log(msg); // When there's no scene view opened, we just print a log
        }
    }
}