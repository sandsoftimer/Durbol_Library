using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ParabolaMove : MonoBehaviour
{
    public bool playOnAwake;
    public AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 90f), new Keyframe(0.5f, 1f, 0f, 0f), new Keyframe(1f, 0f, 90f, 0f));
    public float timeToReach = 1, height = 3;
    public bool lookForwardOnMove, destroyOnComplete;
    public Transform endPoint;

    Vector3 startPosition, endPosition, lastPostion;
    float time, gap = 0.25f;
    float curveActualLengthInGraph;
    bool initialized;
    public Action action;
    // Awake is called before Start
    public void Awake()
    {
        lastPostion = transform.position;
        startPosition = transform.position;

        if (playOnAwake)
            Initialized(endPoint, timeToReach, curve, height, lookForwardOnMove, destroyOnComplete);
    }

    public void Initialized()
    {
        initialized = true;
    }

    public void Initialized(Transform endPoint, float timeToReach, AnimationCurve curve, float height, bool lookForwardOnMove, bool destroyOnComplete, Action action = null)
    {
        this.endPoint = endPoint;
        Execute(timeToReach, curve, height, lookForwardOnMove, destroyOnComplete, action);
    }

    public void Initialized(Vector3 endPositoin, float timeToReach, AnimationCurve curve, float height, bool lookForwardOnMove, bool destroyOnComplete, Action action = null)
    {
        this.endPosition = endPositoin;
        Execute(timeToReach, curve, height, lookForwardOnMove, destroyOnComplete, action);
    }

    public void Initialized(Vector3 endPositoin, Action action = null)
    {
        this.endPosition = endPositoin;
        Execute(timeToReach, curve, height, lookForwardOnMove, destroyOnComplete, action);
    }

    void Execute(float timeToReach, AnimationCurve curve, float height, bool lookForwardOnMove, bool destroyOnComplete, Action action = null)
    {
        this.lookForwardOnMove = lookForwardOnMove;
        this.destroyOnComplete = destroyOnComplete;
        this.action = action;
        this.curve = curve;
        this.height = height;
        this.timeToReach = timeToReach;

        lastPostion = transform.position;
        startPosition = transform.position;
        time = 0;
        curveActualLengthInGraph = curve.length * 2 / 10f;
        initialized = true;
    }

    void Update()
    {
        if (!initialized)
        {
            return;
        }

        if (endPoint != null)
            endPosition = endPoint.position;

        time += Time.deltaTime;
        float lerpValue = Mathf.InverseLerp(0, timeToReach, time);
        Vector3 pos = Vector3.Lerp(startPosition, endPosition, lerpValue);
        pos.y += height * curve.Evaluate(Mathf.Lerp(0, curve.keys[curve.keys.Length - 1].time, lerpValue));
        transform.position = pos;
        if (lookForwardOnMove)
        {
            Quaternion rotation = Quaternion.LookRotation(transform.position - lastPostion, Vector3.up);
            rotation.x = transform.rotation.x;
            rotation.z = transform.rotation.z;
            transform.rotation = rotation;
        }

        if (lerpValue == 1)
        {
            action?.Invoke();
            initialized = false;
        }
        if (destroyOnComplete && time >= timeToReach)
        {
            Destroy(this);
        }
    }

    private void LateUpdate()
    {
        lastPostion = transform.position;
    }
}
