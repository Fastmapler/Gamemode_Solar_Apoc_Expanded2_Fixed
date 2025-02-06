//---
//	@package VCE
//	@title Groups
//	@author Zack0Wack0/www.zack0wack0.com
// 	@auther Monoblaster/46426
//	@time 4:23 PM 15/03/2011
//---
function getVariableGroupFromObject(%obj)
{
	%brickgroup = getBrickGroupFromObject(%obj);

	if(!isObject(%brickgroup.vargroup))
	{
		VCE_createVariableGroup(%brickgroup);
	}

	return %brickgroup.vargroup;
}
function VCE_createVariableGroup(%brickgroup)
{
	if(isObject(%brickgroup.vargroup))
	{
		return;
	}

	%brickgroup.vargroup = new ScriptObject()
	{
		class = "VariableGroup";
	};
	%brickgroup.vargroup.bl_id = %brickgroup.bl_id;
	%brickgroup.vargroup.name = %brickgroup.name;
	%brickgroup.vargroup.client = %brickgroup.client;
}
function VariableGroup::setVariable(%group,%varName,%value,%obj)
{
	%className = %obj.getClassName();

	if(%className $= "ScriptObject" && %obj.class !$= "variablegroup")
		%className = "MinigameSO";	
	
	if(isSpecialVar(%classname,%varName))
	{
		if($Pref::VCE::canEditSpecialVars || %group.client.isAdmin)
		{
			%f = "VCE_" @ $VCE::Server::ObjectToReplacer[%className] @ "_" @ $VCE::Server::SpecialVarEdit[%className,%varName];
			if(isFunction(%f))
				call(%f,%obj,%value,$VCE::Server::SpecialVarEditArg1[%className,%varName],$VCE::Server::SpecialVarEditArg2[%className,%varName],$VCE::Server::SpecialVarEditArg3[%className,%varName],$VCE::Server::SpecialVarEditArg4[%className,%varName]);
		}
			
	}
	else
	{
		%group.value[%className,%obj,%varName] = %value;
	}
}
function VariableGroup::getVariable(%group,%varName,%obj)
{
	if(%obj $= "GLOBAL")
	{
		return eval("return" SPC strReplace($VCE::Server::SpecialVar["GLOBAL",%varname],"%this",%obj) @ ";");
	}

	%className = %obj.getClassName();

	if(%className $= "ScriptObject" && %obj.class !$= "variablegroup")
	{
		%className = "Minigame";
		if(!isSpecialVar(%classname,%varName))
		{
			%className = "MinigameSO";
		}
	}
		
	if(%className $= "AIPlayer")
		%className = "Bot";
	if(strPos(%className,"Vehicle") >= 0 && %obj.class !$= "variablegroup")
		%className = "Vehicle";


	if(isSpecialVar(%classname,%varName))
	{
		return eval("return" SPC strReplace($VCE::Server::SpecialVar[%classname,%varname],"%this",%obj) @ ";");
	}

	return %group.value[%className,%obj,%varName];
}
function VariableGroup::setNamedBrickVariable(%group,%varName,%value,%brickName)
{
	%brickGroup = %group.client.brickgroup;
	%count = %brickGroup.ntObjectCount[%brickName];
	if(isSpecialVar("fxDtsBrick",%varName))
	{
		if($Pref::VCE::canEditSpecialVars || %group.client.isAdmin)
		{
			%f = "VCE_Brick_" @ $VCE::Server::SpecialVarEdit["fxDtsBrick",%varName];
			if(isFunction(%f))
			{
				%arg1 = $VCE::Server::SpecialVarEditArg1["fxDtsBrick",%varName];
				%arg2 = $VCE::Server::SpecialVarEditArg2["fxDtsBrick",%varName];
				%arg3 = $VCE::Server::SpecialVarEditArg3["fxDtsBrick",%varName];
				%arg4 = $VCE::Server::SpecialVarEditArg4["fxDtsBrick",%varName];
				for(%i = 0; %i < %count; %i++)
				{
					call(%f,%brickGroup.NTObject[%brickName,%i],%value,%arg1,%arg2,%arg3,%arg4);
					%group.value["fxDtsBrick",%brickGroup.NTObject[%brickName,%i],%varName] = %value;
				}
			}
		}
		return;
	}

	for(%i = 0; %i < %count; %i++)
	{
		%group.value["fxDtsBrick",%brickGroup.NTObject[%brickName,%i],%varName] = %value;
	}
}
function VariableGroup::getNamedBrickVariable(%group,%varName,%brickName)
{
	%obj = %group.client.brickgroup.NTObject[%brickName,0];

	if(!isObject(%obj))
	{
		return;
	}

	//normal vce value
	if(isSpecialVar("fxDtsBrick",%varName))
	{
		return eval("return" SPC strReplace($VCE::Server::SpecialVar["fxDtsBrick",%varname],"%this",%obj) @ ";");
	}
	return %group.value["fxDtsBrick",%obj,%varName];
}
function isSpecialVar(%classname,%name)
{
	return $VCE::Server::SpecialVar[%classname,%name] !$= "";
}
function VariableGroup::saveVariable(%group,%varName,%obj)
{
	if(!isObject(%obj))
		return;
	
	%className = %obj.getClassName();

	if(isSpecialVar(%classname,%varName))
		return;

	if((%value = %group.getVariable(%varName,%obj)) $= "")
		return;
	if(%classname $= "Player"){
			%id = %obj.client.BL_ID;
	} else if(%classname $= "GameConnection"){
			%id = %obj.BL_ID;
	} else if(%classname $= "fxDTSBrick" || %classname $= "ScriptObject"){
			%id = %obj.getGroup().BL_ID;
	} else{
		warn("VariableGroup::saveVariable - Unable to save "@%obj@" because it is not an accepted class.");
		return;
	}
	%line = VCE_getSaveLine(%group.bl_id,%id,%className,%varName);
	if(%line <= 0)
		$VCE::Server::SaveLine[$VCE::Server::SaveLineCount++] = %group.BL_ID TAB %id TAB %className TAB %varName TAB %value;
	else
		$VCE::Server::SaveLine[%line] = %group.BL_ID TAB %id TAB %className TAB %varName TAB %value;
	if(isEventPending($VCE::Server::SaveSchedule))
		cancel($VCE::Server::SaveSchedule);
	$VCE::Server::SaveSchedule = %group.schedule(300,"saveAllVariables",$VCE::Server::SavePath);
}
function VariableGroup::saveAllVariables(%group,%path)
{
	%file = new FileObject();
	%file.openForWrite(%path);
	%file.writeLine("VCE SAVE FILE (CONTAINS "@$VCE::Server::SaveLineCount@" VALUES)");
	for(%i=1;%i<=$VCE::Server::SaveLineCount;%i++)
		%file.writeLine($VCE::Server::SaveLine[%i]);
	%file.close();
	%file.delete();
}
function VariableGroup::loadVariable(%group,%varName,%obj)
{

	if(!isObject(%obj))
		return;
	
	%className = %obj.getClassName();

	if(isSpecialVar(%classname,%varName))
		return;

	if(%classname $= "Player"){
			%id = %obj.client.BL_ID;
	} else if(%classname $= "GameConnection"){
			%id = %obj.BL_ID;
	} else if(%classname $= "fxDTSBrick" || %classname $= "ScriptObject"){
			%id = %obj.getGroup().BL_ID;
	} else{
		warn("VariableGroup::saveVariable - Unable to load "@%obj@" because it is not an accepted class.");
		return;
	}

	%line = VCE_getSaveLine(%group.BL_ID,%id,%className,%varName);
	if(%line == 0)
		return;
	%group.setVariable(%varName,getField($VCE::Server::SaveLine[%line],4),%obj);

}
function VCE_getSaveLine(%groupid,%id,%type,%name)
{
	if($VCE::Server::SaveLineCount <= 0)
		return 0;
	for(%i=1;%i<=$VCE::Server::SaveLineCount;%i++)
	{
		%line = $VCE::Server::SaveLine[%i];
		if(getField(%line,0) == %groupid && getField(%line,1) == %id && getField(%line,2) $= %type && getField(%line,3) $= %name)
			return %i;
	}
	return 0;
}
function VCE_updateSaveFile()
{
	$VCE::Server::SaveLineCount = 0;
	%file = new FileObject();
	if(!isFile($VCE::Server::SavePath))
	{
		%file.openForWrite($VCE::Server::SavePath);
		%file.writeLine("VCE SAVE FILE (CONTAINS 0 VALUES)");
		%file.close();
		%file.delete();
		return;
	}
	%file.openForRead($VCE::Server::SavePath);
	%file.readLine();
	while(!%file.isEOF())
		$VCE::Server::SaveLine[$VCE::Server::SaveLineCount++] = %file.readLine();
	%file.close();
	%file.delete();
}
