using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class s_fact
{

    public s_fact(s_factEntry f)
    {
        value_int = f.value_int;
        value_float = f.value_float;
        value_string = f.value_string;
        varType = f.varType;
    }

    public s_factEntry.COND_VAR_TYPE varType;
    public int value_int;
    public float value_float;
    public string value_string;
}

static class cond_helper
{
    public static bool IsFufiled(s_ruleEntry.r_cond cond, s_fact fact)
    {

        switch (cond.factToCheck.varType)
        {
            case s_factEntry.COND_VAR_TYPE.INT:
                {
                    int factCheckInt = cond.factToCheck.value_int;
                    switch (cond.condType)
                    {
                        case s_ruleEntry.r_cond.COND_Type.EQ:
                            return fact.value_int == factCheckInt;

                        case s_ruleEntry.r_cond.COND_Type.GR:
                            return fact.value_int > factCheckInt;

                        case s_ruleEntry.r_cond.COND_Type.GR_EQ:
                            return fact.value_int >= factCheckInt;

                        case s_ruleEntry.r_cond.COND_Type.LS:
                            return fact.value_int < factCheckInt;

                        case s_ruleEntry.r_cond.COND_Type.LS_EQ:
                            return fact.value_int <= factCheckInt;

                    }
                }
                break;

            case s_factEntry.COND_VAR_TYPE.FLOAT:
                {
                    float factCheckFloat = cond.factToCheck.value_float;
                    switch (cond.condType)
                    {
                        case s_ruleEntry.r_cond.COND_Type.EQ:
                            return fact.value_float == factCheckFloat;

                        case s_ruleEntry.r_cond.COND_Type.GR:
                            return fact.value_float > factCheckFloat;

                        case s_ruleEntry.r_cond.COND_Type.GR_EQ:
                            return fact.value_float >= factCheckFloat;

                        case s_ruleEntry.r_cond.COND_Type.LS:
                            return fact.value_float < factCheckFloat;

                        case s_ruleEntry.r_cond.COND_Type.LS_EQ:
                            return fact.value_float <= factCheckFloat;

                    }
                }
                break;

            case s_factEntry.COND_VAR_TYPE.STRING:
                return fact.value_string == cond.factToCheck.value_string;
        }
        return false;
    }
}

public class o_dialogueSpeaker : MonoBehaviour
{
    private Dictionary<string, s_ruleEntry[]> m_rules = new Dictionary<string, s_ruleEntry[]>();
    private Dictionary<string, s_fact> m_facts = new Dictionary<string, s_fact>();
    [SerializeField]
    private s_RespDatabase m_responseSystem;
    public O_SpeechContext speechContext;

    public s_ruleEntry current_dialogue { get; private set; }
    public GameObject speechBubble;
    private TMPro.TextMeshProUGUI txt;
    public Guid speechID;

    private void Start()
    {
        if (speechBubble != null)
        {
            txt = speechBubble.transform.Find("Text (TMP)").GetComponent<TMPro.TextMeshProUGUI>();
            speechBubble.gameObject.SetActive(false);
        }
        List<s_ruleEntry> tempRules = m_responseSystem.rules.ToList();
        HashSet<string> concepts = new HashSet<string>();
        foreach (var resp in tempRules)
        {
            if (resp.conditions.Find(x => x.factToCheck.fact_name == "concept") != null)
                concepts.Add(resp.conditions.Find(x => x.factToCheck.fact_name == "concept").factToCheck.value_string);
        }
        foreach (string concept in concepts)
        {
            s_ruleEntry[] rules = tempRules.FindAll(x => x.FindConditionWithConcept(concept)).ToArray();
            m_rules.Add(concept, rules);
        }

        foreach (var fact in m_responseSystem.facts)
        {
            AddFact(fact);
        }
    }

    public void ChangeFact(s_ruleEntry.r_set setter)
    {

        switch (m_facts[setter.factToCheck.fact_name].varType)
        {
            case s_factEntry.COND_VAR_TYPE.INT:
                switch (setter.changeType)
                {
                    case s_ruleEntry.r_set.CHANGE_TYPE.DEC:

                        break;
                    case s_ruleEntry.r_set.CHANGE_TYPE.INC:

                        break;
                    case s_ruleEntry.r_set.CHANGE_TYPE.SET:
                        m_facts[setter.factToCheck.fact_name].value_int = setter.factToCheck.value_int;
                        break;
                    case s_ruleEntry.r_set.CHANGE_TYPE.SUB:
                        m_facts[setter.factToCheck.fact_name].value_int -= setter.factToCheck.value_int;
                        break;
                    case s_ruleEntry.r_set.CHANGE_TYPE.ADD:
                        m_facts[setter.factToCheck.fact_name].value_int += setter.factToCheck.value_int;
                        break;
                }
                break;

            case s_factEntry.COND_VAR_TYPE.FLOAT:
                switch (setter.changeType)
                {
                    case s_ruleEntry.r_set.CHANGE_TYPE.DEC:
                        m_facts[setter.factToCheck.fact_name].value_float--;
                        break;
                    case s_ruleEntry.r_set.CHANGE_TYPE.INC:
                        m_facts[setter.factToCheck.fact_name].value_float++;
                        break;
                    case s_ruleEntry.r_set.CHANGE_TYPE.SET:
                        m_facts[setter.factToCheck.fact_name].value_float = setter.factToCheck.value_float;
                        break;
                    case s_ruleEntry.r_set.CHANGE_TYPE.SUB:
                        m_facts[setter.factToCheck.fact_name].value_float -= setter.factToCheck.value_float;
                        break;
                    case s_ruleEntry.r_set.CHANGE_TYPE.ADD:
                        m_facts[setter.factToCheck.fact_name].value_float += setter.factToCheck.value_float;
                        break;
                }
                break;

            case s_factEntry.COND_VAR_TYPE.STRING:
                m_facts[setter.factToCheck.fact_name].value_string = setter.factToCheck.value_string;
                break;
        }
    }


