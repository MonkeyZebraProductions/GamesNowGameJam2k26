using UnityEngine;

public class StartLoopingAudio : MonoBehaviour
{
    private AudioManager audioManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        foreach(Sound sound in audioManager.sounds)
        {
            if(sound.loop && !sound.mute)
            {
                audioManager.Play(sound.clipName);
            }
        }
    }
}
