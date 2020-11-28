using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public Transform HealthBarTransform;
    public Transform targetTransform;
    public int CurrentValue;
    public float MaxValue;
    // Start is called before the first frame update
    void Start()
    {
        CurrentValue = 100;
    }

    // Update is called once per frame
    void Update()
    {
        if(targetTransform != null)
        {
            SetValue(CurrentValue);
            transform.position = targetTransform.position + Vector3.up;
        }
    }

    public void SetValue(int new_Value)
    {
        CurrentValue = new_Value;
        
        HealthBarTransform.localScale = new Vector3(CurrentValue / MaxValue, 1.0f, 1.0f);
    }
}
