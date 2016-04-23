using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class splashScreenAnimation: MonoBehaviour {
	
	public OTSprite headCenterPrefab;
	public List<OTSprite> teamSpritesLstPrefab;
	public OTSprite QuechuaSpritePrefab;	
	public List<Texture> textLst;
	public AudioClip letterSound;
	public AudioClip intro;
	
	private OTSprite head1, head2, head3, headCenter;
	private List<OTSprite> teamSpritesLst;
	private OTSprite QuechuaSprite;
	
	private float angularSpeed;
	private float angularSum;
	private OTSprite secondCenter;
	
	// Use this for initialization
	void Start () {
		audio.PlayOneShot(intro);
		angularSpeed = 200;
		angularSum = 0;
		headCenter = (OTSprite)Object.Instantiate(headCenterPrefab);
		headCenter.position=new Vector3(-311,125,0);
		head1 = (OTSprite)headCenter.Child("Head1");
		head2 = (OTSprite)headCenter.Child("Head2");
		head3 = (OTSprite)headCenter.Child("Head3");
		teamSpritesLst = new List<OTSprite>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.frameCount %20 == 0 && Time.frameCount/20<5) {
				teamSpritesLst.Add ((OTSprite)Object.Instantiate(teamSpritesLstPrefab[Time.frameCount/20-1]));
			audio.PlayOneShot(letterSound);
		}
		
		if(angularSum >= 720 && angularSum <750) {
				angularSpeed = 0;
				head1.transform.localPosition -= head1.transform.localPosition.normalized *0.5f;
				head2.transform.localPosition -= head2.transform.localPosition.normalized *0.5f;
				head3.transform.localPosition -= head3.transform.localPosition.normalized *0.5f;
				head1.texture = textLst[0];
				head2.texture = textLst[1];
				head3.texture = textLst[2];
				angularSum = 0;
				this.audio.Play();
				QuechuaSprite = (OTSprite)Object.Instantiate(QuechuaSpritePrefab);
				Debug.Log(Time.frameCount);
		} else {
		
		
		if(headCenter.size.x>45) {
			headCenter.size*=0.95f;
		}
		else
		{
				headCenter.transform.Rotate(0,0,Time.deltaTime*angularSpeed);
				head1.transform.Rotate(0,0,Time.deltaTime*(-angularSpeed));
				head2.transform.Rotate(0,0,Time.deltaTime*(-angularSpeed));
				head3.transform.Rotate(0,0,Time.deltaTime*(-angularSpeed));
				angularSum += (Time.deltaTime*angularSpeed);
				
			}			
		
		}
	}
}
