using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTrigger : MonoBehaviour
{
    Animator TriangleAnim;

    private void Start() {
        TriangleAnim = GetComponent<Animator>();  
    }

    public void AnimTrigger() {
        TriangleAnim.ResetTrigger("TriangleStopTrigger");
        TriangleAnim.SetTrigger("TriangleAnimTrigger");
    }
    
    public void StopTrigger() {
        TriangleAnim.ResetTrigger("TriangleAnimTrigger");
        TriangleAnim.SetTrigger("TriangleStopTrigger");
    }
}
