using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Configuration.Editor
{
    [CustomEditor(typeof(AsteroidsConfiguration))]
    public class AsteroidsConfigurationEditor : CustomEditorBase<AsteroidsConfiguration>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Asteroids Configuration", HeaderStyle);
                if (GUILayout.Button("Add settings"))
                {
                    RecordObject("Asteroids Configuration Change");
                    Target.AsteroidsSettings.Add(new AsteroidSettings());
                }

                var settings = serializedObject.FindProperty("AsteroidsSettings");
                var count = settings.arraySize;
                for (int i = 0; i < count; i++)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        var element = settings.GetArrayElementAtIndex(i);
                        var type = element.FindPropertyRelative("Type");
                        var initialVelocity = element.FindPropertyRelative("InitialVelocity");
                        var maxVelocity = element.FindPropertyRelative("MaxVelocity");
                        var maxTorque = element.FindPropertyRelative("MaxTorque");
                        var spawnOnDestroy = element.FindPropertyRelative("SpawnOnDestroy");

                        EditorGUILayout.BeginVertical();
                        {
                            EditorGUILayout.PropertyField(type);
                            EditorGUILayout.PropertyField(initialVelocity);
                            EditorGUILayout.PropertyField(maxVelocity);
                            EditorGUILayout.PropertyField(maxTorque);
                            EditorGUILayout.PropertyField(spawnOnDestroy);

                            if (spawnOnDestroy.boolValue)
                            {
                                EditorGUILayout.LabelField("Spawn Settings", HeaderStyle);
                                EditorGUILayout.BeginVertical(GUI.skin.box);
                                {
                                    var spawnOnDestroySettings = element.FindPropertyRelative("SpawnOnDestroySettings");
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
                        EditorGUILayout.EndVertical();

                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            RecordObject("Asteroids Configuration Change");
                            Target.AsteroidsSettings.RemoveAt(i);
                        }
                    }
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.Space();
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}