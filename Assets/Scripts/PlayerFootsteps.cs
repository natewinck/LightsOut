using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerFootsteps : MonoBehaviour {

  [SerializeField] private GameObject m_CurrentFloor;

  // Used with the Character Controller
  void OnControllerColliderHit(ControllerColliderHit hit)
  {
    //Debug.Log ("blahhhhh");
    if (hit.gameObject.CompareTag("Floor") && m_CurrentFloor != hit.gameObject)
    {
      // So we don't keep changing the audio clips when we're on the same floor,
      // Store the object as a reference
      m_CurrentFloor = hit.gameObject;

      //Debug.Log ("Hey that's a wall");
      // Get the audio source from the floor
      List<AudioClip> clips;
      clips = hit.gameObject.GetComponent<SoundBank> ().Draw (2);

      // Replace the clip in the first person controller for feet
      GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().m_FootstepSounds = clips.ToArray();

      // That's it
    }
  }
}
