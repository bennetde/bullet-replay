using Godot;
using System;
using DemoFile.Sdk;
using DemoReplay.Resources;
using DemoReplay.singletons;

public partial class PlayerCard : VBoxContainer
{
	[Export]
	private Label HealthLabel { get; set; }
	[Export]
	private Label NameLabel { get; set; }
	[Export]
	private Label KADLabel { get; set; }
	[Export]
	private Label MoneyLabel { get; set; }
	[Export]
	private Godot.ProgressBar HealthProgressBar { get; set; }
	
	[Export]
	private TextureRect PrimaryWeaponTexture { get; set; }
	
	[Export]
	private TextureRect SecondaryWeaponTexture { get; set; }
	
	[Export]
	private TextureRect MolotovTexture { get; set; }
	
	[Export]
	private TextureRect SmokeTexture { get; set; }
	
	[Export]
	private TextureRect HETexture { get; set; }
	
	[Export]
	private TextureRect FlashTexture { get; set; }
	
	[Export]
	private CSColors CSColors { get; set; }
	
	[Export]
	private WeaponsDictionary WeaponsDictionary { get; set; }

	private int kills;
	private int assists;
	private int deaths;

	public int Health
	{
		set
		{
			HealthLabel.Text = value.ToString();
			HealthProgressBar.Value = value;
			if (value == 0)
			{
				DisableWeaponTextures();
			}
		}
	}

	public string PlayerName
	{
		set => NameLabel.Text = value;
	}

	public int Money
	{
		set => MoneyLabel.Text = "$" + value;
	}

	public int Kills
	{
		set
		{
			kills = value;
			UpdateKADLabel();
			
		}
		get => kills;
	}

	public int Assists
	{
		set
		{
			assists = value;
			UpdateKADLabel();
		}
		get => assists;
	}

	public int Deaths
	{
		set
		{
			deaths = value;
			UpdateKADLabel();
		}
		get => deaths;
	}

	public int TeamNum
	{
		set => HealthProgressBar.SelfModulate = CSColors.GetColorFromTeam(value);
	}

	public void UpdateKADLabel()
	{
		KADLabel.Text = $"{Kills}/{Assists}/{Deaths}";
	}

	public void AddItem(string weaponName)
	{
		if (WeaponsDictionary.Weapons.TryGetValue(weaponName, out var weapon))
		{
			if (weapon.IsC4) return;
			if (weapon.IsKnife) return;
			switch (weapon.GrenadeType)
			{
				case GrenadeType.Flash:
					FlashTexture.Visible = true;
					return;
				case GrenadeType.Smoke:
					SmokeTexture.Visible = true;
					return;
                case GrenadeType.HE:
	                HETexture.Visible = true;
	                return;
                case GrenadeType.Molotov:
	                MolotovTexture.Texture = weapon.Texture;
	                MolotovTexture.Visible = true;
	                return;
			}
			if (!weapon.IsSecondary)
			{
				PrimaryWeaponTexture.Texture = weapon.Texture;
				PrimaryWeaponTexture.Visible = true;
			}
			else
			{
				SecondaryWeaponTexture.Texture = weapon.Texture;
				SecondaryWeaponTexture.Visible = true;
			}
		}
		else
		{
			GD.PushWarning($"Couldn't find weapon {weaponName}");
		}
	}

	public void DisableWeaponTextures()
	{
		PrimaryWeaponTexture.Visible = false;
		SecondaryWeaponTexture.Visible = false;
		HETexture.Visible = false;
		SmokeTexture.Visible = false;
		FlashTexture.Visible = false;
		MolotovTexture.Visible = false;
	}
}
