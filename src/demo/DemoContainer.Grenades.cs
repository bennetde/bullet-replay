using System;
using DemoFile.Sdk;

namespace DemoReplay.Demo;

public partial class DemoContainer
{
	public event EventHandler<CFlashbangProjectile> FlashGrenadeCreatedEventHandler;
	public event EventHandler<CFlashbangProjectile> FlashGrenadeDeletedEventHandler;
	public event EventHandler<CFlashbangProjectile> FlashGrenadeMovedEventHandler;

	public event EventHandler<CHEGrenadeProjectile> HEGrenadeCreatedEventHandler;
	public event EventHandler<CHEGrenadeProjectile> HEGrenadeDeletedEventHandler;
	public event EventHandler<CHEGrenadeProjectile> HEGrenadeMovedEventHandler;
	
	public event EventHandler<CMolotovProjectile> MolotovGrenadeCreatedEventHandler;
	public event EventHandler<CMolotovProjectile> MolotovGrenadeDeletedEventHandler;
	public event EventHandler<CMolotovProjectile> MolotovGrenadeMovedEventHandler;
	
	    
	public event EventHandler<CSmokeGrenadeProjectile> SmokeGrenadeCreatedEventHandler;
	public event EventHandler<CSmokeGrenadeProjectile> SmokeGrenadeDeletedEventHandler;
	public event EventHandler<CSmokeGrenadeProjectile> SmokeGrenadeMovedEventHandler;
	public event EventHandler<CSmokeGrenadeProjectile> SmokeGrenadePoppedEventHandler;

	public event EventHandler<CPlantedC4> BombCreatedEventHandler;
	public event EventHandler<CPlantedC4> BombDeletedEventHandler;
	public event EventHandler<CPlantedC4> BombMovedEventHandler;

	private void InitializeGrenadeEventHandlers()
	{
		Demo.EntityEvents.CFlashbangProjectile.Create += flash =>
		{
			FlashGrenadeCreatedEventHandler?.Invoke(this, flash);
		};
		
		Demo.EntityEvents.CFlashbangProjectile.Delete += flash =>
		{
			FlashGrenadeDeletedEventHandler?.Invoke(this, flash);
		};
		
		Demo.EntityEvents.CFlashbangProjectile.AddChangeCallback(flash => flash.Origin,
		(flash, _, _)=>
			{
				FlashGrenadeMovedEventHandler?.Invoke(this, flash);
			}
		);
		
		Demo.EntityEvents.CHEGrenadeProjectile.Create += nade =>
		{
			HEGrenadeCreatedEventHandler?.Invoke(this, nade);
		};
		
		Demo.EntityEvents.CHEGrenadeProjectile.Delete += nade =>
		{
			HEGrenadeDeletedEventHandler?.Invoke(this, nade);
		};
		
		Demo.EntityEvents.CHEGrenadeProjectile.AddChangeCallback(nade => nade.Origin,
			(nade, _, _)=>
			{
				HEGrenadeMovedEventHandler?.Invoke(this, nade);
			}
		);

		Demo.EntityEvents.CMolotovProjectile.Create += nade =>
		{
			MolotovGrenadeCreatedEventHandler?.Invoke(this, nade);
		};
		
		Demo.EntityEvents.CMolotovProjectile.Delete += nade =>
		{
			MolotovGrenadeDeletedEventHandler?.Invoke(this, nade);
		};

		Demo.EntityEvents.CMolotovProjectile.AddChangeCallback(nade => nade.Origin,
			(nade, _, _) =>
			{
				MolotovGrenadeMovedEventHandler?.Invoke(this, nade);
			}
		);
		
		Demo.EntityEvents.CSmokeGrenadeProjectile.Create += e =>
		{
			SmokeGrenadeCreatedEventHandler?.Invoke(this, e);
		};
        
		Demo.EntityEvents.CSmokeGrenadeProjectile.Delete += e =>
		{
			SmokeGrenadeDeletedEventHandler?.Invoke(this, e);
		};

		Demo.EntityEvents.CSmokeGrenadeProjectile.AddChangeCallback(
			(smoke => smoke.Origin),
			(smoke, _, _) =>
			{
				SmokeGrenadeMovedEventHandler?.Invoke(this, smoke);
			}
		);
        
		Demo.EntityEvents.CSmokeGrenadeProjectile.AddChangeCallback(
			(smoke => smoke.DidSmokeEffect),
			(smoke, _, _) =>
			{
				SmokeGrenadePoppedEventHandler?.Invoke(this, smoke);
			}
		);

		Demo.EntityEvents.CPlantedC4.Create += e =>
		{
			BombCreatedEventHandler?.Invoke(this, e);
		};
        
		Demo.EntityEvents.CPlantedC4.Delete += e =>
		{
			BombDeletedEventHandler?.Invoke(this, e);
		};

		Demo.EntityEvents.CPlantedC4.AddChangeCallback(bomb => bomb.Origin,
			(bomb, _, _) =>
			{
				BombMovedEventHandler?.Invoke(this, bomb);
			}
		);
	}
}