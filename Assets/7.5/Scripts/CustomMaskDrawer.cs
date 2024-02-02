using UnityEngine;
using UnityEditor;

// —оздаем класс дл€ хранени€ наших пользовательских значений
[System.Serializable]
public class CustomMask
{
    public string name;
    public bool useCustomMask;
    public int customMaskValue;
}

// —оздаем кастомный PropertyDrawer дл€ отображени€ нашего пол€ в инспекторе
[CustomPropertyDrawer(typeof(CustomMask))]
public class CustomMaskDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // ѕолучаем доступ к нашим значени€м
        SerializedProperty useCustomMask = property.FindPropertyRelative("useCustomMask");
        SerializedProperty customMaskValue = property.FindPropertyRelative("customMaskValue");

        // ќтображаем чекбокс дл€ выбора использовани€ пользовательского значени€
        Rect checkboxPosition = new Rect(position.x, position.y, 15f, position.height);
        useCustomMask.boolValue = EditorGUI.Toggle(checkboxPosition, useCustomMask.boolValue);

        // ≈сли пользователь выбрал использовать свое значение, отображаем поле дл€ его выбора
        if (useCustomMask.boolValue)
        {
            Rect customMaskPosition = new Rect(position.x + 20f, position.y, position.width - 20f, position.height);
            customMaskValue.intValue = EditorGUI.MaskField(customMaskPosition, label, customMaskValue.intValue, UnityEditorInternal.InternalEditorUtility.layers);
        }

        EditorGUI.EndProperty();
    }
}

