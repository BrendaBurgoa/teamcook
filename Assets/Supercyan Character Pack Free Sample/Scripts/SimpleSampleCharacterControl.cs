using System.Collections.Generic;
using UnityEngine;

public class SimpleSampleCharacterControl : Photon.MonoBehaviour
{
    public states state;
    public enum states
    {
        IDLE,
        PICKUP
    }
    public PhotonView photonView;

    [SerializeField] private float m_moveSpeed = 2;
    [SerializeField] private float m_turnSpeed = 200;

    [SerializeField] private Animator m_animator = null;

    [SerializeField] private Rigidbody rb= null;

    private float m_currentV = 0;
    private float m_currentH = 0;

    private readonly float m_interpolation = 10;
    private readonly float m_walkScale = 0.33f;
    private readonly float m_backwardsWalkScale = 0.16f;
    private readonly float m_backwardRunScale = 0.66f;

    private Vector3 m_currentDirection = Vector3.zero;

    private bool m_isGrounded;

    private List<Collider> m_collisions = new List<Collider>();

    private void Awake()
    {
        m_animator = gameObject.GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        m_animator.SetBool("Grounded", true);
    }

    private void FixedUpdate()
    {
        DirectUpdate();
    }

    private void TankUpdate()
    {
        if (photonView.isMine){
            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            bool walk = Input.GetKey(KeyCode.LeftShift);

            if (v < 0)
            {
                if (walk) { v *= m_backwardsWalkScale; }
                else { v *= m_backwardRunScale; }
            }
            else if (walk)
            {
                v *= m_walkScale;
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            transform.position += transform.forward * m_currentV * m_moveSpeed * Time.deltaTime;
            transform.Rotate(0, m_currentH * m_turnSpeed * Time.deltaTime, 0);

            m_animator.SetFloat("MoveSpeed", m_currentV);
        }
    }
    public void PickUp()
    {
        if (state == states.PICKUP) return;
        state = states.PICKUP;
        Invoke("Reset", 0.5f);
        m_animator.SetBool("Pickup", true);
    }
    private void Reset()
    {
        state = states.IDLE;
    }
    Vector3 lastPos;
    public float offsetSpeed = 0.005f;
    private void DirectUpdate()
    {
        lastPos = transform.position;
        if (photonView.isMine){

            if (state == states.PICKUP) return;

            float v = Input.GetAxis("Vertical");
            float h = Input.GetAxis("Horizontal");

            Transform camera = Camera.main.transform;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                v *= m_walkScale;
                h *= m_walkScale;
            }

            m_currentV = Mathf.Lerp(m_currentV, v, Time.deltaTime * m_interpolation);
            m_currentH = Mathf.Lerp(m_currentH, h, Time.deltaTime * m_interpolation);

            Vector3 direction = camera.forward * m_currentV + camera.right * m_currentH;

            float directionLength = direction.magnitude;
            direction.y = 0;
            direction = direction.normalized * directionLength;

            if (direction != Vector3.zero)
            {
                m_currentDirection = Vector3.Slerp(m_currentDirection, direction, Time.deltaTime * m_interpolation);

                transform.rotation = Quaternion.LookRotation(m_currentDirection);
                transform.position += m_currentDirection * m_moveSpeed * Time.deltaTime;
                m_animator.SetFloat("MoveSpeed", direction.magnitude);
            }
          
        }
        else
        {
            float speed = 0;
            float dist = Vector3.Distance(transform.position, lastPos);
            lastPos = transform.position;
            if (dist > offsetSpeed)
                speed = 0.9f;

            m_animator.SetFloat("MoveSpeed", speed);
        }
    }

}
