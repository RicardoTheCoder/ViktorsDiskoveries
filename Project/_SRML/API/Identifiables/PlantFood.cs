﻿using System.Collections.Generic;
using SRML.Utils;
using UnityEngine;

namespace VikDisk.SRML.API
{
	/// <summary>
	/// This is the base class to make Veggies or Fruits
	/// </summary>
	public abstract class PlantFood : IdentifiableItem
	{
		/// <summary>The name prefix for this object</summary>
		protected override string NamePrefix => IsFruit ? "fruit" : "veggie";

		/// <summary>The base item to use when creating the one</summary>
		public GameObject BaseItem => GameContext.Instance.LookupDirector?.GetPrefab(Identifiable.Id.POGO_FRUIT) ?? SRObjects.Get<GameObject>("fruitPogo");

		/// <summary>Is this plant a fruit? If not then it's a veggie</summary>
		public abstract bool IsFruit { get; }

		/// <summary>Which slimes have this food as it's favorite</summary>
		public virtual List<Identifiable.Id> IsFavoritedBy { get; } = null;

		/// <summary>The mesh of this plant food</summary>
		public abstract Mesh Mesh { get; }

		/// <summary>The material of the model</summary>
		public Material ModelMat { get; private set; } = null;

		/// <summary>Is this plant plantable in a garden or world?</summary>
		public virtual bool IsPlantable { get; } = true;

		/// <summary>The game hours for it to grow</summary>
		public virtual float UnripeGameHours { get; } = 6f;

		/// <summary>The game hours for it to be ripe</summary>
		public virtual float RipeGameHours { get; } = 6f;

		/// <summary>The game hours that is it edible</summary>
		public virtual float EdibleGameHours { get; } = 36f;

		/// <summary>The game hours for it to rotten</summary>
		public virtual float RottenGameHours { get; } = 6f;

		/// <summary>The rotten mat when this is plantable, null otherwise</summary>
		public virtual Material RottenMat { get; } = null;

		/// <summary>Should it be vacuumable when ripe?</summary>
		public virtual bool VacuumableWhenRipe { get; } = true;

		/// <summary>The scale of the Model</summary>
		public virtual Vector3 ModelScale { get; } = Vector3.one * 0.26f;

		/// <summary>Checks if it is a valid ammo for any storage type</summary>
		public override bool ValidSiloAmmo(SiloStorage.StorageType type)
		{
			return type == SiloStorage.StorageType.NON_SLIMES || type == SiloStorage.StorageType.FOOD;
		}

		/// <summary>Creates the material for the model</summary>
		public abstract Material CreateModelMat();

		/// <summary>Builds this Item</summary>
		public override void Build()
		{
			// Load Material
			ModelMat = CreateModelMat();

			// Get GameObjects
			Prefab = PrefabUtils.CopyPrefab(BaseItem);
			Prefab.name = NamePrefix + Name;
			Prefab.transform.localScale = Scale;

			GameObject child = Prefab.FindChild("model_pogofruit");
			child.transform.localScale = ModelScale;

			// Load Components
			SphereCollider sCol = Prefab.GetComponent<SphereCollider>();
			MeshFilter filter = Prefab.GetComponent<MeshFilter>();
			Rigidbody body = Prefab.GetComponent<Rigidbody>();
			Vacuumable vac = Prefab.GetComponent<Vacuumable>();
			Identifiable iden = Prefab.GetComponent<Identifiable>();

			ResourceCycle cycle = IsPlantable ? Prefab.GetComponent<ResourceCycle>() : null;

			MeshFilter model = child.GetComponent<MeshFilter>();
			MeshRenderer render = child.GetComponent<MeshRenderer>();

			// Setup Components
			sCol.radius = ModelScale.x;
			filter.sharedMesh = Mesh;
			body.mass = Mass;
			vac.size = Size;
			iden.id = ID;

			if (IsPlantable)
			{
				cycle.unripeGameHours = UnripeGameHours;
				cycle.ripeGameHours = RipeGameHours;
				cycle.edibleGameHours = EdibleGameHours;
				cycle.rottenGameHours = RottenGameHours;
				cycle.rottenMat = RottenMat ?? cycle.rottenMat;
				cycle.vacuumableWhenRipe = VacuumableWhenRipe;
			}
			else
				Object.Destroy(Prefab.GetComponent<ResourceCycle>());

			model.sharedMesh = Mesh;
			render.sharedMaterial = ModelMat;
		}

		/// <summary>Registers the item into it's registry</summary>
		public override IdentifiableItem Register()
		{
			base.Register();

			SlimeUtils.AddFoodToGroup(ID, IsFruit ? SlimeEat.FoodGroup.FRUIT : SlimeEat.FoodGroup.VEGGIES, IsFavoritedBy);
			SlimeUtils.AddFoodToGroup(ID, SlimeEat.FoodGroup.NONTARRGOLD_SLIMES, IsFavoritedBy);

			return this;
		}
	}
}
