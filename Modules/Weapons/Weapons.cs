$EOTW::ItemCrafting["ACNItem"] = (999999 TAB "Adamantine");
$EOTW::ItemDescription["ACNItem"] = "If you are able to obtain this then I probably messed up somewhere.";

$EOTW::ItemCrafting["DragonbreathItem"] = (512 TAB "Granite") TAB (128 TAB "Brimstone");
$EOTW::ItemDescription["DragonbreathItem"] = "The same stuff the red guys use. Uses additional Brimstone as ammo.";

exec("./Item_Ammo.cs");
exec("./Support_AmmoGuns.cs");
exec("./Support_Homing.cs");
exec("./Weapon_MG.cs");
exec("./Weapon_SG.cs");
exec("./Weapon_GL.cs");
exec("./Weapon_Crystal.cs");
exec("./Weapon_Nuke.cs");
exec("./Weapon_Bulwark.cs");

function updateWeaponDamage()
{
    if (isObject(BioRifleProjectile))
        BioRifleProjectile.directDamage = 20;
    if (isObject(BioRifleExplosion))
        BioRifleExplosion.radiusDamage = 1;
    if (isObject(acidProjectile))
        acidProjectile.directDamage = 4;
    if (isObject(hammerProjectile))
        hammerProjectile.directDamage = 4;
    if (isObject(DragonbreathImage))
    {
        DragonbreathImage.ammoType = "Brimstone";
        DragonbreathImage.armReady = true;
        Dragonbreathprojectile.directDamage = 12;
        Dragonbreathprojectile.impactImpulse = 200;
        Dragonbreathprojectile.verticalImpulse = 65;
    }
}
schedule(100, 0, "updateWeaponDamage");

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
            %col.damage(%obj, %col.getposition(), 20, $DamageType::CannonBallDirect);
        }
        
        // %speed = vectorLen( %obj.getVelocity() );

        //Pass check
        if( %obj.hasBounced &&  passBallCheck(%obj,%col) )
            return;
        
        %obj.hasBounced = 1;
        //Projectile::onCollision(%this,%obj,%col,%fade,%pos,%normal);
    }
    function DragonbreathImage::onMount(%this, %obj, %slot)
    {
        Parent::onMount(%this, %obj, %slot);
		%obj.playThread(1, armReadyRight);
    }
    function DragonbreathImage::onFire(%this,%obj,%slot)
    {
        if(%obj.getEnergyLevel() < %this.energyUsage)
        {
            %obj.unMountImage(0);
            %obj.client.chatMessage("Not enough stamina!");
            return;
        }
        
        %obj.setEnergyLevel(%obj.getEnergyLevel() - %this.energyUsage);
        %shellcount = 1;
        %ammoType = "Brimstone";
        %shellcount = getMin($EOTW::Material[%obj.client.bl_id, %ammoType], %shellcount);
        if (%shellcount < 1)
        {
            %obj.unMountImage(0);
            %obj.client.chatMessage("Not enough ammo!");
            return;
        }
        if (!%obj.hasEffect("Ranging") || getRandom() > 0.6)
            $EOTW::Material[%obj.client.bl_id, %ammoType] -= %shellcount;
        %obj.client.PrintEOTWInfo();
        %projectile = Dragonbreathprojectile;

        for(%shell=0; %shell<%shellcount; %shell++)
        {
            %vector = %obj.getMuzzleVector(%slot);
            %objectVelocity = %obj.getVelocity();
            %vector1 = VectorScale(%vector, %projectile.muzzleVelocity);
            %vector2 = VectorScale(%objectVelocity, %projectile.velInheritFactor);
            %velocity = VectorAdd(%vector1,%vector2);
            %x = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
            %y = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
            %z = (getRandom() - 0.5) * 10 * 3.1415926 * %spread;
            %mat = MatrixCreateFromEuler(%x @ " " @ %y @ " " @ %z);
            %velocity = MatrixMulVector(%mat, %velocity);

            %p = new (%this.projectileType)()
            {
                dataBlock = %projectile;
                initialVelocity = %velocity;
                initialPosition = %obj.getMuzzlePoint(%slot);
                sourceObject = %obj;
                sourceSlot = %slot;
                client = %obj.client;
            };
            MissionCleanup.add(%p);
            %p.setScale(%obj.getScale());
        }
        
        %obj.setVelocity(VectorAdd(%obj.getVelocity(), VectorScale(%obj.getEyeVector(),"-0.8")));
    }
    function hammerImage::onHitObject (%this, %player, %slot, %hitObj, %hitPos, %hitNormal)
    {
        %client = %player.client;
        ServerPlay3D (hammerHitSound, %hitPos);
        if (!isObject (%client))
        {
            return;
        }
        if (%hitObj.getType () & $TypeMasks::FxBrickAlwaysObjectType)
        {
            if (!isObject (%client))
            {
                return;
            }
            if (!%hitObj.willCauseChainKill ())
            {
                if (getTrustLevel (%player, %hitObj) < $TrustLevel::Hammer)
                {
                    if (%hitObj.stackBL_ID $= "" || %hitObj.stackBL_ID != %client.getBLID ())
                    {
                        %client.sendTrustFailureMessage (%hitObj.getGroup ());
                        return;
                    }
                }
                %hitObj.onToolBreak (%client);
                $CurrBrickKiller = %client;
                %hitObj.killBrick ();
            }
        }
        else if (%hitObj.getType() & $TypeMasks::PlayerObjectType)
        {
            if (miniGameCanDamage (%client, %hitObj) == 1 && getSimTime() - %player.lastHammerAttack >= 200)
            {
                %player.lastHammerAttack = getSimTime();
                %hitObj.Damage (%player, %hitPos, hammerProjectile.directDamage, $DamageType::HammerDirect);
            }
        }
        else if (%hitObj.getClassName () $= "WheeledVehicle" || %hitObj.getClassName () $= "HoverVehicle" || %hitObj.getClassName () $= "FlyingVehicle")
        {
            %mount = %player;
            %i = 0;
            while (%i < 100)
            {
                if (%mount == %hitObj)
                {
                    return;
                }
                if (!%mount.isMounted ())
                {
                    break;
                }
                %mount = %mount.getObjectMount ();
                %i += 1;
            }
            %doFlip = 0;
            if (isObject (%hitObj.spawnBrick))
            {
                %vehicleOwner = findClientByBL_ID (%hitObj.spawnBrick.getGroup ().bl_id);
            }
            else 
            {
                %vehicleOwner = 0;
            }
            if (isObject (%vehicleOwner))
            {
                if (getTrustLevel (%player, %hitObj) >= $TrustLevel::VehicleTurnover)
                {
                    %doFlip = 1;
                }
            }
            else 
            {
                %doFlip = 1;
            }
            if (miniGameCanDamage (%player, %hitObj) == 1)
            {
                %doFlip = 1;
            }
            if (miniGameCanDamage (%player, %hitObj) == 0)
            {
                %doFlip = 0;
            }
            if (%doFlip)
            {
                %impulse = VectorNormalize (%vec);
                %impulse = VectorAdd (%impulse, "0 0 1");
                %impulse = VectorNormalize (%impulse);
                %force = %hitObj.getDataBlock ().mass * 5;
                %impulse = VectorScale (%impulse, %force);
                %hitObj.applyImpulse (%hitPos, %impulse);
            }
        }
    }
};
activatePackage("EOTW_WeaponBalancing");