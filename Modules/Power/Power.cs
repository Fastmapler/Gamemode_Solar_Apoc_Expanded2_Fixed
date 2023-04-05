exec("./Support_PowerNet.cs");
exec("./Brick_PowerGen.cs");
exec("./Brick_PowerUnits.cs");
exec("./Brick_Processing.cs");
exec("./Brick_PowerMachines.cs");
exec("./Brick_Hatches.cs");
exec("./Brick_SolarShield.cs");
//exec("./Brick_Debug.cs");

function fxDtsBrick::isOnPublicBrick(%obj)
{
    for (%i = 0; isObject(%obj.getDownBrick(%i)); %i++)
        if (isObject(%group = %obj.getDownBrick(%i).getGroup()) && %group.bl_id == 888888)
            return true;

    return false;
}