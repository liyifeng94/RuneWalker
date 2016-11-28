using UnityEngine;
using System.Collections;

public class PlayerSoundEvents : MonoBehaviour
{
    public AudioClip AttackSound;
    public AudioClip WalkingSound;
    public AudioClip DeathSound;
    public AudioClip SpecialSound;

    private Transform _transform;

    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    public void OnAttackEvent()
    {
        AudioSource.PlayClipAtPoint(AttackSound, _transform.position);
    }

    public void OnWalkingEvent()
    {
        AudioSource.PlayClipAtPoint(WalkingSound, _transform.position);
    }

    public void OnDeathEvent()
    {
        AudioSource.PlayClipAtPoint(DeathSound, _transform.position);
    }

    public void OnSpecialEvent()
    {
        AudioSource.PlayClipAtPoint(SpecialSound, _transform.position);
    }
}
