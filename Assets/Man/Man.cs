using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Man : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Timing /////////////////////////////////////////////////
    protected float prevTime = 0f;
    protected float speed = 1.0f;
    protected float speedMultiplier = 1.5f; // To configure the 0-1 mark
    // Timing /////////////////////////////////////////
    protected void startTiming()
    {
        prevTime = 0f;
    }

    public virtual bool isFinishedMotion()
    {
        //Debug.Log(prevTime);
        prevTime += Time.deltaTime;
        return prevTime >= (0.125f / (speed * speedMultiplier)); //Screw matching the end of animation to the song, just wait longer
    }

    public virtual void setSpeed(float givenSpeed)
    {
        speed = givenSpeed;
        //anim.speed = (speed * speedMultiplier);
    }
    public virtual float getSpeed()
    {
        return speed;
    }

    protected int state = 0; // Sedia, Senangdiri, tahat etc.
    // States ////////////////////////////////////////////////
    public int getState() { return state; }

    public virtual void sedia(){ startTiming(); state = 1; }
    public virtual void senangdiri() { startTiming(); state = 0; }
    public virtual void lift_left_leg() { startTiming(); }
    public virtual void lift_right_leg() { startTiming(); }
    public virtual void stomp() { startTiming(); }

    public virtual void tahat() { startTiming(); state = 3;}
    public virtual void hormat() { startTiming(); state = 4; }
    public virtual void fist() { startTiming(); state = 4; }
    public virtual void headUpTilt() { startTiming(); }
    public virtual void headNormalTilt() { startTiming(); }
    public virtual void stepForward(float distance) { startTiming(); }
    public virtual void stepBack(float distance) { startTiming(); }

    // Rotation //////////////////////////////////////////////
    public virtual void turn_right(){ startTiming(); }
    public virtual void turn_left(){ startTiming(); }
    public virtual void turn_back() { startTiming(); }

    // March ////////////////////////////////////////////////
    public virtual void march_left() { startTiming(); }
    public virtual void march_right() { startTiming(); }
    public virtual void hentak_left() { startTiming(); }
    public virtual void hentak_right() { startTiming(); }
    
    // Say
    protected string saying;
    public virtual void say(string message)
    {
        startTiming();
        saying = message;
        Debug.Log(message);
    }
}
