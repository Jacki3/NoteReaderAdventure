using UnityEngine;

public static class SoundController
{
    public enum Sound
    {
        None,
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
        CameraZoom,
        LaternOn,
        ChestOpen,
        PearlCollected,
        GateOpen,
        Purchase,
        IncorectNote,
        PillarActive,
        KeyPickup,
        DoorLocked,
        SmashCircle,
        NotationComplete,
        PlayerDeath,
        ZombieHurt,
        ZombieDeath,
        ButtonClick,
        equipCosmetic,
        FlowerUp,
        scrollPickup
    }

    public static void PlaySound(Sound sound)
    {
        //cache previous soundcontroller object - is it playing, if not then use it OR make another (pool them)
        GameObject soundGameObject = new GameObject("SoundController");
        DestroyOverTime destroyOverTime =
            soundGameObject.AddComponent<DestroyOverTime>();
        destroyOverTime.lifeTime = GetAudioClip(sound).length;
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.outputAudioMixerGroup =
            CoreGameElements.i.SFXMixer.FindMatchingGroups("SFX")[0];
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
