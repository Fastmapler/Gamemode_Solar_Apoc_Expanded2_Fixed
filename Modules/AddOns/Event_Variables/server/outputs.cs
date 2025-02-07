//---
//	@package VCE
//	@title Output Events
//	@author Zack0Wack0/www.zack0wack0.com
//	@auther Monoblaster/46426
//	@time 4:39 PM 15/03/2011
//---
function mAdd(%v1, %v2)
{
	return %v1 + %v2;
}
function mSub(%v1, %v2)
{
	return %v1 - %v2;
}
function mMul(%v1, %v2)
{
	return %v1 * %v2;
}
function mMod(%v1, %v2)
{
	return %v1 % %v2;
}
function mDiv(%v1, %v2)
{
	return %v1 / %v2;
}
function mRoot(%v1,%v2)
{
	return mPow(%v1, 1 / %v2);
}
function mAnd(%v1, %v2)
{
	return %v1 && %v2;
}
function mOr(%v1, %v2)
{
	return %v2 || %v2;
}
function mBand(%v1, %v2)
{
	return %v1 & %v2;
}
function mBor(%v1, %v2)
{
	return %v1 | %v2;
}
function mShiftR(%v1, %v2)
{
	return %v1 >> %v2;
}
function mShiftL(%v1, %v2)
{
	return %v1 << %v2;
}
function mXor(%v1, %v2)
{
	return %v1 ^ %v2;
}
function mBN(%v1)
{
	return ~%v1;
}
function mUN(%v1)
{
	return !%v1;
}
function mEq(%v1, %v2)
{
	return %v1 $= %v2;
}
function mNEq(%v1, %v2)
{
	return %v1 !$= %v2;
}
function mGT(%v1,%v2)
{
	return %v1 > %v2;
}
function mLT(%v1, %v2)
{
	return %v1 < %v2;
}
function mGTE(%v1, %v2)
{
	return %v1 >= %v2;
}
function mLTE(%v1, %v2)
{
	return %v1 <= %v2;
}

function mSimilar(%v1,%v2)
{
	return strPos(%v1,getField(%v2, 0)) > -1;
}

$vce_operation_lookupcount = 0; // i don't know why i didn't do this ealier oops
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mAdd";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mSub";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mMul";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mDiv";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mFloor";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mCeil";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mPow";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mRoot";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mPercent";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "getRandom";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
	
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "getWord";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
		
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "strLwr";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;
		
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "strUpr";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;
		
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "strChr";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "strLen";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mMod";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
		
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mAbs";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mClamp";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 3;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mSin";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;
		
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mCos";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;
		
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mTan";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;
		
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mASin";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mACos";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mATan";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "strPos";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 3;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "strReplace";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 3;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "trim";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "getSubStr";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 3;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "getWordCount";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "getWords";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 3;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "removeWord";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "removeWords";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 3;
	
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "setWord";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 3;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "vectorDist";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
	
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "vectorAdd";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "vectorSub";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "vectorScale";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
	
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "vectorLength";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "vectorNormalize";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "vectorDot";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "vectorCross";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "getBoxCenter";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 3;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mAnd";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
	
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mOr";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
	
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mBAnd";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mBor";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
	
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mShiftR";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
	
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mShiftL";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
	
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mXor";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
	
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mBN";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mUN";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 1;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mEq";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mNEQ";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
	
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mGT";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
	
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mLT";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
	
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mGTE";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;
	
$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mLTE";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCE::Server::OperationOperator[$vce_operation_lookupcount++] = "mSimilar";
	$VCE::Server::OperationVariables[$vce_operation_lookupcount] = 2;

