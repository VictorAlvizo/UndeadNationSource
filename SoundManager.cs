using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager instance = null;

    public AudioClip zombieGrunt;
    public AudioClip damageTaken;
    public AudioClip pickUp;
    public AudioClip bulletFire;
    public AudioClip reload;
    public AudioClip emptyGun;
    public AudioClip itemDrop;
    public AudioClip moreHealth;
    public AudioClip gameOver;
    public AudioClip coryHurt;

    private AudioSource soundEffectAudio;

	void Start () {
		if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        AudioSource theSource = GetComponent<AudioSource>();
        soundEffectAudio = theSource;
	}

    public void PlayOneShot(AudioClip clip)
    {
        soundEffectAudio.PlayOneShot(clip);
    }
}
