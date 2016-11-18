using UnityEngine;
using System.Collections;

public abstract class Enemy : MonoBehaviour
{
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb2D;

    private Animator _animator;

    private bool _onHold;

    public Vector3 MovementVelocity;

    public enum AttackMove
    {
        Mid,
        High,
        Low,
        NumOfMoves
    }

    private AttackMove _attackMove;

    // Use this for initialization
    protected virtual void Start ()
    {
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
        _rb2D = GetComponent<Rigidbody2D>();

        _onHold = false;
    }
	
	// Update is called once per frame
	protected virtual void Update ()
    {
        if (_onHold == false)
        {
            Transform thisTransform = GetComponent<Transform>();
            thisTransform.position += MovementVelocity;
        }
	}

    protected virtual void OnCollisionEnter2D(Collision2D colliObject)
    {
        //check if collision is with an enemy.
        if (colliObject.gameObject.tag == "Enemy")
        {
            GameManager.Instance.GameLevelManager.EnemyInCombat(this.gameObject);
        }
    }

    protected virtual void EnterCombat()
    {
        _attackMove = (AttackMove) Random.Range(0, (int)AttackMove.NumOfMoves);
    }

    public AttackMove GetAttackMove()
    {
        return _attackMove;
    }

    // Another enemy is in combat so this one should be on hold or it is in combat
    public virtual void OnHold(bool onHold)
    {
        _onHold = onHold;
    }
}
