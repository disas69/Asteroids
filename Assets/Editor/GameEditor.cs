using Game.Data;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    public static class GameEditor
    {
        [MenuItem("Game/Start Game")]
        public static void StartGame()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
            EditorApplication.isPlaying = true;
        }

        [MenuItem("Game/Delete Game data")]
        public static void ClearGameData()
        {
            FileUtil.DeleteFileOrDirectory(Application.persistentDataPath + "/" + GameData.FileName);
        }
    }
}