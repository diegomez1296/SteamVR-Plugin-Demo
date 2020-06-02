using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class TVController : MonoBehaviour
{
    public List<VideoClip> videoClips;
    public Material lightScreen;
    public Material darkScreen;

    public int currentClip = 0;
    public int currentSpeed = 1;
    public float currentVolume = 0.5f;
    public bool volumeEnabled = true;
    public bool isWorking = false;

    private VideoPlayer videoPlayer;
    private AudioSource audioSource;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponentInChildren<VideoPlayer>();
        audioSource = GetComponentInChildren<AudioSource>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        TurnTVOff();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Power");
            Switch();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Channel(false);

        if (Input.GetKeyDown(KeyCode.RightArrow))
            Channel(true);

        if (Input.GetKeyDown(KeyCode.DownArrow))
            Volume(false);

        if (Input.GetKeyDown(KeyCode.UpArrow))
            Volume(true);

        if (Input.GetKeyDown(KeyCode.Space))
            SetSpeed();


        //Debug.Log("Video: " + videoPlayer.isPlaying);
    }

    public void Switch()
    {
        if (!isWorking)
            TurnTVOn();
        else
            TurnTVOff();
    }

    public void TurnTVOn()
    {
        isWorking = true;
        Play();
        videoPlayer.playbackSpeed = currentSpeed;
        meshRenderer.material = lightScreen;
    }
    public void TurnTVOff()
    {
        isWorking = false;
        videoPlayer.Stop();
        meshRenderer.material = darkScreen;
    }


    public void Volume(bool up)
    {
        if (!isWorking || !volumeEnabled) return;

        if(up)
        {
            audioSource.volume += 0.05f;
            currentVolume = audioSource.volume;
            if (audioSource.volume > 1) audioSource.volume = 1;
        }
        else
        {
            audioSource.volume -= 0.05f;
            currentVolume = audioSource.volume;
            if (audioSource.volume < 0) audioSource.volume = 0;
        }
    }

    public void Channel(bool next)
    {
        if (!isWorking) return;

        if (next)
        {
            currentClip++;
            Play();
        }
        else
        {
            currentClip--;
            Play();
        }
    }

    private void Play()
    {
        if (!isWorking) return;

        if (currentClip > videoClips.Count - 1)
            currentClip = 0;

        if (currentClip < 0)
            currentClip = videoClips.Count - 1;

        if (videoClips[currentClip] == null)
        {
            Debug.Log("No channel movie");
            currentClip = 0;
        }

        if (videoClips[currentClip] != null)
        {
            videoPlayer.clip = videoClips[currentClip];
            videoPlayer.Play();
        }
    }

    public void SetSpeed()
    {
        if (!isWorking) return;

        if(currentSpeed != 1)
        {
            audioSource.volume = currentVolume;
            volumeEnabled = true;
            currentSpeed = 1;
        }
        else
        {
            audioSource.volume = 0;
            volumeEnabled = false;
            currentSpeed = 3;
        }

        videoPlayer.playbackSpeed = currentSpeed;
    }
}
