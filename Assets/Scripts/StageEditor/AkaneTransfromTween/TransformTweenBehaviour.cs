using UnityEngine;
using UnityEngine.Playables;


public class TransformTweenBehaviour : PlayableBehaviour
{
    public Transform StartLocation = null;
    public Transform EndLocation = null;

    public AnimationCurve EasingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {

    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        var transform = playerData as Transform;

        transform.position = Vector3.LerpUnclamped(StartLocation.position, EndLocation.position, (float)playable.GetTime());
    }
}
