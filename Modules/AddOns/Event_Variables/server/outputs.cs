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
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mAdd";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mSub";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mMul";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mDiv";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mFloor";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mCiel";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mPow";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mRoot";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mPercent";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "getRandom";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "getWord";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
		
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "strLwr";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;
		
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "strUpr";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;
		
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "strChr";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "strLen";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mMod";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
		
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mAbs";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mClamp";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 3;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mSin";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;
		
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mCos";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;
		
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mAdd";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
		
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mTan";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;
		
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mASin";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mACos";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mACos";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mATan";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "strPos";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 3;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "strReplace";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 3;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mACos";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "trim";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "getSubStr";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 3;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "getWordCount";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "getWords";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 3;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "removeWord";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "removeWords";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 3;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "setWord";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 3;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "vectorDist";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "vectorAdd";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "vectorSub";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "vectorScale";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "vectorLength";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "vectorNormalize";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "vectorDot";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "vectorCross";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "getBoxCenter";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 3;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mAnd";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mOr";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mBAnd";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mBor";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mShiftR";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mShiftL";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mXor";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mBN";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mUN";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 1;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mEq";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;

$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mNEQ";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mGT";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mLT";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mGTE";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;
	
$VCE::Server::Operation[$vce_operation_lookupcount++,OPERATOR] = "mLTE";
	$VCE::Server::Operation[$vce_operation_lookupcount,VARIABLES] = 2;


