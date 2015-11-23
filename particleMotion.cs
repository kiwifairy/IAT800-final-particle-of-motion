using UnityEngine;
using System.Collections;

public class particleMotion : MonoBehaviour
{
	Vector3[] positions ;
	Vector3[] randomPos ;
	float radius = 0.3f; //the random distance from the center
	float pointSize = 0.6f;
	ParticleSystem m_System;
	ParticleSystem.Particle[] points;

	void Start ()
	{
		if (m_System == null)
			m_System = GetComponent<ParticleSystem> ();
		m_System.maxParticles = 20;
		m_System.startSpeed = 2;
		m_System.startColor = new Color(1f,1f,1f,0.1f);
		m_System.startSize = pointSize;
		if (points == null || points.Length < m_System.maxParticles) {
			points = new ParticleSystem.Particle[m_System.maxParticles]; 
			positions = new Vector3[m_System.maxParticles];
		}
		randomPos = new Vector3[m_System.maxParticles];
		for (int i = 0; i < m_System.maxParticles; i++) {
			randomPos [i] = new Vector3 (Random.Range (-radius, radius), Random.Range (-radius, radius), Random.Range (-radius, radius));
		}
	}

	private void LateUpdate ()
	{
		int numParticlesAlive = m_System.GetParticles (points);
		int which = (Time.frameCount*7 )% m_System.maxParticles;   //* a prime number to adjust the lagging tail length
		positions [which] = Camera.main.ScreenPointToRay (Input.mousePosition).GetPoint (10);  // kinect: change this to the location of the child of impaired joint
		float movedDistance = Vector3.Distance (positions [which], positions [(which + 1) % positions.Length]);
		//particles disappear after stop of motion
		if (movedDistance < 0.1) {
			m_System.enableEmission = false;
		} else {
			m_System.enableEmission = true;
		}
		//lag with motion
		for (int i = 0; i < numParticlesAlive; i++) {
			// which+1 is the smallest (the oldest in the array)
			int index = (which + 1 + i) % m_System.maxParticles;
			points [i].position = positions[index]+ randomPos[i]; //positions[i] shows another effect
		}
		// Apply the particle changes to the particle system
		m_System.SetParticles (points, numParticlesAlive);
	}

}
