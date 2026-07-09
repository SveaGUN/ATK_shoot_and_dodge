using UnityEngine;
using UnityEngine.Timeline;

namespace AkaneTools.BulletHell.Timeline
{
    //Timeline‚É•\Ž¦‚·‚éTrack
    [TrackColor(0.89f, 0.45f, 0.6f)]
    [TrackClipType(typeof(BulletPatternClip))]
    [TrackBindingType(typeof(Transform))]//”­ŽËŒ³
    public class BulletPatternTrack : TrackAsset
    {

    }
}
