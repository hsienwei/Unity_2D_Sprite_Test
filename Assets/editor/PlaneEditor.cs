using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(Plane))]
public class PlaneEditor : Editor {

	private string[] frameSelectList = null;
	private int frameSelectedIdx = 0;
	
	SerializedObject m_Object;
    SerializedProperty m_AtlasProperty;
	SerializedProperty m_FramesProperty;
 	
	public void OnEnable()
    {
		m_Object = new SerializedObject (target);
        m_AtlasProperty = m_Object.FindProperty ("m_Atlas");
		m_FramesProperty = m_Object.FindProperty ("m_FrameName");
		
		var targetObject = target as Plane;
		targetObject.m_Atlas = m_AtlasProperty.objectReferenceValue as Atlas;
		
		if(targetObject.m_Atlas == null)    return;    
		resetFrameField(targetObject);
	}
	
	public override void OnInspectorGUI()
  	{
		var targetObject = target as Plane;
		
		EditorGUILayout.PropertyField (m_AtlasProperty);
		
		targetObject.m_Atlas = m_AtlasProperty.objectReferenceValue as Atlas;//EditorGUILayout.ObjectField("Atlas", targetObject.m_Atlas, typeof(Atlas),true) as Atlas;
		if(frameSelectList != null)
		{
			frameSelectedIdx = EditorGUILayout.Popup("Frame", frameSelectedIdx, frameSelectList);
			targetObject.m_FrameName = frameSelectList[frameSelectedIdx];
		}

		if(GUI.changed)
		{
			resetFrameField(targetObject);
			EditorUtility.SetDirty(targetObject);  //add this , save setting value
			targetObject.setFrame();
			SceneView.RepaintAll();
			MeshFilter mf = targetObject.GetComponent(typeof(MeshFilter)) as MeshFilter;
		}
  	}
	
	void resetFrameField(Object target )
	{
		var targetObject = target as Plane;
		frameSelectList = targetObject.m_Atlas.getFramesArray();
		
		string selectedFrame = targetObject.m_FrameName;
		int searchIdx = 0;
		foreach(string frame in frameSelectList)
		{
			if(frame.Equals(selectedFrame))
			{
				frameSelectedIdx = searchIdx;
				break;
			}
			searchIdx++;
		}
	}
}
