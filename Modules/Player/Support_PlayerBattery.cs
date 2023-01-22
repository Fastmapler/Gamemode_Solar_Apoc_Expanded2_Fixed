function Player::GetBatteryText(%obj)
{
    if (!isObject(%client = %obj.client))
            return;

    %text = "\c6[\c4";

    %i = 0.05;

    while (%i <= (%obj.GetBatteryEnergy() / %obj.GetMaxBatteryEnergy()))
    {
        %text = %text @ "|";
        %i += 0.05;
    }

    %text = %text @ "\c0";

    while (%i <= 1.0)
    {
        %text = %text @ "|";
        %i += 0.05;
    }

    %text = %text @ "\c6] (" @ %obj.GetBatteryEnergy() @ "/" @ %obj.GetMaxBatteryEnergy() @ " EU)";

    return %text;
}

function Player::GetBatteryEnergy(%obj)
{
    if (isObject(%obj.client))
        return %obj.client.GetBatteryEnergy();
    else
        return 0;
}

function Player::SetBatteryEnergy(%obj, %set)
{
     if (isObject(%obj.client))
        return %obj.client.SetBatteryEnergy(%set);
    else
        return 0;
}

function Player::GetMaxBatteryEnergy(%obj)
{
    if (isObject(%obj.client))
        return %obj.client.GetMaxBatteryEnergy();
    else
        return 0;
}

function Player::SetMaxBatteryEnergy(%obj, %set)
{
    if (isObject(%obj.client))
        return %obj.client.SetMaxBatteryEnergy(%set);
    else
        return 0;
}

function Player::ChangeBatteryEnergy(%obj, %change)
{
    if (isObject(%obj.client))
        return %obj.client.ChangeBatteryEnergy(%change);
    else
        return 0;
}

function GameConnection::GetBatteryEnergy(%obj)
{
    if (%obj.BatteryEnergy $= "" || %obj.BatteryEnergy < 0)
        %obj.BatteryEnergy = 0;

    return %obj.BatteryEnergy;
}

function GameConnection::SetBatteryEnergy(%obj, %set)
{
     %obj.BatteryEnergy = %set;

    return %obj.GetBatteryEnergy();
}

function GameConnection::GetMaxBatteryEnergy(%obj)
{
    if (%obj.MaxBatteryEnergy $= "" || %obj.MaxBatteryEnergy < 0)
        %obj.MaxBatteryEnergy = 5000;

    return %obj.MaxBatteryEnergy;
}

function GameConnection::SetMaxBatteryEnergy(%obj, %set)
{
     %obj.MaxBatteryEnergy = %set;

    return %obj.GetMaxBatteryEnergy();
}

function GameConnection::ChangeBatteryEnergy(%obj, %change)
{
    %oldEnergy = %obj.GetBatteryEnergy();
    %obj.BatteryEnergy += %change;
    %obj.BatteryEnergy = mClamp(%obj.BatteryEnergy, 0, %obj.GetMaxBatteryEnergy());

    return %obj.BatteryEnergy - %oldEnergy;
}