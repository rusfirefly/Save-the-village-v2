using UnityEngine;
using UnityEditor;

// ������� ����� ��� �������� ����� ���������������� ��������
[System.Serializable]
public class CustomMask
{
    public string name;
    public bool useCustomMask;
    public int customMaskValue;
}

// ������� ��������� PropertyDrawer ��� ����������� ������ ���� � ����������
[CustomPropertyDrawer(typeof(CustomMask))]
public class CustomMaskDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // �������� ������ � ����� ���������
        SerializedProperty useCustomMask = property.FindPropertyRelative("useCustomMask");
        SerializedProperty customMaskValue = property.FindPropertyRelative("customMaskValue");

        // ���������� ������� ��� ������ ������������� ����������������� ��������
        Rect checkboxPosition = new Rect(position.x, position.y, 15f, position.height);
        useCustomMask.boolValue = EditorGUI.Toggle(checkboxPosition, useCustomMask.boolValue);

        // ���� ������������ ������ ������������ ���� ��������, ���������� ���� ��� ��� ������
        if (useCustomMask.boolValue)
        {
            Rect customMaskPosition = new Rect(position.x + 20f, position.y, position.width - 20f, position.height);
            customMaskValue.intValue = EditorGUI.MaskField(customMaskPosition, label, customMaskValue.intValue, UnityEditorInternal.InternalEditorUtility.layers);
        }

        EditorGUI.EndProperty();
    }
}

