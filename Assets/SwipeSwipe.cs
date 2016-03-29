using UnityEngine;
using System.Collections;
using Leap;

public class SwipeSwipe : MonoBehaviour {
	Controller c;
	public Camera cam;
	public GameObject list;
	GameObject go;
	GameObject inv;
	GameObject sphere;
	public GameObject c1;
	public GameObject c2;
	public GameObject c3;
	public GameObject c4;
	public GameObject c5;
	Vector3 v1,v2,v3,v4,v5;
	GameObject[] body = new GameObject[5];
	bool inven = false;
	Color oldcolor;
	// Use this for initialization
	void Start () {
		c = new Controller ();
		body [0] = c1;
		body [1] = c2;
		body [2] = c3;
		body [3] = c4;
		body [4] = c5;
		go = GameObject.CreatePrimitive(PrimitiveType.Cube);
		sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);	
		sphere.transform.position = cam.transform.position;
		sphere.transform.localScale = new Vector3 (30,30,30);
		sphere.transform.position += new Vector3 (0, 10, 0);
		oldcolor = new Color(1.0f,1.0f,1.0f);
		go.transform.position = new Vector3 (10, 10, 10);
		c.EnableGesture (Gesture.GestureType.TYPESWIPE);
		c.EnableGesture (Gesture.GestureType.TYPE_KEY_TAP);
		c.EnableGesture (Gesture.GestureType.TYPE_SCREEN_TAP);
		c.EnableGesture (Gesture.GestureType.TYPE_CIRCLE);
		c.Config.SetFloat ("Gesture.Swipe.MinLength", 200.0f);
		c.Config.SetFloat ("Gesture.Swipe.MinVelocity", 700f);
		c.Config.SetFloat ("Gesture.KeyTap.MinDownVelocity", 40f);
		c.Config.SetFloat ("Gesture.KeyTap.HistorySeconds", 0.2f);
		c.Config.SetFloat ("Gesture.KeyTap.MinDistance", 5.0f);
		c.Config.SetFloat ("Gesture.ScreenTap.MinForwardVelocity", 30.0f);
		c.Config.SetFloat ("Gesture.ScreenTap.HistorySeconds", .5f);
		c.Config.SetFloat ("Gesture.ScreenTap.MinDistance", 1.0f);
		c.Config.SetFloat ("Gesture.Circle.MinRadius", 10.0f);
		c.Config.SetFloat ("Gesture.Circle.MinArc", .5f);
		c.Config.Save ();
		v1 = c1.transform.position;
		v2 = c2.transform.position;
		v3 = c3.transform.position;
		v4 = c4.transform.position;
		v5 = c5.transform.position;
		//sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
	}
	float gx, gy, gz;
	// Update is called once per frame
	void Update () {
		Frame f = c.Frame ();
		GestureList gs = f.Gestures ();
		Hand lefthand = f.Hands.Leftmost;
		Hand righthand = f.Hands.Rightmost;
		FingerList fl = righthand.Fingers.Extended ();
		float speed = 2.0f;
		float still = 3.0f;
		int i = 0;
		sphere.transform.position = cam.transform.position;
		sphere.transform.position += new Vector3 (0, 10, 0);
		/* control system */
		/* -----------------------------Right Hand Functions --------------------------------------------- */
		if (righthand.IsRight) {
			float xx = righthand.PalmPosition.x;
			float yy = righthand.PalmPosition.y;
			float zz = righthand.PalmPosition.z;
			float x = righthand.PalmNormal.x;
			float z = righthand.PalmNormal.z;
			float y = righthand.PalmNormal.y;
			/* -----------------------------Camera - Way Finding ----------------------------------------- */
			// forward
			if (fl.Count == 5) {
				if (zz < -80.0f)
					cam.transform.position += new Vector3 (cam.transform.forward.x / speed, 0, cam.transform.forward.z / speed);
				if (zz < -40.0f && zz > -80.0f)
					cam.transform.position += new Vector3 (cam.transform.forward.x / speed, 0, cam.transform.forward.z / still);
				// backwards
				if (zz > 80.0f)
					cam.transform.position += new Vector3 (-cam.transform.forward.x / speed, 0, -cam.transform.forward.z / speed);
				if (zz > 40.0f && zz < 80.0f)
					cam.transform.position += new Vector3 (-cam.transform.forward.x / speed, 0, -cam.transform.forward.z / speed);
				// right
				if (xx > 120.0f)
					cam.transform.position += new Vector3 (cam.transform.right.x / speed, 0, cam.transform.right.z / speed);
				// left
				if (xx < 80.0f && xx > 40.0f)
					cam.transform.position += new Vector3 (-cam.transform.right.x / speed, 0, -cam.transform.right.z / speed);
				// rotate up
				//if (yy < 150.0f && yy > 100.0f) cam.transform.Rotate(-cam.transform.right.x,0,-cam.transform.right.z/speed);
				// rotate down
				//if (yy > 250.0f)cam.transform.Rotate (0, -1, 0);
				// rotate right
				if (x > 0.5f)
					cam.transform.Rotate (0, -2 * x, 0);
				// rotate left
				if (x < -0.5f)
					cam.transform.Rotate (0, 2 * -x, 0);
				if (z > 0.5f)
					cam.transform.Rotate (0, 0, z * -2);
				if (z > 0.5f)
					cam.transform.Rotate (0, 0, 2 * z);
				if (y > 0.5f)
					cam.transform.Rotate (0, 0, y * -2);
				if (y > 0.5f)
					cam.transform.Rotate (0, 0, 2 * y);
			}
		}
		/* ----------------------------Left Hand Functions ------------------------------------------------ */
		if (lefthand.IsLeft) {
			bool pinch = false;
			bool fill = false;
			bool rl = false;
			bool rr = false;
			float mag = 0.0f;
			GameObject obj = body [1];
			//foreach (Transform child in list.transform) {
			//	if (fill == false) {
			//		obj = child.gameObject;
			//		fill = true;
			//	} else if (child.transform.position.magnitude > mag) {
			//		mag = child.transform.position.magnitude;
					//obj = child.gameObject;
			//	}
			//}
			Vector3 p = cam.transform.position - obj.transform.position;
			/* ------------------------Selection and Manipulation ----------------------------------------- */
			Vector3 tp = lefthand.Fingers [0].TipPosition.ToUnityScaled ();
			Finger fin = lefthand.Fingers [2];
			Vector3 fp = fin.TipPosition.ToUnityScaled ();
			Vector3 dis = tp - fp;
			if (dis.magnitude < 0.03f) {
				Vector3 pt = (tp + fp) / 2.0f;
				pinch = true;
			}
			/* ------------------------ ScreenTap Gesture -- Enables Manipulation of Position ----------------- */
			if (gs [0].Type == Gesture.GestureType.TYPESCREENTAP) {
				ScreenTapGesture st = new ScreenTapGesture (gs [0]);
				print (cam.transform.position - obj.transform.position);
				print (p.magnitude);
				//if (st.Progress > 0) {
				//	if (rend.material.color == Color.red)
				//		rend.material.color = oldcolor;
				//	else if (rend.material.color == oldcolor && obj.transform.position.magnitude < 3.0f)
				//		rend.material.color = Color.red;
				//}
			}
			if (pinch == true) {
				print ("pinch");
				/* ------------------------ Move Hand Gesture -- Enables Manipulation of Position ------------------ */
				/* ------------------------ Circle Gesture -- Enables Manipulation of Rotation ---------------- */
				if (gs [0].Type == Gesture.GestureType.TYPE_CIRCLE) {
					CircleGesture cg = new CircleGesture (gs [0]);
					if (cg.Pointable.Direction.AngleTo (cg.Normal) <= Mathf.PI / 4) {
						if (cg.Progress > 0) {
							//obj.transform.position += new Vector3 (cam.transform.right.x, 0, camera.transform..z);
							print ("pickup pickup pickup");
							rl = true;
							inven = true;
							inv = obj;
							obj.transform.Rotate (0, -1, 0);
							obj.transform.localScale = new Vector3 (1.0f, 1.0f, 1.0f);
							obj.transform.position = cam.ScreenToWorldPoint (new Vector3 (cam.pixelWidth / 2, 0, 1));
						}
					} else {
						if (cg.Progress > 0) {
							rr = true;
							inven = true;
							inv = obj;
							obj.transform.Rotate (0, 1, 0);
							obj.transform.localScale = new Vector3 (1.0f, 1.0f,1.0f);
							obj.transform.position = cam.ScreenToWorldPoint (new Vector3 (cam.pixelWidth / 2, 0, 1));
						}
					}
				}
			}
			if (gs[0].Type == Gesture.GestureType.TYPESWIPE) {
			SwipeGesture s = new SwipeGesture (gs[0]);
			Vector swipe = s.Direction;
				if (swipe.x < 0) {
					obj = body [i++%5];
				}
			}
		}
	}
}
		/*
		Debug.DrawRay (new Vector3 (x, y, z), new Vector3 (xx, yy, zz));
			//Debug.Log ("Right Hand");
			//if (!h.Fingers.Extended ().FingerType (Finger.FingerType.TYPE_INDEX).IsEmpty)
			//	print ("Index extended");
		//if (!h.Fingers.Extended ().FingerType (Finger.FingerType.TYPE_INDEX).IsEmpty &&
		//     !h.Fingers.Extended ().FingerType (Finger.FingerType.TYPE_MIDDLE).IsEmpty &&
		//     h.Fingers.Extended ().FingerType (Finger.FingerType.TYPE_RING).IsEmpty &&
		//     h.Fingers.Extended ().FingerType (Finger.FingerType.TYPE_PINKY).IsEmpty) {
		if (gs [0].Type == Gesture.GestureType.TYPESWIPE) {
			SwipeGesture s = new SwipeGesture (gs [0]);
			Vector swipe = s.Direction;
			if (swipe.x < 0) {
				print ("swiped");
			}
		}*/
		
			//if (h.IsRight && swipe.x < 0 && h.Fingers.Extended().Count == 2)
					//print ("Swiped with Index and Middle");
			//	cam.transform.eulerAngles += new Vector3 (0.0f, -2.0f, 0.0f);
			//if (h.IsLeft && swipe.x > 0)
			//	print ("Swiped with Index and Middle");
		//}
		//}

		/*
		if (h.IsLeft) {
			//Debug.Log ("Left Hand");
		} 
		for (int i = 0; i < gs.Count; i++) {
			Gesture g = gs [i];

			if (g.Type == Gesture.GestureType.TYPESWIPE) {
				SwipeGesture s = new SwipeGesture (g);
				Vector swipe = s.Direction;
				if (h.IsRight) {
					if (swipe.x < 0) {
						//Debug.Log ("Left")
						FingerList fl = h.Fingers.Extended().FingerType(Finger.FingerType.TYPE_INDEX & Finger.FingerType.TYPE_MIDDLE);
						if (!fl.IsEmpty) {
							print (fl.Count);
						}
						//for (int j = 0; j < h.Fingers.Count; j++)
							//Debug.Log (h.Fingers [j]);
						//cam.transform.eulerAngles += new Vector3 (0.0f, -2.0f, 0.0f);
					}
					if (swipe.x > 0) {
						//Debug.Log ("Right");
						//cam.transform.eulerAngles += new Vector3 (0.0f, 1.0f, 0.0f);

					}
				} 
			} 
			if (g.Type == Gesture.GestureType.TYPE_KEY_TAP) {
				KeyTapGesture kt = new KeyTapGesture (g);
				if (h.IsRight) {
					if (kt.Progress > 0)
						Debug.Log ("Tapped");
				}
			}

}*/
