using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTrackMangament : MonoBehaviour
{
    [SerializeField] AudioSource audoSorceToSoundTracks;
    [SerializeField] AudioSource audoSorceToUISounds;
    [SerializeField] AudioClip[] soundTracks;
    [SerializeField] AudioClip[] soundsToUI;
    int curretNumberSoundTrack;
    int numberDrawnSoundTrack;
    // Start is called before the first frame update
    void Start()
    {
        audoSorceToSoundTracks.volume = 0.30f;
        audoSorceToUISounds.clip = soundsToUI[0];
        DrawSoundTrack();
        SoundTracksMangament();
    }

    // Update is called once per frame
    void Update()
    {
        SoundTracksMangament();
    }

    void SoundTracksMangament()
    {
        if (audoSorceToSoundTracks.isPlaying == false)
        {
            DrawSoundTrack();

            audoSorceToSoundTracks.Stop();
            audoSorceToSoundTracks.clip = soundTracks[curretNumberSoundTrack];
            audoSorceToSoundTracks.Play();
        }
    }

    void DrawSoundTrack()
    {
        numberDrawnSoundTrack = (int)Random.Range(0, soundTracks.Length - 1);
        while (numberDrawnSoundTrack == curretNumberSoundTrack)
        {
            numberDrawnSoundTrack = (int)Random.Range(0, soundTracks.Length - 1);
        }
        curretNumberSoundTrack = numberDrawnSoundTrack;
    }
}
