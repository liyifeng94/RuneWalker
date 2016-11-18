using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb2D;
    private Animator _animator;

    public Vector3 MovementVelocity;

    public enum AttackMove
    {
        Mid,
        High,
        Low,
        NumOfMoves
    }

    public AttackMove CurrentAttackMove;

    // Use this for initialization
    protected virtual void Start ()
    {
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _rb2D = GetComponent<Rigidbody2D>();
    }
	
	// Update is called once per frame
	protected virtual void Update ()
    {
        Transform thisTransform = GetComponent<Transform>();
        thisTransform.position += MovementVelocity;
	}

    protected virtual void OnTriggerEnter2D(Collider2D colliObject)
    {
        //check if collision is with an enemy.
        if (colliObject.gameObject.tag == "Player")
        {
            GameManager.Instance.GetLevelManager().EnemyInCombat(this.gameObject);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D colliObject)
    {
        //check if collision is with an enemy.
        if (colliObject.gameObject.tag == "Player")
        {
            GameManager.Instance.GetLevelManager().EnemyExitCombat(this.gameObject);
        }
    }

    protected virtual void EnterCombat()
    {
        PlayAnimation();
    }

    public void EndCombat(bool alive)
    {
        if (alive == false)
        {
            _animator.SetTrigger("enemyDeath");
        }
    }

    public AttackMove GetAttackMove()
    {
        return CurrentAttackMove;
    }

    void PlayAnimation()
    {
        switch (CurrentAttackMove)
        {
            case AttackMove.High:
                _animator.SetTrigger("enemyHighAtk");
                break;
            case AttackMove.Low:
                _animator.SetTrigger("enemyLowAtk");
                break;
            case AttackMove.Mid:
                _animator.SetTrigger("enemyMidAtk");
                break;
            default:
                break;
        }
    }
}
