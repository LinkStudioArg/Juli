using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public Image lifeImage;
	public Text ammoText;
	public Sprite[] sprites;

	public void UpdateLife(int index)
	{
		if (sprites.Length != 0) {
			if (index <= sprites.Length)
				lifeImage.sprite = sprites [index];
		}
	}

	public void UpdateAmmo(int amount){

		ammoText.text = "x" + amount.ToString();
	}

}
