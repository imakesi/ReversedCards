using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSine : MonoBehaviour {
    public float SineHeight = 2f;
    public float SineSpeed = 1f;
    public Transform OgPos;

    private void Start() {
      OgPos = transform;
    }

    private void Update() {
      float SineValue = Mathf.Sin(Time.time * SineSpeed) * SineHeight;
      transform.position = OgPos.position + (Vector3.up * SineValue);
    }
}
