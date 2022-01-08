using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using MagnumFoundation3D.ResponseSystem;


public class ed_RespFollowup : VisualElement
{
    s_ruleEntry m_rule;
    s_RespDatabase respData;
    VisualElement criteriaList;
    ed_rule m_rootRule;
    s_factEntry m_set;

    public ed_RespFollowup(s_ruleEntry m_rule, s_RespDatabase respData, s_factEntry m_set, ed_rule rootRule)
    {
        this.m_rule = m_rule;
        this.m_set = m_set;
        m_rootRule = rootRule;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ed_RespFollowup.uxml");
        visualTree.CloneTree(this);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/ed_RespFollowup.uss");
        this.styleSheets.Add(styleSheet);

        this.Q<Button>("removeFQueryButton").clicked += RemoveCriteriaOnClick;

        #region Feilds
        TextField nameField = this.Query<TextField>("criteriaName").First();
        nameField.value = this.m_set.fact_name;
        nameField.RegisterCallback<ChangeEvent<string>>(
            e =>
            {
                this.m_set.fact_name = e.newValue;
                EditorUtility.SetDirty(respData);
            }
        );
        TextField stringField = this.Query<TextField>("criteriaFactStr").First();
        IntegerField intField = this.Query<IntegerField>("criteriaFactInt").First();
        FloatField floatField = this.Query<FloatField>("criteriaFactFloat").First();

        EnumField criteriaFactType = this.Q<EnumField>("criteriaType");
        criteriaFactType.Init(this.m_set.varType);
        criteriaFactType.RegisterCallback<ChangeEvent<System.Enum>>(
            e =>
            {
                this.m_set.varType = (s_factEntry.COND_VAR_TYPE)e.newValue;
                EditorUtility.SetDirty(respData);
                UpdateFactField(ref stringField, ref intField, ref floatField);
            }
        );
        
        intField.value = this.m_set.value_int;
        stringField.value = this.m_set.value_string;
        floatField.value = this.m_set.value_float;

        switch (this.m_set.varType)
        {
            case s_factEntry.COND_VAR_TYPE.INT:
                intField.visible = true;
                stringField.visible = false;
                floatField.visible = false;
                {
                    intField.value = this.m_set.value_int;
                    intField.RegisterCallback<ChangeEvent<int>>(
                        e =>
                        {
                            this.m_set.value_int = e.newValue;
                            EditorUtility.SetDirty(respData);
                        }
                    );
                }
                break;

            case s_factEntry.COND_VAR_TYPE.STRING:
                stringField.visible = true;
                intField.visible = false;
                floatField.visible = false;
                {
                    stringField.value = this.m_set.value_string;
                    stringField.RegisterCallback<ChangeEvent<string>>(
                        e =>
                        {
                            this.m_set.value_string = e.newValue;
                            EditorUtility.SetDirty(respData);
                        }
                    );
                }
                break;

            case s_factEntry.COND_VAR_TYPE.FLOAT:
                floatField.visible = true;
                stringField.visible = false;
                intField.visible = false;
                {
                    floatField.value = this.m_set.value_float;
                    floatField.RegisterCallback<ChangeEvent<float>>(
                        e =>
                        {
                            this.m_set.value_float = e.newValue;
                            EditorUtility.SetDirty(respData);
                        }
                    );
                }
                break;
        }
        UpdateFactField(ref stringField, ref intField, ref floatField);


        #endregion
    }
    private void RemoveCriteriaOnClick()
    {
        m_rule.querySendFacts.Remove(m_set);
        m_rootRule.UpdateFollowBackQueryList();
    }
    public void UpdateFactField(ref TextField stringField, ref IntegerField intField, ref FloatField floatField)
    {

        switch (this.m_set.varType)
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
}