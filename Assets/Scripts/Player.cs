using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
	public CharacterController controller; //Contoller componenti
	public Animator anim; //Animator componenti
	public GameObject KameraObjesi; //Kamera objesi (Kamera oyuncunun sað ve sol hareketlerine göre takip edecek. Sahnedeki kamera transformunun tüm deðerleri sabit kalacak, yalnýzca position.x deðeri karakterin position.x deðerini takip edecek.)
	public GameObject mermiPrefab; //Duvar ya da npc objelerine týklandýðýnda spawnlanacak olan mermi prefabý

	// Use this for initialization
	void Start()
	{
	}

	// Update is called once per frame
	void Update()
	{
		AnimasyonVeRotasyon();
		YerCekimiVeZiplama();
		KameraTakip();
		KarakterHareket();
	}

	void AnimasyonVeRotasyon()
	{
	}

	void YerCekimiVeZiplama()
	{
	}

	void KameraTakip()
	{
	}

	void KarakterHareket()
	{

	}
}
