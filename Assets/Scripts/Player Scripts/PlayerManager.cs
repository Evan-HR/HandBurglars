using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    // radius 
    public float handRadius;

    // movement constants; change from inspector
    public float walkSpeed, sneakSpeed, jumpSpeed;
    Transform handTransform, hingeTransform, bodyTransform;



    // other unity components of the player

    public Animator m_animator;
    public SpriteRenderer m_spriteRenderer;
    public Rigidbody2D m_rigidBody2D;

    public BoxCollider2D m_BoxCollider;
    public Rigidbody2D hand_rigidBody2D;

    // collider booleans; determines player movement state
    bool onGround, onLadder, on1WayGround;

    public LayerMask whatIsGround;

    // horizontal movement things ---------------------------------
    int moveHInput;
    bool facingRight = true;

    // movement booleans TEMPORARY -- KONG
    bool leftMove, rightMove;
    //jump things -------------------------------------------------
    // set through inspector
    bool jumpInput; //TEMPORARY -- KONG
    bool jumpHoldInput; // TEMPORARY -- KONG
    public float jumpMultiplier;
    public float fallMultiplier;
    public float lowJumpMultiplier;

    //climbing things ---------------------------------------------

    int moveVInput;
    bool isClimbing;

    public BoxCollider2D climber;
    public float climbSpeed;

    // more movement booleans TEMPORARY -- KONG
    bool upwardMove, downwardMove;
    

    //handMovement things ----------------------------------------

    Vector2 mousePos, handPos, bodyMouseVector;
    float bodyMouseDir, bodyMouseMag; // angle of the hand, magnitude of the hand

    Vector2 targetDir, handTarget;

    public float maxVelocity, maxForce, toVelocity, gain;

    SliderJoint2D PhysicsTranslationJoint;
    RelativeJoint2D MovementTranslationJoint;


    //grab things ------------------------------------------------
    //grab input booleans TEMPORARY  -- KONG
    bool grabInput, grabHoldInput, releaseInput;

    // for gripping animation
    public Animator hand_animator;

    public BoxCollider2D fist_box;
    
    bool isHolding, isFist, isSuspended;
    GameObject toGrabObject;
    GameObject heldObject;
    public FixedJoint2D handGrabJoint;
    public HingeJoint2D handDragHingeJoint;
    public SpringJoint2D handDragSpringJoint;
    Vector2 toGrabDist;
    Vector2 tempDist;

    enum HANDSTATE {
        Movement,
        Dragging,
        Suspended
    }

    HANDSTATE HandState;




    // class methods --------------------------------------------------------//
    // ----------------------------------------------------------------------//








    //--------------------------------------------------------------------------------------------COLLISION ENTER
    //-----------------------------------------------------------------------------------------------------------

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Ground")
        {
            onGround = true;
        } else if (collision.gameObject.tag == "1WayGround"){
            on1WayGround = true;
        }
     }

    void OnCollisionStay2D(Collision2D collision){
            if(collision.gameObject.tag == "Ground")
            {
                onGround = true;
            }
        }
//--------------------------------------------------------------------------------------------COLLISION EXIT
//----------------------------------------------------------------------------------------------------------
    void OnCollisionExit2D(Collision2D collision){
        if(collision.gameObject.tag == "Ground")
        {
            onGround = false;
        } else if (collision.gameObject.tag == "1WayGround"){
            on1WayGround = false;
        }
    }

    //public static void AccelerateTo(this Rigidbody body, Vector2 targetVelocity, float maxAccel)
    //{
    //    Vector2 deltaV = targetVelocity - (Vector2) body.velocity;
    //    Vector2 accel = deltaV/Time.deltaTime;

    //    if(accel.sqrMagnitude > maxAccel * maxAccel)
    //        accel = accel.normalized * maxAccel;

    //    body.AddForce(accel, ForceMode.Acceleration);
    //}
//--------------------------------------------------------------------------------------------START
//-------------------------------------------------------------------------------------------------
	// Use this for initialization
	void Start () {
        bodyTransform = this.gameObject.transform;
        hingeTransform = bodyTransform.GetChild(0);
        Debug.Log(hingeTransform.childCount);
        handTransform = hingeTransform.GetChild(0);
        PhysicsTranslationJoint = handTransform.gameObject.GetComponent<SliderJoint2D>();
        MovementTranslationJoint = handTransform.gameObject.GetComponent<RelativeJoint2D>();
        hand_rigidBody2D = handTransform.gameObject.GetComponent<Rigidbody2D>();
        JointTranslationLimits2D tempLimits = PhysicsTranslationJoint.limits;
        tempLimits.min = -handRadius;
        PhysicsTranslationJoint.limits = tempLimits;
        toGrabObject = null;
        HandState = HANDSTATE.Movement;

	}

