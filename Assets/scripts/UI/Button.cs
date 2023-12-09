using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Button))]

public class Button : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] AudioClip hoverClip;
    [SerializeField] AudioClip clickClip;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (hoverClip)
        {
            audioSource.clip = hoverClip;
            audioSource.volume = 0.8f;
            audioSource.Play();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (clickClip)
        {
            audioSource.clip = clickClip;
            audioSource.Play();
        }
    }
}
