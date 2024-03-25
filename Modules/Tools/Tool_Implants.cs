function GameConnection::hasImplant(%client, %implant)
{
    return hasField(%client.implantList, %implant);
}

function Player::hasImplant(%client, %implant)
{
    return isObject(%player.client) && %player.client.hasImplant(%implant);
}

function GameConnection::grantImplant(%client, %implant)
{
    if (hasField(%client.implantList, %implant))
        return false;

    %client.implantList = trim(%client.implantList TAB %implant);

    EOTW_applyBooze(%client.player, 18);

    return true;
}


//Mending

//Adrenline

//Smelting

//Leatherskin