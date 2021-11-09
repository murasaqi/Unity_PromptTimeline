using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using PromptTimeline;

[Serializable]
public class PromptTimelineBehaviour : PlayableBehaviour
{
    public Color textColor = Color.white;
    public Color backgroundColor = new Color(0,0,0,0.8f);
    public string prompt;
    public int startIndex;
    public int endIndex;
    public Vector2 position = Vector2.zero;
    public int promptIndex = 0;
    public List<string> lines =new List<string>(){"a","b","c"};
    public override void OnPlayableCreate (Playable playable)
    {
        
    }
}
