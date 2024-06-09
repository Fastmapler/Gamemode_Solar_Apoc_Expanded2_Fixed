//---
//	@package VCE
//	@title Replacers
//	@author Clockturn/www.clockler.com
//	@author Zack0Wack0/www.zack0wack0.com
//  @auther Monoblaster/46426
//	@time 4:44 PM 14/04/2011
//---

function VCE_getObjectFromVarType(%id,%brick,%client,%player,%vehicle,%bot,%minigame)
{
	//Variable replacer ID
	if(%id $= "Brick" || %id $= "Br")
		return %brick;
	if(%id $= "Player" || %id $= "Pl")
		return %player;
	if(%id $= "Client" || %id $= "Cl")
		return %client;
	if(%id $= "Minigame" || %id $= "Mg")
		return %minigame;
	if(%id $= "Vehicle" || %id $= "Ve")
		return %vehicle;
	if(%id $= "Bot" || %id $= "Bo")
		return %bot;
	if(%id $= "Local" || %id $= "Lo")
		return %brick.getGroup().vargroup;

	//Number ID
	if(%id == 0)
		return %brick;
	if(%id == 1)
		return %player;
	if(%id == 2)
		return %client;
	if(%id == 3)
		return %minigame;
	if(%id == 4)
		return %vehicle;
	if(%id == 5)
		return %bot;
	if(%id == 6)
		return %brick.getGroup().vargroup;
}
//Actual objects names to replacer vars
$VCE::Server::ObjectToReplacer["GameConnection"] = "Client";
$VCE::Server::ObjectToReplacer["Player"] = "Player";
$VCE::Server::ObjectToReplacer["fxDTSBrick"] = "Brick";
$VCE::Server::ObjectToReplacer["Vehicle"] = "Vehicle";
$VCE::Server::ObjectToReplacer["AIPlayer"] = "Bot";
$VCE::Server::ObjectToReplacer["Bot"] = "Bot";
$VCE::Server::ObjectToReplacer["MinigameSO"] = "Minigame";
$VCE::Server::ObjectToReplacer["Minigame"] = "Minigame";
$VCE::Server::ObjectToReplacer["Global"] = "Global";
//Event Targets to Object Classes
$VCE::Server::TargetToObject["Client"] = "GameConnection";
$VCE::Server::TargetToObject["GameConnection"] = "GameConnection";
$VCE::Server::TargetToObject["Player"] = "Player";
$VCE::Server::TargetToObject["fxDTSBrick"] = "fxDTSBrick";
$VCE::Server::TargetToObject["Self"] = "fxDTSBrick";
$VCE::Server::TargetToObject["Vehicle"] = "Vehicle";
$VCE::Server::TargetToObject["Bot"] = "AIPlayer";
$VCE::Server::TargetToObject["AIPlayer"] = "AIPlayer";
$VCE::Server::TargetToObject["Minigame"] = "Minigame";
//Index to Object Classes
$VCE::Server::TargetToObject[0] = "fxDTSBrick";
$VCE::Server::TargetToObject[1] = "Player";
$VCE::Server::TargetToObject[2] = "GameConnection";
$VCE::Server::TargetToObject[3] = "Minigame";
$VCE::Server::TargetToObject[4] = "Vehicle";
$VCE::Server::TargetToObject[5] = "AIPlayer";

//acts like advanced vce's expression but not as an event we skip 0 because 0 is just a set function
$VCE::Server::Operator["+",NUMBER] = 1;
	$VCE::Server::Operator["+",PRECEDENCE] = 11;
$VCE::Server::Operator["-",NUMBER] = 2;
	$VCE::Server::Operator["-",PRECEDENCE] = 11;
$VCE::Server::Operator["*",NUMBER] = 3;
	$VCE::Server::Operator["*",PRECEDENCE] = 12;
$VCE::Server::Operator["/",NUMBER] = 4;
	$VCE::Server::Operator["/",PRECEDENCE] = 12;
$VCE::Server::Operator["%",NUMBER] = 16;
	$VCE::Server::Operator["%",PRECEDENCE] = 12;
$VCE::Server::Operator["&&",NUMBER] = 43;
	$VCE::Server::Operator["&&",PRECEDENCE] = 4;
$VCE::Server::Operator["||",NUMBER] = 44;
	$VCE::Server::Operator["||",PRECEDENCE] = 3;
