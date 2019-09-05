using UnityEngine;

namespace Es.InkPainter.Sample
{
	public class MousePainter : MonoBehaviour
	{
        public static MousePainter Instance;
		/// <summary>
		/// Types of methods used to paint.
		/// </summary>
		[System.Serializable]
		private enum UseMethodType
		{
			RaycastHitInfo,
			WorldPoint,
			NearestSurfacePoint,
			DirectUV,
		}

		public Brush brush;

		[SerializeField]
		private UseMethodType useMethodType = UseMethodType.RaycastHitInfo;

		public bool erase = false;

        public bool IsGamestart = false;

        private Vector3 lastPoint;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            lastPoint = Vector3.zero;
        }

        private void Update()
		{
            //Debug.Log("1111");
			if(Input.GetMouseButton(0)&& IsGamestart)
			{
                brush.Color = ColorSelector.GetColor();
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				bool success = true;
				RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    var paintObject = hitInfo.transform.GetComponent<InkCanvas>();
                    if (paintObject != null)
                    {
                        switch (useMethodType)
                        {
                            case UseMethodType.RaycastHitInfo:
                                //Debug.Log("1111");
                                success = erase ? paintObject.Erase(brush, hitInfo) : paintObject.Paint(brush, hitInfo);
                                break;

                            case UseMethodType.WorldPoint:
                                success = erase ? paintObject.Erase(brush, hitInfo.point) : paintObject.Paint(brush, hitInfo.point);
                                break;

                            case UseMethodType.NearestSurfacePoint:
                                success = erase ? paintObject.EraseNearestTriangleSurface(brush, hitInfo.point) : paintObject.PaintNearestTriangleSurface(brush, hitInfo.point);
                                break;

                            case UseMethodType.DirectUV:
                                if (!(hitInfo.collider is MeshCollider))
                                    Debug.LogWarning("Raycast may be unexpected if you do not use MeshCollider.");
                                success = erase ? paintObject.EraseUVDirect(brush, hitInfo.textureCoord) : paintObject.PaintUVDirect(brush, hitInfo.textureCoord);
                                break;
                        }
                        if (!success)
                            Debug.LogError("Failed to paint.");

                        if (lastPoint == Vector3.zero)
                        {
                            lastPoint = hitInfo.point;
                            return;
                        }
                        float distance = Vector3.Distance(hitInfo.point, lastPoint);
                        if (distance > brush.Scale)
                        {
                            Vector3 direction = (hitInfo.point - lastPoint).normalized;
                            int num = (int)(distance / (brush.Scale));
                            //Debug.Log("补间个数===" + num/5);
                            for (int i = 0; i <= num - 1; i=i+3)
                            {
                                Vector3 lerpPoint = lastPoint + direction * (i + 1) * brush.Scale;
                                Ray mRay = new Ray(ray.origin, (lerpPoint - ray.origin).normalized);
                                Debug.DrawLine(mRay.origin, lerpPoint);
                                RaycastHit newHitInfo;
                                if (Physics.Raycast(mRay, out newHitInfo))
                                {
                                    success = erase ? paintObject.Erase(brush, newHitInfo) : paintObject.Paint(brush, newHitInfo);
                                }
                            }
                            if (!success)
                            {
                                Debug.LogError("Failed lerp to point");
                            }
                        }
                        lastPoint = hitInfo.point;
                    }
                }
			}


           
            if (Input.GetMouseButtonUp(0))
            {
                lastPoint = Vector3.zero;
            }
        }

        //public void OnGUI()
        //{
        //    if (GUILayout.Button("Reset"))
        //    {
        //        foreach (var canvas in ModelControl.Instance.ModelGroup)
        //            canvas.GetComponent< InkCanvas >().ResetPaint();
        //    }
        //}

        //public void ResetMaterial()
        //{
        //    foreach (var canvas in FindObjectsOfType<InkCanvas>())
        //    {
        //        canvas.ResetPaint();
        //        Debug.Log("复原所有模型材质");
        //    }
        //}

    }
}