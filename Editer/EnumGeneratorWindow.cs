using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnumGeneratorWindow : EditorWindow
{
    private MonoScript _targetEnumScript;
    private Type _targetEnumType;
    private List<string> _names = new List<string>();
    private string _newName = "";

    public static void Open(MonoScript targetScript, Type enumType)
    {
        var window = GetWindow<EnumGeneratorWindow>("Enum Generator");
        window._targetEnumScript = targetScript;
        window._targetEnumType = enumType;
    }

    private void OnGUI()
    {
        // 対象Enum指定
        _targetEnumScript = (MonoScript)EditorGUILayout.ObjectField(
            "対象のScript",
            _targetEnumScript,
            typeof(MonoScript),
            false
        );

        GUILayout.Space(10);

        EditorGUILayout.LabelField("対象のEnum", _targetEnumType?.Name ?? "(None)");

        GUILayout.Space(10);

        // 追加入力欄
        EditorGUILayout.BeginHorizontal();
        _newName = EditorGUILayout.TextField("追加するEnum", _newName);

        if (GUILayout.Button("Add", GUILayout.Width(60)))
        {
            if (!string.IsNullOrWhiteSpace(_newName))
            {
                _names.Add(_newName);
            }
        }
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);

        // リスト表示
        for (int i = 0; i < _names.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            _names[i] = EditorGUILayout.TextField(_names[i]);

            if (GUILayout.Button("×", GUILayout.Width(20)))
            {
                _names.RemoveAt(i);
                i--;
            }

            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(10);

        // 生成ボタン
        GUI.enabled = _targetEnumScript != null;

        if (GUILayout.Button("Generate Enum"))
        {
            EnumGenerator.Generate(new EnumData { Script = _targetEnumScript, EnumType = _targetEnumType }, _names.ToArray());
            Close();
        }

        GUI.enabled = true;

        // 注意表示
        if (_targetEnumScript == null)
        {
            EditorGUILayout.HelpBox("Enumの.csファイルを指定してください", MessageType.Warning);
        }
    }
}