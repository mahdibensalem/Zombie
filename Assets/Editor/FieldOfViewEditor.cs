using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(AttackRadius))]
public class FieldOfViewEditor : Editor
{

	void OnSceneGUI()
	{
        AttackRadius fow = (AttackRadius)target;
		Handles.color = Color.white;
		Handles.DrawWireArc(fow.transform.position, Vector3.up, Vector3.forward, 360, fow.viewRadius);
        Vector3 viewAngleA = fow.DirFromAngle(-fow.viewAngle / 2);
        Vector3 viewAngleB = fow.DirFromAngle(fow.viewAngle / 2);

        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleA * fow.viewRadius);
        Handles.DrawLine(fow.transform.position, fow.transform.position + viewAngleB * fow.viewRadius);

        Handles.color = Color.red;
        //foreach (Transform visibleTarget in fow.visibleTargets)
        //{
        //	Handles.DrawLine(fow.transform.position, visibleTarget.position);
        //}
    }

}