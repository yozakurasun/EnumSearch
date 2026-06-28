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
        var attr = attribute as EnumSearchAttribute;
        var script = GetScriptByName(attr.ScriptName);
        var enumType = fieldInfo.FieldType;
        EnumData enumData = new EnumData { Script = script, EnumType = enumType };

        if (script == null)
        {
            Debug.LogError("Enum定義スクリプトが見つかりませんでした: " + attr.ScriptName);
            return;
        }

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
                },
                enumData
            );
        }

        EditorGUI.EndProperty();
    }

    private MonoScript GetScriptByName(string scriptName)
    {
        var guids = AssetDatabase.FindAssets($"{scriptName} t:MonoScript");

        if (guids.Length == 0) return null;

        var path = AssetDatabase.GUIDToAssetPath(guids[0]);
        return AssetDatabase.LoadAssetAtPath<MonoScript>(path);
    }
}
#endif