using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallJumpUpgrade : Upgrade
{
    static bool used = false;

    private void Awake()
    {
        if (used)
        {
            Destroy(gameObject);
        }
    }
    protected override void GiveUpgrade(PlayerMovement mover)
    {
        mover.canWallJump = true;
        used = true;
    }
}
