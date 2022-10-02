using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
//using Sirenix.OdinInspector;

public class EnableExtension : TweenAddition
{
    public UnityEvent _OnEnable;
    public UnityEvent _OnEnableEffectEnd;
    public UnityEvent _OnDisable;
    public UnityEvent _OnDisableEffectEnd;

    Vector3 initialScale;
    [HideInInspector] public bool sequenced = false;
    public Ease easing = Ease.OutElastic;

    public bool useDelay = false;
    //[MinMaxSlider(0,10),ShowIf("@useDelay == true")]
    public Vector2 delay = Vector2.zero;

    public bool effectScale = true;

    //[ShowIf("@effectScale == true")]
    public bool customScale = false;

    //[ShowIf("@customScale == true")]
    public Vector3 startScale, endScale;

    private void Awake()
    {
        initialScale = transform.localScale;
    }
    private void Start()
    {
        if (tweenType == TweenAdditionType.Start) TweenEffect();
    }

    private void OnEnable()
    {
        _OnEnable.Invoke();
        switch (tweenType)
        {
            case TweenAdditionType.Enable:
                TweenEffect();
                break;

            case TweenAdditionType.Sequence:
                _OnEnableEffectEnd.Invoke();
                break;

            case TweenAdditionType.SequenceEnable:
                if (sequenced) { 
                    TweenEffect(); 
                }
                break;
        }
    }
    private void OnDisable()
    {
        _OnDisable?.Invoke();
        tween?.Kill();
        
    }
    //[Button("TweenDisable")]
    [ContextMenu("TweenDisable")]
    public void TweenDisable()
    {
        _OnDisable?.Invoke();

        Tween tw = TweenEffect(endScale,startScale);
        tw.OnComplete(() =>
        {
            _OnDisableEffectEnd?.Invoke();
            gameObject.SetActive(false);
        });
        
    }

    public override Tween TweenEffect()
    {
        if (effectScale)
        {
            tween.Kill();
            transform.localScale = Vector3.zero;
            Vector3 targetScale = initialScale;
            if (customScale)
            {
                transform.localScale = startScale;
                targetScale = endScale;
            }

            if (useDelay)
            {
                tween = transform.DOScale(targetScale, duration)
                .SetDelay(Random.Range(delay.x, delay.y))
                .SetEase(easing)
                .OnComplete(() =>
                {
                    _OnEnableEffectEnd.Invoke();
                });
            }
            else
            {
                tween = transform.DOScale(targetScale, duration)
                .SetEase(easing)
                .OnComplete(() =>
                {
                    _OnEnableEffectEnd.Invoke();
                })
                ;
            }
        }
        if (tweenType == TweenAdditionType.SequenceEnable) sequenced = true;

        return tween;
    }

    public virtual Tween TweenEffect(Vector3 from,  Vector3 to)
    {
        if (effectScale)
        {
            tween.Kill();
            transform.localScale = from;
            Vector3 targetScale = to;

            if (useDelay)
            {
                tween = transform.DOScale(targetScale, duration)
                .SetDelay(Random.Range(delay.x, delay.y))
                .SetEase(easing)
                .OnComplete(() =>
                {
                    _OnEnableEffectEnd.Invoke();
                });
            }
            else
            {
                tween = transform.DOScale(targetScale, duration)
                .SetEase(easing)
                .OnComplete(() =>
                {
                    _OnEnableEffectEnd.Invoke();
                })
                ;
            }
        }
        if (tweenType == TweenAdditionType.SequenceEnable) sequenced = true;

        return tween;
    }

    public void CallTween()
    {
        if (effectScale)
        {
            tween.Kill();
            transform.localScale = Vector3.zero;
            Vector3 targetScale = initialScale;
            if (customScale)
            {
                transform.localScale = startScale;
                targetScale = endScale;
            }

            if (useDelay)
            {
                tween = transform.DOScale(targetScale, duration)
                .SetDelay(Random.Range(delay.x, delay.y))
                .SetEase(easing)
                .OnComplete(() =>
                {
                    _OnEnableEffectEnd.Invoke();
                });
            }
            else
            {
                tween = transform.DOScale(targetScale, duration)
                .SetEase(easing)
                .OnComplete(() =>
                {
                    _OnEnableEffectEnd.Invoke();
                })
                ;
            }
        }
        if (tweenType == TweenAdditionType.SequenceEnable) sequenced = true;

    }
}
