using UnityEngine;
using UnityEngine.UI;

public class CompassArrow : MonoBehaviour
{
    private Transform goal;
    private Transform cameraTransform;
    private Transform arrow;

    private void Start()
    {
        cameraTransform = GameManager.Instance.mainCamera.transform;
        arrow = transform;
        enabled = false; // Disable until all references are set
    }
    public void StartIndicating(Transform goal)
    {
        enabled = true;
        this.goal = goal;
        Image image = GetComponent<Image>();
        image.enabled = true;
    }
    private void Update()
    {
        // Get direction from camera to goal
        Vector3 directionToGoal = (goal.position - cameraTransform.position).normalized;

        // Project vectors to horizontal plane (XZ)
        Vector3 cameraForward2D = new Vector3(cameraTransform.forward.x, 0, cameraTransform.forward.z).normalized;
        Vector3 directionToGoal2D = new Vector3(directionToGoal.x, 0, directionToGoal.z).normalized;

        // Handle edge case: goal directly above/below camera
        if (directionToGoal2D.sqrMagnitude < 0.0001f)
        {
            arrow.rotation = Quaternion.identity; // Default to straight
            return;
        }

        // Calculate signed angle between camera forward and direction to goal
        float angle = SignedAngleBetween(cameraForward2D, directionToGoal2D, Vector3.up);

        // Adjust for arrow orientation (e.g., if arrow points up by default)
        float finalAngle = -angle; // Adjust based on your arrow's default direction

        // Apply rotation to the arrow
        arrow.rotation = Quaternion.Euler(0, 0, finalAngle);
    }

    private float SignedAngleBetween(Vector3 from, Vector3 to, Vector3 up)
    {
        float angle = Vector3.Angle(from, to);
        float sign = Mathf.Sign(Vector3.Dot(up, Vector3.Cross(from, to)));
        return angle * sign;
    }
}