$VCE::Server::Operator["&",NUMBER] = 45;
	$VCE::Server::Operator["&",PRECEDENCE] = 7;
$VCE::Server::Operator["|",NUMBER] = 46;
	$VCE::Server::Operator["|",PRECEDENCE] = 5;
$VCE::Server::Operator["BSR",NUMBER] = 47;
	$VCE::Server::Operator["BSR",PRECEDENCE] = 10;
$VCE::Server::Operator["BSL",NUMBER] = 48;
	$VCE::Server::Operator["BSL",PRECEDENCE] = 10;
$VCE::Server::Operator["^",NUMBER] = 49;
	$VCE::Server::Operator["^",PRECEDENCE] = 6;
$VCE::Server::Operator["==",NUMBER] = 52;
	$VCE::Server::Operator["==",PRECEDENCE] = 8;
$VCE::Server::Operator["!=",NUMBER] = 53;
	$VCE::Server::Operator["!=",PRECEDENCE] = 8;
$VCE::Server::Operator["gT",NUMBER] = 54;
	$VCE::Server::Operator["gT",PRECEDENCE] = 9;
$VCE::Server::Operator["lT",NUMBER] = 55;
	$VCE::Server::Operator["lT",PRECEDENCE] = 9;
$VCE::Server::Operator["gT",NUMBER] = 56;
	$VCE::Server::Operator["gT",PRECEDENCE] = 9;
$VCE::Server::Operator["lT",NUMBER] = 57;
	$VCE::Server::Operator["lT",PRECEDENCE] = 9;
$VCE::Server::Operator["s=",NUMBER] = 58;
	$VCE::Server::Operator["s=",PRECEDENCE] = 9;
