using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation3D.ResponseSystem;

public class O_SpeechContext : MonoBehaviour
{
    private o_dialogueSpeaker speaker;
    private List<o_dialogueSpeaker> particpiants = new List<o_dialogueSpeaker>();

    public void RemoveAllParticipants() {
        particpiants.ForEach(x => x.StopTalking());
        particpiants.ForEach(x => x.speechContext = null);
        particpiants.Clear();
    }

    public void Dispose() {
        RemoveAllParticipants();
        Destroy(gameObject);
    }

    public void SetSpeaker(o_dialogueSpeaker dialogueSp) {
        speaker = dialogueSp;
    }

    public void AddParticipants(string speakerN, string[] participants)
    {
        GameObject.Find(speakerN).TryGetComponent(out o_dialogueSpeaker oDiaSpk);
        speaker = oDiaSpk;
        speaker.speechContext = this;
        particpiants.Add(speaker);
        foreach (var m_part in participants) {
            GameObject.Find(m_part).TryGetComponent(out o_dialogueSpeaker oDiaPart);
            oDiaPart.speechContext = this;
            particpiants.Add(oDiaPart);
        }
    }

    public bool CanInterrupt(int priority) {
        if (speaker.current_dialogue.priority <= priority) {
            return true;
        }
        return false;
    }
}
