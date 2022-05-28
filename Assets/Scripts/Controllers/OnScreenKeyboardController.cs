using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnScreenKeyboardController : MonoBehaviour
{
    public Key[] keys;

    [System.Serializable]
    public class Key
    {
        public CoreUIElements.ImageUI keyImage;

        public Color32 offColour;

        public Color32 onColour;

        public bool on;
    }

    private void Awake()
    {
        MIDIController.NoteOn += SetKeyColourOn;
        MIDIController.NoteOff += SetKeyColourOff;
    }

    private void Start()
    {
        //set key colours to the their off colours if any were on
        for (int i = keys.Length / 2; i < keys.Length; i++)
        {
            keys[i].onColour = keys[i - keys.Length / 2].onColour;
            keys[i].offColour = keys[i - keys.Length / 2].offColour;
        }
        for (int i = 0; i < keys.Length; i++)
        {
            keys[i].keyImage.image.color = keys[i].offColour;
            keys[i].on = false;
        }
    }

    private void SetKeyColourOn(int value, float t)
    {
        value = value - MIDIController.startingMIDINumber;
        if (value >= 0 && value < keys.Length)
            keys[value].keyImage.image.color = keys[value].onColour;
    }

    private void SetKeyColourOff(int value)
    {
        value = value - MIDIController.startingMIDINumber;
        if (value >= 0 && value < keys.Length)
            keys[value].keyImage.image.color = keys[value].offColour;
        else
            return;
    }
}
