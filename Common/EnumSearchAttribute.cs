using UnityEngine;
public class EnumSearchAttribute : PropertyAttribute
{
    public string ScriptName { get; private set; }

    /// <summary>
    /// 検索機能を付けたいシリアライズ変数の上に[EnumSearch]を入れたら検索出来るようになります。
    /// 使用例）
    /// [EnumSearch]
    /// [SerializeField] private EnumType _enumType;
    /// </summary>
    public EnumSearchAttribute() => this.ScriptName = string.Empty;

    /// <summary>
    /// Enum追加機能も付けたい場合は[EnumSearch]の中に対象のEnumがあるスクリプト名を入れてください
    /// 使用例）EnumType.cs
    /// [EnumSearch("EnumType")]
    /// [SerializeField] private EnumType _enumType;
    /// </summary>
    public EnumSearchAttribute(string ScriptName) => this.ScriptName = ScriptName;
}