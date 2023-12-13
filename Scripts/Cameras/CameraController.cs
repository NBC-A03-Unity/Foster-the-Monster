using Cinemachine;
using UnityEngine;
public enum CameraModes { OnAndOff, Toggle }
public class CameraController : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private CinemachineConfiner confiner;
    [SerializeField] private Transform curTarget;


    [Header("Need to connect")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform freeTarget;
    [SerializeField] private Transform mapBoundary;

    [Header("Settings")]
    [SerializeField] private CameraModes mode;
    public float zoomMaxSize = 10.0f;
    public float zoomMinSize = 5.0f;
    public float freeCameraMoveSpeed = 10.0f;
    public Camera mainCamera;
    private bool isFreeCameraMode;

    private PolygonCollider2D polygonCollider;
    
    [Header("MapBoundary")]
    [SerializeField] private float maxY = 5000f;
    [SerializeField] private float minY = -5000f;
    [SerializeField] private float maxX = 5000f;
    [SerializeField] private float minX = -5000f;

    private void Awake()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        confiner = GetComponent<CinemachineConfiner>();
        polygonCollider = mapBoundary.GetComponent<PolygonCollider2D>();
        mainCamera = Camera.main;
    }

    private void Start()
    {
        SetTarget(player);
        SetBoundary(maxY, minY, maxX, minX);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (mode == CameraModes.OnAndOff) { OnFreeCameraMode(); }

            if (mode == CameraModes.Toggle) { ToggleCameraMode(); }
        }
        if (Input.GetMouseButtonUp(2))
        {
            if (mode == CameraModes.OnAndOff) { OffFreeCameraMode(); }
        }
        if (isFreeCameraMode)
        {
            MoveFreeCamera();
        }
        MouseWheelCheck();
    }
    public void MapChange(Collider2D map_PolygonCollider)
    {
        if (map_PolygonCollider != null)
        {
            confiner.m_BoundingShape2D = map_PolygonCollider;
        }
    }

    public void SetTarget(Transform target)
    {
        if (target == null) return;
        virtualCamera.m_Follow = target;
        curTarget = target;
    }

    public void ToggleCameraMode()
    {
        isFreeCameraMode = !isFreeCameraMode;
        if (isFreeCameraMode && !(Cursor.lockState == CursorLockMode.Locked))
        {
            Cursor.lockState = CursorLockMode.Locked;
            virtualCamera.m_Follow = freeTarget;
            freeTarget.position = curTarget.position;
        }
        if (!isFreeCameraMode && Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            virtualCamera.m_Follow = curTarget;
        }
    }

    public void OnFreeCameraMode()
    {
        isFreeCameraMode = true;
        if (Cursor.lockState != CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            virtualCamera.m_Follow = freeTarget;
            freeTarget.position = curTarget.position;
        }
    }
    public void OffFreeCameraMode()
    {
        isFreeCameraMode = false;
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Cursor.lockState = CursorLockMode.None;
            virtualCamera.m_Follow = curTarget;
        }
    }

    public void MoveFreeCamera()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        Vector3 moveDirection = new Vector3(mouseX, mouseY, 0);
        Vector3 newPosition = freeTarget.position + moveDirection * freeCameraMoveSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        freeTarget.position = newPosition;
    }

    public void MouseWheelCheck()
    {
        float wheelScrollValue = Input.GetAxis("Mouse ScrollWheel");
        if (wheelScrollValue != 0)
        {
            float newSize = virtualCamera.m_Lens.OrthographicSize;
            newSize += wheelScrollValue > 0 ? -1f : 1f;
            newSize = Mathf.Clamp(newSize, zoomMinSize, zoomMaxSize);
            virtualCamera.m_Lens.OrthographicSize = newSize;
        }
    }

    public void SetBoundary(float _maxY, float _minY, float _maxX = 5000, float _minX = 5000)
    {
        maxY = _maxY;
        minY = _minY;
        maxX = _maxX;
        minX = _minX;
        polygonCollider.points = new Vector2[4]{ new Vector2(minX, maxY) , new Vector2( maxX, maxY) , new Vector2(maxX, minY), new Vector2(minX, minY) }; 
    }


    public void SetBoundary(BoxCollider2D boxCollider)
    {
        Vector3 min = boxCollider.bounds.min;
        Vector3 max = boxCollider.bounds.max;
        minX = min.x;
        minY = min.y;
        maxX = max.x;
        maxY = max.y;
        polygonCollider.points = new Vector2[4] { new Vector2(minX, maxY), new Vector2(maxX, maxY), new Vector2(maxX, minY), new Vector2(minX, minY) };
    }
}
