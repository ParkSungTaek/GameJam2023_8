using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    Define.Sound _soundPlayer;
    
    public Define.Sound SoundPlayer { get { return _soundPlayer; } }

    public Action StateAction { get; set; }

    State _state;

    public bool IsPlaying { get; set; } = false;

    public void StartPlayingAnim(ButtonData data = null)
    {
        if(_state != null)
        {
            
            _state.StartPlayingAnim(this, data);

        }
        else
        {
            Debug.Log(gameObject.name + " _stateNone");
        }
        //_state.NowPlayingAnim(this);
    }
    public void EndPlayingAnim()
    {
        if (_state != null)
        {
            _state.StartPlayingAnim(this);

        }
        else
        {
            Debug.Log(gameObject.name + " _stateNone");
        }
        //_state.NowPlayingAnim(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        
        switch (_soundPlayer) 
        {
            case Define.Sound.Play0:
                _state = GetComponent<Player0State>();
                break;
            case Define.Sound.Play1:
                _state = GetComponent<Player1State>();
                break;
            case Define.Sound.Play2:
                _state = GetComponent<Player2State>();
                break;
            case Define.Sound.Play3:
                _state = GetComponent<Player3State>();
                break;
            default:
                Debug.Log("Error PlayerController switch (_soundPlayer)");
                break;
        }

    }

    
}
