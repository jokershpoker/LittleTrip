using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public Animator SkinAnimator;
    public GameManager GM;
    CapsuleCollider selfCollider;
    Rigidbody rb;

    public Transform PlayerTransf;

    public float JumpSpeed = 12;

    public float SideSpeed;

    bool isRolling = false;
    bool falling = false;

    Vector3 ccCenterNorm = new Vector3(0, 1f, 0),
            ccCenterRoll = new Vector3(0, .2f, 0),
            StartPlayerPos;

    float ccHeightNorm = 2,
          ccHeightRoll = .4f;

    bool wannaJump = false;

    void Start()
    {
        selfCollider = GetComponent<CapsuleCollider>();
        rb = GetComponent<Rigidbody>();

        StartGame();
    }

    private void FixedUpdate()
    {
        rb.AddForce(new Vector3(0, Physics.gravity.y * 4, 0), ForceMode.Acceleration);

        if (wannaJump && isGrounded())
        {
            
            SkinAnimator.SetTrigger("jumping");
            rb.AddForce(new Vector3(0, JumpSpeed, 0), ForceMode.Impulse);
            wannaJump = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isGrounded())
        {
            SkinAnimator.ResetTrigger("falling");

            if (GM.CanPlay)
            {
                if (!isRolling)
                {
                    if (Input.GetAxisRaw("Vertical") > 0)
                        wannaJump = true;
                    else if (Input.GetAxisRaw("Vertical") < 0)
                        StartCoroutine(DoRoll());

                }
            }
        }
        
            SkinAnimator.SetBool("falling", rb.velocity.y < -8);   

        Vector3 Pos = transform.position;
        Pos.z = 0;
        transform.position = Pos;
    }

    bool isGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.05f);
    }

    public void StartGame()
    {
        
    }


    IEnumerator DoRoll()
    {
        isRolling = true;
        SkinAnimator.SetBool("rolling", true);
       
        selfCollider.center = ccCenterRoll;
        selfCollider.height = ccHeightRoll;

        yield return new WaitForSeconds(1.5f);
                
        SkinAnimator.SetBool("rolling", false);

        selfCollider.center = ccCenterNorm;
        selfCollider.height = ccHeightNorm;

        yield return new WaitForSeconds(.3f);

        isRolling = false;
    }

    IEnumerator JumpFalling()
    {
        falling = true;
        SkinAnimator.SetBool("falling", true);
        Debug.Log("falling");

        yield return new WaitForSeconds(.5f);

        falling = false;
        SkinAnimator.SetBool("falling", false);
        Debug.Log("falling");
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.gameObject.CompareTag("Trap") || !GM.CanPlay)
            return;



        StartCoroutine(GameOver());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if ((!collision.gameObject.CompareTag("Trap")&&
            !collision.gameObject.CompareTag("DeathPlane")) || 
            !GM.CanPlay)
            return;
        if (GM.CanPlay == false)
        {
            return;
        }

        StartCoroutine(GameOver());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Coin5"))
            return;
        GM.AddCoins(5);
        Destroy(other.gameObject);
    }



    IEnumerator GameOver()
    {
        GM.CanPlay = false;

        SkinAnimator.SetTrigger("death");

        yield return new WaitForSeconds(2);

        SkinAnimator.ResetTrigger("death");

        GM.ShowResult();
        
    }

}
