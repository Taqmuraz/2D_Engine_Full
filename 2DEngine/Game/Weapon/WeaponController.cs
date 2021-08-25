namespace _2DEngine.Game
{
	public class WeaponController
	{
		IResolver actionResolver;
		ITimedResolver fireResolver;
		ITimedResolver reloadResolver;
		public int patrones { get; private set; }
		public int patronesCapacity { get; private set; }
		SoldierController soldier;
		float damage;
		float spread;
		AudioPlayer fireSound;
		AudioPlayer reloadSound;

		public WeaponController(SoldierController soldier, string weaponName, int patronesCapacity, float fps, float reloadTime, float damage, float spread)
		{
			fireResolver = new TimedResolver(1f / fps);
			reloadResolver = new TimedResolver(reloadTime);

			actionResolver = fireResolver.Concat(reloadResolver);

			this.patronesCapacity = patronesCapacity;
			this.soldier = soldier;
			this.damage = damage;
			this.spread = spread;
			patrones = patronesCapacity;

			fireSound = new AudioPlayer($"Fire_{weaponName}.wav", 1f);
			reloadSound = new AudioPlayer($"Reload_{weaponName}.mp3", 0.5f);
		}

		public void Fire()
		{
			if (actionResolver.resolving && patrones > 0)
			{
				var bullet = new GameObject("Bullet").AddComponent<Bullet>();
				Vector2 fwd = soldier.transform.forward;
				bullet.transform.position = soldier.transform.position + fwd * 0.75f;
				bullet.transform.forward = fwd;
				bullet.transform.rotation += Random.Range(-spread, spread);
				bullet.endDistance = 1000f;
				bullet.damage = damage;
				bullet.offset = soldier.weaponOffset;

				fireResolver.Reset();
				patrones--;

				if (Collider.Raycast(new Ray(soldier.transform.position, bullet.transform.forward), out RaycastHit hit))
				{
					bullet.endDistance = hit.distance;
					bullet.target = hit.collider.gameObject.GetComponent<IDamagable>();
				}

				fireSound.Play();
			}
		}

		public bool IsReloading()
		{
			return !reloadResolver.resolving;
		}

		public void Reload ()
		{
			if (reloadResolver.resolving)
			{
				reloadResolver.Reset();
				patrones = patronesCapacity;
				reloadSound.Play();
			}
		}
	}
}
