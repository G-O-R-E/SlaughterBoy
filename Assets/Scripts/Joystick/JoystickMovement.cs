using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickMovement : MonoBehaviour
{
    public GameObject joystick;
    public GameObject joystickBG;
    public Vector2 joystickVec;
    private Vector2 joystickTouchPos;
    private Vector2 joystickOriginalPos;
    private float joystickRadius;
    public bool touchJoystickStart = false;
    private Color newColor = Color.white;
    private Image imageJoystick;
    private Image imageJoystickBG;

    [Header("Payer Component")]
    [SerializeField] GameObject player;
    [SerializeField] float speed = 5f;

    private Rigidbody2D rigidBody2D;
    private Animator animator = null;
    private SpriteRenderer spriteRenderer = null;
    void Start()
    {
        rigidBody2D = player.GetComponent<Rigidbody2D>();
        imageJoystick = joystick.GetComponent<Image>();
        imageJoystickBG = joystickBG.GetComponent<Image>();
        newColor.a = 0f;
        imageJoystick.color = newColor;
        imageJoystickBG.color = newColor;
        joystickOriginalPos = joystickBG.transform.position;
        joystickRadius = joystickBG.GetComponent<RectTransform>().sizeDelta.y / 4;
    }

    void Update()
    {
        if (player.transform.GetChild(0).transform.childCount > 0)
        {
            InitPlayerComponent();
            rigidBody2D.velocity = Vector2.zero;
            if (touchJoystickStart)
            {
                UpdateColor();
                
                Vector2 offset = joystickVec;
                Vector2 direction = Vector2.ClampMagnitude(offset, 1f);
                direction = DeadZone(direction);
                MovePlayer(direction);
                ChangeDirectionAnim(direction);
                transform.position = new Vector2(joystickOriginalPos.x + direction.x, joystickOriginalPos.y + direction.y);
            }
            else
            {
                Vector2 direction = Vector2.zero;
                ChangeDirectionAnim(direction);
            }
            // Utilisation de Input.touches pour gérer les touches tactiles
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        PointerDown(touch.position);
                        break;

                    case TouchPhase.Moved:
                        Drag(touch.position);
                        break;
                    case TouchPhase.Stationary:
                        Drag(touch.position);
                        break;

                    case TouchPhase.Ended:
                        if(Input.touchCount == 0)
                        {
                            PointerUp();
                        }
                        
                        break;
                    case TouchPhase.Canceled:
                        break;
                }
            }
            if (Input.touchCount == 0)
            {
                PointerUp();
            }
        }
    }

    // Utilisation de l'écran tactile pour détecter l'appui
    private void PointerDown(Vector2 touchPosition)
    {
        joystick.transform.position = touchPosition;
        joystickBG.transform.position = touchPosition;
        joystickTouchPos = touchPosition;
        touchJoystickStart = true;
    }

    // Utilisation de l'écran tactile pour détecter le glissement
    private void Drag(Vector2 touchPosition)
    {
        float joystickDist = Vector2.Distance(touchPosition, joystickTouchPos);

        if (joystickDist < joystickRadius)
        {
            joystick.transform.position = joystickTouchPos + (touchPosition - joystickTouchPos);
            joystickVec = (touchPosition - joystickTouchPos).normalized * (joystickDist / joystickRadius);
        }
        else
        {
            joystick.transform.position = joystickTouchPos + (touchPosition - joystickTouchPos).normalized * joystickRadius;
            joystickVec = (touchPosition - joystickTouchPos).normalized;
        }
    }

    // Utilisation de l'écran tactile pour détecter la libération
    private void PointerUp()
    {
        if (imageJoystick.color.a > 0f)
        {
            newColor.a = 0f;
            imageJoystick.color = newColor;
        }

        if (imageJoystickBG.color.a > 0f)
        {
            newColor.a = 0f;
            imageJoystickBG.color = newColor;
        }

        touchJoystickStart = false;
        joystickVec = Vector2.zero;
        joystick.transform.position = joystickOriginalPos;
        joystickBG.transform.position = joystickOriginalPos;
    }
    void MovePlayer(Vector2 direction)
    {
        Vector2 velocity = (direction * speed);
        rigidBody2D.velocity = velocity;
    }

    void ChangeDirectionAnim(Vector2 direction)
    {
        animator.SetFloat("X", direction.x);
        animator.SetFloat("Y", direction.y);

        if (direction.x > 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = false;
        }
    }

    private void InitPlayerComponent()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = player.transform.GetChild(0).GetComponentInChildren<SpriteRenderer>();
        }
        if (animator == null)
        {
            animator = player.transform.GetChild(0).GetComponentInChildren<Animator>();
        }
    }

    private void UpdateColor()
    {
        if (imageJoystick.color.a == 0f)
        {
            newColor.a = 0.8f;
            imageJoystick.color = newColor;
        }

        if (imageJoystickBG.color.a == 0f)
        {
            newColor.a = 0.10f;
            imageJoystickBG.color = newColor;
        }
    }

    private Vector2 DeadZone(Vector2 direction)
    {
        // Appliquer la "deadzone" uniquement si nécessaire
        if (Mathf.Abs(direction.x) < 0.08f)
        {
            direction.x = 0f;
        }

        if (Mathf.Abs(direction.y) < 0.08f)
        {
            direction.y = 0f;
        }

        return direction;
    }
}
