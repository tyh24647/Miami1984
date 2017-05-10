using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script that sets up the underwater effects when the user goes into
/// the ocean prefab.
/// 
/// This object provides the ability to add bubbles, fog, materials, and 
/// visual offsets.
/// 
/// Created by Tyler Hostager, 2017.
/// All rights reserved.
/// </summary>
[ExecuteInEditMode]
public class Underwater : MonoBehaviour {

	#region public data
	public float m_UnderwaterCheckOffset = 0.001F;
	public float skyFogDensity = 0.005f;
	public float waterFogDensity = 0.05f;
	public Color envFogColor;
	public Color underwaterFogColor = Color.blue;
	public Color mUnderWaterBubblesColor = new Color(0.27f, 0.27f, 0.27f, 1f);
	public Color mUpWaterBubblesColor = new Color(0.019607843f, 0.019607843f, 0.019607843f, 1f);
	public GameObject underwaterStuff;
	public Material waterBubblesMat;
	#endregion

	#region private data
	private bool wasUnderwater = false;
	#endregion

	/// <summary>
	/// Determines whether this instance is underwater the specified cam.
	/// </summary>
	/// <returns>
	/// <c>true</c> if this instance is underwater the specified 
	/// cam; otherwise, <c>false</c>.
	/// </returns>
	/// <param name="cam">The <code>Camera</code> instance.</param>
	public bool IsUnderwater(Camera cam) {
		return ((cam.transform.position.y + m_UnderwaterCheckOffset) < transform.position.y);
	}

	/// <summary>
	/// Whether or not the camera is a valid object to apply the <code>UnderwaterEffect</code>
	/// script on
	/// </summary>
	/// <returns><c>true</c>, if the camera object is valid, <c>false</c> otherwise.</returns>
	/// <param name="tmp">Temporary allocation of memory for the camera object</param>
	private bool CamIsValid(Camera tmp) {
		return tmp ? false : Camera.main == tmp && !tmp.gameObject.GetComponent(
			typeof(UnderwaterEffect)
		);
	}

	/// <summary>
	/// Is executed when the object will be rendered
	/// </summary>
	public void OnWillRenderObject() {
		Camera cam = Camera.current;

		if (IsUnderwater(cam)) {
			if (CamIsValid(cam)) {
				cam.gameObject.AddComponent(
					typeof(UnderwaterEffect)
				);
			}

			UnderwaterEffect effect = (UnderwaterEffect)cam.gameObject.GetComponent(
										  typeof(UnderwaterEffect)
									  );

			if (effect) {
				effect.enabled = true;
			}

			GetComponent<Renderer>().sharedMaterial.shader.maximumLOD = 50;

			if (!wasUnderwater) {
				wasUnderwater = true;

				// update the fog
				RenderSettings.fogDensity = waterFogDensity;
				RenderSettings.fogColor = underwaterFogColor;

				// change reflection mode
				WaterMirrorReflection reflScript = (WaterMirrorReflection)GetComponent(
													   typeof(WaterMirrorReflection)
												   );

				reflScript.m_BackSide = true;

				// enable 'caustic'
				underwaterStuff.SetActive(true);

				// enable bubbles
				waterBubblesMat.SetVector("_TintColor", mUnderWaterBubblesColor);
			}
		} else {
			UnderwaterEffect effect = (UnderwaterEffect)cam.gameObject.GetComponent(
										  typeof(UnderwaterEffect)
									  );

			if (effect && effect.enabled) {
				effect.enabled = false;
			}

			GetComponent<Renderer>().sharedMaterial.shader.maximumLOD = 100;

			if (wasUnderwater) {
				// change the fog appropriately
				RenderSettings.fogDensity = skyFogDensity;
				RenderSettings.fogColor = envFogColor;
				wasUnderwater = false;

				// change reflection mode
				WaterMirrorReflection reflScript = (WaterMirrorReflection)GetComponent(
													   typeof(WaterMirrorReflection)
												   );

				reflScript.m_BackSide = false;

				// disable 'caustic'
				underwaterStuff.SetActive(false);

				// disable bubbles
				waterBubblesMat.SetVector("_TintColor", mUpWaterBubblesColor);
			}
		}
	}
}
