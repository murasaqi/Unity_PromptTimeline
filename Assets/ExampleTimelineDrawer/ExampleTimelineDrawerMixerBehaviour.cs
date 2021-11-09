using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using PromptTimeline;

public class ExampleTimelineDrawerMixerBehaviour : PlayableBehaviour
{
    // NOTE: This function is called at runtime and edit time.  Keep that in mind when setting the values of properties.
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        PromptControl trackBinding = playerData as PromptControl;

        if (!trackBinding)
            return;

        int inputCount = playable.GetInputCount ();

        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            ScriptPlayable<ExampleTimelineDrawerBehaviour> inputPlayable = (ScriptPlayable<ExampleTimelineDrawerBehaviour>)playable.GetInput(i);
            ExampleTimelineDrawerBehaviour input = inputPlayable.GetBehaviour ();
            
            // Use the above variables to process each frame of this playable.
            
        }
    }
}
