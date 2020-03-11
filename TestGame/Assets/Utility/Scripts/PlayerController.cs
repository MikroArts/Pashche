using InventorySystem;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerCharacter))]
public class PlayerController : MonoBehaviour {

    //private GameController gameController;
    private PlayerCharacter m_Character;      // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    
    private void Start()
    {
        //gameController = FindObjectOfType<GameController>();
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<PlayerCharacter>();

        //Cursor.visible = false;

    }

    private void Update()
    {
        if (!m_Jump)
        {
            m_Jump = Input.GetButtonDown("Jump");
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            transform.position = transform.position - new Vector3(.1f, .1f, .1f);
        }

        //TODO: Figure out how to save Player stats and progress

    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read inputs
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool crouch = Input.GetKey(KeyCode.C);

        // calculate move direction to pass to character
        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = (v * m_CamForward + h * m_Cam.right) / 2;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }
#if !MOBILE_INPUT
        // walk speed multiplier
        if (Input.GetKey(KeyCode.LeftShift)) m_Move *= 2f;
#endif

        // pass all parameters to the character control script
        m_Character.Move(m_Move, crouch, m_Jump);
        Take();
        Hit();
        EquipWeapon();
    }

    private void Take()
    {
        m_Jump = false;
        bool m;

        if (Input.GetButtonDown("Fire2"))
        {
            m = true;
        }
        else
        {
            m = false;
        }
        m_Character.GetComponent<Animator>().SetBool("IsTake", m);
    }
    private void Hit()
    {
        m_Jump = false;
        bool m;

        if (Input.GetButtonDown("Fire1"))
        {
            m = true;
        }
        else
        {
            m = false;
        }
        m_Character.GetComponent<Animator>().SetBool("isHit", m);
        
    }
    void EquipWeapon()
    {
        if (Input.GetMouseButtonDown(2))
        {
            if (Inventory.instance.equiped)
            {
                Inventory.instance.DequipWeapon("Sword");
            }
            else
            {
                Inventory.instance.EquipWeapon("Sword");
            }
        }
    }
}
