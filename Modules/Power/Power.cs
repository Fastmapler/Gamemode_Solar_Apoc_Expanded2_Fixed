$EOTW::PowerLevel[0] = 64; //LV
$EOTW::PowerLevel[1] = 256; //MV
$EOTW::PowerLevel[2] = 1024; //HV

exec("./Support_PowerNet.cs");
exec("./Brick_PowerGen.cs");
exec("./Brick_PowerUnits.cs");
exec("./Brick_PowerMachines.cs");
exec("./Brick_Debug.cs");

function fxDtsBrick::isOnPublicBrick(%obj)
{
    for (%i = 0; isObject(%obj.getDownBrick(%i)); %i++)
        if (isObject(%group = %obj.getDownBrick(%i).getGroup()) && %group.bl_id == 888888)
            return true;

    return false;
}