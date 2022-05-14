using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Mechanics;

public class PlayerDrag : MonoBehaviour
{
    public float force = 100;

    private Vector3 _clickPosition;
    private Vector3 _spritePosition;
    private Vector3 _deltaPosition;

    private SpriteRenderer _spriteRenderer;
    private PlayerController _rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _rigidbody = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        if (Input.GetMouseButtonDown(0))
        {
            _clickPosition = mousePosition;
            _spritePosition = _spriteRenderer.transform.localPosition;
        }
        if (Input.GetMouseButton(0))
        {
            _deltaPosition = mousePosition - _clickPosition;
        }
        else
        {
            _deltaPosition = Vector3.Lerp(_deltaPosition, Vector3.zero, 0.2f);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("jump: " + _deltaPosition);
            _rigidbody.force = (_deltaPosition * force);
            _rigidbody.jumpState = PlayerController.JumpState.PrepareToJump;
        }
        _spriteRenderer.transform.localPosition = _spritePosition + _deltaPosition;
    }
}
