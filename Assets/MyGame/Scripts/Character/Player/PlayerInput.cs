using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private Button jumpBtn;

    Vector2 moveInputVector = Vector2.zero;
    PlayerMovement playerMovement;

    //private void Awake()
    //{
    //    joystick.OnDrag(GameEvent.OnPlayerInputChange?.Invoke());
    //}

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

#if UNITY_ANDROID || UNITY_IOS
        jumpBtn.onClick.AddListener(() => { 
            playerMovement.Jump();
            if (playerMovement.isInLadder)
            {
                GameEvent.OnJumpOutFromLadder?.Invoke();
            }
        });
#endif
    }

#if UNITY_ANDROID || UNITY_IOS
    private void OnDestroy()
    {
        jumpBtn.onClick.RemoveAllListeners();
    }
#endif

    // Update is called once per frame
    void Update()
    {
#if UNITY_EDITOR
        //Move input
        moveInputVector.x = Input.GetAxisRaw("Horizontal");
        moveInputVector.y = Input.GetAxisRaw("Vertical");
        
        //Jump
        if (Input.GetKeyDown(KeyCode.Space))
        {
            playerMovement.Jump();
            if (playerMovement.isInLadder)
            {
                GameEvent.OnJumpOutFromLadder?.Invoke();
            }
        }
#elif UNITY_ANDROID || UNITY_IOS
        //Move input
        moveInputVector.x = joystick.Horizontal;
        moveInputVector.y = joystick.Vertical;
#endif
        playerMovement.SetMoveInputVector(moveInputVector);
    }
}
