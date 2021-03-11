using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Humanoid: Man
{
    private Animator anim;
    private int leg = 0;
    private int direction = 0;

    public override void setSpeed(float givenSpeed)
    {
        base.setSpeed(givenSpeed);
        anim.speed = (givenSpeed * speedMultiplier);
    }
    public override bool isFinishedMotion()
    {
        return base.isFinishedMotion();
    }
    private void verticalCorrection()
    {
        transform.position = transform.position + new Vector3(0f, 0.02f, 0f);
    }
    // Commands //////////////////////////////////////
    public override void sedia()
    {
        if (state == 3 || state == 4) { anim.SetInteger("Action", 51); }
        base.sedia();

    }
    public override void senangdiri()
    {
        anim.SetInteger("Action", 23);
        base.senangdiri();
    }
    public override void lift_left_leg() {
        anim.SetInteger("Action", 11);
        leg = 1;
        base.lift_left_leg();
        verticalCorrection();
    }
    public override void lift_right_leg() {
        anim.SetInteger("Action", 12);
        leg = 2;
        base.lift_right_leg();
        verticalCorrection();
    }

    public override void headUpTilt()
    {
        base.headUpTilt();
        anim.SetInteger("Head", 1);
    }
    public override void headNormalTilt()
    {
        base.headNormalTilt();
        anim.SetInteger("Head", 0);
    }

    public override void stomp() {
        base.stomp();
        anim.SetInteger("Action", 20 + leg);
        verticalCorrection();
    }

    public override void tahat()
    {
        base.tahat();
        anim.SetInteger("Action", 50);
    }
    public override void hormat()
    {
        base.hormat();
        anim.SetInteger("Action", 52);
    }
    public override void fist()
    {
        base.hormat();
        anim.SetInteger("Action", 53);
    }
    public override void stepForward(float distance)
    {
        base.stepForward(distance);
        transform.Translate(new Vector3(0f, 0f, -distance));
    }

    public override void stepBack(float distance)
    {
        base.stepBack(distance);
        transform.Translate(new Vector3(0f, 0f, distance));
    }

    public override void march_left() {
        base.march_left();
        anim.SetInteger("Action", 61);
    }
    public override void march_right() {
        base.march_right();
        anim.SetInteger("Action", 62);
    }
    IEnumerator alternateDirections (int givenLeg)
    {
        anim.speed = (speed * speedMultiplier * 4);

        anim.SetInteger("Action", 20 + leg);
        yield return new WaitForSeconds(0.2f);
        leg = givenLeg;
        anim.SetInteger("Action", 10 + leg);
        yield return new WaitForSeconds(0.1f);

        anim.speed = (speed * speedMultiplier);
    }
    public override void hentak_left()
    {
        base.hentak_left();
        StartCoroutine(alternateDirections(2));
        verticalCorrection();
    }
    public override void hentak_right()
    {
        base.hentak_right();
        StartCoroutine(alternateDirections(1));
        verticalCorrection(); 
    }

    public override void turn_left()
    {
        base.turn_left();
        direction--; if (direction < 0) { direction += 4; }
        anim.SetInteger("Action", 31);
    }

    public override void turn_right()
    {
        base.turn_right();
        direction++; if (direction > 4) { direction -= 4; }
        anim.SetInteger("Action", 32);
    }
    public override void turn_back()
    {
        base.turn_back();
        direction-=2; if (direction < 0) { direction += 4; }
        anim.SetInteger("Action", 33);
    }

    public override void say(string message)
    {
        base.say(message);
        StartCoroutine(speechStretch());
    }

    IEnumerator speechStretch()
    {
        transform.localScale = new Vector3(1f, 1.1f, 1f);
        yield return new WaitForSeconds(0.1f / speed);
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        setSpeed(speed);
        //senangdiri();
    }

    // Update is called once per frame
    void Update()
    {
        // Correcting direction

        if (
            (anim.GetInteger("Action") / 10 == 1 || anim.GetInteger("Action") / 10 == 2) &&
            anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
        {
            float rotationY = 0;
            switch (direction)
            {
                case 0: rotationY = 0f; break;
                case 1: rotationY = 90f; break;
                case 2: rotationY = 180f; break;
                case 3: rotationY = 270f; break;
            }
            transform.rotation = Quaternion.Euler(0, rotationY, 0);
        }
        
    }
}
