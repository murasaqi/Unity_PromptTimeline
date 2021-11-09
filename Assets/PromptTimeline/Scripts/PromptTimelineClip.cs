using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using PromptTimeline;

[Serializable]
public class PromptTimelineClip : PlayableAsset, ITimelineClipAsset
{
    public PromptTimelineBehaviour template = new PromptTimelineBehaviour ();
    public ClipCaps clipCaps
    {
        get { return ClipCaps.Blending; }
    }


    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        
        // template.lines = lines;
        // template.prompt = prompt;
        // template.startIndex = startIndex;
        // template.endIndex = endIndex;
        var playable = ScriptPlayable<PromptTimelineBehaviour>.Create (graph, template);
        PromptTimelineBehaviour clone = playable.GetBehaviour ();
        
        
        
        // prompt = clone.prompt;
        return playable;
        
        
    }
    
}
