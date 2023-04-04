$EOTW::ItemCrafting["ACNItem"] = (999999 TAB "Adamantine");
$EOTW::ItemDescription["ACNItem"] = "If you are able to obtain this then I probably messed up somewhere.";

exec("./Item_Ammo.cs");
exec("./Support_AmmoGuns.cs");
exec("./Support_Homing.cs");
exec("./Weapon_MG.cs");
exec("./Weapon_SG.cs");
exec("./Weapon_GL.cs");
exec("./Weapon_Crystal.cs");
exec("./Weapon_Nuke.cs");

function updateWeaponDamage()
{
    if (isObject(BioRifleProjectile))
        BioRifleProjectile.directDamage = 1;
    if (isObject(acidProjectile))
        acidProjectile.directDamage = 4;
}
schedule(100, 0, "updateWeaponDamage");

datablock ProjectileData (hammerAttackProjectile : hammerProjectile)
{
	directDamage = 4;
	Explosion = "";
	lightRadius = 0;
};

package EOTW_WeaponBalancing
{
    function dodgeballProjectile::onCollision(%this,%obj,%col,%fade,%pos,%normal)
    {
        //Play bounce sound and check that we haven't played it too recently
        %obj.playSportBallSound( dodgeballBounceSound );
        
        // do onBallHit event, only call this on bricks obviously
        if( %col.getType() & $TypeMasks::FxBrickObjectType )
            %col.onBallHit( %obj.sourceObject, %obj );
            
        //Kill check
        if(!%obj.hasBounced && %col.getType() & $TypeMasks::PlayerObjectType && minigameCanDamage(%obj.sourceObject, %col) == 1)
        {
            //do minigame checks then kill player use the cannon death image
            //%col.kill();
            %col.damage(%obj, %col.getposition(), 75, $DamageType::CannonBallDirect);
        }
        
        // %speed = vectorLen( %obj.getVelocity() );

        //Pass check
        if( %obj.hasBounced &&  passBallCheck(%obj,%col) )
            return;
        
        %obj.hasBounced = 1;
        //Projectile::onCollision(%this,%obj,%col,%fade,%pos,%normal);
    }
    function hammerImage::onFire (%this, %player, %slot)
    {
        %start = %player.getEyePoint ();
        %muzzleVec = %player.getMuzzleVector (%slot);
        %muzzleVecZ = getWord (%muzzleVec, 2);
        if (%muzzleVecZ < -0.9)
        {
            %range = 5.5;
        }
        else 
        {
            %range = 5;
        }
        %vec = VectorScale (%muzzleVec, %range * getWord (%player.getScale (), 2));
        %end = VectorAdd (%start, %vec);
        %mask = $TypeMasks::FxBrickAlwaysObjectType | $TypeMasks::PlayerObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::StaticShapeObjectType | $TypeMasks::StaticObjectType;
        if (%player.isMounted ())
        {
            %raycast = containerRayCast (%start, %end, %mask, %player, %player.getObjectMount ());
        }
        else 
        {
            %raycast = containerRayCast (%start, %end, %mask, %player);
        }
        if (!%raycast)
        {
            return;
        }
        %hitObj = getWord (%raycast, 0);
        %hitPos = getWords (%raycast, 1, 3);
        %hitNormal = getWords (%raycast, 4, 6);
        %projectilePos = VectorSub (%hitPos, VectorScale (%player.getEyeVector (), 0.25));
        %p = new Projectile ("")
        {
            dataBlock = hammerProjectile;
            initialVelocity = %hitNormal;
            initialPosition = %projectilePos;
            sourceObject = %player;
            sourceSlot = %slot;
            client = %player.client;
        };
        %p.setScale (%player.getScale ());
        MissionCleanup.add (%p);
        %this.onHitObject (%player, %slot, %hitObj, %hitPos, %hitNormal);

        if (getSimTime() - %player.lastHammerAttack > 200)
        {
            %player.lastHammerAttack = getSimTime();

            %p = new Projectile ("")
            {
                dataBlock = hammerAttackProjectile;
                initialVelocity = %hitNormal;
                initialPosition = %projectilePos;
                sourceObject = %player;
                sourceSlot = %slot;
                client = %player.client;
            };
            %p.setScale (%player.getScale ());
            MissionCleanup.add (%p);
        }
    }
};
activatePackage("EOTW_WeaponBalancing");