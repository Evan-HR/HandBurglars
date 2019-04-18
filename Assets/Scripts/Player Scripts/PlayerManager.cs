using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using InControl;

public class PlayerManager : MonoBehaviour {
    private GameManager gameManager;
    private PlayerData player1Data;
    private PlayerData player2Data;


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

    public CircleCollider2D m_CircleCollider;
    public Rigidbody2D hand_rigidBody2D;
    public SpriteRenderer hand_spriteRenderer;

    // collider booleans; determines player movement state
    bool onGround, onLadder, on1WayGround, onFist;

    public LayerMask whatIsGround;

    // horizontal movement things ---------------------------------
    float moveHInput;
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

    // is true only when character is in a state where they can jump
    bool canJump;

    // set to true in Update when a character is going to jump, set to false in fixedUpdate when jumping is performed. 
    bool doJump;

    //climbing things ---------------------------------------------

    float moveVInput;
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


    //flag of controller type
    private bool isController;

    private int indexDevice;

    enum HANDSTATE {
        Movement,
        Dragging,
        Suspended
    }

    HANDSTATE HandState;

    // hiding things

    bool isHiding;
    bool inCover;
    bool hideInput;

    LayerMask tempLayer;
    String tempHandTag;

    // health things

    //DUSTIN EDIT --> made public int health to work with healthUI, used to be PRIVATE!
    public int health;
    bool isDead;

    public int damage_coolDown;
    public bool canBeHit;

    // death things
    Vector2 respawnPos;

    public GameObject tombstone;

    GameObject myTombstone;

    // class methods --------------------------------------------------------//
    // ----------------------------------------------------------------------//








//--------------------------------------------------------------------------------------------COLLISION ENTER
//-----------------------------------------------------------------------------------------------------------

    void OnCollisionEnter2D(Collision2D collision){
        if(collision.gameObject.tag == "Fist1" || collision.gameObject.tag == "Fist2")
        {
            if (m_CircleCollider.IsTouchingLayers(LayerMask.GetMask("Hand1", "Hand2"))) {
            onFist = true;
            print("on fist is true");
        } else {
            onFist = false;
            }
        }
    }

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

        //jump stuff
        canJump = false;
        print("ONFIST IS" + onFist);
        doJump = false;

        // hide stuff
        isHiding = false;
        tempLayer = this.gameObject.layer;
        tempHandTag = handTransform.gameObject.tag;

        // health
        health = 3;
        isDead = false;
        canBeHit = true;

        // death
        respawnPos = GameObject.Find("Respawn").transform.position;

        gameManager = GameManager.Instance;
        if (GameManager.playerDataDict.Count == 0)
        {
            Debug.Log("PlayerManager playerDataDic.Count == 0");
            GameManager.playerDataDict.Add(1, new PlayerData(PlayerData.ControlDevice.KEYBOARD, "Bru"));
            GameManager.playerDataDict.Add(2, new PlayerData(PlayerData.ControlDevice.CONTROLLER, "Jotun"));
        }
        Debug.Log("size of dic PlayerManager: " + GameManager.playerDataDict.Count);



        if (GameManager.playerDataDict[1].characterName.Equals("Bru"))
        {
            player1Data = GameManager.playerDataDict[2];
            player2Data = GameManager.playerDataDict[1];
        }
        else
        {
            player1Data = GameManager.playerDataDict[1];
            player2Data = GameManager.playerDataDict[2];
        }

        Debug.Log("player1 controller type:" + player1Data.controlDevice);
        Debug.Log("player2 controller type:" + player2Data.controlDevice);
        Debug.Log("devices count:" + InputManager.Devices.Count);
        Debug.Log("current gameobject tag:" + this.gameObject.tag);

        if (this.gameObject.tag == "Player1")
        {
            switch (player1Data.controlDevice)
            {
                case PlayerData.ControlDevice.KEYBOARD:
                    isController = false;
                    break;
                default:
                    isController = true;
                    indexDevice = 0;
                    break;
            }
        }
        else if (this.gameObject.tag == "Player2")
        {
            switch (player2Data.controlDevice)
            {
                case PlayerData.ControlDevice.KEYBOARD:
                    isController = false;
                    break;
                default:
                    isController = true;
                    if (player1Data.controlDevice == PlayerData.ControlDevice.KEYBOARD)
                    {
                        indexDevice = 0;
                    }
                    else
                    {
                        indexDevice = 1;
                    }

                    break;
            }
        }
    }

    void Awake(){

        //more else case if more than two players
    }

