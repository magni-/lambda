using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class gridScript : MonoBehaviour {
	
	
	/* TODO : stocker la distance entre l'épicentre et le point dans un tableau
	 * et effectuer la translation unité par unité (variable permettant de faire plus ou moins vite
	 * Rebond : ajouter décrément de la distance au cours du temps pour atténuer l'effet jusqu'a l'arret total
	 * (faire distanceFromEpicentre*(-1)
	 */ 
	
	//Deformation parameters
	public float radius=25;
	public float bubble=1;
	
	//Effect data
	private List<Vector3> distances;
	private List<Vector3> distancesVariables;
	private Vector3[] vert;
	private Vector3 vertWCS ;
	
	public float effectStep=0.03f;
	private List<Vector3> epicentres;
	private int sens;
	private bool changeSens=false;

	// Use this for initialization
	void Start () {
	
		vert = this.GetComponent<MeshFilter>().mesh.vertices;
		distances=new List<Vector3>(700);
		Debug.Log(distances.Count);
		distancesVariables=new List<Vector3>(700);
			for(int i=0;i<700;i++) {
			distances.Add(Vector3.zero);
			distancesVariables.Add (Vector3.zero);
		}
		epicentres=new List<Vector3>();
		applyEffect(Vector3.zero);
		applyEffect(new Vector3(300,50,0));
		sens=1;
	}
	
	// Update is called once per frame
	void Update () {
		
		//if there is epicentre(s), take it into account
		for(int j=0;j<epicentres.Count;++j) {
			for(int i=0;i<vert.Length;++i) {
				vertWCS=this.transform.TransformPoint(vert[i]);
				if((vertWCS-epicentres[j]).magnitude<radius){
					distances[i] =(vertWCS-epicentres[j]);
					distancesVariables[i] =(vertWCS-epicentres[j]);
				}
			}
		}
	
		//Applying effect from previous data
		for(int i=0;i<vert.Length;++i) 
			{
					Vector3 direction=distancesVariables[i];
					float mag=distancesVariables[i].magnitude;
				
					direction.Normalize();
					if(mag>0.5f) {
						if(mag/radius<4)  {	
							direction*=sens*5*Mathf.Abs(Time.deltaTime*5/Mathf.Sqrt(2)*Mathf.Exp(1/2*(mag/radius)*(mag/radius)));
							vert[i]+=this.transform.InverseTransformPoint(direction);
							
							distancesVariables[i].Normalize();
							distancesVariables[i]*=mag-direction.magnitude;
						}
					}
			else if(distancesVariables[i]!=new Vector3(0,0,0)) {
							distancesVariables[i].Normalize();
							distancesVariables[i]*=-2*distances[i].magnitude;
							Debug.Log ("changement");
			}
			
		
		}
			if(Time.frameCount/120==1)
			{
				Debug.Log("gogogo");
				sens*=-1;
			}
			if(Time.frameCount%240==0){
			sens*=-1;
		}	
		this.GetComponent<MeshFilter>().mesh.vertices=vert;	
	}
		
	
	public void applyEffect (Vector3 position) {
		epicentres.Add (position);
	}
}
