using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCloseKitchenDoor : MonoBehaviour
{
    private bool _isOpened = false;
    private Animator _anim;

    private void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void OpenOrClose()
    {
        _isOpened = !_isOpened;
        _anim.SetBool("isOpened", _isOpened);
    }
}