    public bool SayDialogue(string eventName, List<s_factEntry> query, bool isFollowUp = true)
    {

        s_ruleEntry[] rules = m_rules[eventName];

        int findRuleWithMostConditions(s_ruleEntry rul)
        {
            int conditionAmounts = 0;
            foreach (var cond in rul.conditions)
            {

                s_fact fac = null;

                switch (cond.objType)
                {
                    case s_ruleEntry.r_cond.COND_OBJ_TYPE.SELF:
                        if (m_facts.ContainsKey(cond.factToCheck.fact_name))
                            fac = m_facts[cond.factToCheck.fact_name];
                        else
                            return -1;
                        break;
                    case s_ruleEntry.r_cond.COND_OBJ_TYPE.QUERY:
                        if (cond.factToCheck.fact_name == "concept")
                            continue;
                        fac = new s_fact(query.Find(x => x.fact_name == cond.factToCheck.fact_name));
                        break;
                }

                bool condChecked = cond_helper.IsFufiled(cond, fac);
                if (!condChecked)
                {
                    return -1;
                }
                else
                {
                    conditionAmounts++;
                }
            }
            return conditionAmounts;
        }

        s_ruleEntry bestDialgoueRule = null;
        int bestConditionScore = 0;
        foreach (var rule in rules)
        {
            int currentRuleScoreFind = findRuleWithMostConditions(rule);
            if (currentRuleScoreFind >= bestConditionScore)
            {
                bestDialgoueRule = rule;
                bestConditionScore = currentRuleScoreFind;
            }
        }
        if (bestDialgoueRule == null)
        {
            return false;
        }
        else
        {
            if (speechContext != null)
            {
                //Response is rejected because the speaker's response has a higher priority.
                if (!isFollowUp)
                {
                    if (!speechContext.CanInterrupt(bestDialgoueRule.priority))
                        return false;
                    else
                        speechContext.Dispose();
                }
            }
            /*
            if (S_SpeechContextHandler.instance.SpeechContextExists(speechID)) {

                if (bestDialgoueRule.priority <= S_SpeechContextHandler.instance.GetSpeechContextPriority(speechID))
            }
            */
            if (current_dialogue != null)
            {
                if (bestDialgoueRule.priority <= current_dialogue.priority)
                    return false;
            }
            if (speechContext != null)
                speechContext.SetSpeaker(this);
            current_dialogue = bestDialgoueRule;
            StartCoroutine(DialogueBubble(bestDialgoueRule));
        }

        return true;
    }

    public void StopTalking()
    {
        current_dialogue = null;
    }

    public IEnumerator DialogueBubble(s_ruleEntry dialogue)
    {

        speechBubble.gameObject.SetActive(true);
        txt.text = dialogue.m_dialogue;
        float timer = 1.5f;
        while (timer > 0)
        {
            if (dialogue != current_dialogue)
            {
                speechBubble.gameObject.SetActive(false);
                print("We're stopping this.");
                yield break;
            }
            yield return new WaitForSeconds(Time.deltaTime);
            timer -= Time.deltaTime;
        }
        speechBubble.gameObject.SetActive(false);

        if (dialogue.modifiers.Count > 0)
        {
            foreach (var modifiers in dialogue.modifiers)
            {
                ChangeFact(modifiers);
            }
        }
        current_dialogue = null;
        switch (dialogue.triggersObject)
        {
            case "":
                if (speechContext != null)
                    speechContext.Dispose();
                break;

            case "!self":
                SayDialogue(dialogue.querySendFacts.Find(x => x.fact_name == "concept").value_string, dialogue.querySendFacts);
                break;

            default:
                GameObject triggerObj = GameObject.Find(dialogue.triggersObject);
                if (triggerObj != null)
                {
                    triggerObj.TryGetComponent<o_dialogueSpeaker>(out o_dialogueSpeaker sp);
                    if (sp != null)
                        sp.SayDialogue(dialogue.querySendFacts.Find(x => x.fact_name == "concept").value_string, dialogue.querySendFacts);
                }
                break;
        }
    }

    public void AddFact(s_factEntry fac)
    {
        if (!m_facts.ContainsKey(fac.fact_name))
        {
            m_facts.Add(fac.fact_name, new s_fact(fac));
        }
    }

}

namespace MagnumFoundation3D
{
    namespace ResponseSystem
    {
    }
}