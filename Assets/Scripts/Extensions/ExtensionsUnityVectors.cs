﻿using UnityEngine;
using System.Collections;
namespace ExtensionsUnityVectors{
	static class ExtensionsUnityVectors
	{
		//helper
		static float getDir(float n)
		{
			if (n > 0) return 1;
			if (n < 0) return -1;
			return 0;
		}

		public static Vector3 XYZ(this Vector2 me,float z = 0){
			return new Vector3 (me.x, me.y, z);
		}
		public static Vector2 dir(this Vector2 me)
		{
			return new Vector2(getDir(me.x), getDir(me.y));
		}
		public static Vector2 damp(this Vector2 me, float xMin, float xMax, float yMin, float yMax)
		{
			return new Vector2(Mathf.Max(  Mathf.Min(me.x,xMax),xMin) ,Mathf.Max( Mathf.Min(me.y,yMax),yMin) );
		}
		public static Vector2 divide(this Vector2 me, Vector2 other){
			return new Vector2 (me.x / other.x, me.y / other.y);
		}
		public static Vector2 XY(this Vector3 me){
			return new Vector2 (me.x, me.y);
		}
        public static Vector2 mult(this Vector2 me, Vector2 other)
        {
            return new Vector2(me.x * other.x, me.y * other.y);
        }
        public static Vector2 mult(this Vector2 me, float x, float y)
        {
            return new Vector2(me.x * x, me.y * y);
        }
        public static Vector3 absolute(this Vector3 me)
        {
            me.x = Mathf.Abs(me.x);
            me.y = Mathf.Abs(me.y);
            me.z = Mathf.Abs(me.z);
            return me;
        }
        public static Vector3 sqrAbs(this Vector3 me)
        {
            return new Vector3(me.x * Mathf.Abs(me.x), me.y * Mathf.Abs(me.y), me.z * Mathf.Abs(me.z));
        }
        public static Vector3 divide(this Vector3 me, Vector3 other){
			return new Vector3(me.x/ other.x,me.y/other.y,me.z/other.z );
		}
        public static Vector3 mult(this Vector3 me, float x, float y=1.0f, float z = 1.0f)
        {
            return new Vector3(me.x * x, me.y * y, me.z * z);

        }
        public static Vector3 mult(this Vector3 me, Vector3 other)
        {
            return new Vector3(me.x * other.x, me.y * other.y, me.z * other.z);
        }
        public static Vector3 mult(this Vector3 me, Vector2 other)
        {
            return new Vector3(me.x * other.x, me.y * other.y, 0);
        }
	}
}