//MIM between proccessing and actual event calling
function SimObject::VCECallEvent(%obj, %outputEvent, %brick, %client,%player,%vehicle,%bot,%minigame, %passClient,%eventLineNumber, %par1, %par2, %par3, %par4)
{
	%classname = %obj.getClassName();

	%targetIDX = %brick.eventTargetIdx[%eventLineNumber];
	if(%targetIDX < 0)
		%targetIDX = 0;
	%targetClass = inputEvent_GetTargetClass("fxDTSBrick", %brick.eventInputIdx[%eventLineNumber], %targetIDX);

	//is this brick's event's parsed? we need to parse this now or replacers won't work
	if(!%brick.VCE_Parsed)
	{
		%parameterWords = verifyOutputParameterList(%targetClass, outputEvent_GetOutputEventIdx(%targetClass, %outputEvent));
		%parameterWordCount = getWordCount(%parameterWords);
		%c = 1;

		for(%j = 0; %j < %parameterWordCount; %j++)
		{
			%word = getWord(%parameterWords, %j);
			if(%word $= "string"){
				//cleansing strings because you can crash by self referencing
				%par[%c] = strReplace(%par[%c], "RF_", "");
				%par[%c] = strReplace(%par[%c], "RL_", "");
				//filtering and creating a reference string
				$VCE_ReferenceString[%brick,%eventLineNumber,%c] = trim(%brick.filterVCEString(%par[%c],%client,%player,%vehicle,%bot,%minigame));
				
			}
			if($VCEisEventParameterType[%word])
			{
				%c++;
			}	

		}
		%brick.VCE_Parsed = true;
	}

	//loop through replacing parameter string with the eval string equivilent
	for(%i = 1; %i <= 4; %i++)
	{
		
		if((%referenceString = $VCE_ReferenceString[%brick,%eventLineNumber,%i]) $= "")
			continue;

		%par[%i] = strReplace(%brick.doVCEReferenceString(%referenceString),"\t","");
	}

	%parCount = outputEvent_GetNumParametersFromIdx(%targetClass, %brick.eventOutputIdx[%eventLineNumber]);

	%vargroup = %brick.getGroup().vargroup;

	//there's some special vce functions we want to call within this scope so they have access to needed references
	if(%outPutEvent $= "VCE_modVariable")
	{
		//is this setting a named brick's variable?
		%toNamedBrick = %obj != %brick || %obj.getName() !$= "";

		//adding context to parameters
		if(%obj.getClassName() $= "fxDtsBrick")
		{
			%obj = VCE_getObjectFromVarType(%par1,%obj,%client,%player,%vehicle,%bot,%minigame);

			%varName = %par2;
			%logic = %par3;
			%value = %par4;

		}
		else
		{
			%varName = %par1;
			%logic = %par2;
			%value = %par3;

			
		}	
		
		%oldvalue = %vargroup.getVariable(%varName,%obj);

		%newvalue = %value;
		%v0 = %oldValue;
		%v1 = getField(%newValue,0);
		%v2 = getField(%newValue,0);
		%v3 = getField(%newValue,0);
		%v4 = getField(%newValue,0);

		if(%logic != 0)
			%newValue = doVCEVarFunction(%logic, %v0, %v1,%v2,%v3,%v4);

		%vargroup.setVariable(%varName,%newValue,%obj);
		if(%toNamedBrick)
			%varGroup.setNamedBrickVariable(%varName,%newValue,%obj.getName());

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

		%test = doVCEVarFunction(%logic + 52,%vala,%valb);

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

function doVCEVarFunction(%opNum,%v0,%v1,%v2,%v3,%v4)
{
	%operationName = $VCE::Server::Operation[%opNum,OPERATOR];
	%operationCount = $VCE::Server::Operation[%opNum,VARIABLES];
	
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


// $VCE::Server::ImmuneTime = 5000;
// function doVCEVarFunction(%function, %oldValue, %newValue){
// 	%newValue = strReplace(%newValue, ",", "\t");
// 	if(%function == 0)
// 		return getSubStr(%newValue,0,32768); //we do nothing as it's done already + substring to prevent overflows
// 	if(%function == 1)
// 		return %oldvalue + %newValue;
// 	if(%function == 2)
// 		return %oldvalue - %newValue;
// 	if(%function == 3)
// 		return %oldvalue * %newValue;
// 	if(%function == 4)
// 		return %oldvalue / %newValue;
// 	if(%function == 16)
// 		return %oldvalue % %newValue;
// 	if(%function == 7)
// 		return mPow(%oldValue, %newValue);
// 	if(%function == 8)
// 		return mPow(%oldValue, 1 / %newValue);
// 	if(%function == 9)
// 		return mPercent(%oldvalue, %newValue);
// 	if(%function == 10)
// 		return getRandom(%oldValue, %newValue);
// 	if(%function == 17)
// 		return mAbs(%oldValue);
// 	if(%function == 5)
// 		return mFloor(%oldValue);
// 	if(%function == 6)
// 		return mCeil(%oldValue);
// 	if(%function == 18)
// 		return mClamp(%oldValue, (getField(%newvalue, 0) + 0), (getField(%newvalue, 1) + 0));
// 	if(%function == 19)
// 		return mSin(%oldValue);
// 	if(%function == 20)
// 		return mCos(%oldValue);
// 	if(%function == 21)
// 		return mTan(%oldValue);
// 	if(%function == 22)
// 		return mASin(%oldValue);
// 	if(%function == 23)
// 		return mACos(%oldValue);
// 	if(%function == 24)
// 		return mATan(%oldValue);
// 	if(%function == 15)
// 		return strLen(%oldValue);
// 	if(%function == 25)
// 		return strPos(%oldValue, getField(%newvalue, 0), (getField(%newvalue, 1) + 0));
// 	if(%function == 12)
// 		return strLwr(%oldValue);
// 	if(%function == 13)
// 		return strUpr(%oldValue);
// 	if(%function == 14) 
// 		return strChr(%oldValue, (getField(%newvalue, 0) + 0));
// 	if(%function == 26)
// 		return strReplace(%oldValue, getField(%newvalue, 0), (getField(%newvalue, 1) + 0));
// 	if(%function == 27)
// 		return trim(%oldValue);
// 	if(%function == 28)
// 		return getSubStr(%oldValue, (getField(%newvalue, 0) + 0), (getField(%newvalue, 1) + 0));
// 	if(%function == 11)
// 		return getWord(%oldValue, (getField(%newvalue, 0) + 0));
// 	if(%function == 29)
// 		return getWordCount(%oldValue);
// 	if(%function == 30)
// 		return getWords(%oldValue, (getField(%newvalue, 0) + 0), (getField(%newvalue, 1) + 0));
// 	if(%function == 31)
// 		return removeWord(%oldValue, (getField(%newvalue, 0) + 0));
// 	if(%function == 32)
// 		return removeWords(%oldValue,(getField(%newvalue, 0) + 0),(getField(%newvalue, 1) + 0));
// 	if(%function == 33)
// 		return setWord(%oldValue, (getField(%newvalue, 0) + 0), (getField(%newvalue, 1) + 0));
// 	if(%function == 34)
// 		return vectorDist(%oldValue, %newValue);
// 	if(%function == 35)
// 		return vectorAdd(%oldValue, %newValue);
// 	if(%function == 36)
// 		return vectorSub(%oldValue, %newValue);
// 	if(%function == 37)
// 		return vectorScale(%oldValue, %newValue);
// 	if(%function == 38)
// 		return vectorLen(%oldValue);
// 	if(%function == 39)
// 		return vectorNormalize(%oldValue);
// 	if(%function == 40)
// 		return vectorDot(%oldValue, %newValue);
// 	if(%function == 41)
// 		return vectorCross(%oldValue, %newValue);
// 	if(%function == 42)
// 		return getBoxCenter(%oldValue SPC %newValue);
// 	if(%function == 43)
// 		return %oldValue && %newValue;
// 	if(%function == 44)
// 		return %oldValue || %newValue;
// 	if(%function == 45)
// 		return %oldValue & %newValue;
// 	if(%function == 46)
// 		return %oldValue | %newValue;
// 	if(%function == 47)
// 		return %oldValue >> %newValue;
// 	if(%function == 48)
// 		return %oldValue << %newValue;
// 	if(%function == 49)
// 		return %oldValue ^ %newValue;
// 	if(%function == 50)
// 		return ~%oldValue;
// 	if(%function == 51)
// 		return !%oldValue;
// 	if(%function == 52)
// 		return %oldValue $= %newValue;
// 	if(%function == 53)
// 		return %oldValue !$= %newValue;
// 	if(%function == 54)
// 		return %oldValue > %newValue;
// 	if(%function == 55)
// 		return %oldValue < %newValue;
// 	if(%function == 56)
// 		return %oldValue >= %newValue;
// 	if(%function == 57)
// 		return %oldValue <= %newValue;
// 	if(%function == 58)
// 		return strPos(%oldValue,getField(%newValue, 0)) > -1;
// }
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
	if(!isObject(%varGroup = %brick.getGroup().vargroup))
		return;

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
	if(!isObject(%client))
		return;
	if(isObject(%obj.getGroup().vargroup))
	{
		%varGroup = %obj.getGroup().vargroup;

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

			%obj.VCE_ProcessVCERange(%subStart, %subEnd, "onVariableFunction", %client);
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

				%localbrick.VCE_ProcessVCERange(%subStart, %subEnd, "onVariableFunction", %client);
			}
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
	%varGroup = %brick.getGroup().vargroup;
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
	if(isObject(%brick.getGroup().vargroup))
	{
		%obj = VCE_getObjectFromVarType(%type,%brick,%client,%client.player,%brick.vehicle,%brick.hbot,getMinigameFromObject(%brick));

		if(!isObject(%obj))
			return;
		%vargroup = %brick.getGroup().vargroup;
		%vars = strReplace(%vars,",","\t");
		%count = getFieldCount(%vars);
		for(%i=0;%i<%count;%i++)
			%vargroup.loadVariable(trim(getField(%vars,%i)),%obj);
	}
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