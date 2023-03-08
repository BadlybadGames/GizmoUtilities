using BBG.GizmoUtility.Common;
using BBG.GizmoUtility.GizmoUtility.Runtime;
using UnityEngine;

[Gizmo(r=0.2f, g=1f, b=0.2f, a=0.3f)]
public class aaa : MonoBehaviour
{

    public float radius;

    public Transform rt;

    public float testRadius2;
    
    
    [HandleGizmo]
    public float test;

    [HandleGizmo]
    public Vector3 vec;
    // Update is called once per frame

    [ButtonHandle]
    public void Test()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //GizmoUtility.Arrow(Vector3.zero, Vector3.one, 4);
            //GizmoUtility.Arrow(Vector3.zero, Vector3.one, 4, duration:4, animatedColor:true, endcolor:Color.blue);
        }
    }
}