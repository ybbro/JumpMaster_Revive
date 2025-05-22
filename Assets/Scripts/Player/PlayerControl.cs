using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] private LayerMask notPlayer;
    Transform camera;
    
    Rigidbody _rigidbody;
    AnimationControl _animationControl;
    private bool isOnGround;
    private bool isJumpEntered;
    
    Vector3 moveVector;

    private Vector2 inputDir;

    private float h, v;
    
    // ���� ȸ���� ������ �Ǵ� �� (CameraControl ���� ����)
    public float currentDirection = 0f;

    private void Awake()
    {
        TryGetComponent(out _rigidbody);
        TryGetComponent(out _animationControl);
        camera = Camera.main.transform;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // ü�� ���� üũ
        GroundCheck();
        
        if (isOnGround)
        {   
            // �������� ���� ����
            Jump();
        }
        else
        {
            // ���� Ű �Է� ���� �ʱ�ȭ
            isJumpEntered = false;
        }
        
        // Ű �Է� �������� ĳ���� ȸ��
        PlayerRotate();
        // ĳ���� �̵�
        Move();
    }

    // �̵� Ű�� ������ �� ȣ��
    public void OnMove(InputAction.CallbackContext context)
    {
        // ���� : ���� �߿� �̵� Ű�� �� ������ �־ ������ ������ �̵��� �������� ����..
        if (context.performed)
        {
            // �Է��� ��ǥ�κ��� �̵��� ����
            inputDir = context.ReadValue<Vector2>().normalized;
            // �ȱ� �ִϸ��̼� ���
            _animationControl.Animator.SetBool(_animationControl.state[(int)AnimState.Running], true);
        }
        // �̵� ��ư���� ���� �����ٸ�, �̵� �ִϸ��̼� ����
        else if (context.canceled)
        {
            inputDir = Vector2.zero;
            _animationControl.Animator.SetBool(_animationControl.state[(int)AnimState.Running], false);
        }
    }

    void Move()
    {
        // ī�޶� ��ǥ�� �������� �������� �ڿ�������
        Vector3 dir = (camera.forward * inputDir.y + camera.right * inputDir.x);
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;
        
        _rigidbody.velocity = dir;
    }

    // ���� Ű�� ������ �� ȣ��
    public void OnJump(InputAction.CallbackContext context)
    {
        // �� �������� ����
        if (isOnGround && context.started)
        {
            isJumpEntered = true;
        }
    }

    void Jump()
    {
        if (isJumpEntered)
        {
            // �� �������� ���� ���Ͽ� ����
            _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            // ���� �ִϸ��̼� ���
            _animationControl.Animator.SetBool(_animationControl.state[(int)AnimState.Jump], true);
            isJumpEntered = false; 
        }
    }
    
    // �̵� Ű �Է¿� ���� ĳ���� ȸ��
    void PlayerRotate()
    {
        float h = inputDir.x; // �¿� ���� �Է�
        float v = inputDir.y; // �յ� ���� �Է�
        
        if (v > 0.1f) // W
        {
            if (h > 0.1f) // +D >> ������ �� ����
                transform.eulerAngles = Vector3.up *
                                        (currentDirection + 45f);
            else if (h < -0.1f) // +A >> ���� �� ����
                transform.eulerAngles = Vector3.up *
                                        (currentDirection -45f);
            else // W�� �Է� >> �� ����
                transform.eulerAngles = Vector3.up * currentDirection;
        }
        else if (v < -0.1f) // S
        {
            if (h > 0.1f)  // +D >> ������ �� ����
                transform.eulerAngles = Vector3.up *
                                        (currentDirection + 135f);
            else if (h < -0.1f) // +A >> ���� �� ����
                transform.eulerAngles = Vector3.up *
                                        (currentDirection -135f);
            else // S�� >> �� ����
                transform.eulerAngles = Vector3.up * (currentDirection + 180f);
        }
        else if (h > 0.1f) // D�� �Է� >> ������
            transform.eulerAngles = Vector3.up * (currentDirection + 90f);
        else if (h < -0.1f) // A�� �Է� >> ����
            transform.eulerAngles = Vector3.up * (currentDirection - 90f);
    }
    
    void GroundCheck()
    {
        // ������ ���� üũ ����
        bool isOnGround_before = isOnGround;
        // ĳ���� �ߺ��� ��¦ ���� ����
        Vector3 RayStartPos = transform.position + Vector3.up * 0.01f;
        // ĳ���� �� �Ʒ� �������� ���� ���� ���� �ݶ��̴��� �ִ��� ���ο� ���� ü�� ���� ���ֱ�
        isOnGround = Physics.Raycast(RayStartPos, Vector3.down, 0.15f, notPlayer);
        
        // ���߿� �� �ִ� ���� �������� �� 1����
        if (!isOnGround_before && isOnGround)
        {
            // �����ִϸ��̼� ����
            _animationControl.Animator.SetBool(_animationControl.state[(int)AnimState.Jump], false);
            // ���� �ִϸ��̼� ���
            _animationControl.Animator.SetBool(_animationControl.state[(int)AnimState.Land], true);
        }
        else
        {
            // ���� �ִϸ��̼� ����(Ʈ���ŷ� �Ϸ��� ������ ���� ���ۺ��� Ȱ��ȭ�Ǵ� ������ �߻�.. �׳� bool�� ����)
            _animationControl.Animator.SetBool(_animationControl.state[(int)AnimState.Land], false);
        }
    }
}
