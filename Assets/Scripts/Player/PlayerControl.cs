using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    private float moveSpeedOrigin;
    public float GetOriginSpeed
    {
        get => moveSpeedOrigin;
    }
    Coroutine ChangingMoveSpeed;
    [SerializeField] float jumpForce;
    [SerializeField] private LayerMask notPlayer;
    Transform camera;
    
    Rigidbody _rigidbody;
    AnimationControl _animationControl;
    private bool isOnGround;
    private bool isJumpEntered;
    
    Vector3 moveVector;

    private Vector2 inputDir;
    
    // 현재 회전의 기준이 되는 값 (CameraControl 에서 변경)
    public float currentDirection = 0f;

    private void Awake()
    {
        TryGetComponent(out _rigidbody);
        TryGetComponent(out _animationControl);
        camera = Camera.main.transform;
        moveSpeedOrigin = moveSpeed;
    }

    private void Update()
    {
        // 체공 여부 체크
        GroundCheck();
        
        if (isOnGround)
        {
            if (isJumpEntered)
            {
                // 땅에서만 점프 가능
                Jump(jumpForce);
            }
        }
        else
        {
            // 점프 키 입력 여부 초기화
            isJumpEntered = false;
        }
        
        // 키 입력 방향으로 캐릭터 회전
        PlayerRotate();
        // 캐릭터 이동
        Move();
    }

    // 이동 키를 눌렀을 때 호출
    public void OnMove(InputAction.CallbackContext context)
    {
        // 문제 : 점프 중에 이동 키를 꾹 누르고 있어도 점프가 끝나면 이동이 동작하지 않음..
        if (context.performed)
        {
            // 입력한 좌표로부터 이동값 연산
            inputDir = context.ReadValue<Vector2>().normalized;
            // 걷기 애니메이션 재생
            _animationControl.Animator.SetBool(_animationControl.state[(int)AnimState.Running], true);
        }
        // 이동 버튼에서 손을 떼었다면, 이동 애니메이션 정지
        else if (context.canceled)
        {
            inputDir = Vector2.zero;
            _animationControl.Animator.SetBool(_animationControl.state[(int)AnimState.Running], false);
        }
    }

    void Move()
    {
        // 카메라가 좌표를 기준으로 움직여야 자연스러움
        Vector3 dir = (camera.forward * inputDir.y + camera.right * inputDir.x);
        dir *= moveSpeed;
        dir.y = _rigidbody.velocity.y;
        
        _rigidbody.velocity = dir;
    }

    // 점프 키를 눌렀을 때 호출
    public void OnJump(InputAction.CallbackContext context)
    {
        // 땅 위에서만 점프
        if (isOnGround && context.started)
        {
            isJumpEntered = true;
        }
    }

    public void Jump(float _jumpForce)
    {
        // 윗 방향으로 힘을 가하여 점프
        _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
        // 점프 애니메이션 재생
        _animationControl.Animator.SetBool(_animationControl.state[(int)AnimState.Jump], true);
        isJumpEntered = false;
    }

    // 이동 키 입력에 따른 캐릭터 회전
    void PlayerRotate()
    {
        float h = inputDir.x; // 좌우 방향 입력
        float v = inputDir.y; // 앞뒤 방향 입력
        
        if (v > 0.1f) // W
        {
            if (h > 0.1f) // +D >> 오른쪽 앞 방향
                transform.eulerAngles = Vector3.up *
                                        (currentDirection + 45f);
            else if (h < -0.1f) // +A >> 왼쪽 앞 방향
                transform.eulerAngles = Vector3.up *
                                        (currentDirection -45f);
            else // W만 입력 >> 앞 방향
                transform.eulerAngles = Vector3.up * currentDirection;
        }
        else if (v < -0.1f) // S
        {
            if (h > 0.1f)  // +D >> 오른쪽 뒷 방향
                transform.eulerAngles = Vector3.up *
                                        (currentDirection + 135f);
            else if (h < -0.1f) // +A >> 왼쪽 뒷 방향
                transform.eulerAngles = Vector3.up *
                                        (currentDirection -135f);
            else // S만 >> 뒷 방향
                transform.eulerAngles = Vector3.up * (currentDirection + 180f);
        }
        else if (h > 0.1f) // D만 입력 >> 오른쪽
            transform.eulerAngles = Vector3.up * (currentDirection + 90f);
        else if (h < -0.1f) // A만 입력 >> 왼쪽
            transform.eulerAngles = Vector3.up * (currentDirection - 90f);
    }
    
    void GroundCheck()
    {
        // 직전의 착지 체크 상태
        bool isOnGround_before = isOnGround;
        // 캐릭터 발보다 살짝 위의 지점
        Vector3 RayStartPos = transform.position + Vector3.up * 0.01f;
        // 캐릭터 발 아래 방향으로 빔을 쏴서 맞은 콜라이더가 있는지 여부에 따라 체공 여부 써주기
        isOnGround = Physics.Raycast(RayStartPos, Vector3.down, 0.15f, notPlayer);
        
        // 공중에 떠 있다 지상에 안착했을 때 1번만
        if (!isOnGround_before && isOnGround)
        {
            // 점프애니메이션 끄고
            _animationControl.Animator.SetBool(_animationControl.state[(int)AnimState.Jump], false);
            // 착지 애니메이션 재생
            _animationControl.Animator.SetBool(_animationControl.state[(int)AnimState.Land], true);
        }
        else
        {
            // 착지 애니메이션 끄기(트리거로 하려고 했으나 게임 시작부터 활성화되는 문제가 발생.. 그냥 bool로 관리)
            _animationControl.Animator.SetBool(_animationControl.state[(int)AnimState.Land], false);
        }
    }

    // 인벤토리 키를 눌렀을 때 호출
    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameManager.Instance.Inventory.ActiveChange();
        }
    }
    
    // 일정 시간 동안 이동속도 증가
    public void Call_Change_MoveSpeed(float _moveSpeed, float _time)
    {
        if(ChangingMoveSpeed != null)
            StopCoroutine(Change_MoveSpeed(_moveSpeed, _time));
        StartCoroutine(Change_MoveSpeed(_moveSpeed, _time));
    }
    
    IEnumerator Change_MoveSpeed(float _moveSpeed, float _time)
    {
        // 바꿀 속도로 변화
        moveSpeed = _moveSpeed;
        // 버프 시간 동안 유지
        yield return new WaitForSeconds(_time);
        // 원래 속도로 되돌아가기
        moveSpeed = moveSpeedOrigin;
    }
}
