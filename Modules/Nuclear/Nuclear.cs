exec("./Brick_FissionHull.cs");
exec("./Brick_FissionPorts.cs");
exec("./Brick_FissionHeatPlating.cs");
exec("./Brick_FissionReactionPlate.cs");
exec("./Brick_FissionComponents.cs");
exec("./Brick_FissionDisplayPanel.cs");

function fxDtsBrick::ChangeHeat(%obj, %change)
{
    %data = %obj.getDatablock();

    %initalheat = %obj.fissionHeat;
    %obj.fissionHeat += %change;

    %maxCapacity = %data.maxHeatCapacity;

    for (%i = 0; isObject(%brick = %obj.getUpBrick(%i)); %i++)
        if (%brick.getDatablock().maxFissionHeatBonus > 0)
            %maxCapacity += %brick.getDatablock().maxFissionHeatBonus;

    if (%maxCapacity > 0 && %obj.fissionHeat > %maxCapacity)
        %obj.killBrick();
    else if (%obj.fissionHeat < 0)
        %obj.fissionHeat = 0;

    return %obj.fissionHeat - %initalheat;
}

package EOTW_Fission
{
    function fxDtsBrick::onPlant(%brick)
    {
        Parent::onPlant(%brick);

        if (!isObject(%brick))
            return;

        %data = %brick.getDatablock();
        if (%data.reqFissionPart !$= "" && isObject(%downBrick = %brick.getDownBrick(0)))
        {
            if (%downBrick.getDatablock().getName() $= %data.reqFissionPart)
            {
                if (isObject(%fission = %downBrick.fissionParent))
                    %fission.AddFissionPart(%brick);
            }
            else
            {
                if (isObject(%client = %brick.getGroup().client))
                    %client.chatMessage("This brick must be planted on a " @ %data.reqFissionPart.uiName @ "!");

                %brick.killBrick();
            }
            
        }
    }
    function fxDtsBrick::onRemove(%brick)
	{
		%data = %brick.getDatablock();

        if (isObject(%fission = %brick.fissionParent))
            %fission.RemoveFissionPart(%brick);
        
		Parent::onRemove(%brick);
	}
};
activatePackage("EOTW_Fission");