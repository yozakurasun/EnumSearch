using UnityEngine;
public class EnumSearchAttribute : PropertyAttribute
{
    public string ScriptName { get; private set; }
    public EnumSearchAttribute(string ScriptName) => this.ScriptName = ScriptName;
}