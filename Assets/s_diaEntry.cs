using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MagnumFoundation3D
{

    namespace ResponseSystem
    {
        public class s_baseEntry
        {
            // public int ID;
        }
        [Serializable]
        public class s_factEntry
        {
            public enum COND_VAR_TYPE
            {
                INT,
                FLOAT,
                STRING
            }
            public string fact_name;
            public COND_VAR_TYPE varType;
            public int value_int;
            public float value_float;
            public string value_string;
        }

        public class s_respEntry : s_baseEntry
        {
            public string m_dialogue;
        }

        [Serializable]
        public class s_ruleEntry
        {
            public string key;
            public string m_dialogue;
            public string m_ob_name;
            public float m_timer;
            public int priority;

            public bool FindConditionWithConcept(string concept) {
                return conditions.FindAll(x => x.factToCheck.fact_name == "concept" && x.factToCheck.value_string == concept) != null;
            }
            [Serializable]
            public class r_cond
            {
                public string m_ob_name;
                public enum COND_OBJ_TYPE
                {
                    SELF,
                    GLOBAL,
                    QUERY
                }
                public COND_OBJ_TYPE objType;
                public COND_Type condType;
                public s_factEntry factToCheck = new s_factEntry();
                public enum COND_Type
                {
                    EQ,
                    GR_EQ,
                    LS_EQ,
                    LS,
                    GR
                }
            }
            [Serializable]
            public class r_set
            {
                public string m_ob_name;
                public s_factEntry factToCheck = new s_factEntry();
                public CHANGE_TYPE changeType;
                public enum CHANGE_TYPE
                {
                    SET,
                    ADD,
                    SUB,
                    INC,
                    DEC
                }
            }
            public string eventTriggeredBy;
            public string triggersObject;
            public List<s_factEntry> querySendFacts = new List<s_factEntry>();
            public List<r_cond> conditions = new List<r_cond>();
            public List<r_set> modifiers = new List<r_set>();
            public int value_int;
        }

    }
}