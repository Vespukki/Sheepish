using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Upgrade : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out PlayerMovement mover))
        {
            GiveUpgrade(mover);
            PlayPowerupSound(mover);
            Destroy(gameObject);
        }
    }

    protected abstract void GiveUpgrade(PlayerMovement mover);

    void PlayPowerupSound(PlayerMovement mover)
    {
        mover.gameObject.GetComponent<AudioSource>().PlayOneShot(mover.audioBank.upgrade);
    }
}
