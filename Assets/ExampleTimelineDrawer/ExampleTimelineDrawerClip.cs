using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using PromptTimeline;

[Serializable]
public class ExampleTimelineDrawerClip : PlayableAsset, ITimelineClipAsset
{
    public ExampleTimelineDrawerBehaviour template = new ExampleTimelineDrawerBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.All; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<ExampleTimelineDrawerBehaviour>.Create (graph, template);
        ExampleTimelineDrawerBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
