using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Gameplay.Spawn.Editor
{
    [CustomEditor(typeof(Spawner))]
    public class SpawnerEditor : CustomEditorBase<Spawner>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Spawn Settings", HeaderStyle);
                
                var activateOnAwake = serializedObject.FindProperty("_activateOnAwake");
                EditorGUILayout.PropertyField(activateOnAwake);
                
                var spawnOnDestroySettings = serializedObject.FindProperty("_spawnSettings");
                var objectPrefab = spawnOnDestroySettings.FindPropertyRelative("ObjectPrefab");
                var objectsCount = spawnOnDestroySettings.FindPropertyRelative("Count");
                var poolCapacity = spawnOnDestroySettings.FindPropertyRelative("PoolCapacity");

                EditorGUILayout.PropertyField(objectPrefab);
                EditorGUILayout.PropertyField(objectsCount);
                EditorGUILayout.PropertyField(poolCapacity);
            }
            EditorGUILayout.EndVertical();
        }
    }
}