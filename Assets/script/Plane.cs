using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Plane : MonoBehaviour {
	
	
	[SerializeField]
	public Atlas m_Atlas;
	[SerializeField, HideInInspector]
	public string m_FrameName;
	
	Vector2[] uv = new Vector2[4];
	//Mesh mesh;
	
	// Use this for initialization
	void Start () {
		setFrame();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void setFrame()
	{
		if(m_Atlas == null)    return;
		
		FrameData frameData = m_Atlas.getFrameData(m_FrameName);
		
		if(frameData == null)    return;
	
		MeshFilter mf = GetComponent(typeof(MeshFilter)) as MeshFilter;
		if(mf == null) {
			mf = gameObject.AddComponent(typeof(MeshFilter)) as MeshFilter;		
		}
		
		Mesh mesh = new Mesh();
		mf.mesh = mesh;
		
		Vector3[] vertices = new Vector3[4];
		vertices[0] = new Vector3(-0.5f * frameData.frameW, -0.5f * frameData.frameH, 0);
		vertices[1] = new Vector3(0.5f * frameData.frameW, -0.5f * frameData.frameH, 0);
		vertices[2] = new Vector3(-0.5f * frameData.frameW, 0.5f * frameData.frameH, 0);
		vertices[3] = new Vector3(0.5f * frameData.frameW, 0.5f * frameData.frameH, 0);
		mesh.vertices = vertices;
		
		int [] tri = new int[6];
		tri[0] = 0;
		tri[1] = 2;
		tri[2] = 1;
		tri[3] = 2;
		tri[4] = 3;
		tri[5] = 1;
		mesh.triangles = tri;
	
		uv[0] = new Vector2((float)frameData.frameX / m_Atlas.textureW, 
							(float)((m_Atlas.textureH - frameData.frameY) - frameData.frameH) / m_Atlas.textureH);
		uv[1] = new Vector2((float)(frameData.frameX + frameData.frameW )/ m_Atlas.textureW, 
							(float)((m_Atlas.textureH - frameData.frameY) - frameData.frameH) / m_Atlas.textureH);
		uv[2] = new Vector2((float)frameData.frameX / m_Atlas.textureW, 
							(float)(m_Atlas.textureH - frameData.frameY) / m_Atlas.textureH);
		uv[3] = new Vector2((float)(frameData.frameX + frameData.frameW )/ m_Atlas.textureW, 
							(float)(m_Atlas.textureH - frameData.frameY) / m_Atlas.textureH);
		
		mesh.uv = uv;
		
		
		MeshRenderer mr = GetComponent(typeof(MeshRenderer)) as MeshRenderer;
		if(mr == null) {
			mr = gameObject.AddComponent<MeshRenderer>();
		}
		mr.material= m_Atlas.atlasMaterial;
	}
	
}
