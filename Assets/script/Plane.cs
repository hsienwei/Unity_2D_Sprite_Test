using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class Plane : MonoBehaviour {
	
	
	[SerializeField]
	public Atlas m_Atlas = null;
	[SerializeField, HideInInspector]
	public string m_FrameName = null;
	
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
		
		Vector2 origin = new Vector2(-frameData.sourceW/2, -frameData.sourceH/2);
		Vector3[] vertices = new Vector3[4];
		vertices[0] = new Vector3(origin.x + frameData.spriteSourceX, 
								  origin.y + frameData.sourceH - frameData.frameH - frameData.spriteSourceY, 0);
		vertices[1] = new Vector3(origin.x + frameData.frameW + frameData.spriteSourceX, 
								  origin.y + frameData.sourceH - frameData.frameH - frameData.spriteSourceY, 0);
		vertices[2] = new Vector3(origin.x + frameData.spriteSourceX, 
								  origin.y + frameData.sourceH - frameData.spriteSourceY, 0);
		vertices[3] = new Vector3(origin.x + frameData.frameW + frameData.spriteSourceX, 
								  origin.y + frameData.sourceH - frameData.spriteSourceY, 0);
		
		mesh.vertices = vertices;
		
		int [] tri = new int[6];
		tri[0] = 0;
		tri[1] = 2;
		tri[2] = 1;
		tri[3] = 2;
		tri[4] = 3;
		tri[5] = 1;
		mesh.triangles = tri;
	
		Vector2[] uv = new Vector2[4];
		uv[0] = new Vector2((float)frameData.frameX / m_Atlas.m_TextureW, 
							(float)((m_Atlas.m_TextureH - frameData.frameY) - frameData.frameH) / m_Atlas.m_TextureH);
		uv[1] = new Vector2((float)(frameData.frameX + frameData.frameW )/ m_Atlas.m_TextureW, 
							(float)((m_Atlas.m_TextureH - frameData.frameY) - frameData.frameH) / m_Atlas.m_TextureH);
		uv[2] = new Vector2((float)frameData.frameX / m_Atlas.m_TextureW, 
							(float)(m_Atlas.m_TextureH - frameData.frameY) / m_Atlas.m_TextureH);
		uv[3] = new Vector2((float)(frameData.frameX + frameData.frameW )/ m_Atlas.m_TextureW, 
							(float)(m_Atlas.m_TextureH - frameData.frameY) / m_Atlas.m_TextureH);
		mesh.uv = uv;
		
		BoxCollider boxColloder = GetComponent(typeof(BoxCollider)) as BoxCollider;
		if(boxColloder == null) {
			boxColloder = gameObject.AddComponent<BoxCollider>(); 
		}
		boxColloder.center = new Vector3(0.0f, 0.0f, 0.0f);
		boxColloder.size = new Vector3(frameData.sourceW,frameData.sourceH, 1.0f );
		
		MeshRenderer mr = GetComponent(typeof(MeshRenderer)) as MeshRenderer;
		if(mr == null) {
			mr = gameObject.AddComponent<MeshRenderer>();
		}
		mr.material= m_Atlas.m_AtlasMaterial;
	}
	
}
