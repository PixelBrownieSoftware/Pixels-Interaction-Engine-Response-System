using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections;
using System;
using System.Linq;
using System.Collections.Generic;

public class ed_PIE : EditorWindow
{
    [MenuItem("Window/UI Toolkit/ed_PIE")]
    public static void ShowWindow()
    {
        ed_PIE wnd = GetWindow<ed_PIE>();
        wnd.titleContent = new GUIContent("PIE Editor");
        //wnd.Close();
    }

    //private VisualElement m_rootElement;
    private VisualElement m_rulesFactsList;
    private VisualElement m_ruleEdit;
    private VisualElement m_characterResponseDataList;
    private ListView m_ruleList;
    private ListView m_factsList;
    private ListView m_charadataList;
    private s_RespDatabase m_responseData;
    private s_RespDatabase m_currentSelectedresponseData;
    private s_ruleEntry m_currentEditRule;
    private s_factEntry m_currentEditFact;
    private s_ruleEntry m_currentSelectedRule;
    private s_factEntry m_currentSelectedFact;
    
    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement m_rootElement = rootVisualElement;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ed_PIE.uxml");
        visualTree.CloneTree(m_rootElement);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/ed_PIE.uss");
        m_rootElement.styleSheets.Add(styleSheet);

        m_characterResponseDataList = m_rootElement.Q<VisualElement>("responseCharacterDataList");
        m_rulesFactsList = m_rootElement.Q<VisualElement>("responseRules");
        m_rootElement.Add(m_characterResponseDataList);
        //UpdateRules();
        //
        GetCharacters();
        //m_rootElement.Q<Button>("addRuleButton").clicked += AddRuleOnClick;
        //m_rootElement.Q<Button>("addFactButton").clicked += AddFactOnClick;
        
    }
    public void SelectFact(IEnumerable<object> selectedFacts)
    {
        m_currentSelectedFact = (s_factEntry)selectedFacts.First();
    }

    public void OpenFact(IEnumerable<object> selectedFacts)
    {
        m_currentEditFact = (s_factEntry)selectedFacts.First();
        Debug.Log(m_currentEditRule);
        ed_fact m_factToAdd = new ed_fact(this, m_currentSelectedFact, m_responseData);

        m_rulesFactsList.Clear();
        m_rulesFactsList.Add(m_factToAdd);
    }
    public void SelectRule(IEnumerable<object> selectedRules)
    {
        m_currentSelectedRule = (s_ruleEntry)selectedRules.First();
    }
    public void SelectChar(IEnumerable<object> selectedCharas)
    {
        m_currentSelectedresponseData = (s_RespDatabase)selectedCharas.First();
    }

    public void OpenRule(IEnumerable<object> selectedRules)
    {
        m_currentEditRule = (s_ruleEntry)selectedRules.First();
        ed_rule m_ruleToAdd = new ed_rule(this, m_currentSelectedRule, m_responseData);

        m_rulesFactsList.Clear();
        m_rulesFactsList.Add(m_ruleToAdd);
    }
    private void AddFactOnClick()
    {
        s_factEntry rul = new s_factEntry();
        rul.fact_name = "New Fact";
        m_responseData.facts.Add(rul);

        m_factsList.Refresh();
    }

    private void AddRuleOnClick()
    {
        s_ruleEntry rul = new s_ruleEntry();
        rul.key = "New Rule";
        m_responseData.rules.Add(rul);

        m_ruleList.Refresh();
    }

    public void GetCharacters()
    {
        string[] names = AssetDatabase.FindAssets("t:s_RespDatabase");

        List<s_RespDatabase> responseDataObjects = new List<s_RespDatabase>();

        foreach (string nm in names)
        {
            responseDataObjects.Add(AssetDatabase.LoadAssetAtPath<s_RespDatabase>(AssetDatabase.GUIDToAssetPath(nm)));
        }

        Func<VisualElement> makeItem = () => new Label();

        Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = responseDataObjects[i].name;
        
        m_charadataList = rootVisualElement.Q<ListView>("responseCharacterDataList");

        m_charadataList.makeItem = makeItem;
        m_charadataList.bindItem = bindItem;
        m_charadataList.itemsSource = responseDataObjects;
        m_charadataList.selectionType = SelectionType.Single;

        // Callback invoked when the user double clicks an item
        m_charadataList.onItemsChosen += OpenCharacterDatabase;
        m_charadataList.onSelectionChange += SelectChar;
    }

    public void OpenCharacterDatabase(IEnumerable<object> selectedCharas)
    {
        m_factsList.Clear();
        m_ruleList.Clear();
        m_responseData = (s_RespDatabase)selectedCharas.First();
        UpdateFacts();
        UpdateRules();
    }

    public void UpdateFacts()
    {
        VisualElement m_rootElement = rootVisualElement;
        int itemCount = m_responseData.facts.Count;
        var items = m_responseData.facts;

        Func<VisualElement> makeItem = () => new Label();

        Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = items[i].fact_name;

        m_factsList = m_rootElement.Q<ListView>("factsList");

        m_factsList.makeItem = makeItem;
        m_factsList.bindItem = bindItem;
        m_factsList.itemsSource = items;
        m_factsList.selectionType = SelectionType.Single;

        // Callback invoked when the user double clicks an item
        m_factsList.onItemsChosen += OpenFact;
        m_factsList.onSelectionChange += SelectFact;

    }
    public void UpdateRules()
    {
        VisualElement m_rootElement = rootVisualElement;
        int itemCount = m_responseData.rules.Count;
        var items = m_responseData.rules;

        Func<VisualElement> makeItem = () => new Label();

        Action<VisualElement, int> bindItem = (e, i) => (e as Label).text = items[i].key;

        m_ruleList = m_rootElement.Q<ListView>("rulesList");

        m_ruleList.makeItem = makeItem;
        m_ruleList.bindItem = bindItem;
        m_ruleList.itemsSource = items;
        m_ruleList.selectionType = SelectionType.Single;

        // Callback invoked when the user double clicks an item
        m_ruleList.onItemsChosen += OpenRule;
        m_ruleList.onSelectionChange += SelectRule;

    }
}