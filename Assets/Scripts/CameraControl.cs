using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    [SerializeField] private PlayerControl player;
    [SerializeField] private Transform camPos;
    
    // 카메라 초기화 시 목표 위치, 회전 각도로 할 유니티짱 자식 오브젝트
    // 유니티짱을 기준으로 항상 동일한 로컬 위치 (등 뒤 약간 위에서 유니티짱 정면을 약간 내려다보도록)
    Transform initPos;

    // 카메라와 플레이어의 이격 좌표
    Vector3 positionDistance;

    // 카메라 현재 회전 값
    Vector3 rotation_temp;

    // 시점 원점 이동
    // 원점 이동 시 카메라 이동 속도
    public float originSpeed = 20f;

    // 원점 이동 기능 off 카운터
    float count;

    // 원점 이동 기능 지속 시간
    const float bQuickSwitch_toggle_delay = 1f;

    // 마우스 조작으로 인한 시점 이동
    // 카메라 원점 이동 기능 on/off
    bool isCameraInit;

    // 카메라 회전 값
    float xRotate, yRotate;

    // 마우스 이동 초기화 값
    float xRotateOrigin, yRotateOrigin;

    // 마우스 좌우 이동에 따른 카메라 공전 지름
    float diameter_LR;

    // 마우스 이동에 따른 카메라 위아래, 좌우 회전 속도 !!!!! 시점 이동 민감도 설정에서 조절할 값
    public float cameraSensitivity = 0.5f;

    // 위, 아래 시점 이동 상하한
    public float[] UpDownRotateClamp = new float[2] { -20f, 20f };
    
    // 인풋 시스템에서 마우스 움직임을 받아올 변수
    Vector2 _mouseDelta;

    void Start()
    {
        // 카메라의 원점으로 할 유니티짱 자식 오브젝트
        initPos = camPos;

        // 원점 위치, 회전값과 동일하게
        transform.position = initPos.position;
        transform.forward = initPos.forward;
        positionDistance = initPos.localPosition;
        // 원점 위치를 토대로 회전 지름 값을 초기화
        diameter_LR = Mathf.Abs(initPos.localPosition.z) * 2f;
        // 원점으로 할 xRotate, yRotate 값 초기화
        xRotateOrigin = initPos.eulerAngles.x;
        yRotateOrigin = diameter_LR;
        rotation_temp = transform.eulerAngles;
        
        // 바로 초기화하면 되지 않았기에 약간의 시간 텀을 두고 진행
        Invoke("InitCamera", 0.1f);
    }

    void InitCamera()
    {
        // 마우스 현재 위치를 원점으로
        xRotate = xRotateOrigin;
        yRotate = yRotateOrigin;
    }

    private void LateUpdate()
    {
        // 카메라 리셋 중이면
        if (isCameraInit)
        {
            // 원점으로 카메라 이동, 회전
            transform.position = Vector3.Slerp(transform.position, initPos.position, Time.deltaTime * originSpeed);
            transform.forward = Vector3.Lerp(transform.forward, initPos.forward, Time.deltaTime * originSpeed);
            // 기능 켠 후 잠시 후 끄기(그 사이 원점으로 이동)
            count += Time.fixedDeltaTime;
            if (count > bQuickSwitch_toggle_delay)
            {
                count = 0f;
                isCameraInit = false;
                // 원점 리셋이 끝날 때, 1번만 리셋할 파라미터
                initMouseOrigin();
            }
        }
        // 카메라 리셋 중이 아닐 때는, 
        else
            CameraMove();
    }

    // 현재 마우스 위치를 0점으로 조정
    void initMouseOrigin()
    {
        // 유니티짱의 기준 각도를 0점으로 잡은 각도로 리셋
        player.currentDirection = player.transform.eulerAngles.y;
        // 원점과 유니티짱의 떨어진 위치값을 리셋(회전값에 따라 달라진다)
        positionDistance = initPos.position - player.transform.position;
        // 원점의 회전 값으로 리셋
        rotation_temp = initPos.eulerAngles;

        // 리셋 후 positionDistance의 위치, initPos의 상하 회전값에 맞게 마우스 위치 리셋
        if (positionDistance.z >= 0f)
            yRotateOrigin = positionDistance.x;
        else if (positionDistance.x >= 0f)
            yRotateOrigin = -positionDistance.x + diameter_LR;
        else
            yRotateOrigin = -positionDistance.x - diameter_LR;
        xRotate = xRotateOrigin;
        yRotate = yRotateOrigin;
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnInit(InputAction.CallbackContext context)
    {
        // 카메라 리셋 키를 누르면 기능 on
        isCameraInit = true;
    }
    
    // 플레이 도중 카메라 무브
    void CameraMove()
    {
        // 유니티짱의 위치를 따라 이동하도록 위치 갱신
        transform.position = player.transform.position + positionDistance;
        
        // 마우스 좌우 이동에 따라 캐릭터 주변을 공전
        yRotate += _mouseDelta.x * cameraSensitivity;
        transform.RotateAround(player.transform.position + Vector3.up * initPos.position.y, Vector3.up,
            yRotate);
        // 좌우 회전에 맞춰 유니티짱을 바라보게
        transform.LookAt(player.transform.position);
        // 현재의 회전 값을 저장
        rotation_temp = transform.eulerAngles;
        
        // 마우스 앞뒤 이동에 따라 위아래 시점 이동값 산출
        xRotate += -_mouseDelta.y * cameraSensitivity;
        // 위, 아래 상한 각 고정
        xRotate = Mathf.Clamp(xRotate, UpDownRotateClamp[0], UpDownRotateClamp[1]);
        // 마우스 상하 이동에 대해 시점 회전
        transform.eulerAngles = new Vector3(xRotate, rotation_temp.y, rotation_temp.z);
        
        // 유니티짱의 이동좌표계 정면 각도를 카메라 y회전에 맞게 변화
        player.currentDirection = rotation_temp.y;
    }
}