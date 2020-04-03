using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HexMetrics{
	/* ratios between outer and inner radii */
	public const float outerToInner = 0.866025404f;
	public const float innerToOuter = 1f / outerToInner;

	/* radius of one hex tile */
	public const float outerRadius = 2f;
	public const float innerRadius = outerRadius * outerToInner;

	/* percentage of the hex tile that remains solid (for the honeycomb effect) */
	public const float solidEdge = 0.9f;
	public const float dropEdge = 1f - solidEdge;

	/* how high (in m) one step of elevation raises one tile */
	public const float elevationStep = 0.5f;

	/* locations of tile corners */
	static Vector3[] corners = {
		new Vector3(0f, 0f, outerRadius),                       //north
		new Vector3(innerRadius, 0f, 0.5f * outerRadius),       //northeast
		new Vector3(innerRadius, 0f, -0.5f * outerRadius),      //southeast
		new Vector3(0f, 0f, -outerRadius),                      //south
		new Vector3(-innerRadius, 0f, -0.5f * outerRadius),     //southwest
		new Vector3(-innerRadius, 0f, 0.5f * outerRadius),      //northwest
		new Vector3(0f, 0f, outerRadius)                        //north again
	};

	public static Vector3 GetFirstCorner (HexDirection direction) {
		return corners[(int)direction];
	}

	public static Vector3 GetSecondCorner (HexDirection direction) {
		return corners[(int)direction + 1];
	}

	public static Vector3 GetFirstSolidCorner (HexDirection direction) {
		return corners[(int)direction] * solidEdge;
	}

	public static Vector3 GetSecondSolidCorner (HexDirection direction) {
		return corners[(int)direction + 1] * solidEdge;
	}
}
