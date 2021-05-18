using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RasenganAnimationBehav : MonoBehaviour
{
    public void DestroyAfterAnim()
    {
        GetComponentInParent<ThrowableBehaviour>().DestroyFromChild();
    }
}
