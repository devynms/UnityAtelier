using UnityEngine;
using UnityEditor;

namespace Atelier.Scenes {

    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public class SceneDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            if (property.propertyType != SerializedPropertyType.String) {
                EditorGUI.LabelField(position, label.text, "Use [Scene] with strings.");
            } else {
                SceneAsset sceneAsset = this.GetSceneAsset(property.stringValue);
                Object scene = EditorGUI.ObjectField(position, label, sceneAsset, typeof(SceneAsset), true);
                if (scene == null) {
                    property.stringValue = "";
                } else if (scene.name != property.stringValue) {
                    sceneAsset = this.GetSceneAsset(scene.name);
                    if (sceneAsset == null) {
                        Debug.LogWarning($"Scene [{scene.name}] cannot be used. Add this scene to 'Scenes in the Build' in build settings.");
                    } else {
                        property.stringValue = scene.name;
                    }
                }
            }
        }

        private SceneAsset GetSceneAsset(string sceneName) {
            if (string.IsNullOrEmpty(sceneName)) return null;
            foreach (var editorScene in EditorBuildSettings.scenes) {
                if (editorScene.path.IndexOf(sceneName) != -1) {
                    return AssetDatabase.LoadAssetAtPath(editorScene.path, typeof(SceneAsset)) as SceneAsset;
                }
            }
            Debug.LogWarning($"Scene [{sceneName}] cannot be used. Add this scene to 'Scenes in the Build' in build settings.");
            return null;
        }

    }

}

