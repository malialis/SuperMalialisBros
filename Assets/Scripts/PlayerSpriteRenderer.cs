using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteRenderer : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private PlayerMovement _movement;

    public Sprite idle;
    public Sprite jump;
    public Sprite slide;
    public AnimatedSprites run;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _movement = GetComponentInParent<PlayerMovement>();
    }

    private void LateUpdate()
    {
        run.enabled = _movement.running;

        if (_movement.jumping)
        {
            _spriteRenderer.sprite = jump;
        }
        else if (_movement.sliding)
        {
            _spriteRenderer.sprite = slide;
        }        
        else if(!_movement.running)
        {
            _spriteRenderer.sprite = idle;
        }
    }

    private void OnEnable()
    {
        _spriteRenderer.enabled = true;
    }

    private void OnDisable()
    {
        _spriteRenderer.enabled = false;
    }

}
