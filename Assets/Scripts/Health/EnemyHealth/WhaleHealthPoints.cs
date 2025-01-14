using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhaleHealthPoints : HealthPoints
{
	[SerializeField] private GameObject spriteHolder;
	[SerializeField] private GameObject attackBox;
	[SerializeField] private float spriteFadeSpeed;
	[SerializeField] private float spriteFadeTime;
	[SerializeField] private GameObject spritePlane;
	[SerializeField] private Material deathMaterial;
	[SerializeField] private EnemyDeathSound deathSound;
	private float startTime;

	public override void kill()
	{
		deathSound.playDeathSound();
		this.gameObject.GetComponent<WhaleNav>().enabled = false;
		this.gameObject.GetComponent<WhaleAttack>().enabled = false;
        this.gameObject.GetComponent<Collider>().enabled = false;
//		attackBox.SetActive(false);
		StartCoroutine(fadeSprite());
	}

	IEnumerator fadeSprite()
	{
		startTime = Time.time;
		spriteHolder.GetComponent<DirectionalSprite>().calculateOnce = true;
		spritePlane.GetComponent<MeshRenderer>().material = deathMaterial;
		while (startTime + spriteFadeTime > Time.time)
		{
			Vector3 tempPos = spriteHolder.transform.localPosition;
			tempPos.y += spriteFadeSpeed * Time.deltaTime;
			spriteHolder.transform.localPosition = tempPos;
			yield return new WaitForEndOfFrame();
		}
		Destroy(this.gameObject);
		yield return null;
	}
}