$VCE::Server::Function["Power"] = 7;
$VCE::Server::Function["Pow"] = 7;
$VCE::Server::Function["radical"] = 8;
$VCE::Server::Function["rad"] = 8;
$VCE::Server::Function["precent"] = 9;
$VCE::Server::Function["perc"] = 9;
$VCE::Server::Function["random"] = 10;
$VCE::Server::Function["rand"] = 10;
$VCE::Server::Function["absolute"] = 17;
$VCE::Server::Function["abs"] = 17;
$VCE::Server::Function["floor"] = 5;
$VCE::Server::Function["ceil"] = 6;
$VCE::Server::Function["clamp"] = 18;
$VCE::Server::Function["sin"] = 19;
$VCE::Server::Function["cos"] = 20;
$VCE::Server::Function["tan"] = 21;
$VCE::Server::Function["asin"] = 22;
$VCE::Server::Function["acos"] = 23;
$VCE::Server::Function["atan"] = 24;
$VCE::Server::Function["length"] = 15;
$VCE::Server::Function["stringposition"] = 25;
$VCE::Server::Function["strpos"] = 25;
$VCE::Server::Function["lowercase"] = 12;
$VCE::Server::Function["lower"] = 12;
$VCE::Server::Function["uppercase"] = 13;
$VCE::Server::Function["upper"] = 13;
$VCE::Server::Function["character"] = 14;
$VCE::Server::Function["char"] = 14;
$VCE::Server::Function["replace"] = 26;
$VCE::Server::Function["trim"] = 27;
$VCE::Server::Function["subString"] = 28;
$VCE::Server::Function["subStr"] = 28;
$VCE::Server::Function["words"] = 11;
$VCE::Server::Function["countWord"] = 29;
$VCE::Server::Function["subWords"] = 30;
$VCE::Server::Function["removeWord"] = 31;
$VCE::Server::Function["removeWords"] = 32;
$VCE::Server::Function["setWord"] = 33;
$VCE::Server::Function["vectorDist"] = 34;
$VCE::Server::Function["vectorAdd"] = 35;
$VCE::Server::Function["vectorSub"] = 36;
$VCE::Server::Function["vectorScale"] = 37;
$VCE::Server::Function["vectorLen"] = 38;
$VCE::Server::Function["vectorNormalize"] = 39;
$VCE::Server::Function["vectorDot"] = 40;
$VCE::Server::Function["vectorCross"] = 41;
$VCE::Server::Function["vectorCenter"] = 42;
$VCE::Server::Function["bitwiseComponenet"] = 50;
$VCE::Server::Function["bitComp"] = 50;
$VCE::Server::Function["booleanInverse"] = 51;
$VCE::Server::Function["boolInv"] = 51;
//varLinks?
//enter the string and the start of the header
function VCE_getReplacerHeaderEnd(%string,%headerStart){
	if((%headerEnd = strPos(%string,":",%headerStart)) != -1 && (strPos(%string,"<",%headerStart + 1) > %headerEnd || strPos(%string,"<",%headerStart + 1) == -1) && (strPos(%string,">",%headerStart) > %headerEnd || strPos(%string,">",%headerStart) == -1))
		return %headerEnd;
	return -1;
}
function fxDTSBrick::doVCEReferenceString(%brick,%string,%brick,%client,%player,%vehicle,%bot,%minigame)
{	
	%referenceCount = getFieldCount(%string);
	for(%i = 0; %i < %referenceCount; %i++)
	{
		%reference = getField(%string, %i);
		%data = $VCE[%reference];
		//is this a function, literal, or none of the above?
		if(strPos(%reference,"RF_") == 0)
		{
			//is this a object function or a normal function?
			%obj = getField(%data,0);
			if(isObject(%obj))
			{

				//scirptobject classes aren't stored in classname
				%className = %obj.getClassName();
				if(%obj.getClassname() $= "ScriptObject")
					%className = %obj.class;
				//is the function real?
				%function = getField(%data, 1);
				if(isFunction(%className,%function))
				{
					//is there any references in the parameters?
					%parameters = getFields(%data,2, getFieldCount(%data) - 1);
					%parameterCount = getFieldCount(%parameters);
					for(%j = 0; %j < %parameterCount; %j++)
					{
						%parameter[%j] =  trim(%brick.doVCEReferenceString(getField(%parameters,%j),%brick,%client,%player,%vehicle,%bot,%minigame));
					}

					//seperate paramters into a list
					if(%function $= "getVariable")
					{
						%parameter1 = VCE_getObjectFromVarType(%parameter1,%brick,%client,%player,%vehicle,%bot,%minigame);
						if(strPos(%parameter1,"nb_") == 0)
						{
							%function = "getNamedBrickVariable";
							%parameter1 = getSubStr(%parameter1,2,999999);
						}
					}
					//call function and get return
					%product = %obj.VCE_call(%function,%parameter0,%parameter1,%parameter2,%parameter3,%parameter4,%parameter5,%parameter6,%parameter7,%parameter8,%parameter9,%parameter10,%parameter11,%parameter12,%parameter13,%parameter14);
				}

			}
			else
			{
				//is the function real?
				%function = getField(%data, 0);
				if(isFunction(%function))
				{
					//is there any references in the parameters?
					%parameters = getFields(%data,1, getFieldCount(%data) - 1);
					%parameterCount = getFieldCount(%parameters);
					for(%j = 0; %j < %parameterCount; %j++)
					{
						%parameter[%j] = trim(%brick.doVCEReferenceString(getField(%parameters,%j),%brick,%client,%player,%vehicle,%bot,%minigame));
					}
					//call function and get return
					%product = call(%function,%parameter0,%parameter1,%parameter2,%parameter3,%parameter4,%parameter5,%parameter6,%parameter7,%parameter8,%parameter9,%parameter10,%parameter11,%parameter12,%parameter13,%parameter14);
				}

			}
			%string = setField(%string,%i,%product);
		}
		else if(strPos(%reference,"RL_") == 0)
		{
			//a reference to a literal string of some kind
		    %string = setField(%string,%i,%brick.doVCEReferenceString(%data,%brick,%client,%player,%vehicle,%bot,%minigame));
		}
	}
	return %string;
}
//DO NOT CALL THIS AS THIS IS DONE AUTOMATICALY NOW
//recursive getting of all things within <> replacers part of string with result
function fxDTSBrick::filterVCEString(%brick,%string,%client,%player,%vehicle,%bot,%minigame)
{
	talk("egg");
	//looks for the first header
	%headerStart = -1;
	while((%headerStart = strPos(%string,"<",%headerStart + 1)) != -1 && (%headerEnd = VCE_getReplacerHeaderEnd(%string,%headerStart)) == -1){}
	//there is no valid header
	if(%headerStart == -1)
	{
		if(%string !$= "")
		{
			$VCE[RL,%brick,$VCE[RLC,%brick]++] = %string;
			%string = "RL_" @ %brick @ "_" @ $VCE[RLC,%brick];
		}
		return %string;
	}
	//find the end of the replacer
	%replacerEnd = strPos(%string,">",%headerEnd + 1);
	%checkStart = %headerEnd;
	while(%replacerEnd != -1 && (%checkStart = strPos(%string,"<",%checkStart + 1)) != -1 && %replacerEnd > %checkStart)
	{
		if(VCE_getReplacerHeaderEnd(%string,%checkStart))
			%replacerEnd = strPos(%string,">",%replacerEnd + 1);
	}
	//if there is no valid end
	if(%replacerEnd == -1)
	{
		if(%string !$= "")
		{
			$VCE[RL,%brick,$VCE[RLC,%brick]++] = %string;
			%string = "RL_" @ %brick @ "_" @ $VCE[RLC,%brick];
		}
		return %string;
	}
	//get parts before replacer
	%prev = getSubStr(%string,0,%headerStart);
	
	if(%prev !$= "")
	{
		$VCE[RL,%brick,$VCE[RLC,%brick]++] = %prev;
		%prev = "RL_" @ %brick @ "_" @ $VCE[RLC,%brick];
	}
	//get unparsed parts after the replacer
	%next = getSubStr(%string,%replacerEnd + 1,strLen(%string) - %replacerEnd - 1);
	%fnext = %brick.filterVCEString(%next, %client,%player,%vehicle,%bot,%minigame);
	%header = getSubStr(%string,%headerStart,%headerEnd - %headerStart + 1);
	//everything between header and the end
	%things = getSubStr(%string,%headerEnd + 1, %replacerEnd - %headerEnd - 1);
	%fthings = %brick.filterVCEString(%things, %client,%player,%vehicle,%bot,%minigame);
	if("<e:" $= %header)
	{	
		%count = getWordCount(%things);
		for(%i = 0; %i < %count; %i++)
		{
			%word = getWord(%things, %i);
			
			if(strPos(%word,"RL_") == 0)
			{
				%things = setWord(%things,%i,%brick.getField(%word));
			}
		}
		//shunting yard algorithm
		%count = getWordCount(%things);
		%outputCount = 0;
		%operatorStackCount = 0;
		for(%i = 0; %i < %count; %i++)
		{
			%word = getWord(%things, %i);

			if(%word $= "")
				continue;
			if($VCE::Server::Operator[%word,NUMBER] $= "")
			{
				//push onto output queue
				if(%word $= "("){
					%operatorStack[%operatorStackCount] = %word;
					%operatorStackCount++;
				} else if(%word $= ")"){
					//pop until you find matching paranthesis
					while(%operatorStack[%operatorStackCount - 1] !$= "(" && %operatorStackCount > 0)
					{
						//pop and push top operator onto output stack
						%output[%outputCount] = %operatorStack[%operatorStackCount--];
						%outputCount++;
					}
					//mismatched parathesis
					if(%c <= 0 && %operatorStack[%operatorStackCount - 1] !$= "(")
						break;
					//parathesis found
					%operatorStackCount--;
				} else{
					//normal
					//we didn't filter this earlier so do it now
					%output[%outputCount] = %brick.filterVCEString(%word, %client,%player,%vehicle,%bot,%minigame);
					%outputCount++;
				}
				continue;
			}

			//see if we have any precedence problems
			while(%operatorStackCount > 0 && $VCE::Server::Operator[%operatorStack[%operatorStackCount - 1],PRECEDENCE] >= $VCE::Server::Operator[%word,PRECEDENCE] && %word !$= "("){
				//pop and push top operator onto output stack
				%output[%outputCount] = %operatorStack[%operatorStackCount--];
				%outputCount++;
			}
			//push current onto the operator stack
			%operatorStack[%operatorStackCount] = %word;
			%operatorStackCount++;
		}
		//push remaining operators onto the output stack
		for(%i = %operatorStackCount - 1; %i >= 0; %i--){
			%output[%outputCount] = %operatorStack[%i];
			%outputCount++;
		}
		//do some rpn magic
		//value for the shifting function
		%firstValue = 0;
		for(%i = 2; %i < %outputCount; %i++){
			//if it's an operator
			if($VCE::Server::Operator[%output[%i],NUMBER] !$= ""){
				$VCE[RF,%brick,$VCE[RFC,%brick]++] = "doVCEVarFunction" TAB $VCE::Server::Operator[%output[%i],NUMBER] TAB %output[%i - 2] @ "," @ %output[%i - 1];
				%output[%i] = "RF_" @ %brick @ "_" @ $VCE[RFC,%brick];
				//shift all values between %i - 1 and %last to starting at %i
				%firstValue = %i;
			}
		}
		for(%i = %firstValue; %i < %outputcount; %i++)
		{
			%product = %product TAB %output[%i];
		}
		
		return ltrim(%product);
	} else
	//variable
	if("<var:" $= %header)
	{
		%count = getFieldCount(%fthings);
		for(%i = 0; %i < %count; %i++)
		{
			%field = getField(%fthings,%i);
			if(strPos(%field,"RL") != 0)
			{
				continue;
			}

			%field = $VCE[%field];
			%div = strPos(%field,":");
			if(%div == -1)
			{
				continue;
			}
	
			%half = getSubStr(%field,0,%div);//we are defining the mode part of the variable within the vce replacer space
			if(%half !$= "")
			{
				%literal = "RL_" @ %brick @ "_" @ $VCE[RLC,%brick]++;
				$VCE[%literal] = %half;
			}
			%mode = "RL_" @ %brick @ "_" @ $VCE[RLC,%brick]++;
			$VCE[%mode] = rTrim(getFields(%fthings,0,%i - 1) TAB %literal);

			%literal = "";
			%half = getSubStr(%field,%div + 1,999999);//we are defining the var part of the variable within the vce replacer space
			if(%half !$= "")
			{
				%literal = "RL_" @ %brick @ "_" @ $VCE[RLC,%brick]++;
				$VCE[%literal] = %half;
			}
			%var = "RL_" @ %brick @ "_" @ $VCE[RLC,%brick]++;
			$VCE[%var] = lTrim(%literal TAB getFields(%fthings,%i + 1,%count));

			break;
		}

		if(%div >= 0)
		{
			%product = "RF_" @ %brick @ "_" @ $VCE[RFC,%brick]++;
			$VCE[%product] = %brick.getGroup().varGroup TAB "getVariable" TAB %var TAB %mode;
			talk($VCE[%product]);
			return trim(%prev TAB %product TAB %fnext);
		}
	} else
	//functions
	if("<func:" $= %header)
	{
		%count = getFieldCount(%fthings);
		for(%i = 0; %i < %count; %i++)
		{
			%field = getField(%fthings,%i);
			if(strPos(%field,"RL") != 0)
			{
				continue;
			}

			%field = $VCE[%field];
			%div = strPos(%field,":");
			if(%div == -1)
			{
				continue;
			}
	
			%half = getSubStr(%field,0,%div);//we are defining the mode part of the variable within the vce replacer space
			if(%half !$= "")
			{
				%literal = "RL_" @ %brick @ "_" @ $VCE[RLC,%brick]++;
				$VCE[%literal] = %half;
			}
			%func = "RL_" @ %brick @ "_" @ $VCE[RLC,%brick]++;
			$VCE[%func] = rTrim(getFields(%fthings,0,%i - 1) TAB %literal);

			%literal = "";
			%half = getSubStr(%field,%div + 1,999999);//we are defining the var part of the variable within the vce replacer space
			if(%half !$= "")
			{
				%literal = "RL_" @ %brick @ "_" @ $VCE[RLC,%brick]++;
				$VCE[%literal] = %half;
			}
			%args = "RL_" @ %brick @ "_" @ $VCE[RLC,%brick]++;
			$VCE[%args] = lTrim(%literal TAB getFields(%fthings,%i + 1,%count));

			break;
		}

		if(%div >= 0)
		{
			%product = "RF_" @ %brick @ "_" @ $VCE[RFC,%brick]++;
			$VCE[%product] = "doVCEVarFunction" TAB %func TAB %args;
			return trim(%prev TAB %product TAB %fnext);
		}
	} 

	if(%prev $= "")
	{
		$VCE[RL,%brick,$VCE[RLC,%brick]++] = %header @ %fthings @ ">";
		return trim("RL_" @ %brick @ "_" @ $VCE[RLC,%brick] TAB %fnext);
	}

	$VCE[%prev] = $VCE[%prev] @ %header @ %fthings @ ">";
	return trim(%prev TAB %fnext);
}

