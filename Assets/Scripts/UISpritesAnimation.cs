using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
 
[RequireComponent(typeof(Image))]
public class UISpritesAnimation : MonoBehaviour
{
    private Animator animator;
    private Image image;
    private List<Sprite> sprites;
    
    [SerializeField] private int spritePerFrame;
    private int index = 0;
    private int frame = 0;
    public bool loop = true;
    public bool destroyOnEnd = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        image = GetComponent<Image>();
        sprites = GetSpritesFromAnimator(animator);
        Debug.Log(sprites.Count);
    }

    private void Update () {
        if (!loop && index == sprites.Count) return;
        frame ++;
        if (frame < spritePerFrame) return;
        image.sprite = sprites [index];
        frame = 0;
        index ++;
        
        if (index >= sprites.Count) {
            if (loop) index = 0;
            if (destroyOnEnd) Destroy (gameObject);
        }
    }

    private List<Sprite> GetSpritesFromAnimator(Animator anim)
    {
        List<Sprite> _allSprites = new List<Sprite> ();
        foreach(AnimationClip ac in anim.runtimeAnimatorController.animationClips)
        {
            _allSprites.AddRange(GetSpritesFromClip(ac));
        }
        return _allSprites;
    }
 
    private List<Sprite> GetSpritesFromClip(AnimationClip clip)
    {
        var _sprites = new List<Sprite> ();
        if (clip != null)
        {
            foreach (var binding in AnimationUtility.GetObjectReferenceCurveBindings (clip))
            {
                ObjectReferenceKeyframe[] keyframes = AnimationUtility.GetObjectReferenceCurve (clip, binding);
                foreach (var frame in keyframes) {
                    _sprites.Add ((Sprite)frame.value);
                }
            }
        }
        return _sprites;
    }
}