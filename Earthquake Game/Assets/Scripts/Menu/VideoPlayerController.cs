using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour {

    public VideoClip movie;
    public bool playOnAwake = true, muteMusic = true;
    private AudioSource audioSource;
    private VideoPlayer player;
    private RectTransform timeIndicator, panel;

    private AudioSource music;
    private float musicVolume;

    private Text buttonText;


    void Awake () {
        player = GetComponentInChildren<VideoPlayer>();
        player.clip = movie;

        Button playPause = GetComponentInChildren<Button>();
        playPause.onClick.AddListener(togglePlaying);
        buttonText = playPause.GetComponentInChildren<Text>();

        timeIndicator = transform.Find("TimeIndicator").GetComponent<RectTransform>();
        panel = GetComponent<RectTransform>();

        music = MusicManager.instance.GetComponent<AudioSource>();
        musicVolume = music.volume;

        setPlaying(playOnAwake);

        player.loopPointReached += Player_loopPointReached;
    }

    private void Player_loopPointReached(VideoPlayer source)
    {
        setPlaying(false);
    }

    void Update () {
		if (Input.GetKeyDown(KeyCode.Space))
        {
            togglePlaying();
        }

        Vector2 timeSize = timeIndicator.sizeDelta;
        timeSize.x = (float) (player.time / player.clip.length) * panel.sizeDelta.x;
        timeIndicator.sizeDelta = timeSize;
    }

    void togglePlaying()
    {
        setPlaying(!player.isPlaying);
    }

    void setPlaying(bool play)
    {
        if (!play)
        {
            player.Pause();
            if (muteMusic)
                music.volume = musicVolume;
            buttonText.text = "|>";
        }
        else
        {
            player.Play();
            if (muteMusic)
                music.volume = 0;
            buttonText.text = "||";
        }  
    }

    public void playVideo(VideoClip clip)
    {
        gameObject.SetActive(true);
        this.movie = clip;
        setPlaying(true);
    }


}
