using UnityEngine;

public static class SoundController
{
    public enum Sound
    {
        PlayerMove,
        PotBreak,
        BoxBreak,
        PlayerFire,
        Treasure,
        CoinPickup,
        HealthPickup,
        PlayerHurt,
        PlayerLvlUp,
        MissionComplete,
        CameraZoom
        //etc.
    }

    public static void PlaySound(Sound sound)
    {
        //cache previous soundcontroller object - is it playing, if not then use it OR make another (pool them)
        GameObject soundGameObject = new GameObject("SoundController");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (CoreSoundElements.SoundAudioClip
            soundAudioClip
            in
            CoreSoundElements.i.soundAudioClips
        )
        {
            if (soundAudioClip.sound == sound) return soundAudioClip.audioClip;
        }
        Debug.LogError("Sound" + sound + "missing!");
        return null;
    }
}
