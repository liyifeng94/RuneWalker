using UnityEngine;
using System.Collections;

public class Parallaxing
{
    public void Parallax(Vector3 currentPos, Vector3 targetPos , Transform[] backgrounds, float distanceScale, float smoothing = 1f)
    {
        foreach (Transform layer in backgrounds)
        {
            float parallax = 0;
            if (layer.position.z != 0.0)
            {
                parallax = (targetPos.x - currentPos.x)*(-1/layer.position.z);
            }

            parallax *= distanceScale;

            float backgroundTargetPosX = layer.position.x + parallax;

            Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX, layer.position.y, layer.position.z);

            layer.position = Vector3.Lerp(layer.position, backgroundTargetPos, smoothing * Time.deltaTime);
        }
    }
}
