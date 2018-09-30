using Framework.Editor;
using UnityEditor;
using UnityEngine;

namespace Game.Gameplay.Spawn.Editor
{
    [CustomEditor(typeof(AsteroidsSpawner))]
    public class AsteroidsSpawnerEditor : CustomEditorBase<AsteroidsSpawner>
    {
        protected override void DrawInspector()
        {
            base.DrawInspector();

            EditorGUILayout.BeginVertical(GUI.skin.box);
            {
                EditorGUILayout.LabelField("Asteroids Spawner Setup", HeaderStyle);
                if (GUILayout.Button("Add setup"))
                {
                    RecordObject("Asteroids Spawner Change");
                    Target.SpawnSetups.Add(new SpawnSetup());
                }

                var setups = serializedObject.FindProperty("SpawnSetups");
                var count = setups.arraySize;
                for (int i = 0; i < count; i++)
                {
                    EditorGUILayout.BeginHorizontal(GUI.skin.box);
                    {
                        var element = setups.GetArrayElementAtIndex(i);
                        var spawner = element.FindPropertyRelative("Spawner");
                        var probability = element.FindPropertyRelative("Probability");

                        EditorGUILayout.BeginVertical();
                        {
                            EditorGUILayout.PropertyField(spawner);
                            EditorGUILayout.PropertyField(probability);
                        }
                        EditorGUILayout.EndVertical();

                        if (GUILayout.Button("X", GUILayout.Width(20)))
                        {
                            RecordObject("Asteroids Spawner Change");
                            Target.SpawnSetups.RemoveAt(i);
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