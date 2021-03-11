using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMan : Man
{
    private Animator anim;

    public override void sedia()
    {
        anim.SetBool("sedia", true);
        base.sedia();

    }
    public override void senangdiri()
    {
        anim.SetBool("sedia", false);
        base.senangdiri();
    }
    public override void lift_left_leg() {
        anim.SetInteger("march", 0);
        anim.SetInteger("lift", -1);
    }
    public override void lift_right_leg() {
        anim.SetInteger("march", 0);
        anim.SetInteger("lift", 1);
    }

    public override void stomp() {
        anim.SetInteger("lift", 0);
    }

    public override void march_left() {
        anim.SetInteger("march", -1);
    }
    public override void march_right() {
        anim.SetInteger("march", 1);
    }


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("sedia", false);
        //senangdiri();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
