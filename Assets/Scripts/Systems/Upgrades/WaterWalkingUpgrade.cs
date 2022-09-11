using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterWalkingUpgrade : Upgrade
{
    static bool used = false;

    private void Awake()
    {
        if(used)
        {
            Destroy(gameObject);
        }
    }
    protected override void GiveUpgrade(PlayerMovement mover)
    {
        mover.canWaterWalk = true;
        used = true;
    }
}
