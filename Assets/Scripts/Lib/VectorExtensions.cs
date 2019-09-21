using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class VectorExtensions
{
    public static Vector3 ToXYVector3(this Vector2 vector, float z_value = 0f)
    {
        return new Vector3(vector.x, vector.y, z_value);
	}
	public static Vector3 ToXZVector3(this Vector2 vector, float y_value = 0f)
	{
		return new Vector3(vector.x, y_value, vector.y);
	}
}