$VCEisEventParameterType["int"] = 1;
$VCEisEventParameterType["float"] = 1;
$VCEisEventParameterType["list"] = 1;
$VCEisEventParameterType["bool"] = 1;
$VCEisEventParameterType["intList"] = 1;
$VCEisEventParameterType["datablock"] = 1;
$VCEisEventParameterType["string"] = 1;
$VCEisEventParameterType["vector"] = 1;
$VCEisEventParameterType["paintColor"] = 1;
//MITM between proccessing and actual event calling
function SimObject::VCECallEvent(%obj, %outputEvent, %eventBrick, %client,%player,%vehicle,%bot,%minigame, %passClient,%eventLineNumber, %par1, %par2, %par3, %par4)
{
	%eventbrick.VCE_CompileBrick();
	%classname = %obj.getClassName();
	%targetIDX = %eventBrick.eventTargetIdx[%eventLineNumber];
	if(%targetIDX <= 0)
	{
		%brick = %obj;
		%targetClass = inputEvent_GetTargetClass("fxDTSBrick", %eventBrick.eventInputIdx[%eventLineNumber], inputEvent_GetTargetIndex("fxDTSBrick",%eventBrick.eventInputIdx[%eventLineNumber],"Self"));
	}
	else
	{
		%brick = %eventBrick;
		%targetClass = inputEvent_GetTargetClass("fxDTSBrick", %eventBrick.eventInputIdx[%eventLineNumber], %targetIDX);
	}

	for(%i = 1; %i <= 4; %i++)
	{
		if(%eventbrick.VCE_CompiledParameter[%eventLineNumber,%i])
		{
			%par[%i] = VCE_CompiledParameter_Run(%eventbrick,%eventLineNumber,%i,%obj,%client,%player,%vehicle,%bot,%minigame);
		}
	}
	
	%parCount = outputEvent_GetNumParametersFromIdx(%targetClass, %eventBrick.eventOutputIdx[%eventLineNumber]);

	%vargroup = getVariableGroupFromObject(%brick);
	//there's some special vce functions we want to call within this scope so they have access to needed references
	if(%outPutEvent $= "VCE_modVariable")
	{
		%toNamedBrick = false;
		//adding context to parameters
		if(%obj.getClassName() $= "fxDtsBrick")
		{
			//is this setting a named brick's variable?
			%toNamedBrick = %eventBrick != %brick && %brick.getName() !$= "";
			%modtarget = VCE_getObjectFromVarType(%par1,%brick,%client,%player,%vehicle,%bot,%minigame);

			%varName = %par2;
			%logic = %par3;
			%value = %par4;
		}
		else
		{
			%modtarget = %obj;
			%varName = %par1;
			%logic = %par2;
			%value = %par3;
		}

		if(%logic != 0)
			%value = doVCEVarFunction(%logic,%vargroup.getVariable(%varName,%modtarget)@","@%value);

		
		if(%toNamedBrick)
		{
			%varGroup.setNamedBrickVariable(%varName,%value,%obj.getName());
		}
		else
		{
			%vargroup.setVariable(%varName,%value,%modtarget);
		}

		%obj.processInputEvent("onVariableUpdate", %client);
	}
	else if (%outPutEvent $= "VCE_retroCheck" || %outPutEvent $= "VCE_ifVariable" || %outPutEvent $= "VCE_ifValue")
	{
		if(%outPutEvent $= "VCE_retroCheck")
		{
			//adding context to parameters
			%vala = %par1;
			%logic = %par2;
			%valb = %par3;
			%subdata = %par4;

			//ifPlayerName 0 ifPlayerID 1 ifAdmin 2 ifPlayerEnergy 3 ifPlayerDamage 4 ifPlayerScore 5 ifLastPlayerMsg 6 ifBrickName 7 ifRandomDice 8
			if(%vala == 0)
				%vala = %client.name;
			else if(%vala == 1)
				%vala = %client.BL_ID;
			else if(%vala == 2){
				%vala = %client.isAdmin;
				%valb = %client.isAdmin == 1 ? 1 : -1;
			} else if(%vala == 3){
				%vala = 0;
				if(isObject(%client.player))
					%vala = %client.player.getEnergyLevel();
			} else if(%vala == 4){
				%vala = 0;
				if(isObject(%client.player))
					%vala = %client.player.getDamageLevel();
			} else if(%vala == 5)
				%vala = %client.score;
			else if(%vala == 6)
				%vala = %client.lastMessage;
			else if(%vala == 7){
				if(strLen(%brick.getName()) >= 1)
					%vala = getSubStr(%brick.getName(),1,strLen(%brick.getName()) - 1);
			} else if(%vala == 8)
				%vala = getRandom(1,6);
			
		}
		else if(%outPutEvent $= "VCE_ifVariable")
		{
			//adding context to parameters
			%var = %par1;
			%logic = %par2;
			%valb = %par3;
			%subdata = %par4;

			for(%i = 0; %i < getWordCount(%var); %i++){
				if((%value = %vargroup.getVariable(getWord(%var ,%i), %obj)) !$= ""){
					%var = setWord(%var, %i, %value);
				}
			}
			%vala = %var;
		}
		else
		{
			//adding context to parameters
			%vala = %par1;
			%logic = %par2;
			%valb = %par3;
			%subdata = %par4;
		}

		if(!isObject(%client))
			return;

		%test = doVCEVarFunction(%logic + 52,%vala @ "," @ %valb);

		%subStart = getWord(%subData,0);
		%subEnd = getWord(%subData,1);

		if(%subStart $= "")
			%subStart = -1;
		if(%subEnd $= "")	
			%subEnd =  -1;
		if(%test)
			%brick.VCE_ProcessVCERange(%subStart, %subEnd, "onVariableTrue", %client);
		else
			%brick.VCE_ProcessVCERange(%subStart, %subEnd, "onVariableFalse", %client);
	}
	else if(%passClient)
	{
		//call the event correctly with the right number of paramter
		//for some reason some events have error detection for the wrong number of paramters
		//we could use eval but i'd rather not be compiling code during runtime
		if(%parCount == 0)
				%obj.VCE_Call(%outputEvent,%client);
		if(%parCount == 1)
				%obj.VCE_Call(%outputEvent,%par1,%client);
		if(%parCount == 2)
				%obj.VCE_Call(%outputEvent,%par1,%par2,%client);
		if(%parCount == 3)
				%obj.VCE_Call(%outputEvent,%par1,%par2,%par3,%client);
		if(%parCount == 4)
				%obj.VCE_Call(%outputEvent,%par1,%par2,%par3,%par4,%client);	
	}
	else
	{
		if(%parCount == 0)
			%obj.VCE_Call(%outputEvent);
		if(%parCount == 1)
			%obj.VCE_Call(%outputEvent,%par1);
		if(%parCount == 2)
			%obj.VCE_Call(%outputEvent,%par1,%par2);
		if(%parCount == 3)
			%obj.VCE_Call(%outputEvent,%par1,%par2,%par3);
		if(%parCount == 4)
			%obj.VCE_Call(%outputEvent,%par1,%par2,%par3,%par4);	
	}
}

