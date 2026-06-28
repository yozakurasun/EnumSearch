using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class EnumGenerator
{
    private static string _enumName;

    public static void Generate(EnumData enumData, string[] newNames)
    {
        if (enumData == null || enumData.Script == null)
        {
            Debug.LogError("Enumスクリプトが指定されていません");
            return;
        }

        string path = AssetDatabase.GetAssetPath(enumData.Script);
        string code = File.ReadAllText(path);

        _enumName = enumData.EnumType.Name;

        // Enumブロック取得
        var match = Regex.Match(code,
            $@"public\s+enum\s+{_enumName}\s*\{{([\s\S]*?)\}}",
            RegexOptions.Multiline);

        if (!match.Success)
        {
            Debug.LogError("対象のEnumが見つかりません");
            return;
        }

        string body = match.Groups[1].Value;

        // 既存要素抽出
        var existing = Regex.Matches(body, @"\b(\w+)\b\s*(=|,)")
            .Cast<Match>()
            .Select(m => m.Groups[1].Value)
            .Where(n => n != "None" && n != "Max")
            .Distinct()
            .ToList();

        // 新規＋既存マージ
        var merged = existing
            .Concat(newNames)
            .Where(n => !string.IsNullOrWhiteSpace(n))
            .Select(SanitizeName)
            .Where(n => !StartsWithNumber(n))
            .Distinct()
            .ToList();

        // インデント取得
        string indent = GetIndent(code, match.Index);

        // ヘッダー取得
        var headerMatch = Regex.Match(match.Value, @"(public|private|internal)?\s*enum\s+\w+");
        string header = headerMatch.Value;

        // enum生成
        string newEnum = FormatEnum(header, _enumName, merged, indent);

        // 置換
        string newCode = code.Replace(match.Value, newEnum);

        File.WriteAllText(path, newCode);
        AssetDatabase.Refresh();
    }

    //  共通処理 
    private static string SanitizeName(string input)
    {
        string cleaned = Regex.Replace(input, @"[^a-zA-Z0-9_]", "");
        if (string.IsNullOrEmpty(cleaned)) return "";
        return char.ToUpper(cleaned[0]) + cleaned.Substring(1);
    }

    private static bool StartsWithNumber(string input)
    {
        return input.Length > 0 && char.IsDigit(input[0]);
    }

    private static string FormatEnum(string header, string enumName, List<string> values, string indent)
    {
        string innerIndent = indent + "    ";

        var lines = new List<string>();

        lines.Add($"{header}");
        lines.Add($"{indent}{{");
        lines.Add($"{innerIndent}None = 0,");

        for (int i = 0; i < values.Count; i++)
        {
            lines.Add($"{innerIndent}{values[i]} = {i + 1},");
        }

        lines.Add($"{innerIndent}Max = {values.Count + 1}");
        lines.Add($"{indent}}}");

        return string.Join("\n", lines);
    }

    private static string GetIndent(string code, int index)
    {
        int lineStart = code.LastIndexOf('\n', index);
        if (lineStart == -1) lineStart = 0;

        int i = lineStart + 1;
        while (i < code.Length && (code[i] == ' ' || code[i] == '\t'))
        {
            i++;
        }

        return code.Substring(lineStart + 1, i - (lineStart + 1));
    }
}

public class EnumData
{
    public MonoScript Script;
    public Type EnumType;
}