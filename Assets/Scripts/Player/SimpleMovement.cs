using UnityEngine;

public class SimpleMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody m_Rb;
    [SerializeField] private float m_Speed;

    private void FixedUpdate() {
        Vector3 moveDir = Vector2.zero;

        if (Input.GetKey(KeyCode.W)) {
            moveDir += Vector3.forward;    
        }
        if (Input.GetKey(KeyCode.S)) {
            moveDir += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A)) {
            moveDir += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D)) {
            moveDir += Vector3.right;
        }

        moveDir.Normalize();

        var camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        var camRot = Quaternion.FromToRotation(Vector3.forward, camForward);
        moveDir = camRot * moveDir;

        moveDir = m_Speed * moveDir;
        moveDir.y = m_Rb.velocity.y;
        m_Rb.velocity = moveDir;
    }
}
