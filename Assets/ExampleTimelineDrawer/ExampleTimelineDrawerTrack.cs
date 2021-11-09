using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using PromptTimeline;

[TrackColor(0.855f, 0.8623f, 0.87f)]
[TrackClipType(typeof(ExampleTimelineDrawerClip))]
[TrackBindingType(typeof(PromptControl))]
public class ExampleTimelineDrawerTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        return ScriptPlayable<ExampleTimelineDrawerMixerBehaviour>.Create (graph, inputCount);
    }
}
