using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Response data")]
public class s_RespDatabase : ScriptableObject
{
    public List<s_ruleEntry> rules = new List<s_ruleEntry>();
    public List<s_factEntry> facts = new List<s_factEntry>();
}
