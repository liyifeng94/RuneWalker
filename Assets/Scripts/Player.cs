using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public LayerMask UnitsLayer;

    public enum PlayerState
    {
        Block,
        Duck,
        Jump,
        Idle,
        Death,
        Special,
        NumOfStates
    }

    public int MaxHealth = 1;

    private bool _inCombat = false;
    private int _playerHealth = 1;
    private BoxCollider2D _boxCollider;
    private Rigidbody2D _rb2D;
    private PlayerState _currentPlayerState;
    private PlayerState _currentPlayerAction;
    private PlayerState _playerAttack;

    private Animator _animator;

    // Use this for initialization
    void Start ()
	{
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
	    _rb2D = GetComponent<Rigidbody2D>();
	    _currentPlayerState = PlayerState.Idle;
	    _currentPlayerAction = PlayerState.Idle;
        _playerAttack = PlayerState.Idle;
        _playerHealth = MaxHealth;

        //TODO: set to true for testing
	    _inCombat = false;

	}
	
	// Update is called once per frame
	void Update ()
    {
        // Wait for combat action to finish
        if (_inCombat)
        {
            return;
        }

        if (_playerHealth <= 0)
        {
            _animator.SetTrigger("playerDeath");
        }

        // Check for player action and update
        if (GetUserInput() == true)
        {
            Debug.Log("Has Input");
            CheckPlayerAction();
        }        

        //Trigger animation
        UpdateAnimationTrigger();
	}

    public void TakeDamage(int damage)
    {
        Debug.Log("TakeDamage");
        --_playerHealth;
        ExitCombat();
    }

    public void ExitCombat()
    {
        _inCombat = false;
    }

    public void PlayerDeath()
    {
        GameManager.Instance.GameOver();
        Destroy(gameObject);
    }

    void CheckPlayerAction()
    {
        _currentPlayerState = PlayerState.Idle;
        if (_currentPlayerAction == PlayerState.Special)
        {
            
            //Check power meter
            if (GameManager.Instance.HasSpecialAttack())
            {
                _currentPlayerState = PlayerState.Special;
            }
            else
            {
                _currentPlayerAction = PlayerState.Idle;
            }
            
        }
        else
        {
            _currentPlayerState = _currentPlayerAction;
        }
    }

    bool GetUserInput()
    {
        bool hasInput = false;

        if (Input.GetKeyUp(KeyCode.Space))
        {
            _currentPlayerAction = PlayerState.Special;
            hasInput = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow))
        {
            _currentPlayerAction = PlayerState.Block;
            hasInput = true;
        }
        else if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            _currentPlayerAction = PlayerState.Jump;
            hasInput = true;
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow))
        {
            _currentPlayerAction = PlayerState.Duck;
            hasInput = true;
        }

        return hasInput;
    }

    void UpdateAnimationTrigger()
    {
        if (_currentPlayerState == PlayerState.Idle)
        {
            return;
        }
        _playerAttack = _currentPlayerAction;
        switch (_currentPlayerState)
        {
            case PlayerState.Jump:
                _animator.SetTrigger("playerJump");
                _inCombat = true;
                break;
            case PlayerState.Block:
                _animator.SetTrigger("playerBlock");
                _inCombat = true;
                break;
            case PlayerState.Duck:
                _animator.SetTrigger("playerDuck");
                _inCombat = true;
                break;
            case PlayerState.Death:
                _animator.SetTrigger("playerDeath");
                break;
            case PlayerState.Special:
                _animator.SetTrigger("playerSpecial");
                break;
        }

        //Animation triggered reset
        _currentPlayerState = PlayerState.Idle;
    }

    void OnDestory()
    {
        GameManager.Instance.GameOver();
    }

    public void HitTarget()
    {
        GameManager.Instance.GetLevelManager().ResolveCombat(_playerAttack);
        _playerAttack = PlayerState.Idle;
    }

    public void UseSpecial()
    {
        GameManager.Instance.GetLevelManager().PlayerUseSpecial();
    }
}
