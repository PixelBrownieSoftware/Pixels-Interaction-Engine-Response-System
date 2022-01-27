using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation3D.ResponseSystem;

[System.Serializable]
public class s_speechContext {
    public Guid ID = Guid.NewGuid();
    public int priority;
}

public class S_SpeechContextHandler : MonoBehaviour
{
    public static S_SpeechContextHandler instance;
    public List<s_speechContext> contexts = new List<s_speechContext>();

    public bool SpeechContextExists(Guid ID) {
        return contexts.Find(x => x.ID == ID) != null;
    }
    public int GetSpeechContextPriority(Guid ID)
    {
        return contexts.Find(x => x.ID == ID).priority;
    }

    public void AddSpeechContext() {
        contexts.Add(new s_speechContext());
    }
}
