using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine;
using MagnumFoundation3D.ResponseSystem;

public class ed_rule : VisualElement
{
    s_ruleEntry m_rule;
    s_RespDatabase m_respData;
    VisualElement m_criteriaList;
    VisualElement m_followbackQueryList;
    VisualElement m_modificationList;
    ed_RespDataObj m_responseDataEditor;

    public ed_rule(ed_RespDataObj m_responseDataEditor, s_ruleEntry m_rule, s_RespDatabase m_respData) {
        this.m_rule = m_rule;
        this.m_respData = m_respData;
        this.m_responseDataEditor = m_responseDataEditor;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ed_rule.uxml");
        visualTree.CloneTree(this);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/ed_rule.uss");
        this.styleSheets.Add(styleSheet);

        #region Fields
        TextField nameField = this.Query<TextField>("key").First();
        nameField.value = m_rule.key;
        nameField.RegisterCallback<ChangeEvent<string>>(
            e =>
            {
                m_rule.key = (string)e.newValue;
                EditorUtility.SetDirty(m_respData);
                m_responseDataEditor.UpdateRules();
            }
        );
        TextField diaField = this.Query<TextField>("dialogueText").First();
        diaField.value = m_rule.m_dialogue;
        diaField.RegisterCallback<ChangeEvent<string>>(
            e =>
            {
                m_rule.m_dialogue = e.newValue;
                EditorUtility.SetDirty(m_respData);
            }
        );
        FloatField diaTimer = this.Query<FloatField>("dialogueTimer").First();
        diaTimer.value = m_rule.m_timer;
        diaTimer.RegisterCallback<ChangeEvent<float>>(
            e =>
            {
                m_rule.m_timer = e.newValue;
                EditorUtility.SetDirty(m_respData);
            }
        );
        TextField diaTriggObjField = this.Query<TextField>("dialogueTriggerObj").First();
        diaTriggObjField.value = m_rule.triggersObject;
        diaTriggObjField.RegisterCallback<ChangeEvent<string>>(
            e =>
            {
                m_rule.triggersObject = e.newValue;
                EditorUtility.SetDirty(m_respData);
            }
        );
        IntegerField diaPriorityField = this.Query<IntegerField>("dialoguePriority").First();
        diaPriorityField.value = this.m_rule.priority;
        diaPriorityField.RegisterCallback<ChangeEvent<int>>(
            e =>
            {
                m_rule.priority = e.newValue;
                EditorUtility.SetDirty(m_respData);
            }
        );


        this.Q<Button>("removeRuleButton").clicked += RemoveOnClick;
        m_criteriaList = this.Q<VisualElement>("responseCriteria");
        m_followbackQueryList = this.Q<VisualElement>("responseFollowupQuery");
        m_modificationList = this.Q<VisualElement>("responseModifications");
        UpdateCriteriaList();
        UpdateFollowBackQueryList();
        UpdateSetModificationsList();
        this.Q<Button>("addFUQueryButton").clicked += AddFollowbackQueryOnClick;
        this.Q<Button>("addModificationButton").clicked += AddModificationOnClick;
        this.Q<Button>("addCriteriaButton").clicked += AddCriteriaOnClick;

        #endregion
    }
    private void RemoveOnClick()
    {
        m_respData.rules.Remove(m_rule);
        m_responseDataEditor.UpdateRules();
        this.visible = false;
    }
    public void UpdateFollowBackQueryList()
    {
        m_followbackQueryList.Clear();

        for (int i = 0; i < m_rule.querySendFacts.Count; i++)
        {
            var m_criterion = m_rule.querySendFacts[i];
            ed_RespFollowup m_followBackQuery = new ed_RespFollowup(m_rule, m_respData, m_criterion, this);

            m_followbackQueryList.Add(m_followBackQuery);
        }
    }

    public void UpdateSetModificationsList()
    {
        m_modificationList.Clear();

        for (int i = 0; i < m_rule.modifiers.Count; i++)
        {
            var m_modification = m_rule.modifiers[i];
            ed_RespModification m_modificationToAdd = new ed_RespModification(m_rule, m_respData, m_modification, this);

            m_modificationList.Add(m_modificationToAdd);
        }
    }

    public void UpdateCriteriaList()
    {
        m_criteriaList.Clear();

        for ( int i = 0; i < m_rule.conditions.Count; i++)
        {
            var m_criterion = m_rule.conditions[i];
            ed_ruleCriterion m_criteriaToAdd = new ed_ruleCriterion(m_rule, m_respData, m_criterion, this);

            m_criteriaList.Add(m_criteriaToAdd);
        }
    }

    private void AddFollowbackQueryOnClick()
    {
        s_factEntry m_query = new s_factEntry();
        m_rule.querySendFacts.Add(m_query);
        EditorUtility.SetDirty(m_respData);
        UpdateFollowBackQueryList();
    }
    private void AddModificationOnClick()
    {
        s_ruleEntry.r_set m_crit = new s_ruleEntry.r_set();
        m_rule.modifiers.Add(m_crit);
        EditorUtility.SetDirty(m_respData);
        UpdateSetModificationsList();
    }
    private void AddCriteriaOnClick()
    {
        s_ruleEntry.r_cond m_crit = new s_ruleEntry.r_cond();
        m_rule.conditions.Add(m_crit);
        EditorUtility.SetDirty(m_respData);
        UpdateCriteriaList();
    }
}
