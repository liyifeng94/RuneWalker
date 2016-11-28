using UnityEngine;
using System.Collections;

public class EnemySoundEvents : MonoBehaviour {

    public AudioClip AttackSound;
    public AudioClip WalkingSound;
    public AudioClip DeathSound;

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
}
