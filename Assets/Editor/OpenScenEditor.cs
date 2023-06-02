using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
public class OpenScenEditor : EditorWindow
{
    private static string _scenePath = "Assets/Scenes/{0}.unity";

    [MenuItem("OpenScene/MainMenu", false, 1)]
    public static void GameScene()
    {

        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        EditorSceneManager.OpenScene
           (string.Format(_scenePath, "MainMenu"), OpenSceneMode.Single);
    }
    [MenuItem("OpenScene/GameLevel", false, 1)]
    public static void Demo()
    {
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        EditorSceneManager.OpenScene
           (string.Format(_scenePath, "GameLevel"), OpenSceneMode.Single);
    }

    [MenuItem("OpenScene/TestScene", false, 1)]
    public static void Demo2Scene()
    {
        EditorSceneManager.SaveScene(SceneManager.GetActiveScene());

        EditorSceneManager.OpenScene
           (string.Format(_scenePath, "TestScene"), OpenSceneMode.Single);
    }
}