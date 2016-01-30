using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class RoomTrigger : MonoBehaviour {
    public AudioMixerSnapshot inside;
    public AudioMixerSnapshot outside;
    
    void OnTriggerEnter (Collider c)
    {
        inside.TransitionTo(.25f);

    }
    void OnTriggerExit(Collider c)
    {
        outside.TransitionTo(.25f);

    }
}
