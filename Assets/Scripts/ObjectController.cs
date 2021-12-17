using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExpandState
{
    Spawning,
    Despawning,
    Expanding,
    Condensing,
    Idle
}

public class ObjectController : MonoBehaviour
{
    public ExpandState expandState = ExpandState.Spawning;

    private float step;
    private Vector3 target;
    private Vector3 originalScale;
    private bool canExpand = false;

    // Start is called before the first frame update
    void Start()
    {
        target = Random.insideUnitCircle * Generate.instance.radius * 2.5f;
        
        // offset X by a stupid amount so that by the time it gets to its target it has already hit a collider. this keeps it from despawning in view of the camera
        if (Mathf.Abs(target.x) == target.x)
        {
            target += new Vector3(50, 0);
        }
        else
        {
            target -= new Vector3(50, 0);
        }
        originalScale = transform.localScale;
        transform.localScale = new Vector3(.01f, .01f, .01f);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        UpdateExpand();
    }

    private void Move()
    {
        float step = Generate.instance.speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target, step);
    }

    private void UpdateExpand()
    {
        step = Mathf.Pow(Generate.instance.speed * Time.deltaTime, (Generate.instance.maxSize - step) * Generate.instance.scaleSpeedReduce); // calculate distance to move
        if (expandState == ExpandState.Spawning)
        {
            float step = Generate.instance.speed * Time.deltaTime * Generate.instance.spawnScaleSpeedIncrease;
            transform.localScale += new Vector3(step, step, step);
            if (transform.localScale.x >= 1f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f);
                expandState = ExpandState.Idle;
            }
        }
        else if (expandState == ExpandState.Despawning)
        {
            transform.localScale -= new Vector3(step, step, step);
            if (transform.localScale.x <= 0)
            {
                Destroy(gameObject); 
            }
        }
        else if (expandState == ExpandState.Expanding && canExpand)
        {
            transform.localScale += new Vector3(step, step, step);
            if (transform.localScale.x >= Generate.instance.maxSize)
            {
                canExpand = false;
                expandState = ExpandState.Idle;
            }
        }
        else if (expandState == ExpandState.Condensing)
        {
            transform.localScale -= new Vector3(step, step, step);
            if (transform.localScale.x - originalScale.x <= 0)
            {
                expandState = ExpandState.Idle;
            }
        }
    }

    private void OnMouseEnter()
    {
#if !UNITY_ANDROID
        if (expandState == ExpandState.Despawning)
            return;
        canExpand = true;
        expandState = ExpandState.Expanding;
#endif
    }


    private void OnMouseExit()
    {
#if !UNITY_ANDROID
        if (expandState == ExpandState.Despawning)
            return;
        canExpand = true;
        expandState = ExpandState.Condensing;
#endif
    }

    private void OnMouseDown()
    {
        Generate.instance.RemoveObject(gameObject, true);
    }
}
