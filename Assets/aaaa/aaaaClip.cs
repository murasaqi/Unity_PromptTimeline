using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[Serializable]
public class aaaaClip : PlayableAsset, ITimelineClipAsset
{
    public aaaaBehaviour template = new aaaaBehaviour ();

    public ClipCaps clipCaps
    {
        get { return ClipCaps.None; }
    }

    public override Playable CreatePlayable (PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<aaaaBehaviour>.Create (graph, template);
        aaaaBehaviour clone = playable.GetBehaviour ();
        return playable;
    }
}
