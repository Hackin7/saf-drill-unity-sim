using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeMan : Man
{
    public override void sedia()
    {
        base.sedia();
        transform.localScale = new Vector3(1f, 2f, 1f);

    }
    public override void senangdiri()
    {
        base.senangdiri();
        Debug.Log(state);
        transform.localScale = new Vector3(1f, 1f, 1f);
    }
    public override void lift_left_leg() { base.lift_left_leg();  transform.Translate(new Vector3(0f, 1f, 0f)); }
    public override void lift_right_leg() { base.lift_right_leg(); transform.Translate(new Vector3(0f, 1f, 0f)); }
    public override void stomp() { base.stomp();  transform.Translate(new Vector3(0f, -1f, 0f)); }
    public override void turn_right() { base.turn_right();  startTiming(); transform.Rotate(0f, 90.0f, 0.0f, Space.Self); }
    public override void turn_left() { base.turn_left(); startTiming(); transform.Rotate(0f, -90.0f, 0.0f, Space.Self); }

    public override void march_left() { base.march_left();  transform.Rotate(0f, 10.0f, 0.0f, Space.Self); }
    public override void march_right() { base.march_right(); transform.Rotate(0f, -10.0f, 0.0f, Space.Self);  }
}
