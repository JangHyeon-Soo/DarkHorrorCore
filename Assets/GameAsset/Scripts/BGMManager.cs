using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public AudioClip[] bgm;
    public AudioSource bgmSource;

    private void Start()
    {
        if(bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            //bgmSource = GetComponent<AudioSource>();
            
        }
    }

    private void Update()
    {
        if(bgmSource.isPlaying == false)
        {
            int index = Random.Range(0, bgm.Length);
            bgmSource.PlayOneShot(bgm[index]);   
        }
    }
}
