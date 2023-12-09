using UnityEngine;
using DG.Tweening;
public class PlayerCam : MonoBehaviour
{
    [Header("References")]
    [SerializeField] Transform orientation;
    [SerializeField] Transform camPosition;
    [SerializeField] PlayerMovement pm;
    [SerializeField] float startingRotaition;
    [Header("Settings")]
    [SerializeField] private float defaultSensitivity = 100f; 
    private float sensitivity;


    float xRotation;
    float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        yRotation = startingRotaition;
        sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", defaultSensitivity);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void updateSens() {

        sensitivity = PlayerPrefs.GetFloat("MouseSensitivity", defaultSensitivity);

    }

    // Update is called once per frame
    void Update()
    {
        PlayerCamera();
    }

    private void PlayerCamera() {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -85f, 85f);

        camPosition.localRotation = Quaternion.Euler(xRotation, yRotation, 0);

        if (!(pm.wallrunning || pm.isSliding))
        {
            orientation.rotation = Quaternion.Euler(0, yRotation, 0);
        }
    }

    public void DoFov(float endValue, float duration) 
    {


        GetComponent<Camera>().DOFieldOfView(endValue, duration);

    }
    public void DoTilt(float zTilt, float duration)
    {

        transform.DOLocalRotate(new Vector3(0, 0, zTilt), duration);

    }




}