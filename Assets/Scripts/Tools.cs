using UnityEngine;


public class Tools  {

	public static bool IsInLayerMask(GameObject obj, LayerMask mask)
	{
		int objLaterMask = (1 << obj.layer);

		if ((mask.value & objLaterMask) > 0) {
			return true;
		} else
			return false;

	}
}
