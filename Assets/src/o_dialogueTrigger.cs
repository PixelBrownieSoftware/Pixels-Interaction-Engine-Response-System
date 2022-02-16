using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class o_dialogueTrigger : MonoBehaviour
{
    public string eventName;
    public string characterName;
    public string[] participantNames;
    //TODO: Use object pooler instead
    public O_SpeechContext speechContextPrefab;

    BoxCollider2D bx;

    public List<s_factEntry> prompts = new List<s_factEntry>();

    private void Start()
    {
        bx = GetComponent<BoxCollider2D>();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pt = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (bx != null)
            {
                if (bx.OverlapPoint(pt))
                {
                    GameObject ob = GameObject.Find(characterName);
                    if (ob != null) {
                        ob.TryGetComponent(out o_dialogueSpeaker speaker);
                        if (speaker != null)
                        {
                            if (speaker.SayDialogue(eventName, prompts, false))
                            {
                                O_SpeechContext sCtx = Instantiate(speechContextPrefab);
                                sCtx.AddParticipants(characterName, participantNames);
                            }
                        }
                    }
                }

            }
        }
    }
}
