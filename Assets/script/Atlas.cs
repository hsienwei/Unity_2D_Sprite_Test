using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using LitJson;

[ExecuteInEditMode]
public class Atlas : MonoBehaviour {
	public TextAsset 	m_TextAsset;
	public int 			m_TextureW, m_TextureH;
	public string 		m_TextureFileName;
	public Texture2D 	m_AtlasTexture;
	public Material 	m_AtlasMaterial;
	private bool        m_IsAtlasSet = false;
	
	private Dictionary<string, FrameData> m_FramesList = null;
	
	
	void Start () {
		loadJSONObject();
	}
	
	// Update is called once per frame
	void Update () {
		#if UNITY_EDITOR
		
		if(!m_IsAtlasSet) {
			Debug.Log("true");
			loadJSONObject();
		}
		else {
			if(m_TextAsset == null)
			{
				Debug.Log("false");
			
				m_IsAtlasSet = false;
				m_TextureW = 0;
				m_TextureH = 0;
				m_TextureFileName = null;
				Destroy(m_AtlasTexture);
				Destroy(m_AtlasMaterial);
			}
		}
		#endif
	}
	
	void loadJSONObject()
	{
		if(m_TextAsset != null && m_FramesList == null)
		{
			m_FramesList = new Dictionary<string, FrameData>();
			parseJSONObject(m_TextAsset.ToString());
		}
	}
	
	void parseJSONObject(string pJsonStr)
	{
		//JSONObject atlasObj = new JSONObject(pJsonStr);
		
		JsonData atlasObj = JsonMapper.ToObject (pJsonStr);
		if(atlasObj.GetJsonType() == LitJson.JsonType.None)    return;
		
		JsonData metaDataObj = atlasObj["meta"];
		if(metaDataObj.GetJsonType() == LitJson.JsonType.None)    return;
		m_TextureW = (int)metaDataObj["size"]["w"];
		m_TextureH = (int)metaDataObj["size"]["h"];
		m_TextureFileName = (string)metaDataObj["image"];
		
		m_AtlasTexture = Resources.Load(m_TextureFileName.Substring(0, m_TextureFileName.IndexOf('.'))) as Texture2D;
		m_AtlasMaterial= new Material(Shader.Find("Unlit/Transparent"));
		m_AtlasMaterial.mainTexture = m_AtlasTexture;
		
		Debug.Log(m_TextureFileName);
		Debug.Log(m_AtlasTexture);
		JsonData framesDataObj = atlasObj["frames"];
		if(framesDataObj.GetJsonType() == LitJson.JsonType.None)    return;
		
		IDictionary framesDataDic = (IDictionary)framesDataObj;
		
		
		foreach(string key in framesDataDic.Keys)
		{
			JsonData frameDataObj = framesDataObj[key];
			FrameData frameData = new FrameData();
			
			frameData.frameX 	= (int)frameDataObj["frame"]["x"];
			frameData.frameY 	= (int)frameDataObj["frame"]["y"];
			frameData.frameW 	= (int)frameDataObj["frame"]["w"];
			frameData.frameH 	= (int)frameDataObj["frame"]["h"];
			
			frameData.rotated 	= (bool)frameDataObj["rotated"];
			
			frameData.trimmed 	= (bool)frameDataObj["trimmed"];
			
			frameData.spriteSourceX = (int)frameDataObj["spriteSourceSize"]["x"];
			frameData.spriteSourceY = (int)frameDataObj["spriteSourceSize"]["y"];
			frameData.spriteSourceW = (int)frameDataObj["spriteSourceSize"]["w"];
			frameData.spriteSourceH = (int)frameDataObj["spriteSourceSize"]["h"];
			
			frameData.sourceW 	= (int)frameDataObj["sourceSize"]["w"];
			frameData.sourceH 	= (int)frameDataObj["sourceSize"]["h"];
			
			m_FramesList.Add(key, frameData);
		}
		Debug.Log("parseJSONObject");
	}
	
	public string[] getFramesArray()
	{
		Debug.Log("getFramesArray");
		if(m_FramesList == null)    loadJSONObject();
		string[] frameArray = new string[m_FramesList.Keys.Count];
		int idx = 0;
		
		foreach(string key in m_FramesList.Keys)
		{
			frameArray[idx] = key;
			idx++;
		}
		return frameArray;
	}
	public FrameData getFrameData(string frameName)
	{
		if(m_FramesList.ContainsKey(frameName))
		{
			return m_FramesList[frameName];
		}
		return null;
	}
}


