using UnityEditor;
using UnityEngine;

namespace KSIShareable.Editor
{
    [CustomPropertyDrawer(typeof(ShowScriptableObjectAttribute))]
    public class ShowScriptableObjectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            // 기본 필드를 그립니다.
            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            Rect propertyRect = new Rect(position.x, position.y, position.width, singleLineHeight);
            EditorGUI.PropertyField(propertyRect, property, label, true);

            // ScriptableObject가 연결된 경우에만 추가 필드들을 그립니다.
            if (property.objectReferenceValue != null && property.objectReferenceValue is ScriptableObject) {
                ScriptableObject scriptableObject = (ScriptableObject)property.objectReferenceValue;
                SerializedObject serializedObject = new SerializedObject(scriptableObject);

                SerializedProperty iterator = serializedObject.GetIterator();
                iterator.NextVisible(true); // 첫 번째 필드는 무시 (스크립트 필드)

                EditorGUI.indentLevel++;
                while (iterator.NextVisible(false)) {
                    position.y += singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    // 현재 필드의 높이를 계산
                    float propertyHeight = EditorGUI.GetPropertyHeight(iterator, true);

                    Rect fieldRect = new Rect(position.x, position.y, position.width, propertyHeight);
                    EditorGUI.PropertyField(fieldRect, iterator, true);

                    // 다음 필드를 그릴 위치를 반영 (필드 높이만큼 추가)
                    position.y += propertyHeight - singleLineHeight; // 이미 기본 라인을 더했으므로 그 높이만큼만 추가
                }
                EditorGUI.indentLevel--;

                serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float height = EditorGUIUtility.singleLineHeight; // 기본 필드 높이

            if (property.objectReferenceValue != null && property.objectReferenceValue is ScriptableObject) {
                ScriptableObject scriptableObject = (ScriptableObject)property.objectReferenceValue;
                SerializedObject serializedObject = new SerializedObject(scriptableObject);

                SerializedProperty iterator = serializedObject.GetIterator();
                iterator.NextVisible(true); // 첫 번째 필드 (스크립트) 무시

                while (iterator.NextVisible(false)) {
                    // 각 필드의 실제 높이를 동적으로 계산
                    height += EditorGUI.GetPropertyHeight(iterator, true) + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return height;
        }
    }
}