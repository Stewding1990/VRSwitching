using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private WhitdeboardMarker m_Marker;
    [SerializeField] private WhitdeboardMarker m_Marker1;
    [SerializeField] private WhitdeboardMarker m_Marker2;
    private float SFXtimer;
    // Start is called before the first frame update
    void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
        m_Marker = GetComponent<WhitdeboardMarker>();

    }

    // Update is called once per frame
    void Update()
    {
    }
    public void PlaySound()
    {
      m_AudioSource.Play();

    }
}
