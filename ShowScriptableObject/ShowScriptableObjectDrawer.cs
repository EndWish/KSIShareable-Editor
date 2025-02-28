using UnityEditor;
using UnityEngine;

namespace KSIShareable.Editor
{
    [CustomPropertyDrawer(typeof(ShowScriptableObjectAttribute))]
    public class ShowScriptableObjectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            // �⺻ �ʵ带 �׸��ϴ�.
            float singleLineHeight = EditorGUIUtility.singleLineHeight;
            Rect propertyRect = new Rect(position.x, position.y, position.width, singleLineHeight);
            EditorGUI.PropertyField(propertyRect, property, label, true);

            // ScriptableObject�� ����� ��쿡�� �߰� �ʵ���� �׸��ϴ�.
            if (property.objectReferenceValue != null && property.objectReferenceValue is ScriptableObject) {
                ScriptableObject scriptableObject = (ScriptableObject)property.objectReferenceValue;
                SerializedObject serializedObject = new SerializedObject(scriptableObject);

                SerializedProperty iterator = serializedObject.GetIterator();
                iterator.NextVisible(true); // ù ��° �ʵ�� ���� (��ũ��Ʈ �ʵ�)

                EditorGUI.indentLevel++;
                while (iterator.NextVisible(false)) {
                    position.y += singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

                    // ���� �ʵ��� ���̸� ���
                    float propertyHeight = EditorGUI.GetPropertyHeight(iterator, true);

                    Rect fieldRect = new Rect(position.x, position.y, position.width, propertyHeight);
                    EditorGUI.PropertyField(fieldRect, iterator, true);

                    // ���� �ʵ带 �׸� ��ġ�� �ݿ� (�ʵ� ���̸�ŭ �߰�)
                    position.y += propertyHeight - singleLineHeight; // �̹� �⺻ ������ �������Ƿ� �� ���̸�ŭ�� �߰�
                }
                EditorGUI.indentLevel--;

                serializedObject.ApplyModifiedProperties();
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            float height = EditorGUIUtility.singleLineHeight; // �⺻ �ʵ� ����

            if (property.objectReferenceValue != null && property.objectReferenceValue is ScriptableObject) {
                ScriptableObject scriptableObject = (ScriptableObject)property.objectReferenceValue;
                SerializedObject serializedObject = new SerializedObject(scriptableObject);

                SerializedProperty iterator = serializedObject.GetIterator();
                iterator.NextVisible(true); // ù ��° �ʵ� (��ũ��Ʈ) ����

                while (iterator.NextVisible(false)) {
                    // �� �ʵ��� ���� ���̸� �������� ���
                    height += EditorGUI.GetPropertyHeight(iterator, true) + EditorGUIUtility.standardVerticalSpacing;
                }
            }

            return height;
        }
    }
}