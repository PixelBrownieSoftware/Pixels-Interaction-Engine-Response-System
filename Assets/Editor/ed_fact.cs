using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using MagnumFoundation3D.ResponseSystem;

public class ed_fact : VisualElement
{
    s_factEntry m_fact;
    s_RespDatabase m_respData;
    VisualElement m_criteriaList;
    VisualElement m_followbackQueryList;
    VisualElement m_modificationList;
    ed_RespDataObj m_responseDataEditor;

    public ed_fact(ed_RespDataObj m_responseDataEditor, s_factEntry m_fact, s_RespDatabase m_respData)
    {
        this.m_fact = m_fact;
        this.m_respData = m_respData;
        this.m_responseDataEditor = m_responseDataEditor;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ed_fact.uxml");
        visualTree.CloneTree(this);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/ed_fact.uss");
        this.styleSheets.Add(styleSheet);

        #region Fields
        TextField nameField = this.Query<TextField>("fName").First();
        nameField.value = m_fact.fact_name;
        nameField.RegisterCallback<ChangeEvent<string>>(
            e =>
            {
                m_fact.fact_name = (string)e.newValue;
                EditorUtility.SetDirty(m_respData);
                m_responseDataEditor.UpdateRules();
            }
        );

        TextField stringField = this.Query<TextField>("fStr").First();
        IntegerField intField = this.Query<IntegerField>("fInt").First();
        FloatField floatField = this.Query<FloatField>("fFloat").First();

        EnumField criteriaFactType = this.Q<EnumField>("fType");
        criteriaFactType.Init(this.m_fact.varType);
        criteriaFactType.RegisterCallback<ChangeEvent<System.Enum>>(
            e =>
            {
                this.m_fact.varType = (s_factEntry.COND_VAR_TYPE)e.newValue;
                UpdateFactField(ref stringField, ref intField, ref floatField);
                EditorUtility.SetDirty(m_respData);
            }
        );

        intField.value = this.m_fact.value_int;
        stringField.value = this.m_fact.value_string;
        floatField.value = this.m_fact.value_float;
        UpdateFactField(ref stringField, ref intField, ref floatField);
        #endregion

        this.Q<Button>("removeFactButton").clicked += RemoveOnClick;
    }
    public void UpdateFactField(ref TextField stringField, ref IntegerField intField, ref FloatField floatField)
    {

        switch (this.m_fact.varType)
        {
            case s_factEntry.COND_VAR_TYPE.INT:
                intField.style.display = DisplayStyle.Flex;
                stringField.style.display = DisplayStyle.None;
                floatField.style.display = DisplayStyle.None;
                intField.visible = true;
                stringField.visible = false;
                floatField.visible = false;
                break;

            case s_factEntry.COND_VAR_TYPE.STRING:
                stringField.style.display = DisplayStyle.Flex;
                intField.style.display = DisplayStyle.None;
                floatField.style.display = DisplayStyle.None;
                stringField.visible = true;
                intField.visible = false;
                floatField.visible = false;
                break;

            case s_factEntry.COND_VAR_TYPE.FLOAT:
                floatField.style.display = DisplayStyle.Flex;
                stringField.style.display = DisplayStyle.None;
                intField.style.display = DisplayStyle.None;
                floatField.visible = true;
                stringField.visible = false;
                intField.visible = false;
                break;
        }
    }
    private void RemoveOnClick()
    {
        m_respData.facts.Remove(m_fact);
        m_responseDataEditor.UpdateFacts();
        this.visible = false;
    }
}