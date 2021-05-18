using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGamePauseMenuBehav : MonoBehaviour
{
    public Animator sAnim;

    private void Start()
    {
        sAnim = GetComponent<Animator>();
    }
    public void Pause()
    {
        sAnim.SetTrigger("pause");
    }
    public void Unpause()
    {
        sAnim.SetTrigger("unpause");
        sAnim.ResetTrigger("pause");
    }
}
