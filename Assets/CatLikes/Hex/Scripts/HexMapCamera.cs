using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HexMapCamera : MonoBehaviour
{
    public Transform Swivel,Stick;
    public float stickMinZoom, stickMaxZoom;
    public float swivelMinZoom, swivelMaxZoom;
    public float moveSpeedMinZoom, moveSpeedMaxZoom;
    public float moveSpeed;
    public float rotationSpeed;
    public HexGrid grid;

    float zoom = 1f;
    float rotationAngle;   //地图旋转的角度//

    private void Awake()
    {
        Swivel = transform.GetChild(0);
        Stick = Swivel.GetChild(0);
    }


    private void Update()
    {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");

        if (zoomDelta != 0)
        {
            AdjustZoom(zoomDelta);
        }

        float rotationDelta = Input.GetAxis("Rotation");
        if (rotationDelta != 0f) {
            AdjustRotation(rotationDelta);
        }

        float xDelta = Input.GetAxis("Horizontal");
        float zDelta = Input.GetAxis("Vertical");
        if (xDelta != 0f || zDelta != 0f)
        {
            AdjustPosition(xDelta, zDelta);
        }
    }

    void AdjustZoom(float delta) {
        zoom = Mathf.Clamp01(zoom + delta);

        float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom,zoom);
        Stick.localPosition = new Vector3(0,0,distance);

        float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom,zoom);
        Swivel.localRotation = Quaternion.Euler(angle,0,0);
    }


    void AdjustPosition(float xDelta,float zDelta) {
        Vector3 direction = transform.localRotation * new Vector3(xDelta, 0f, zDelta).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
        float distance =
            Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) * damping * Time.deltaTime;

        Vector3 position = transform.localPosition;
        position += direction * distance;
        transform.localPosition = ClampPosition(position);
    }

    Vector3 ClampPosition(Vector3 position) {
        float xMax = (grid.chunkCountX * HexMetrics.chunkSizeX - 0.5f) * (2 * HexMetrics.innerRadius);
        position.x = Mathf.Clamp(position.x, 0f, xMax);
        float zMax = (grid.chunkCountZ * HexMetrics.chunkSizeZ - 1) * (1.5f * HexMetrics.outerRadius);
        position.z = Mathf.Clamp(position.z, 0f, zMax);
        return position;
    }

    void AdjustRotation(float delta) {
        rotationAngle += delta * rotationSpeed * Time.deltaTime;
        if (rotationAngle < 0)
        {
            rotationAngle += 360f;
        }
        else if (rotationAngle > 360) {
            rotationAngle -= 360f;
        }
        transform.localRotation = Quaternion.Euler(0f, rotationAngle,0f);
    }

}