//--------------------------------------------------------------------------------------------UPDATE
//--------------------------------------------------------------------------------------------------
	// Update is called once per frame
	void Update () {
        //----------------------------------------HORIZONTAL MOVEMENT UPDATE

        print(onGround);

        // TEMPMOVEINPUT START
        leftMove = Input.GetKey("a");
        rightMove = Input.GetKey("d");
        
        moveHInput = 0;
        if (leftMove) { moveHInput -=1; }
        if (rightMove) { moveHInput +=1; }
        
        // TEMPMOVEINPUT END

        m_animator.SetFloat("Speed",Mathf.Abs(moveHInput));

        // moveInput must be -1, 0, or 1. 
        m_rigidBody2D.AddForce(new Vector2(moveHInput * walkSpeed, 0));
        //print(m_rigidBody2D.velocity);
        //m_rigidBody2D.velocity = new Vector2(moveInput * walkSpeed, m_rigidBody2D.velocity.y);
        //print(onGround);
        if (moveHInput < 0 && facingRight){ 
            facingRight = false;
            m_spriteRenderer.flipX = true;
        } 
        else if (moveHInput > 0 && !facingRight){
            facingRight = true;
            m_spriteRenderer.flipX = false;
        }

        //------------------------------------------------------------------------------ JUMPING UPDATE

        if (m_BoxCollider.IsTouchingLayers(LayerMask.NameToLayer("Ground"))) {
            onGround = true;
        } else if (m_BoxCollider.IsTouchingLayers(LayerMask.NameToLayer("1WayGround"))) {
            on1WayGround = true;
        }
        jumpInput = Input.GetKeyDown(KeyCode.Space);
        jumpHoldInput = Input.GetKey(KeyCode.Space);
        if ((onGround || on1WayGround || onLadder) && jumpInput) {
            m_rigidBody2D.velocity = Vector2.up * jumpMultiplier;
            isClimbing = false;
        }
        /* 
        if (m_rigidBody2D.velocity.y < 0)
        {
            m_rigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier) * Time.deltaTime;
        }
        else if (m_rigidBody2D.velocity.y > 0 && !jumpHoldInput)
        {
            m_rigidBody2D.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier) * Time.deltaTime;
        }
        */

        //------------------------------------------------------------------------------------ CLIMBING UPDATE
        // if climbing: 

        // onLadder = GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.NameToLayer("Ladder"));
        // if (onLadder) {print("fuckyou");}
        // print(GetComponent<BoxCollider2D>().IsTouchingLayers(LayerMask.NameToLayer("PlayerBody")));\
        print(isClimbing);

        
        upwardMove = Input.GetKey("w");
        downwardMove = Input.GetKey("s");

        if (onLadder && (upwardMove || downwardMove)){
            isClimbing = true;
        }

        if (downwardMove && on1WayGround) {
            gameObject.layer = LayerMask.NameToLayer("PlayerBodyGoingDown");
        }

        if (gameObject.layer == LayerMask.NameToLayer("PlayerBodyGoingDown") && !downwardMove){
            gameObject.layer = LayerMask.NameToLayer("PlayerBody");
        }

        else if (!onLadder) {
            isClimbing = false;
        }

        if (isClimbing){
            // vertical input detection
            moveVInput = 0;
            if (upwardMove) { moveVInput += 1; }
            if (downwardMove) { moveVInput -= 1; }
            m_rigidBody2D.AddForce(new Vector2(0, moveVInput * climbSpeed));
            m_rigidBody2D.gravityScale = 0;
            hand_rigidBody2D.gravityScale = 0;
        }
            
            
        else {
            // gravity on
            m_rigidBody2D.gravityScale = 2;

        }
            

        //---------------------------------------- HAND POSITION UPDATE---------------------------------------------------

		mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bodyMouseVector = mousePos - (Vector2) gameObject.transform.position;
        bodyMouseDir = Mathf.Rad2Deg * Mathf.Atan2(bodyMouseVector.x, bodyMouseVector.y);
        bodyMouseMag = bodyMouseVector.magnitude;

        //while Dragging, more accurate physics is desired. 
        //When suspended to an object like a rope forces related to the hand should be disabled.
        if (HandState == HANDSTATE.Dragging){

            MovementTranslationJoint.enabled = false;
            PhysicsTranslationJoint.enabled = true;
            hand_rigidBody2D.mass = 1;
            hand_rigidBody2D.freezeRotation = false;

            if (bodyMouseMag > handRadius){
                handTarget = (Vector2) gameObject.transform.position + new Vector2(handRadius * Mathf.Sin(bodyMouseDir * Mathf.Deg2Rad), handRadius * Mathf.Cos(bodyMouseDir * Mathf.Deg2Rad));
            } else {
                handTarget = (Vector2) gameObject.transform.position + bodyMouseVector;
            }

            Vector2 dist = handTarget - (Vector2) handTransform.position;
            Vector2 targetVelocity = Vector2.ClampMagnitude(toVelocity * dist, maxVelocity);
            Vector2 error = targetVelocity-hand_rigidBody2D.velocity;
            Vector2 force = Vector2.ClampMagnitude(gain * error, maxForce);
            hand_rigidBody2D.AddForce(force);

        } else if (HandState == HANDSTATE.Movement){
            PhysicsTranslationJoint.enabled = false;
            MovementTranslationJoint.enabled = true;
            MovementTranslationJoint.linearOffset = new Vector2(0, Mathf.Min(bodyMouseMag, handRadius));
            //hingeTransform.rotation = Quaternion.Euler(0, 0, 180 - bodyMouseDir);
            handTransform.rotation = Quaternion.Euler(0, 0, 180 - bodyMouseDir);
            //handTransform.position = (Vector2) gameObject.transform.position + bodyMouseVector;
            //hand_rigidBody2D.mass = 0;
            hand_rigidBody2D.freezeRotation = true;
        }
        


        // --------------------------------------

        //tempLimits handSliderJoint.limits

        //----------------------------------------------------- HAND GRAB UPDATE---------------------------------------
        //get click, get click down
        //TEMPORARY GRAB INPUTS -- KONG
        grabHoldInput = Input.GetMouseButton(0);
        grabInput = Input.GetMouseButtonDown(0);
        releaseInput = Input.GetMouseButtonUp(0);
        

        //if click and hand is free:
        if (grabInput && !isHolding){
            if (toGrabObject != null){
                heldObject = toGrabObject;
                //print("step 1");
                if (toGrabObject.layer == LayerMask.NameToLayer("HandObjectGrab")){
                    handGrabJoint.enabled = true;
                    handGrabJoint.connectedBody = heldObject.GetComponent<Rigidbody2D>();
                } else if (toGrabObject.layer == LayerMask.NameToLayer("HandObjectDrag")){

                    // Drag Hinge Joint formation
                    handDragHingeJoint.enabled = true;
                    handDragHingeJoint.connectedBody = heldObject.GetComponent<Rigidbody2D>();

                    // Drag Spring Joint formation
                    handDragSpringJoint.enabled = true;
                    handDragSpringJoint.connectedBody = heldObject.GetComponent<Rigidbody2D>();

                    //Change isSuspended to TRUE:
                    if (heldObject.CompareTag("Rope")){
                        HandState = HANDSTATE.Suspended;
                    } else {
                        HandState = HANDSTATE.Dragging;
                    }
                }
                
                

                isHolding = true;
            }
                    
            else {
                isFist = true;
            }
        }
        //if click is held:
        else if (grabHoldInput){
            if (isHolding){
                //print("step 2");
                //keep holding
            } else {
                // stay in fist mode
            }
        }

        //if unclick:
        else if (releaseInput){
            if (isFist){
                //exit fist mode
                isFist = false;
            }
            else if (isHolding){
                //print("step 3");
                if (handGrabJoint.enabled){
                    handGrabJoint.enabled = false;
                    handGrabJoint.connectedBody = null;
                } else if (handDragHingeJoint.enabled){

                    //Drag Hinge Joint Termination
                    handDragHingeJoint.enabled = false;
                    handDragHingeJoint.connectedBody = null;

                    //Drag Spring Joint Termination
                    handDragSpringJoint.enabled = false;
                    handDragSpringJoint.connectedBody = null;

                    HandState = HANDSTATE.Movement;
                }
                
                heldObject = null;
                isHolding = false;
            }
        }
        hand_animator.SetBool("isGrip", (isFist || isHolding));
        if (isFist){ fist_box.enabled = true;}
        else { fist_box.enabled = false;}

            
    }

    public void HandTriggerEnter2D(Collider2D other){
        if (toGrabObject == null){
            toGrabObject = other.gameObject;
            toGrabDist = other.gameObject.transform.position - handTransform.position;

        } 
        else {
            tempDist = other.gameObject.transform.position - handTransform.position;
            if (tempDist.magnitude < toGrabDist.magnitude){
                toGrabObject = other.gameObject;
            }
        } 
    }

    public void HandTriggerStay2D(Collider2D other){
        if (other.gameObject != toGrabObject){
                tempDist = other.gameObject.transform.position - handTransform.position;
                if (tempDist.magnitude < toGrabDist.magnitude){
                    toGrabObject = other.gameObject;
                }
        }
        
    }

    public void HandTriggerExit2D(Collider2D other){
        if (other.gameObject == toGrabObject){
            //print("it worked");
            toGrabObject = null;
        }
    }

    public void LadderEnter(){
        onLadder = true;
    }

    public void LadderExit(){
        onLadder = false;
    }
}


