using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using PromptTimeline;
using TMPro;
using UnityEngine.UI;

public class PromptTimelineMixerBehaviour : PlayableBehaviour
{
    
    public List<TimelineClip> clips { get; set; }
    public PlayableDirector director { get; set; }
    
    private PromptControl m_TrackBinding;
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        m_TrackBinding = playerData as PromptControl;

        if (!m_TrackBinding)
            return;

        int inputCount = playable.GetInputCount ();

        
        Debug.Log(clips.Count);
        var pos = Vector2.zero;
        // ToggleSubtitle(false);
        for (int i = 0; i < inputCount; i++)
        {
            
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<PromptTimelineBehaviour> inputPlayable = (ScriptPlayable<PromptTimelineBehaviour>)playable.GetInput(i);
            PromptTimelineBehaviour input = inputPlayable.GetBehaviour ();
            var clip = clips[i];
            if(inputWeight > 0)
            {
                pos += inputWeight * input.position;
            }
            
            
            
        }
        
        m_TrackBinding.SetPosition(-pos);
    }
    
 
}
