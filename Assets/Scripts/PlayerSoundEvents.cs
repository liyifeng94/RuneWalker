using UnityEngine;
using System.Collections;

public class PlayerSoundEvents : MonoBehaviour
{
    public AudioClip AttackSound;
	public AudioClip DodgeSound;
    public AudioClip Walking1Sound;
	public AudioClip Walking2Sound;
	public AudioClip Walking3Sound;
	public AudioClip Walking4Sound;
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

	public void OnDodgeEvent()
	{
		AudioSource.PlayClipAtPoint(DodgeSound, _transform.position);
	}

    public void OnWalking1Event()
    {
        AudioSource.PlayClipAtPoint(Walking1Sound, _transform.position);
    }

	public void OnWalking2Event()
	{
		AudioSource.PlayClipAtPoint(Walking2Sound, _transform.position);
	}

	public void OnWalking3Event()
	{
		AudioSource.PlayClipAtPoint(Walking3Sound, _transform.position);
	}

	public void OnWalking4Event()
	{
		AudioSource.PlayClipAtPoint(Walking4Sound, _transform.position);
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
