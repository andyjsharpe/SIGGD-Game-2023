using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class HandMechanics : MonoBehaviour
{
    private float lifeTime;
    private float reelTime;
    private float DAMAGE;
    private Transform parentSiren = null;
    private float pullStrength;
    private float grabTime;
    private float birthTime;
    private bool attached;
    private GameObject curPlayer;
    [SerializeField] private GameObject spriteRenderer;
    [SerializeField] private AudioSource soundEffect;


    public void SetFields(Transform parent, float dmg, float life, float reel, float pull, float grab) {
        parentSiren = parent;
        lifeTime = life;
        DAMAGE = dmg;
        reelTime = reel;
        pullStrength = pull;
        grabTime = grab;
    }

    // Start is called before the first frame update
    void Start()
    {
        birthTime = Time.time;
        attached = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (attached == false && birthTime + lifeTime < Time.time) {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider col) {
        Debug.Log(col.gameObject.tag);
        if (col.gameObject.tag == "Player") {
            attached = true;
            curPlayer = col.gameObject;
            StartCoroutine(waitAndDie());
        }
        else if (col.gameObject.tag == "Wall") {
            Destroy(this.gameObject);
        }
    }

    public void ShutItDown() {
        StopAllCoroutines();
        if (curPlayer != null) {
            curPlayer.GetComponent<Movement>().sirend = false;
        }
        Destroy(this.gameObject);
    }

    IEnumerator waitAndDie() {
        soundEffect.Play();
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        spriteRenderer.SetActive(false);
        this.gameObject.GetComponent<SphereCollider>().enabled = false;
        curPlayer.GetComponent<HealthPoints>().damageEntity(DAMAGE);
        Movement playerMove = curPlayer.GetComponent<Movement>();
        if (playerMove.sirend == false) {
            playerMove.sirend = true;
            curPlayer.GetComponent<Rigidbody>().velocity = Vector3.zero;
            yield return new WaitForSeconds(grabTime);
            IEnumerator pull = pullPlayer();
            StartCoroutine(pull);
            yield return new WaitForSeconds(reelTime);
            StopCoroutine(pull);            
            if (curPlayer == null) {  //in case the player dies before the hand un-sticks
                Destroy(this.gameObject);
                yield return null;
            }
            playerMove.sirend = false;
        }
        //this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        //spriteRenderer.SetActive(false);
        Destroy(this.gameObject);
        yield return null;
    }

    IEnumerator pullPlayer() {
        Rigidbody playerRB = curPlayer.GetComponent<Rigidbody>();
        while (curPlayer != null) {
            Debug.Log("moving");
            Vector3 dir = parentSiren.position - playerRB.position;
            dir.y = curPlayer.transform.position.y;
            Vector3 newPos = curPlayer.transform.position + (Time.deltaTime * pullStrength * dir.normalized);
            playerRB.MovePosition(newPos);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
}
