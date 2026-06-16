using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(KeyBinding))]
public class KeyBindingDrawer : PropertyDrawer
{
    private float labelWidth;
    private const int SCROLLBAR_WIDTH = 36;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        labelWidth = EditorGUIUtility.labelWidth;
        EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int maxWidth = (Screen.width - (int)labelWidth - SCROLLBAR_WIDTH) / 2;
        Rect priKey = new Rect(position.x + labelWidth, position.y, maxWidth, position.height);
        Rect secKey = new Rect(position.x + labelWidth + maxWidth, position.y, maxWidth, position.height);

        EditorGUI.PropertyField(priKey, property.FindPropertyRelative("primaryKey"), GUIContent.none);
        EditorGUI.PropertyField(secKey, property.FindPropertyRelative("secondaryKey"), GUIContent.none);
        EditorGUI.EndProperty();
    }

}