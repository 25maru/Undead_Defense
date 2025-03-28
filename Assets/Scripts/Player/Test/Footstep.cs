using UnityEngine;

/// <summary>
/// 발소리 재생을 위한 임시 클래스입니다.
/// </summary>
public class Footstep : MonoBehaviour
{
    [Range(0, 1)]
    [SerializeField] private float footstepAudioVolume = 0.5f;
    [SerializeField] private AudioClip[] footstepAudioClips;
    [SerializeField] private CharacterController _controller;

    // 걷기 & 달리기 애니메이션의 이벤트 수신
    private void OnFootstep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            if (footstepAudioClips.Length > 0)
            {
                var index = Random.Range(0, footstepAudioClips.Length);
                AudioSource.PlayClipAtPoint(footstepAudioClips[index], transform.TransformPoint(_controller.center), footstepAudioVolume);
            }
        }
    }
}