using UnityEngine;
using UnityEngine.Timeline;

namespace AkaneTools.Timeline
{
    [TrackColor(0.458f, 0.568f, 0.470f)]
    [TrackClipType(typeof(TransformTweenClip))]
    [TrackBindingType(typeof(Transform))]
    public class TransformTweenTrack : PlayableTrack
    {

    }
}