//for compatibility; this is depricated and does nothing
function filterVariableString(%string,%brick,%client,%player,%vehicle){
	return %string;
}

function hookFunctionToVCEEventFunction(%functionClass,%functionName,%functionArgs,%onlyCallIf,%outArgs,%eventFunctionName)
{
	if(!isFunction(%functionName) && !isFunction(%functionClass,%functionName))
		return;

	if(%outArgs $= "")
		%outArgs = "\"\"";
	
	if(%onlyCallIf $= "")
		%onlyCallIf = true;
	//set up the dictionary for this
	if(!$VCE::Server::EventDictionaryCatagoryExists[%functionClass])
	{
		$VCE::Server::EventDictionaryCatagory[$VCE::Server::EventDictionaryCatagoryCount++ - 1] = %functionClass;
		$VCE::Server::EventDictionaryCatagoryExists[%functionClass] = 1;
	}
	if(!$VCE::Server::EventDictionaryCatagoryEntryExists[%functionClass,%eventFunctionName]){
		$VCE::Server::EventDictionaryCatagoryEntry[%functionClass,$VCE::Server::EventDictionaryCatagoryEntryCount[%functionClass]++ - 1] = %eventFunctionName;
		$VCE::Server::EventDictionaryCatagoryEntryExists[%functionClass,%eventFunctionName] = 1;
	}
	eval("package VCE_HookEventFunctions {function "@ %functionClass @"::"@ %functionName @"("@ %functionArgs @"){if(!isObject(%client))%client = %player.client;if("@ %onlyCallIf @")callVCEEventFunction(\""@ %eventFunctionName @"\","@ %outArgs @", %client);Parent::"@ %functionName @"("@ %functionArgs @");}};");
}
function activateVCEEventFunctionHooks()
{
	activatePackage(VCE_HookEventFunctions);
}
//go through brick groups and call the function on a local level
function callVCEEventFunction (%eventFunctionName, %arg, %client)
{
	%group = MainBrickGroup;
	%groupSize = %group.getCount();
	for(%i = 0; %i < %groupSize; %i++)
	{
		%vargroup = %group.getObject(%i).vargroup;
		if(!isObject(%vargroup) || ($Pref::VCE::EventFunctionsAdminOnly && !%vargroup.client.isAdmin))
			continue;

		%localCount = %vargroup.vceLocalFunctionCount[%eventFunctionName];
		for(%j = 1; %j <= %localCount; %j++)
		{
			%sentence =  %vargroup.vceLocalFunction[%eventFunctionName,%j];

			%localBrick = getWord(%sentence,0);				

			if(!isObject(%localBrick))
				continue;
			
			%subStart = getWord(%sentence,1);
			%subEnd = getWord(%sentence,0);
			
			%fc = getWordCount(%arg);

			for(%k=0;%k<%fc;%k++)
			{
				%arg[%k] = getWord(%arg,%k);
				%varGroup.setVariable("arg" @ %k,%arg[%k],%localBrick);
			}

			%varGroup.setVariable("argcount",%fc,%localBrick);

			if(!isobject(%client))
				%client = %vargroup.client;
			
			%localbrick.VCE_ProcessVCERange(%subStart, %subEnd, "onVariableFunction", %client);
		}
	}
}
function serverCmdFD(%client,%page)
{
	%pageLength = 6;
	if(%page $= "")
		%page = 1;
		
	%client.chatMessage("<font:palatino linotype:20>\c2ModVariable Functions:");
	//display Event
	
	%c = (%page - 1) * %pageLength;
	while((%name = $VCE::Server::EventDictionaryCatagoryEntry[%catagoryName,%c]) !$= "" && %c < (%pageLength * (%page))){
		%client.chatMessage("<font:palatino linotype:20>\c3" @ %c SPC "\c6|\c4" SPC %name);
		%c++;
	}
	%client.chatMessage("<font:palatino linotype:20>\c2Page" SPC %page SPC "out of" SPC mCeil($VCE::Server::EventDictionaryCatagoryEntryCount[%catagoryName] / %pageLength) SPC ", Input the page you want to go to.");

}
function serverCmdSVD(%client,%catagory,%page)
{
	//we only want special variable dictionarys visible if you need it
	if(!($Pref::VCE::canEditSpecialVars && %client.isAdmin && %client.brickgroup.vargroup))
		return;

	%pageLength = 6;
	if(%page $= "")
		%page = 1;
	%catagoryName = $VCE::Server::ReplacerDictionaryCatagory[%catagory - 1];
	if(%catagory > 0 && %catagory <= $VCE::Server::ReplacerDictionaryCatagoryCount && %page > 0 && %page <= mCeil($VCE::Server::ReplacerDictionaryCatagoryEntryCount[%catagoryName] / %pageLength)){
		
		%client.chatMessage("<font:palatino linotype:20>\c2" @ $VCE::Server::ObjectToReplacer[%catagoryName] SPC "Variable Replacers:");
		//display replacers
		%c = (%page - 1) * %pageLength;
		
		while((%name = $VCE::Server::ReplacerDictionaryCatagoryEntry[%catagoryName,%c]) !$= "" && %c < (%pageLength * (%page))){
			%client.chatMessage("<font:palatino linotype:20>\c3" @ %c SPC "\c6|\c4" SPC %name SPC "(ex:" SPC trim(%client.brickgroup.vargroup.getVariable(%name,$VCE::Server::SpecialVariableObject[%client,$VCE::Server::ObjectToReplacer[%catagoryName]])) @ ")");
			%c++;
		}
		%client.chatMessage("<font:palatino linotype:20>\c2Page" SPC %page SPC "out of" SPC mCeil($VCE::Server::ReplacerDictionaryCatagoryEntryCount[%catagoryName] / %pageLength) SPC ", Input the page you want to go to.");
	} else{
		%client.chatMessage("<font:palatino linotype:20>\c2Welcome to the replacer dictionary, enter this command with the catagory's index to see its repalcers.");
		//display catagorys
		%c = 0;
		while((%name = $VCE::Server::ReplacerDictionaryCatagory[%c]) !$= ""){
			%client.chatMessage("<font:palatino linotype:20>\c3" @ %c + 1 SPC "\c6|\c4" SPC $VCE::Server::ObjectToReplacer[%name]);
			%c++;
		}
		%client.chatMessage("<font:palatino linotype:20>\c2You may not be able to see the whole list, Page Up and Page Down to browse it.");
	}
}
function registerSpecialVar(%classname,%name,%script,%editscript,%arg1,%arg2,%arg3,%arg4)
{
	if($VCE::Server::SpecialVar[%classname,%name] !$= "")
		echo("registerSpecialVar() - Variable" SPC %name SPC "already exists on" SPC %classname @ ". Overwriting...");
	//replacer dictionary
	if(!$VCE::Server::ReplacerDictionaryCatagoryExists[$VCE::Server::ObjectToReplacer[%className]]){
		$VCE::Server::ReplacerDictionaryCatagory[$VCE::Server::ReplacerDictionaryCatagoryCount++ - 1] = %className;
		$VCE::Server::ReplacerDictionaryCatagoryExists[$VCE::Server::ObjectToReplacer[%className]] = 1;
	}
	if(!$VCE::Server::ReplacerDictionaryCatagoryEntryExists[$VCE::Server::ObjectToReplacer[%className],%name]){
		$VCE::Server::ReplacerDictionaryCatagoryEntry[%className,$VCE::Server::ReplacerDictionaryCatagoryEntryCount[%className]++ - 1] = %name;
		$VCE::Server::ReplacerDictionaryCatagoryEntryExists[$VCE::Server::ObjectToReplacer[%className],%name] = 1;
	}
	$VCE::Server::SpecialVar[%classname,%name] = %script;
	$VCE::Server::SpecialVarEdit[%classname,%name] = %editscript;
	$VCE::Server::SpecialVarEditArg1[%classname,%name] = %arg1;
	$VCE::Server::SpecialVarEditArg2[%classname,%name] = %arg2;
	$VCE::Server::SpecialVarEditArg3[%classname,%name] = %arg3;
	$VCE::Server::SpecialVarEditArg4[%classname,%name] = %arg4;
}
function unregisterSpecialVar(%classname,%name)
{
	if($VCE::Server::SpecialVar[%classname,%name] $= "")
		return echo("unregisterSpecialVar() - Variable" SPC %name SPC " does not exist on" SPC %classname @ ". Can not un-register.");
	$VCE::Server::SpecialVar[%classname,%name] = "";
	$VCE::Server::SpecialVarEdit[%classname,%name] = "";
	$VCE::Server::SpecialVarEditArg1[%classname,%name] = "";
	$VCE::Server::SpecialVarEditArg2[%classname,%name] = "";
	$VCE::Server::SpecialVarEditArg3[%classname,%name] = "";
	$VCE::Server::SpecialVarEditArg4[%classname,%name] = "";
}