exec("./Support_PowerNet.cs");
exec("./Brick_Processing_Basic.cs");
exec("./Brick_Processing_Ore.cs");
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

function fxDTSBrick::playSoundLooping(%obj, %data)
{
	if (isObject(%obj.AudioEmitter))
		%obj.AudioEmitter.delete();

	%obj.AudioEmitter = 0;
	if (!isObject(%data) || %data.getClassName () !$= "AudioProfile" || !%data.getDescription().isLooping || %data.fileName $= "")
		return;

	%audioEmitter = new AudioEmitter("")
	{
		profile = %data;
		useProfileDescription = 1;
	};
	MissionCleanup.add(%audioEmitter);
	%obj.AudioEmitter = %audioEmitter;
	%audioEmitter.setTransform(%obj.getTransform());
}