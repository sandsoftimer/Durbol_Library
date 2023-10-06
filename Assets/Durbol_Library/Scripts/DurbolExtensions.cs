using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Collections;

#if DOTWEEN
using DG.Tweening;
#endif

public static class DurbolExtensions
{
    public static bool DL_IsInLayerMask(this LayerMask mask, GameObject obj)
    {
        return (mask.value & (1 << obj.layer)) > 0;
    }

    public static void DL_SetClip(this Animator anim, string statName, AnimationClip clip, float animationSpeed = 1)
    {
        AnimatorOverrideController aoc = new AnimatorOverrideController(anim.runtimeAnimatorController);
        var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        foreach (var a in aoc.animationClips)
        {
            if(a.name.Equals(statName))
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, clip));
            else
                anims.Add(new KeyValuePair<AnimationClip, AnimationClip>(a, a));
        }
        aoc.ApplyOverrides(anims);
        anim.runtimeAnimatorController = aoc;
        anim.speed = animationSpeed;
    }

    public static void DL_Reset(this Transform transfrom)
    {
        transfrom.localPosition = Vector3.zero;
        transfrom.localEulerAngles = Vector3.zero;
        transfrom.localScale = Vector3.one;
    }

    public static bool DL_IsOffScreen(this Transform transform, Camera camera, Vector2 viewOffset)
    {
        Vector3 viewPos = camera.WorldToViewportPoint(transform.position);
        if (viewPos.x >= -viewOffset.x && viewPos.x <= 1 + viewOffset.x && viewPos.y >= -viewOffset.y && viewPos.y <= 1 + viewOffset.y && viewPos.z > 0)
            return false;
        else
            return true;
    }

    public static Vector2 DL_OffScreenIndicatorPoint(this Transform transform, Camera camera, Vector2 iconOffset)
    {
        Vector2 point = Vector2.zero;
        Vector3 viewPos = camera.WorldToScreenPoint(transform.position);
        point.x = Mathf.Clamp(viewPos.x, 0, Screen.width);
        point.y = Mathf.Clamp(viewPos.y, 0, Screen.height);

        point.x += iconOffset.x * (point.x > (Screen.width / 2) ? -1 : 1);
        point.y += iconOffset.y * (point.y > (Screen.height / 2) ? -1 : 1);

        return point;
    }

    public static bool DL_IsGrounded(this Transform transform, Vector3 raycastOffse, float groundThreshold)
    {
        return Physics.Raycast(transform.position + raycastOffse, Vector3.down, groundThreshold, 1 << ConstantManager.GROUND_LAYER);
    }

    public static void DL_TryUpdateShapeToAttachedSprite(this PolygonCollider2D collider)
    {
        collider.DL_UpdateShapeToSprite(collider.GetComponent<SpriteRenderer>().sprite);
    }

    public static void DL_UpdateShapeToSprite(this PolygonCollider2D collider, Sprite sprite)
    {
        // ensure both valid
        if (collider != null && sprite != null)
        {
            // update count
            collider.pathCount = sprite.GetPhysicsShapeCount();

            // new paths variable
            List<Vector2> path = new List<Vector2>();

            // loop path count
            for (int i = 0; i < collider.pathCount; i++)
            {
                // clear
                path.Clear();
                // get shape
                sprite.GetPhysicsShape(i, path);
                // set path
                collider.SetPath(i, path.ToArray());
            }
        }
    }

    public static Vector3 DL_ClampVector(this Vector3 value, Vector3 minVector, Vector3 maxVector)
    {
        value.x = Mathf.Clamp(value.x, minVector.x, maxVector.x);
        value.y = Mathf.Clamp(value.y, minVector.y, maxVector.y);
        value.z = Mathf.Clamp(value.z, minVector.z, maxVector.z);
        return value;
    }

    public static Texture2D DL_Texture2D(this RenderTexture renderTexture)
    {
        Texture2D tex = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        var old_rt = RenderTexture.active;
        RenderTexture.active = renderTexture;

        tex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex.Apply();

        RenderTexture.active = old_rt;
        return tex;
    }

    public static void DL_DestroyAllChild(this Transform transform, bool imidiatly = false)
    {
        for (int i = transform.childCount - 1; i > -1; i--)
        {
            if(imidiatly)
                GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
            else
                GameObject.Destroy(transform.GetChild(i).gameObject);
        }
    }

    public static void DL_GetRandomColor(this Color color)
    {
        color = Random.ColorHSV();
    }

    public static RaycastHit DL_GetRaycastHitPoint(this Transform t, Vector3 direction, int layerMask)
    {
        RaycastHit hit;
        Physics.Raycast(new Ray(t.position, direction), out hit, Mathf.Infinity, layerMask);
        return hit;
    }

    public static RaycastHit DL_GetRaycastHitPoint(this Transform t, Vector3 direction, Vector3 positionOffset, float distance, int layerMask)
    {
        RaycastHit hit;
        Physics.Raycast(new Ray(t.position + positionOffset, direction), out hit, distance, layerMask);
        return hit;
    }

    public static void DL_GetRaycastFromScreenTouch(ref this RaycastHit hit, Vector3 offset, int layerMask = 1)
    {
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition + offset), out hit, Mathf.Infinity, layerMask);
        //return hit;
    }

    // Generate random normalized direction
    public static Vector2 DL_GetRandomDirection2D()
    {
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    // Generate random normalized direction
    public static Vector3 DL_GetRandomDirection3D()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }

    public static GameObject DL_ActiveChild(this Transform transform)
    {
        return transform.DL_ActiveChild(Random.Range(0, transform.childCount));
    }

    public static GameObject DL_ActiveChild(this Transform transform, int childIndex)
    {
        GameObject go = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            if(i == childIndex)
            {
                go = transform.GetChild(i).gameObject;
            }
            transform.GetChild(i).gameObject.SetActive(i == childIndex);
        }
        return go;
    }

    public static GameObject DL_ActiveChild(this Transform transfrom, GameObject gameObject)
    {
        GameObject go = null;
        for (int i = 0; i < transfrom.childCount; i++)
        {
            if(transfrom.GetChild(i).gameObject == gameObject)
            {
                go = transfrom.GetChild(i).gameObject;
            }
            transfrom.GetChild(i).gameObject.SetActive(transfrom.GetChild(i).gameObject == gameObject);
        }
        return go;
    }

    public static GameObject DL_ActiveChild(this Transform transfrom, string name)
    {
        GameObject go = null;
        for (int i = 0; i < transfrom.childCount; i++)
        {
            if (transfrom.GetChild(i).gameObject.name.Equals(name))
            {
                go = transfrom.GetChild(i).gameObject;
            }
            transfrom.GetChild(i).gameObject.SetActive(transfrom.GetChild(i).name.Equals(name));
        }
        return go;
    }

    public static float DL_DistanceFrom(this Transform _transform, Transform comparingTransform, DL_Axis aPAxis = DL_Axis.ALL)
    {
        return DL_DistanceFrom(_transform.position, comparingTransform.position, aPAxis);
    }

    public static float DL_DistanceFrom(this Transform _transform, Vector3 comparingPosition, DL_Axis aPAxis = DL_Axis.ALL)
    {
        return DL_DistanceFrom(_transform.position, comparingPosition, aPAxis);
    }

    public static float DL_DistanceFrom(this Vector3 _transform, Vector3 comparingPosition, DL_Axis aPAxis = DL_Axis.ALL)
    {
        float distance = Mathf.Infinity;
        switch (aPAxis)
        {
            case DL_Axis.ALL:
                distance = Vector3.Distance(_transform, comparingPosition);
                break;
            case DL_Axis.X:
                distance = Mathf.Abs(_transform.x - comparingPosition.x);
                break;
            case DL_Axis.Y:
                distance = Mathf.Abs(_transform.y - comparingPosition.y);
                break;
            case DL_Axis.Z:
                distance = Mathf.Abs(_transform.z - comparingPosition.z);
                break;
        }
        return distance;
    }

    public static Vector3 DL_ModifyThisVector(this Vector3 value, float x, float y, float z)
    {
        return new Vector3(value.x + x, value.y + y, value.z + z);
    }

    public static Vector3 DL_ModifyThisVector(this Vector3 value, Vector3 vector)
    {
        return value.DL_ModifyThisVector(vector.x, vector.y, vector.z);
    }

    public static Transform DL_GetClosestTransform(this Transform t, List<Transform> list)
    {
        List<Transform> transforms = list.OrderBy(i => Vector3.Distance(t.position, i.position)).ToList();
        return transforms.Count > 0? transforms[0] : null;
    }

    public static RaycastHit[] DL_Get_Surrounding_Enemies(this Transform transform, float radious, LayerMask enemyLayer)
    {
        return Physics.SphereCastAll(transform.position, radious, Vector3.one, radious / 2, enemyLayer);
    }

    public static void ToFloat(this float value, float destinationValue, float time)
    {
#if DOTWEEN
        DOTween.To(() => value, x => value = x, destinationValue, time)
            .OnUpdate(() =>
            {
                Debug.Log(Mathf.Sin(value * Mathf.Rad2Deg));
            });
#else
        Debug.LogError("DO TWEEN not installed");
#endif
    }
}
