using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public enum TrackID
    {
        Start = 0,
        Overworld,
        Battle
    }

    [SerializeField]
    List<AudioClip> musicTracks;

    [SerializeField]
    AudioSource musicSource;
    private static MusicManager instance = null;
    private MusicManager() { }
    public static MusicManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MusicManager>();
            }
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    void Start()
    {
        PlayerEvents events = FindObjectOfType<PlayerEvents>();
        events.onEnterEncounterEvent.AddListener(OnEnterEncounterHandler);
        events.onExitEncounterEvent.AddListener(OnExitEncounterHandler);
        events.onEnterMainEvent.AddListener(OnEnterMainHandler);
        DontDestroyOnLoad(Instance.transform.root);
        Instance.PlayTrack(TrackID.Start);
    }
    private void OnEnterMainHandler()
    {
        Instance.PlayTrack(TrackID.Overworld);
    }
    private void OnEnterEncounterHandler()
    {
        Debug.Log("changed music");
        Instance.PlayTrack(TrackID.Battle);
    }
    private void OnExitEncounterHandler()
    {
        StartCoroutine(FadeInOverDuration(TrackID.Overworld, 5f));
    }
    public void PlayTrack(TrackID id)
    {
        if (id == TrackID.Battle)
        {
            musicSource.volume = 0.5f;
        }
        else
            musicSource.volume = 1;
        musicSource.clip = musicTracks[(int)id];
        musicSource.Play();
    }
    IEnumerator FadeInOverDuration(TrackID id, float duration)
    {
        Instance.PlayTrack(id);
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            float fadeValue = timer / duration;
            musicSource.volume = Mathf.SmoothStep(0.0f, 1.0f, fadeValue);
            yield return new WaitForEndOfFrame();
        }
    }

}
