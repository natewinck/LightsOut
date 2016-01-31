using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerFootsteps : MonoBehaviour {

  private GameObject m_CurrentFloor;

  // Used with the Character Controller
  void OnControllerColliderHit(ControllerColliderHit hit)
  {
    if (hit.gameObject.CompareTag("Floor") && m_CurrentFloor != hit.gameObject && hit.gameObject.GetComponent<SoundBank>() != null)
    {
      // So we don't keep changing the audio clips when we're on the same floor,
      // Store the object as a reference
      m_CurrentFloor = hit.gameObject;

      // Get the audio source from the floor
      List<AudioClip> clips;
      clips = hit.gameObject.GetComponent<SoundBank> ().DrawAll(SoundBank.FOOTSTEPS);

      // Replace the clip in the first person controller for feet
      GetComponent<UnityStandardAssets.Characters.FirstPerson.TankPersonController>().ChangeFootstepSounds(clips);

      // That's it
    }
  }
}
