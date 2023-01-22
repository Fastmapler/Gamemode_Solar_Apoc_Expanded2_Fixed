function GameConnection::DisplayWeaponAmmo(%client)
{
    if (!isObject(%player = %client.player))
        return;

    if (!isObject(%image = %player.getMountedImage(0)) || %image.ammoType $= "")
        return;
}