$EOTW::ItemCrafting["ACNItem"] = (999999 TAB "Adamantine");
$EOTW::ItemDescription["ACNItem"] = "If you are able to obtain this then I probably messed up somewhere.";

exec("./Support_AmmoGuns.cs");
exec("./Weapon_MG.cs");
exec("./Weapon_SG.cs");
exec("./Weapon_GL.cs");
exec("./Weapon_Crystal.cs");

function updateWeaponDamage()
{
    if (isObject(BioRifleProjectile))
        BioRifleProjectile.directDamage = 1;
    if (isObject(acidProjectile))
        acidProjectile.directDamage = 4;
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
            %col.damage(%obj, %col.getposition(), 75, $DamageType::CannonBallDirect);
        }
        
        // %speed = vectorLen( %obj.getVelocity() );

        //Pass check
        if( %obj.hasBounced &&  passBallCheck(%obj,%col) )
            return;
        
        %obj.hasBounced = 1;
        //Projectile::onCollision(%this,%obj,%col,%fade,%pos,%normal);
    }
};
activatePackage("EOTW_WeaponBalancing");