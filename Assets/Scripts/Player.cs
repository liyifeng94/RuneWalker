using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public LayerMask UnitsLayer;

    public enum PlayerState
    {
        Combat,
        Jump,
        Duck,
        Block,
        Death,
        Walking
    }

    public int MaxHealth = 1;

    private int _playerHealth = 1;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb2D;
    private PlayerState _currentPlayerState;
    private PlayerState _currentPlayerAction;

    private Animator _animator;

    //TODO: Add Death delay

    //Calls when enemy is in range
    public void StartCombat()
    {
        //TODO: change animation state to combat

        _currentPlayerState = PlayerState.Combat;
    }

    // Use this for initialization
    void Start ()
	{
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
	    _rb2D = GetComponent<Rigidbody2D>();
	    _currentPlayerState = PlayerState.Walking;
	    _currentPlayerAction = PlayerState.Walking;
        _playerHealth = MaxHealth;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (_currentPlayerState == PlayerState.Walking)
        {
            return;
        }

        //TODO: Get player combat action


        //TODO: Resolve combat


        UpdateAnimationTrigger();


        // End game
        if (_playerHealth <= 0)
        {
            GameManager.Instance.GameOver();
        }
	}

    void UpdateAnimationTrigger()
    {
        switch (_currentPlayerAction)
        {
            case PlayerState.Combat:
                _animator.SetTrigger("playerCombat");
                break;
            case PlayerState.Jump:
                _animator.SetTrigger("playerJump");
                break;
            case PlayerState.Block:
                _animator.SetTrigger("playerBlock");
                break;
            case PlayerState.Duck:
                _animator.SetTrigger("playerDuck");
                break;
            case PlayerState.Death:
                _animator.SetTrigger("playerDeath");
                break;
            default:
                _animator.SetTrigger("playerWalking");
                break;
        }
    }
}
