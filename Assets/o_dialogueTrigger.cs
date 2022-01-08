using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MagnumFoundation3D.ResponseSystem;

public class o_dialogueTrigger : MonoBehaviour
{
    public string eventName;
    public string characterName;

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
                            speaker.SayDialogue(eventName, prompts);
                        }
                    }
                }

            }
        }
    }
}
