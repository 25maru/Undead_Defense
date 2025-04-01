using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Move,
    Attack,
    Death
}

public class MonsterSound : MonoBehaviour
{
    [Header("Sound")]
    [SerializeField] private List<Sound> clips;
    [Range(0f, 1f)]
    [SerializeField] private float volume = 0.5f;
    

    public void PlaySound(SoundType soundType)
    {
        foreach (var clip in clips)
        {
            if(clip.type == soundType)
                AudioSource.PlayClipAtPoint(clip.clip, transform.position, volume);
        }
        
    }
}

[Serializable]
public class Sound
{
    public SoundType type;
    public AudioClip clip;
}
