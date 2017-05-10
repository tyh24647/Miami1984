using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UnderwaterEffect : MonoBehaviour {

	#region public vars
	public float m_UnderwaterColorFade = 0.125F;
	public Shader m_UnderwaterShader;
	public Color m_BlendColor = new Color(0.086f, 0.216f, 0.714f);
	#endregion

	#region private vars
	private Material m_UnderwaterMaterial;
	#endregion

	void Start() {
		if (m_UnderwaterShader) {
			m_UnderwaterMaterial = new Material(m_UnderwaterShader);
		}
	}

	void OnRenderImage(RenderTexture source, RenderTexture destination) {
		RenderTexture tmp = RenderTexture.GetTemporary(source.width, source.height);
		m_UnderwaterMaterial.SetColor("_DepthColor", m_BlendColor);
		m_UnderwaterMaterial.SetFloat("_UnderwaterColorFade", m_UnderwaterColorFade);
		m_UnderwaterMaterial.SetVector("offsets", new Vector4(1.0F, 0.0F, 0.0F, 0.0F));
		Graphics.Blit(source, tmp, m_UnderwaterMaterial, 0);
		m_UnderwaterMaterial.SetVector("offsets", new Vector4(0.0F, 1.0F, 0.0F, 0.0F));
		Graphics.Blit(tmp, destination, m_UnderwaterMaterial, 0);
		RenderTexture.ReleaseTemporary(tmp);
	}
}

