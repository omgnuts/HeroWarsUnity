﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public enum UnitType { footman, archer, scout, catapult, knight, guard, bombard, greatKnight };
public enum UnitGroup { infantry, cavalry, artillery, flying };
public enum Behaviour { none, hold, defend, capture };

public class Unit : MonoBehaviour {

	// position properties
   public Vector2 xy;
      public int x {get{return (int)xy.x;}}
      public int y {get{return (int)xy.y;}}
   public Vector2 RoundedTransformPosition { 
      get {
         float roundedX = (float) Math.Round(transform.position.x);
         float roundedY = (float) Math.Round(transform.position.y);
         Vector2 roundedPosition = new Vector2(roundedX, roundedY);
         return roundedPosition;
      }
   }
   // general properties
	public UnitType type;
	public UnitGroup grouping;
	public bool activated = true;
	public int owner;
	public int movePoints;
   public Dictionary<TileType,int> moveCosts;
	public int[] range = new int[2];
	// ride/drop properties
	public bool canCarry = false;
	public Unit cargo;
	// combat properties
	public int cost;
	public int health = 100;
	public int damage;
	public int bonusDamage;
	public float pen;
	public float bonusPen;
	public UnitGroup bonusCondition;
	public float armor;
	// UI references
	private Color inactiveColor = Color.gray;
	private SpriteRenderer spriteRenderer;
	private GameObject healthLabel;
	private Text healthText;
	private GameObject carryLabel;
	private Text carryText;
	private GameObject damageLabel;
	private Text damageText;
	// AI properties
	public float powerConstant = 1;
	public Behaviour behaviour = Behaviour.none;
	public Vector2 captureAssignment = new Vector2(-100,-100);

	void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		healthLabel = transform.GetChild(0).GetChild(0).gameObject;
		healthText = transform.GetChild(0).GetChild(0).GetChild(1).GetComponent<Text>();
		carryLabel = transform.GetChild(0).GetChild(1).gameObject;
		carryText = transform.GetChild(0).GetChild(1).GetChild(1).GetComponent<Text>();
		damageLabel = transform.GetChild(0).GetChild(2).gameObject;
		damageText = transform.GetChild(0).GetChild(2).GetChild(1).GetComponent<Text>();
	}

   void Start() {
      Register();
      SetMoveCosts();
   }

   private void SetMoveCosts() {
      switch (grouping) {
         case UnitGroup.infantry:
            moveCosts = MoveCosts.Infantry;
            break;
         case UnitGroup.cavalry:
            moveCosts = MoveCosts.Cavalry;
            break;
         case UnitGroup.artillery:
            moveCosts = MoveCosts.Artillery;
            break;
         case UnitGroup.flying:
            moveCosts = MoveCosts.Flying;
            break;
         default:
            Debug.Log("Unknown UnitType for moveCosts");
            break;
      }
   }

	void OnMouseUpAsButton()
	{
		if (!InputManager.IsPointerOverUIButton()) {
			InputManager.HandleInput("tapUnit", this);
		}
	}

   private void Register() {
      int x = (int)Math.Round(transform.position.x);
      int y = (int)Math.Round(transform.position.y);
      xy = new Vector2(x,y);
      transform.position = xy;
      GridManager.RegisterUnit(this);
   }

	public void ChangeHealth(int amount)
	{
		health += amount;
		if (health > 100) health = 100;
		int h = HealthInt();
		if (h <= 9)
		{
			healthText.text = h.ToString();
			healthLabel.SetActive(true);
		} else {
			healthText.text = "";
			healthLabel.SetActive(false);
		}
		GridManager.UpdateUnit(this);
	}

	public int HealthInt()
	{
		return Mathf.CeilToInt(health / 10f);
	}

	public void Activate()
	{
		activated = true;
		if (spriteRenderer.Equals(null))
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
		spriteRenderer.color = Color.white;
	}

	public void Deactivate()
	{
		activated = false;
		if (spriteRenderer.Equals(null))
		{
			spriteRenderer = GetComponent<SpriteRenderer>();
		}
		spriteRenderer.color = inactiveColor;
	}

	public void ShowDamageLabel(int amount)
	{
		damageText.text = Math.Round(amount / 10f, 1).ToString();
		damageLabel.SetActive(true);
	}

	public void HideDamageLabel()
	{
		damageLabel.SetActive(false);
	}

	// AI relevant methods
	public float GetPower()
	{
		return cost * powerConstant * health / 100f;
	}

}
