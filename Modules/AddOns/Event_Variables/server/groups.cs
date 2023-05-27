//---
//	@package VCE
//	@title Groups
//	@author Zack0Wack0/www.zack0wack0.com
// 	@auther Monoblaster/46426
//	@time 4:23 PM 15/03/2011
//---
function getVariableGroupFromObject(%obj)
{
	%classname = %obj.getClassname();
	if(%classname $= "Player")
		%vargroup = nameToID("VariableGroup_"@%obj.client.BL_ID);
	else if(%classname $= "fxDTSBrick")
		%vargroup = nameToID("VariableGroup_"@%obj.getGroup().BL_ID);
	else if(%classname $= "GameConnection")
		%vargroup = nameToID("VariableGroup_"@%obj.BL_ID);
	else if(%classname $= "MinigameSO")
		%vargroup = nameToID("VariableGroup_"@%obj.owner.BL_ID);
	else if(%classname $= "Vehicle")
		%vargroup = nameToID("VariableGroup_"@%obj.brickGroup.BL_ID);
	else if(%classname $= "Local")
		%vargroup = nameToID("VariableGroup_"@%obj.brickGroup.BL_ID);
	else if(%classname $= "Bot")
		%vargroup = nameToID("VariableGroup_"@%obj.brick.getGroup().BL_ID);
	if(isObject(%vargroup))
		return %vargroup;
	return -1;
}
function VCE_createVariableGroup(%brick)
{
	%brickgroup = getBrickGroupFromObject(%brick);
	
	if(isObject(%brickgroup) && !isObject(%brickgroup.vargroup) && %brickgroup.bl_id !$= "")
	{
		%brickgroup.vargroup = new ScriptObject("VariableGroup_" @ %brickgroup.bl_id)
		{
			class = "VariableGroup";
		};
		%brickgroup.vargroup.bl_id = %brickgroup.bl_id;
		%brickgroup.vargroup.name = %brickgroup.name;
		%brickgroup.vargroup.client = %brickgroup.client;
	}
}
function VariableGroup::setVariable(%group,%varName,%value,%obj)
{
	%className = %obj.getClassName();

	if(%className $= "ScriptObject" && %obj.class !$= "variablegroup")
		%className = "MinigameSO";

	if(!isObject(%group))
		return;
	
	if(isSpecialVar(%classname,%varName))
	{
		if($Pref::VCE::canEditSpecialVars)
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
	%className = %obj.getClassName();

	if(%className $= "ScriptObject" && %obj.class !$= "variablegroup")
		%className = "MinigameSO";

	%val = 0;

	if(isSpecialVar(%classname,%varName))
		%val = eval("return" SPC strReplace($VCE::Server::SpecialVar[%className,%varName],"%this",%obj) @ ";");
	else
		%val = %group.value[%className,%obj,%varName];
	
	if(%val $= "")
		%val = 0;
	return %val;
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
