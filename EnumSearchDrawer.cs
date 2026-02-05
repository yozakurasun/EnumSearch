#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(EnumSearchAttribute))]
public class EnumSearchDrawer : PropertyDrawer
{
    /// <summary>
    /// インスペクター上の表示管理
    /// </summary>
    /// <param name="position">描画位置</param>
    /// <param name="property">選択中の要素</param>
    /// <param name="label">変数のラベル</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //インスペクターに変数のラベルと選択中の要素を表示
        EditorGUI.BeginProperty(position, label, property);
        EditorGUI.PrefixLabel(position, label);

        //ボタンのサイズと位置
        Rect buttonRect = new Rect(
            position.x + EditorGUIUtility.labelWidth,
            position.y,
            position.width - EditorGUIUtility.labelWidth,
            position.height
        );

        //ボタンに選択中の要素名を表示
        if (GUI.Button(buttonRect, property.enumDisplayNames[property.enumValueIndex], EditorStyles.popup))
        {
            string[] names = property.enumDisplayNames;

            //ポップアップ表示
            SearchablePopup.Show(
                buttonRect,
                names,
                property.enumValueIndex,
                (index) =>
                {
                    property.enumValueIndex = index;
                    property.serializedObject.ApplyModifiedProperties();
                }
            );
        }

        EditorGUI.EndProperty();
    }
}
#endif