//--------------------------------------------------------------------------------------------UPDATE
//--------------------------------------------------------------------------------------------------
	// Update is called once per frame
	void Update () {
        //----------------------------------------HORIZONTAL MOVEMENT UPDATE

        //print(onGround);
    //print(onLadder);
        moveHInput = 0;

        // keyboard
        if (!isController){
            leftMove = Input.GetKey("a");
            rightMove = Input.GetKey("d");
        } else {
            moveHInput = InputManager.Devices[indexDevice].LeftStickX;
        }
        
        
        // controller 

        // end controller

        
        if (leftMove) { moveHInput -=1; }
        if (rightMove) { moveHInput +=1; }
        
        // TEMPMOVEINPUT END

        m_animator.SetFloat("Speed",Mathf.Abs(moveHInput));

        // moveInput must be -1, 0, or 1. 
        // m_rigidBody2D.AddForce(new Vector2(moveHInput * walkSpeed, 0));
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

        if (m_CircleCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Water"))) {
            onGround = true;
            print("onGround is true");
        } else {
            onGround = false;
        }
        if (m_CircleCollider.IsTouchingLayers(LayerMask.GetMask("1WayGround"))) {
            on1WayGround = true;
            print("on1Way is true");
        } else {
            on1WayGround = false;
        }
        if (!m_CircleCollider.IsTouchingLayers(LayerMask.GetMask("Hand1", "Hand2"))) {
            onFist = false;
        }

        if (!isController) {
            jumpInput = Input.GetKeyDown(KeyCode.Space);
            jumpHoldInput = Input.GetKey(KeyCode.Space);
        }else{
            jumpInput = InputManager.Devices[indexDevice].LeftTrigger.WasPressed;
            jumpHoldInput = InputManager.Devices[indexDevice].LeftTrigger.IsPressed;
        }
        
        if ((onGround || on1WayGround || isClimbing || onFist )) {
            canJump = true;
            print(canJump);
        } else {
            canJump = false;
        }

        if (canJump && m_rigidBody2D.velocity.y <= jumpMultiplier && jumpInput){
            FindObjectOfType<AudioManager>().Play("jump");
            doJump = true;
            isClimbing = false;
        }

        // if (!canJump && m_rigidBody2D.velocity.y < 0 && !isSuspended)
        // {
        //     m_rigidBody2D.velocity -=(Vector2.up * (fallMultiplier - 1));
        // }
        // else if (!canJump && m_rigidBody2D.velocity.y > 0 && !jumpHoldInput && !isSuspended)
        // {
        //     m_rigidBody2D.velocity -= (Vector2.up * (lowJumpMultiplier - 1));
        // }

        //------------------------------------------------------------------------------------ CLIMBING UPDATE
        // if climbing: 

        //print(isClimbing);

        if (!isController) {
            upwardMove = Input.GetKey("w");
            downwardMove = Input.GetKey("s");
        }else {
            upwardMove = InputManager.Devices[indexDevice].LeftStickY;
            downwardMove = InputManager.Devices[indexDevice].LeftStickY;
        }
        

        if (onLadder && (upwardMove || downwardMove)){
            isClimbing = true;
        }
        //print("downward move: " + downwardMove + ", on1WayGround: " + on1WayGround);
        if (downwardMove && on1WayGround) {
            gameObject.layer = LayerMask.NameToLayer("PlayerBodyGoingDown");
        }

        if (gameObject.layer == LayerMask.NameToLayer("PlayerBodyGoingDown") && !downwardMove){
            gameObject.layer = tempLayer;
        }

        else if (!onLadder) {
            isClimbing = false;
        }

        // if (isClimbing){
        //     // vertical input detection
        //     if (!isController) {
        //         moveVInput = 0;
        //         if (upwardMove) { moveVInput += 1; }
        //         if (downwardMove) { moveVInput -= 1; }
        //     } else {
        //         moveVInput = InputManager.Devices[indexDevice].LeftStickY;
        //     }
        //     m_rigidBody2D.AddForce(new Vector2(0, moveVInput * climbSpeed));
        //     m_rigidBody2D.gravityScale = 0;
        //     hingeTransform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
        //     hand_rigidBody2D.gravityScale = 0;
        // }
            
            
        // else {
        //     // gravity on
        //     m_rigidBody2D.gravityScale = 2;
        //     hingeTransform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        //     hand_rigidBody2D.gravityScale = 1;

        // }
        

        //---------------------------------------- HEALTH UPDATE ---------------------------------------------------------
        if (health <= 0){
            isDead = true;
        }

        if (isDead){

            isHolding = false;
            toGrabObject = null;
            heldObject = null;

            Vector2 tempPos = this.transform.position;
            this.gameObject.SetActive(false);
            //spawn a tombstone at current location
            
            myTombstone = Instantiate(tombstone, transform.position,Quaternion.identity);
            this.transform.position = respawnPos;
            Invoke("Respawn", 3.0f);
            //decrement global health
            GameManager.globalHealthLoss();
            



        }



        //---------------------------------------- HAND POSITION UPDATE---------------------------------------------------

        if (!isController) {
            mousePos = (Vector2) Camera.main.ScreenToWorldPoint(Input.mousePosition);
            bodyMouseVector = mousePos - (Vector2)gameObject.transform.position;
        }
        else {
            bodyMouseVector = new Vector2(InputManager.Devices[indexDevice].RightStickX, InputManager.Devices[indexDevice].RightStickY);
            bodyMouseVector = bodyMouseVector * handRadius;
        }
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
            try{
                PhysicsTranslationJoint.enabled = false;
                MovementTranslationJoint.enabled = true;
                MovementTranslationJoint.linearOffset = new Vector2(0, Mathf.Min(bodyMouseMag, handRadius));
                //hingeTransform.rotation = Quaternion.Euler(0, 0, 180 - bodyMouseDir);
                handTransform.rotation = Quaternion.Euler(0, 0, 180 - bodyMouseDir);
                //handTransform.position = (Vector2) gameObject.transform.position + bodyMouseVector;
                //hand_rigidBody2D.mass = 0;
                hand_rigidBody2D.freezeRotation = true;
            }catch (Exception e){
                Debug.Log(e.StackTrace);
            }
        } else if (HandState == HANDSTATE.Suspended){
            MovementTranslationJoint.enabled = false;
            PhysicsTranslationJoint.enabled = true;
            hand_rigidBody2D.freezeRotation = false;
        }
        


        // --------------------------------------

        //tempLimits handSliderJoint.limits

        //----------------------------------------------------- HAND GRAB UPDATE---------------------------------------
        //get click, get click down
        //TEMPORARY GRAB INPUTS -- KONG
        if (!isController) {
            grabHoldInput = Input.GetMouseButton(0);
            grabInput = Input.GetMouseButtonDown(0);
            releaseInput = Input.GetMouseButtonUp(0);
        }else {
            grabHoldInput = InputManager.Devices[indexDevice].RightTrigger.IsPressed;
            grabInput = grabHoldInput;
            releaseInput = !grabInput;
        }

        

        //if click and hand is free:
        if (grabInput && !isHolding){
            // if there is something to grab
            if (toGrabObject != null){
                heldObject = toGrabObject;
                // if it is a handGrab Object it must be ungrabbed (lock system)
                if (toGrabObject.layer == LayerMask.NameToLayer("HandObjectGrab")){
                   if (heldObject.CompareTag("ungrabbed")) {
                         handGrabJoint.enabled = true;
                        handGrabJoint.connectedBody = heldObject.GetComponent<Rigidbody2D>();
                        heldObject.tag = "grabbed";
                   }
                
                // draggable can be lifted with friend
                } else if (toGrabObject.layer == LayerMask.NameToLayer("HandObjectDrag(Works)")){
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
            // if there is nothing to grab, hand input is down, nothing held
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
                    heldObject.tag = "ungrabbed";
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
        } else if (!grabHoldInput && isFist){
            isFist = false;
        } else if (!grabHoldInput && isHolding){
            isHolding = false;
            handGrabJoint.enabled = false;
            handGrabJoint.connectedBody = null;
            handDragHingeJoint.enabled = false;
            handDragHingeJoint.connectedBody = null;
            handDragSpringJoint.enabled = false;
            handDragSpringJoint.connectedBody = null;
        }

        // animator update
        hand_animator.SetBool("isGrip", (isFist || isHolding));

        // fist update
        if (isFist){ 
            fist_box.enabled = true;
            if (tempHandTag == "Hand1"){
                handTransform.gameObject.tag = "Fist1";
            } else if (tempHandTag == "Hand2") {
                handTransform.gameObject.tag = "Fist2";
            }

        }
        else { 
            fist_box.enabled = false;
            handTransform.gameObject.tag = tempHandTag;
        }

        // ----------------------------------------------------------------- HIDE UPDATE ------------------------------
        if (!isController){
            hideInput = Input.GetKey("q");
        } else {
            hideInput = InputManager.Devices[indexDevice].Action3;
        }
        
        if (inCover){
            if (hideInput) {
                isHiding = true;
                this.gameObject.layer = LayerMask.NameToLayer("HiddenPlayerBody");
                // grey out player
                m_spriteRenderer.color = new Color32(0,0,0,184);
                hand_spriteRenderer.color = new Color32(0,0,0,184);
                // restrict movement -- Player State
        }
            }
                

        if ((isHiding && !hideInput) || (isHiding && !inCover)){
            isHiding = false;
            this.gameObject.layer = tempLayer;
            //enable movement
            m_spriteRenderer.color = new Color32(255,255,255,255);
            hand_spriteRenderer.color = new Color32(255,255,255,255);
        }
            
            
    }
    
    // -------------------------------------------------------------------------------- FIXED UPDATE ---------------------------------------------------------------
    // -------------------------------------------------------------------------------------------------------------------------------------------------------------
    // use for physics related changes that require constant fps
    public void FixedUpdate() {


        //walking update
        m_rigidBody2D.AddForce(new Vector2(moveHInput * walkSpeed, 0));

        //jumping update
        if (doJump){
            m_rigidBody2D.velocity += Vector2.up * jumpMultiplier;
            doJump = false;
        }

        // falling
        if (!canJump && m_rigidBody2D.velocity.y < 0 && !jumpHoldInput && !isClimbing && HandState != HANDSTATE.Suspended)
        {
            m_rigidBody2D.velocity -=(Vector2.up * (fallMultiplier - 1));
        }

        // low jump
        else if (!canJump && m_rigidBody2D.velocity.y < 0 && !isClimbing && HandState != HANDSTATE.Suspended)
        {
            m_rigidBody2D.velocity -= (Vector2.up * (lowJumpMultiplier - 1));
        }
        
        // climbing ladder
        if (isClimbing){
            // vertical input detection
            if (!isController) {
                moveVInput = 0;
                if (upwardMove) { moveVInput += 1; }
                if (downwardMove) { moveVInput -= 1; }
            } else {
                moveVInput = InputManager.Devices[indexDevice].LeftStickY;
            }
            m_rigidBody2D.AddForce(new Vector2(0, moveVInput * climbSpeed));
            m_rigidBody2D.gravityScale = 0;
            hingeTransform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            hand_rigidBody2D.gravityScale = 0;
        } else {
            // gravity on
            m_rigidBody2D.gravityScale = 2;
            hingeTransform.gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
            hand_rigidBody2D.gravityScale = 1;

        }

        // end of fixed update
    }

    public void HandTriggerEnter2D(Collider2D other){
        if (toGrabObject == null){
                toGrabObject = other.gameObject;
                toGrabDist = other.gameObject.transform.position - handTransform.position;
        } else {
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

    public void CoverEnter(){
        inCover = true;
    }

    public void CoverExit(){
        inCover = false;
    }

    public void takeDamage(int dmg_val){
        FindObjectOfType<AudioManager>().Play("lostHealth");
        health -= dmg_val;
        canBeHit = false;
        Invoke("resetCooldown", damage_coolDown);
        m_spriteRenderer.color = new Color32(255,0,0,255);
        Invoke("resetColor", 0.25f);
    }

        public bool getHide(){
            return isHiding;
    }

    public void resetCooldown(){
        canBeHit = true;
    }

    public void resetColor(){
        m_spriteRenderer.color = new Color32(255,255,255,255);
    }

    public void Respawn(){
        // if health is zero
        if (this.gameObject.activeSelf == false){
            this.gameObject.SetActive(true);
            //myTombstone.SetActive(false);
            health += 3;
            isDead = false;
        // if you fall off an edge
        } else {
            health -= 3;
            GameManager.globalHealthLoss();
            isDead = true;
            this.gameObject.SetActive(false);
            this.transform.position = respawnPos;
            Invoke("Respawn", 3.0f);

        }
    }
}


