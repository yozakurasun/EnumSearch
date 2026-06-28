#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using System.Linq;

public class SearchablePopup : PopupWindowContent
{
    private string _search = ""; //検索ワード
    private readonly string[] _names; //選択肢
    private readonly int _currentIndex; //現在選択中
    private readonly Action<int> _onSelect; //選択時のコールバック
    private readonly EnumData _targetEnumData; //対象のスクリプトとEnum情報

    private Vector2 _scrollPosition; //スクロール位置

    /// <summary>
    /// ポップアップ表示を呼び出す関数
    /// </summary>
    /// <param name="rect">ボタンのサイズ</param>
    /// <param name="names">候補</param>
    /// <param name="currentIndex">現在選択中</param>
    /// <param name="onSelect">選択時のコールバック</param>
    /// <param name="targetScript">対象のスクリプト</param>
    public static void Show(Rect rect, string[] names, int currentIndex, Action<int> onSelect, EnumData targetEnumData)
    {
        var window = new SearchablePopup(names, currentIndex, onSelect, targetEnumData);
        PopupWindow.Show(rect, window);
    }

    /// <summary>
    /// コンストラクター
    /// </summary>
    /// <param name="names">候補</param>
    /// <param name="currentIndex">現在選択中</param>
    /// <param name="onSelect">選択時のコールバック</param>
    /// <param name="targetScript">対象のスクリプト</param>
    private SearchablePopup(string[] names, int currentIndex, Action<int> onSelect, EnumData enumData)
    {
        this._names = names;
        this._currentIndex = currentIndex;
        this._onSelect = onSelect;
        this._targetEnumData = enumData;
    }

    /// <summary>
    /// ポップアップのウィンドウサイズ
    /// </summary>
    /// <returns></returns>
    public override Vector2 GetWindowSize()
    {
        return new Vector2(200, 300);
    }

    /// <summary>
    /// ウィンドウ内の描画処理
    /// </summary>
    /// <param name="rect">描画領域</param>
    public override void OnGUI(Rect rect)
    {
        // 検索バー
        GUI.SetNextControlName("SearchBar");
        var style = GUI.skin.FindStyle("ToolbarSeachTextField") ?? GUI.skin.textField;
        _search = GUI.TextField(new Rect(5, 5, rect.width - 10, 20), _search, style);

        // ウィンドウを開いたタイミングで検索バーを選択状態に変更
        if (Event.current.type == EventType.Repaint)
            GUI.FocusControl("SearchBar");

        // スクロール範囲設定
        Rect scrollRect = new Rect(0, 30, rect.width, rect.height - 60);
        Rect contentRect = new Rect(0, 0, rect.width - 20, _names.Length * 20);

        _scrollPosition = GUI.BeginScrollView(scrollRect, _scrollPosition, contentRect);

        // 検索フィルター（空なら全表示、大文字小文字区別しないあいまい検索）
        var filtered = _names
            .Select((name, index) => new { name, index })
            .Where(x =>
                string.IsNullOrEmpty(_search) ||
                x.name.Contains(_search, StringComparison.OrdinalIgnoreCase)
            )
            .ToList();

        // 結果表示
        float y = 0;
        foreach (var item in filtered)
        {
            Rect buttonRect = new Rect(5, y, contentRect.width - 10, 20);
            bool isSelected = (item.index == _currentIndex);

            if (GUI.Button(buttonRect, (isSelected ? "→ " : "") + item.name))
            {
                _onSelect(item.index);
                editorWindow.Close();
            }

            y += 20;
        }
        GUI.EndScrollView();

        //Enum追加用ボタン
        Rect enumGenerateButton = new Rect(5, rect.height - 25, rect.width - 10, 20);
        if (GUI.Button(enumGenerateButton, "Enum追加"))
        {
            if(_targetEnumData == null || _targetEnumData.Script == null)
            {
                Debug.LogError("対象のスクリプトが見つかりませんでした。");
                return;
            }
            EnumGeneratorWindow.Open(_targetEnumData.Script, _targetEnumData.EnumType);
        }
    }
}
#endif