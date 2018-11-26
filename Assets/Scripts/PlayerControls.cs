﻿using System.Collections;
using System.Collections.Generic;
using Interaction;
using UnityEngine;

public class PlayerControls : MonoBehaviour {
	private Camera cam;
	private PlayerInfo info;
	public PauseMenuInterface pauseMenu;
	public InventoryInterface inventory;

	[Range(0f, 25f), SerializeField] private float _interactionDistance = 5f;
	
	void Start () {
		cam = Camera.main;
		info = GetComponent<PlayerInfo>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.E))
		{
			Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit) && hit.distance < 5) {
				if (hit.transform.CompareTag("Memory")) {
					hit.transform.gameObject.GetComponent<MemoryInfo>().PickObject();
					Destroy(hit.transform.gameObject);
				}
				else if (hit.transform.CompareTag("Battery")) {
					info.addBattery();
					Destroy(hit.transform.gameObject);
				}
				else if (hit.transform.CompareTag("Pill")) {
					info.addPill();
					Destroy(hit.transform.gameObject);
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			info.HandleFlashlight();
		}

		if (Input.GetButtonDown("Interact"))
		{
			Ray ray = cam.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, _interactionDistance, Layer.Interactable))
			{
				var interactable = hit.collider.GetComponent<InteractableBase>();
				if (interactable != null)
					interactable.Interact();
			}
		}
	}

	void LateUpdate()
	{
		if (Input.GetKeyDown(KeyCode.Tab) && !pauseMenu.isOpen()) {
			inventory.HandleInventory();
		}
		else if (Input.GetKeyDown(KeyCode.Escape)) {
			if (inventory.isOpen())
				inventory.HandleInventory();
			pauseMenu.HandlePauseMenu();
		}
	}
}