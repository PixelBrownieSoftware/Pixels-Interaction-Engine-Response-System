using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using MagnumFoundation3D.ResponseSystem;


public class ed_ruleCriterion : VisualElement
{
    s_ruleEntry m_rule;
    s_RespDatabase respData;
    VisualElement criteriaList;
    ed_rule m_rootRule;
    s_ruleEntry.r_cond m_cond;

    public ed_ruleCriterion(s_ruleEntry m_rule, s_RespDatabase respData, s_ruleEntry.r_cond m_cond, ed_rule rootRule)
    {
        this.m_rule = m_rule;
        this.m_cond = m_cond;
        m_rootRule = rootRule;

        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/ed_ruleCriterion.uxml");
        visualTree.CloneTree(this);

        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Editor/ed_ruleCriterion.uss");
        this.styleSheets.Add(styleSheet);

        this.Q<Button>("removeCriteriaButton").clicked += RemoveCriteriaOnClick;

        #region Feilds
        TextField nameField = this.Query<TextField>("criteriaName").First();
        nameField.value = this.m_cond.factToCheck.fact_name;
        nameField.RegisterCallback<ChangeEvent<string>>(
            e =>
            {
                this.m_cond.factToCheck.fact_name = e.newValue;
                EditorUtility.SetDirty(respData);
            }
        );
        TextField stringField = this.Query<TextField>("criteriaFactStr").First();
        IntegerField intField = this.Query<IntegerField>("criteriaFactInt").First();
        FloatField floatField = this.Query<FloatField>("criteriaFactFloat").First();

        EnumField criteriaFactType = this.Q<EnumField>("criteriaType");
        criteriaFactType.Init(this.m_cond.factToCheck.varType);
        criteriaFactType.RegisterCallback<ChangeEvent<System.Enum>>(
            e =>
            {
                this.m_cond.factToCheck.varType = (s_factEntry.COND_VAR_TYPE)e.newValue;
                EditorUtility.SetDirty(respData);
                UpdateFactField(ref stringField, ref intField, ref floatField);
            }
        );

        EnumField criteriaNumSign = this.Q<EnumField>("criteriaNumSign");
        criteriaNumSign.Init(this.m_cond.condType);
        criteriaNumSign.RegisterCallback<ChangeEvent<System.Enum>>(
            e =>
            {
                this.m_cond.condType = (s_ruleEntry.r_cond.COND_Type)e.newValue;
                EditorUtility.SetDirty(respData);
            }
        );

        EnumField criteriaObjType = this.Q<EnumField>("criteriaObType");
        criteriaObjType.Init(this.m_cond.objType);
        criteriaObjType.RegisterCallback<ChangeEvent<System.Enum>>(
            e =>
            {
                this.m_cond.objType = (s_ruleEntry.r_cond.COND_OBJ_TYPE)e.newValue;
                EditorUtility.SetDirty(respData);
            }
        );

        intField.value = this.m_cond.factToCheck.value_int;
        stringField.value = this.m_cond.factToCheck.value_string;
        floatField.value = this.m_cond.factToCheck.value_float;

        switch (this.m_cond.factToCheck.varType)
        {
            case s_factEntry.COND_VAR_TYPE.INT:
                {
                    intField.value = this.m_cond.factToCheck.value_int;
                    intField.RegisterCallback<ChangeEvent<int>>(
                        e =>
                        {
                            this.m_cond.factToCheck.value_int = e.newValue;
                            EditorUtility.SetDirty(respData);
                        }
                    );
                }
                break;

            case s_factEntry.COND_VAR_TYPE.STRING:
                {
                    stringField.value = this.m_cond.factToCheck.value_string;
                    stringField.RegisterCallback<ChangeEvent<string>>(
                        e =>
                        {
                            this.m_cond.factToCheck.value_string = e.newValue;
                            EditorUtility.SetDirty(respData);
                        }
                    );
                }
                break;

            case s_factEntry.COND_VAR_TYPE.FLOAT:
                {
                    floatField.value = this.m_cond.factToCheck.value_float;
                    floatField.RegisterCallback<ChangeEvent<float>>(
                        e =>
                        {
                            this.m_cond.factToCheck.value_float = e.newValue;
                            EditorUtility.SetDirty(respData);
                        }
                    );
                }
                break;
        }
        UpdateFactField(ref stringField, ref intField, ref floatField);


        #endregion
    }

    public void UpdateFactField(ref TextField stringField,ref IntegerField intField, ref FloatField floatField) {

        switch (this.m_cond.factToCheck.varType)
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

    private void RemoveCriteriaOnClick()
    {
        m_rule.conditions.Remove(m_cond);
        m_rootRule.UpdateCriteriaList();
    }

}