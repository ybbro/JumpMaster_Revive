using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Camera))]
public class CameraControl : MonoBehaviour
{
    [SerializeField] private PlayerControl player;
    [SerializeField] private Transform camPos;
    
    // ī�޶� �ʱ�ȭ �� ��ǥ ��ġ, ȸ�� ������ �� ����Ƽ¯ �ڽ� ������Ʈ
    // ����Ƽ¯�� �������� �׻� ������ ���� ��ġ (�� �� �ణ ������ ����Ƽ¯ ������ �ణ �����ٺ�����)
    Transform initPos;

    // ī�޶�� �÷��̾��� �̰� ��ǥ
    Vector3 positionDistance;

    // ī�޶� ���� ȸ�� ��
    Vector3 rotation_temp;

    // ���� ���� �̵�
    // ���� �̵� �� ī�޶� �̵� �ӵ�
    public float originSpeed = 20f;

    // ���� �̵� ��� off ī����
    float count;

    // ���� �̵� ��� ���� �ð�
    const float bQuickSwitch_toggle_delay = 1f;

    // ���콺 �������� ���� ���� �̵�
    // ī�޶� ���� �̵� ��� on/off
    bool isCameraInit;

    // ī�޶� ȸ�� ��
    float xRotate, yRotate;

    // ���콺 �̵� �ʱ�ȭ ��
    float xRotateOrigin, yRotateOrigin;

    // ���콺 �¿� �̵��� ���� ī�޶� ���� ����
    float diameter_LR;

    // ���콺 �̵��� ���� ī�޶� ���Ʒ�, �¿� ȸ�� �ӵ� !!!!! ���� �̵� �ΰ��� �������� ������ ��
    public float cameraSensitivity = 0.5f;

    // ��, �Ʒ� ���� �̵� ������
    public float[] UpDownRotateClamp = new float[2] { -20f, 20f };
    
    // ��ǲ �ý��ۿ��� ���콺 �������� �޾ƿ� ����
    Vector2 _mouseDelta;

    void Start()
    {
        // ī�޶��� �������� �� ����Ƽ¯ �ڽ� ������Ʈ
        initPos = camPos;

        // ���� ��ġ, ȸ������ �����ϰ�
        transform.position = initPos.position;
        transform.forward = initPos.forward;
        positionDistance = initPos.localPosition;
        // ���� ��ġ�� ���� ȸ�� ���� ���� �ʱ�ȭ
        diameter_LR = Mathf.Abs(initPos.localPosition.z) * 2f;
        // �������� �� xRotate, yRotate �� �ʱ�ȭ
        xRotateOrigin = initPos.eulerAngles.x;
        yRotateOrigin = diameter_LR;
        rotation_temp = transform.eulerAngles;
        
        // �ٷ� �ʱ�ȭ�ϸ� ���� �ʾұ⿡ �ణ�� �ð� ���� �ΰ� ����
        Invoke("InitCamera", 0.1f);
    }

    void InitCamera()
    {
        // ���콺 ���� ��ġ�� ��������
        xRotate = xRotateOrigin;
        yRotate = yRotateOrigin;
    }

    private void LateUpdate()
    {
        // ī�޶� ���� ���̸�
        if (isCameraInit)
        {
            // �������� ī�޶� �̵�, ȸ��
            transform.position = Vector3.Slerp(transform.position, initPos.position, Time.deltaTime * originSpeed);
            transform.forward = Vector3.Lerp(transform.forward, initPos.forward, Time.deltaTime * originSpeed);
            // ��� �� �� ��� �� ����(�� ���� �������� �̵�)
            count += Time.fixedDeltaTime;
            if (count > bQuickSwitch_toggle_delay)
            {
                count = 0f;
                isCameraInit = false;
                // ���� ������ ���� ��, 1���� ������ �Ķ����
                initMouseOrigin();
            }
        }
        // ī�޶� ���� ���� �ƴ� ����, 
        else
            CameraMove();
    }

    // ���� ���콺 ��ġ�� 0������ ����
    void initMouseOrigin()
    {
        // ����Ƽ¯�� ���� ������ 0������ ���� ������ ����
        player.currentDirection = player.transform.eulerAngles.y;
        // ������ ����Ƽ¯�� ������ ��ġ���� ����(ȸ������ ���� �޶�����)
        positionDistance = initPos.position - player.transform.position;
        // ������ ȸ�� ������ ����
        rotation_temp = initPos.eulerAngles;

        // ���� �� positionDistance�� ��ġ, initPos�� ���� ȸ������ �°� ���콺 ��ġ ����
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
        // ī�޶� ���� Ű�� ������ ��� on
        isCameraInit = true;
    }
    
    // �÷��� ���� ī�޶� ����
    void CameraMove()
    {
        // ����Ƽ¯�� ��ġ�� ���� �̵��ϵ��� ��ġ ����
        transform.position = player.transform.position + positionDistance;
        
        // ���콺 �¿� �̵��� ���� ĳ���� �ֺ��� ����
        yRotate += _mouseDelta.x * cameraSensitivity;
        transform.RotateAround(player.transform.position + Vector3.up * initPos.position.y, Vector3.up,
            yRotate);
        // �¿� ȸ���� ���� ����Ƽ¯�� �ٶ󺸰�
        transform.LookAt(player.transform.position);
        // ������ ȸ�� ���� ����
        rotation_temp = transform.eulerAngles;
        
        // ���콺 �յ� �̵��� ���� ���Ʒ� ���� �̵��� ����
        xRotate += -_mouseDelta.y * cameraSensitivity;
        // ��, �Ʒ� ���� �� ����
        xRotate = Mathf.Clamp(xRotate, UpDownRotateClamp[0], UpDownRotateClamp[1]);
        // ���콺 ���� �̵��� ���� ���� ȸ��
        transform.eulerAngles = new Vector3(xRotate, rotation_temp.y, rotation_temp.z);
        
        // ����Ƽ¯�� �̵���ǥ�� ���� ������ ī�޶� yȸ���� �°� ��ȭ
        player.currentDirection = rotation_temp.y;
    }
}