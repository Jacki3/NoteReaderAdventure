using UnityEngine;

public class CoreSoundElements : MonoBehaviour
{
    private static CoreSoundElements _i;

    public static CoreSoundElements i
    {
        get
        {
            return _i;
        }
    }

    private void Awake()
    {
        if (_i != null && _i != this)
            Destroy(this.gameObject);
        else
            _i = this;
    }

    public SoundAudioClip[] soundAudioClips;

    [System.Serializable]
    public class SoundAudioClip
    {
        public SoundController.Sound sound;

        public AudioClip audioClip;
    }
}
