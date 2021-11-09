using System;
using System.Linq;
using PromptTimeline;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using TMPro;
using UnityEditor;
using UnityEngine.UI;




[TrackColor(0.3788042f, 0.8018868f, 0.2622522f)]
[TrackClipType(typeof(PromptTimelineClip))]
[TrackBindingType(typeof(PromptControl))]


public class PromptTimelineTrack : TrackAsset
{
    
    private ScriptPlayable<PromptTimelineMixerBehaviour> mixer;
    private PromptTimelineMixerBehaviour mixerBehaviour;
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {
        mixer = ScriptPlayable<PromptTimelineMixerBehaviour>.Create (graph, inputCount); 
        mixerBehaviour = mixer.GetBehaviour(); 
        mixerBehaviour.clips = GetClips().ToList(); 
        mixerBehaviour.director = go.GetComponent<PlayableDirector>();
        return mixer;
    }
    
    public override void GatherProperties (PlayableDirector director, IPropertyCollector driver)
    {
#if UNITY_EDITOR
        var trackBinding = director.GetGenericBinding(this) as PromptControl;
        // promptControl = trackBinding;
        // binding.defaultValue = trackBinding;
        // playableDirector.defaultValue = director;
        if (trackBinding == null)
            return;

        var serializedObject = new UnityEditor.SerializedObject (trackBinding);
        var iterator = serializedObject.GetIterator();
        while (iterator.NextVisible(true))
        {
            if (iterator.hasVisibleChildren)
                continue;

            driver.AddFromName<Text>(trackBinding.gameObject, iterator.propertyPath);
        }
#endif
        base.GatherProperties (director, driver);
    }

    public void OnValidate()
    {
        // if (mixerBehaviour != null)
        // {
        //     mixerBehaviour.maxLineWidth = maxLineWidth;
        // }
        // throw new NotImplementedException();
        
    }
}