function doVCEVarFunction(%func,%args)
{
	%args = strReplace(%args,",","\t");
	%count = getFieldCount(%args);
	for(%i = 0; %i < %count; %i++)
	{
		%v[%i] = getField(%args,%i);
	}

	if(%func == 0)
	{
		%func = $VCE::Server::Function[%func];

		if(%func $= "")
		{
			return "";
		}
	}
	

	%operationName = $VCE::Server::OperationOperator[%func];
	%operationCount = $VCE::Server::OperationVariables[%func];
	
	if(%operationCount == 1)
		return call(%operationName,%v0);
	if(%operationCount == 2)
		return call(%operationName,%v0,%v1);
	if(%operationCount == 3)
		return call(%operationName,%v0,%v1,%v2);
	if(%operationCount == 4)
		return call(%operationName,%v0,%v1,%v2,%v3);
	if(%operationCount == 5)
		return call(%operationName,%v0,%v1,%v2,%v3,%v4);		
}

function SimObject::VCE_modVariable(%obj){
	//This is empty because it is handled in event processing
}
function SimObject::VCE_ifVariable(%obj){
	//This is empty because it is handled in event processing
}
function fxDTSBrick::VCE_ifValue(%brick)
{
	//This is empty because it is handled in event processing
}
function fxDtsBrick::VCE_retroCheck(%brick)
{
	//This is empty because it is handled in event processing
}
function fxDtsBrick::VCE_stateFunction(%brick,%name,%subdata,%client)
{
	%brick.VCE_StartFunction(0,%name,%subdata,%client);
}
function fxDTSBrick::VCE_startFunction(%brick,%type,%name,%subData,%client)
{
	%varGroup = getVariableGroupFromObject(%brick);

	%subStart = getWord(%subData,0);
	%subEnd = getWord(%subData,1);

	if(%subStart $= "")
		%subStart = -1;
	if(%subEnd $= "")	
		%subEnd =  -1;

	if(%type == 0)
		%brick.vceFunction[%name] = %subStart SPC %subEnd;

	if(%type == 1)
	{
		if((%c = %varGroup.GetLocalFunctionFromBrick(%name,%brick)) > 0)
			%varGroup.vceLocalFunction[%name,%c] = %brick SPC %substart SPC %subEnd;
		else
			%varGroup.vceLocalFunction[%name,%varGroup.vceLocalFunctionCount[%name]++] = %brick SPC %substart SPC %subEnd;
	}

}
function VariableGroup::GetLocalFunctionFromBrick(%varGroup,%name,%brick)
{
	if(!isObject(%brick))
		return;
	%total = %varGroup.vceLocalFunctionCount[%name];
	%c = 1;
	while(%c <= %total && getWord(%varGroup.vceLocalFunction[%name,%c], 0) != %brick)
		%c++;
	if(%c > %total)
		return 0;
	return %c;
}
function fxDTSBrick::VCE_callFunction(%obj,%name,%args,%delay,%client)
{
	%varGroup = getVariableGroupFromObject(%obj);

	if(%delay < 0)
		%delay = 0;
	%args = strReplace(%args,"|","\t");
	%args = strReplace(%args,",","\t");
	%fc = getFieldCount(%args);
	if(%obj.vceFunction[%name] !$= "")
	{
		for(%i=0;%i<%fc;%i++)
		{
			%arg[%i] = getField(%args,%i);		
			%varGroup.setVariable("arg" @ %i,%arg[%i],%obj);
		}	

		%subStart = getWord(%obj.vceFunction[%name], 0);
		%subEnd = getWord(%obj.vceFunction[%name], 1);

		%varGroup.setVariable("argcount",getFieldCount(%args),%obj);

		%obj.VCE_ProcessVCERange(%subStart, %subEnd, "onVariableFunction",%client);
	} 
	else if((%count = %vargroup.vceLocalFunctionCount[%name]) > 0)
	{
		for(%i = 1; %i <= %count; %i++)
		{
			%sentence = %vargroup.vceLocalFunction[%name,%i];
			
			%localbrick = getWord(%sentence,0);
			%subStart = getWord(%sentence,1);
			%subEnd = getWord(%sentence,0);

			if(!isObject(%localbrick))
				continue;
			
			for(%j=0;%j<%fc;%j++)
			{
				%arg[%j] = getField(%args,%j);		
				%varGroup.setVariable("arg" @ %j,%arg[%j],%localBrick);
			}

			%varGroup.setVariable("argcount",getFieldCount(%args),%localBrick);

			%localbrick.VCE_ProcessVCERange(%subStart, %subEnd, "onVariableFunction",%client);
		}
	}
}
function fxDTSBrick::VCE_cancelFunction(%brick,%name,%client){
	%count = %brick.functionScheduleCount[%name];
	for(%i = 0; %i < %count; %i++)
	%schedule = %brick.functionSchedule[%i,%name];
		if(isEventPending(%schedule))
			cancel(%schedule);
	%brick.functionScheduleCount[%name] = 0;
}
function fxDtsBrick::VCE_relayCallFunction(%brick,%direction,%name,%args,%delay,%client)
{
	%WB = %brick.getWorldBox ();
	%sizeX = (getWord (%WB, 3) - getWord (%WB, 0)) - 0.1;
	%sizeY = (getWord (%WB, 4) - getWord (%WB, 1)) - 0.1;
	%sizeZ = (getWord (%WB, 5) - getWord (%WB, 2)) - 0.1;
	%pos = %brick.getPosition ();
	%posX = getWord (%pos, 0);
	%posY = getWord (%pos, 1);
	%posZ = getWord (%pos, 2);
	if(%direction == 0){
		%posZ = getWord (%pos, 2) + %sizeZ / 2 + 0.05;
		%size = %sizeX SPC %sizeY SPC 0.1;
	} else if(%direction == 1){
		%posZ = (getWord (%pos, 2) - %sizeZ / 2) - 0.05;
		%size = %sizeX SPC %sizeY SPC 0.1;
	} else if(%direction == 2){
		%posY = getWord (%pos, 1) + %sizeY / 2 + 0.05;
		%size = %sizeX SPC 0.1 SPC %sizeZ;
	} else if(%direction == 3){
		%posX = getWord (%pos, 0) + %sizeX / 2 + 0.05;
		%size = 0.1 SPC %sizeY SPC %sizeZ;
	} else if(%direction == 4){
		%posY = (getWord (%pos, 1) - %sizeY / 2) - 0.05;
		%size = %sizeX SPC 0.1 SPC %sizeZ;
	} else if(%direction == 5){
		%posX = (getWord (%pos, 0) - %sizeX / 2) - 0.05;
		%size = 0.1 SPC %sizeY SPC %sizeZ;
	}
	%pos = %posX SPC %posY SPC %posZ;
	%mask = $TypeMasks::FxBrickAlwaysObjectType;
	%group = %brick.getGroup ();
	initContainerBoxSearch (%pos, %size, %mask);
	while ((%searchObj = containerSearchNext ()) != 0)
	{
		if (!%searchObj.getGroup () == %group)
		{
			
		}
		else if (%searchObj == %brick)
		{
			
		}
		else if (%searchObj.numEvents <= 0)
		{
			
		}
		else 
		{
			%searchObj.VCE_callFunction(%name,%args,%delay,%client,%brick);
		}
	}
}
function fxDtsBrick::VCE_saveVariable(%brick,%type,%vars,%client)
{
	%varGroup = getVariableGroupFromObject(%brick);
	if(!(isObject(%client) || isObject(%varGroup)))
		return;

	%obj = VCE_getObjectFromVarType(%type,%brick,%client,%client.player,%brick.vehicle,%brick.hbot,getMinigameFromObject(%brick));

	%vars = strReplace(%vars,",","\t");
	%count = getFieldCount(%vars);
	for(%i=0;%i<%count;%i++)
		%vargroup.saveVariable(trim(getField(%vars,%i)),%obj);

}
function fxDtsBrick::VCE_loadVariable(%brick,%type,%vars,%client)
{
	if(!isObject(%client))
		return;

	%obj = VCE_getObjectFromVarType(%type,%brick,%client,%client.player,%brick.vehicle,%brick.hbot,getMinigameFromObject(%brick));

	if(!isObject(%obj))
		return;

	%vargroup = getVariableGroupFromObject(%obj);

	%vars = strReplace(%vars,",","\t");
	%count = getFieldCount(%vars);
	for(%i=0;%i<%count;%i++)
		%vargroup.loadVariable(trim(getField(%vars,%i)),%obj);
}

