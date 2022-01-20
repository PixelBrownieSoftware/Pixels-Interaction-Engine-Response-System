using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class ed_PIERS : EditorWindow
{
    [MenuItem("Window/UI Toolkit/ed_PIERS")]
    public static void ShowExample()
    {
        ed_PIERS wnd = GetWindow<ed_PIERS>();
        wnd.titleContent = new GUIContent("ed_PIERS");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);
    }
}