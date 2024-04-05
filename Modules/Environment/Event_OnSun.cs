registerInputEvent("fxDTSBrick", "onSunRise", "Self fxDTSBrick");
registerInputEvent("fxDTSBrick", "onSunSet", "Self fxDTSBrick");

function fxDTSBrick::onSunRise(%obj, %client)
{
	$InputTarget_["Self"] = %obj;
	%obj.processInputEvent ("onSunRise", %client);
}

function fxDTSBrick::onSunSet(%obj, %client)
{
	$InputTarget_["Self"] = %obj;
	%obj.processInputEvent ("onSunSet", %client);
}

function runSunCheckEvents(%sunRise)
{
    %i = 0;
    while (%i < %obj.numMembers)
    {
        %cl = %obj.member[%i];

        if (!hasWord($EOTW::SunDamageBlacklist, %cl.bl_id))
        {
            %brickGroup = %cl.brickGroup;
            %count = %brickGroup.getCount();
            %j = 0;
            while (%j < %count)
            {
                %checkObj = %brickGroup.getObject (%j);
                if (%checkObj.numEvents > 0)
                {
                    if (%sunRise)
                        %checkObj.onSunRise(%client);
                    else
                        %checkObj.onSunSet(%client);
                }
                %j += 1;
            }
        }
        
        %i += 1;
    }
}