//Stolen from firerelaynum as this is the best and strongest solution
function fxDTSBrick::VCE_ProcessVCERange(%obj, %start, %end, %inputEvent, %client)
{
	// Only check for those events we are interested in
	if (%start < 0 || %end > %obj.numevents || %start > %end){
		%start = 0;
		%end = %obj.numevents;
	}
	for (%i = %start; %i <= %end; %i++)
	{
		// Already processed
		if (%tempEvent[%i])
			continue;

		// Enabled event
		if (!%obj.eventEnabled[%i])
			continue;
		
		// Not onRelay
		if (%obj.eventInput[%i] !$= %inputEvent)
			continue;
		
		// Target brick(s)
		if (%obj.eventTargetIdx[%i] == -1)
		{
			%type = "fxDTSBrick";
			%group = getBrickGroupFromObject(%obj);
			%name = %obj.eventNT[%i];
			for (%objs = 0; %objs < %group.NTObjectCount[%name]; %objs++)
				%objs[%objs] = %group.NTObject[%name, %objs];
		}
		// Self
		else
		{
			%type = inputEvent_GetTargetClass("fxDTSBrick", %obj.eventInputIdx[%i], %obj.eventTargetIdx[%i]);
			%objs = 1;
			// Get object from type (Event_onRelay)
			switch$ (%type)
			{
			case "Bot":
				%objs0 = %obj.hBot;
			case "Player":
				%objs0 = %client.player;
			case "GameConnection":
				%objs0 = %client;
			case "Minigame":
				%objs0 = getMinigameFromObject(%client);
			default:
				%objs0 = %obj;
			}
		}

		// Parameters
		%numParams = outputEvent_GetNumParametersFromIdx(%type, %obj.eventOutputIdx[%i]);
		
		// Get parameters
		%param = "";
		for (%n = 1; %n <= %numParams; %n++)
			%p[%n] = %obj.eventOutputParameter[%i, %n];
		

		%eventDelay = %obj.eventDelay[%i];
		%eventOutput = %obj.eventOutput[%i];
		%eventTarget = %obj.eventTarget[%i];
		// Go through list/brick
		for (%n = 0; %n < %objs; %n++)
		{
			%next = %objs[%n];

			if (!isObject(%next))
				continue;

			// Call for event function
			%event = %next.schedule(%eventDelay,"VCECallEvent",%eventOutput, %obj, %client,%client.player,%obj.vehicle,%obj.hbot,getMinigameFromObject(%obj), %obj.eventOutputAppendClient[%i],%i, %p1, %p2, %p3, %p4);
			
			// To be able to cancel an event
			if (%delay > 0)
				%obj.addScheduledEvent(%event);
		}

		// Mark as processed
		%tempEvent[%i] = 1;
	}
	return "";
}