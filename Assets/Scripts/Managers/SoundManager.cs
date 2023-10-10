using System;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class SoundManager
{
    AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    public AudioSource[] AudioSources { get { return _audioSources; } }
    Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();
    float _volume = 1.0f;
    public float Volume { get { return _volume; } set { _volume = value; PlayerPrefs.SetFloat("Volume", value); } } 
    #region Init
    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            UnityEngine.Object.DontDestroyOnLoad(root);

            _volume = PlayerPrefs.GetFloat("Volume", 1.0f);

            for (Define.Sound s = Define.Sound.Play0; s < Define.Sound.MaxCount; s++)
            {
                GameObject go = new GameObject { name = $"{s}" };
                _audioSources[(int)s] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
                _audioSources[(int)s].volume = Volume;
            }

            //GetOrAddAudioClip 전체 로딩
            string[] soundtrackType = Enum.GetNames(typeof(SoundtrackType));
            for (int i = 0; i < (int)SoundtrackType.MaxCount; i++)
            {
                GetOrAddAudioClip(soundtrackType[i]);
            }

          
        }
        
    }

    #endregion Init

    #region Play

    /// <summary>
    /// SFX용 PlayOneShot으로 구현 
    /// </summary>
    /// <param name="SFXSound"> Define.SFX Enum 에서 가져오기를 바람 </param>
    /// <param name="volume"></param>

    public void Play(Define.SFX SFXSound, float volume = 1.0f)
    {
        string path = $"{SFXSound}";
        AudioClip audioClip = GetOrAddAudioClip(path, Define.Sound.SFX);
        Play(audioClip, Define.Sound.SFX, volume);
    }

    public void Play(int audioIdx, Define.SoundtrackType BGMSound, float volume = 1.0f)
    {
        Define.Sound player = (Define.Sound)audioIdx;

        string path = $"{BGMSound}";
        AudioClip audioClip = GetOrAddAudioClip(path, player);
        Play(audioClip, player, volume);
    }
    
    void Play(AudioClip audioClip, Define.Sound type = Define.Sound.SFX, float volume = 1.0f)
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.SFX)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.SFX];
            audioSource.volume = volume;
            audioSource.PlayOneShot(audioClip);

        }
        else
        {
            AudioSource audioSource = _audioSources[(int)type];
            //if (audioSource.isPlaying)
            //    audioSource.Stop();

            audioSource.volume = volume;
            audioSource.clip = audioClip;
            audioSource.PlayOneShot(audioClip);
            //audioSource.Play();
        }
    }
    #endregion Play
    AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.SFX)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip = null;

        if (_audioClips.TryGetValue(path, out audioClip) == false)
        {
            audioClip = GameManager.Resource.Load<AudioClip>(path);
            _audioClips.Add(path, audioClip);
        }
        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }

    public void SetVolume(Define.Sound type, float volume )
    {
        _audioSources[(int)type].volume = volume;
    }
    public float GetVolume(Define.Sound type)
    {
        return _audioSources[(int)type].volume;
    }

    public void Clear()
    {
        foreach (AudioSource audioSource in _audioSources)
        {
            audioSource.clip = null;
            audioSource.Stop();
        }
        _audioClips.Clear();
    }
}
