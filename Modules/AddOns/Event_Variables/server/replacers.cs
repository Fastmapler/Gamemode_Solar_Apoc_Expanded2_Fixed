//---
//	@package VCE
//	@title Replacers
//	@author Clockturn/www.clockler.com
//	@author Zack0Wack0/www.zack0wack0.com
//  @auther Monoblaster/46426
//	@time 4:44 PM 14/04/2011
//---

function VCE_getObjectFromVarType(%id,%brick,%client,%player,%vehicle,%bot,%minigame){
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
$VCE::Server::ObjectToReplacer["MinigameSO"] = "Minigame";
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
$VCE::Server::Operator["+"] = 1 TAB 11;
$VCE::Server::Operator["-"] = 2 TAB 11;
$VCE::Server::Operator["*"] = 3 TAB 12;
$VCE::Server::Operator["/"] = 4 TAB 12;
$VCE::Server::Operator["%"] = 16 TAB 12;
$VCE::Server::Operator["&&"] = 43 TAB 4;
$VCE::Server::Operator["||"] = 44 TAB 3;
$VCE::Server::Operator["&"] = 45 TAB 7; 
$VCE::Server::Operator["|"] = 46 TAB 5;
$VCE::Server::Operator["BSR"] = 47 TAB 10;
$VCE::Server::Operator["BSL"] = 48 TAB 10;
$VCE::Server::Operator["^"] = 49 TAB 6;
$VCE::Server::Operator["=="] = 52 TAB 8;
$VCE::Server::Operator["!="] = 53 TAB 8;
$VCE::Server::Operator["gT"] = 54 TAB 9;
$VCE::Server::Operator["lT"] = 55 TAB 9;
$VCE::Server::Operator["gT="] = 56 TAB 9;
$VCE::Server::Operator["lT="] = 57 TAB 9;
$VCE::Server::Operator["s="] = 58 TAB 9;
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
//DO NOT CALL THIS AS THIS IS DONE AUTOMATICALY NOW
//recursive getting of all things within <> replaces part of string with result
function fxDTSBrick::filterVCEString(%brick,%string,%client,%player,%vehicle,%bot,%minigame){
	//looks for the first header
	%headerStart = -1;
	while((%headerStart = strPos(%string,"<",%headerStart + 1)) != -1 && (%headerEnd = VCE_getReplacerHeaderEnd(%string,%headerStart)) == -1){}
	//there is no valid header
	if(%headerStart == -1)
		return %string;
	//find the end of the replacer
	%replacerEnd = strPos(%string,">",%headerEnd + 1);
	%checkStart = %headerEnd;
	while(%replacerEnd != -1 && (%checkStart = strPos(%string,"<",%checkStart + 1)) != -1 && %replacerEnd > %checkStart){
		if(VCE_getReplacerHeaderEnd(%string,%checkStart))
			%replacerEnd = strPos(%string,">",%replacerEnd + 1);
	}
	//if there is no valid end
	if(%replacerEnd == -1)
		return %string;
	//get parts before replacer
	%prev = getSubStr(%string,0,%headerStart);
	//get unparsed parts after the replacer
	%next = %brick.filterVCEString(getSubStr(%string,%replacerEnd + 1,strLen(%string) - %replacerEnd - 1), %client,%player,%vehicle,%bot,%minigame);
	%header = getSubStr(%string,%headerStart,%headerEnd - %headerStart + 1);
	//everything between header and the end
	%things = %brick.filterVCEString(getSubStr(%string,%headerEnd + 1, %replacerEnd - %headerEnd - 1), %client,%player,%vehicle,%bot,%minigame);
	//expression
	if("<e:" $= %header){
		//shunting yard algorithm
		%count = getWordCount(%things);
		%outputCount = 0;
		%operatorStackCount = 0;
		for(%i = 0; %i < %count; %i++){
			%word = getWord(%things, %i);
			if((%currentOperator = $VCE::Server::Operator[%word]) $= ""){
				//push onto output queue
				if(%word $= "("){
					%operatorStack[%operatorStackCount] = %word;
					%operatorStackCount++;
				} else if(%word $= ")"){
					//pop until you find matching paranthesis
					while(%operatorStack[%operatorStackCount - 1] !$= "(" && %operatorStackCount > 0){
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
					%output[%outputCount] = %word;
					%outputCount++;
				}
				continue;
			}
			//see if we have any precedence problems
			while((%operatorStackCount > 0 && getField(%operatorStack[%operatorStackCount - 1], 1) >= getField(%currentOperator,1)) && %word !$= "("){
				//pop and push top operator onto output stack
				%output[%outputCount] = %operatorStack[%operatorStackCount--];
				%outputCount++;
			}
			//push current onto the operator stack
			%operatorStack[%operatorStackCount] = %currentOperator;
			%operatorStackCount++;
		}
		//push remaining operators onto the output stack
		for(%i = %operatorStackCount - 1; %i >= 0; %i--){
			%output[%outputCount] = %operatorStack[%i];
			%outputCount++;
		}
		//do some rpn magic
		//value for the shifting function
		%last = 0;
		for(%i = 2; %i < %outputCount; %i++){
			//if it's an operator
			if(getfieldCount(%operator = %output[%i]) > 1){
				%output[%i] = doVCEVarFunction(getField(%operator,0), %output[%i - 2],%output[%i - 1]);
				//shift all values between %i - 1 and %last to starting at %i
				for(%j = %i - 3; %j >= %last; %j--){
					%output[%j + 2] = %output[%j];
				}
				%last = %last + 2;
			}
		}
		%product = %output[%outputcount - 1];
	} else
	//variable
	if("<var:" $= %header && strPos(%things,":") > 0){
		%ogBrick = %brick;
		%mode = getSubStr(%things,0,strPos(%things,":"));
		%var = getSubStr(%things,strPos(%things,":") + 1, 4000);
		%c = 0;
		if(striPos(%mode,"nb_") == 0){
			%brick = %brick.getGroup().NTObject_[getSubStr(%mode,3,strlen(%mode) - 3), 0];
			%mode = "br";
		}
		if(isObject(%brick) && isObject(%client)){
			if(isObject(%obj = VCE_getObjectFromVarType(%mode,%brick,%client,%player,%vehicle,%bot,%minigame))){
				%product = %brick.getGroup().varGroup.getVariable(%var, %obj);
			}
		}
		%brick = %ogBrick;
	} else
	//functions
	if("<func:" $= %header && strPos(%things,":") > 0){
		%mode = getSubStr(%things,0,strPos(%things,":"));
		%args = strReplace(getSubStr(%things,strPos(%things,":") + 1, 4000),",","\t");
		if((%func = $VCE::Server::Function[%mode]) !$= "")
			%product = doVCEVarFunction(%func, getField(%args,0),getFields(%args,1, getFieldCount(%args) - 1));
	} else{
		return %prev @ %header @ %things @ ">" @ %next;
	}
	return %prev @ %product @ %next;
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
function serverCmdEFD(%client,%catagory,%page)
{
	%pageLength = 6;
	if(%page $= "")
		%page = 1;
	%catagoryName = $VCE::Server::EventDictionaryCatagory[%catagory - 1];
	if(%catagory > 0 && %catagory <= $VCE::Server::EventDictionaryCatagoryCount && %page > 0 && %page <= mCeil($VCE::Server::EventDictionaryCatagoryEntryCount[%catagoryName] / %pageLength)){
		
		%client.chatMessage("<font:palatino linotype:20>\c2" @ $VCE::Server::ObjectToReplacer[%catagoryName] SPC "Event Functions:");
		//display Event
		
		%c = (%page - 1) * %pageLength;
		while((%name = $VCE::Server::EventDictionaryCatagoryEntry[%catagoryName,%c]) !$= "" && %c < (%pageLength * (%page))){
			%client.chatMessage("<font:palatino linotype:20>\c3" @ %c SPC "\c6|\c4" SPC %name);
			%c++;
		}
		%client.chatMessage("<font:palatino linotype:20>\c2Page" SPC %page SPC "out of" SPC mCeil($VCE::Server::EventDictionaryCatagoryEntryCount[%catagoryName] / %pageLength) SPC ", Input the page you want to go to.");
	} else{
		%client.chatMessage("<font:palatino linotype:20>\c2Welcome to the Event Function dictionary, enter this command with the catagory's index to see its repalcers.");
		//display catagorys
		%c = 0;
		while((%name = $VCE::Server::EventDictionaryCatagory[%c]) !$= ""){
			%client.chatMessage("<font:palatino linotype:20>\c3" @ %c + 1 SPC "\c6|\c4" SPC $VCE::Server::ObjectToReplacer[%name]);
			%c++;
		}
		%client.chatMessage("<font:palatino linotype:20>\c2You may not be able to see the whole list, Page Up and Page Down to browse it.");
	}
}
function serverCmdSVD(%client,%catagory,%page)
{
	%pageLength = 6;
	if(%page $= "")
		%page = 1;
	%catagoryName = $VCE::Server::ReplacerDictionaryCatagory[%catagory - 1];
	if(%catagory > 0 && %catagory <= $VCE::Server::ReplacerDictionaryCatagoryCount && %page > 0 && %page <= mCeil($VCE::Server::ReplacerDictionaryCatagoryEntryCount[%catagoryName] / %pageLength)){
		
		%client.chatMessage("<font:palatino linotype:20>\c2" @ $VCE::Server::ObjectToReplacer[%catagoryName] SPC "Variable Replacers:");
		//display replacers
		
		%c = (%page - 1) * %pageLength;
		while((%name = $VCE::Server::ReplacerDictionaryCatagoryEntry[%catagoryName,%c]) !$= "" && %c < (%pageLength * (%page))){
			%client.chatMessage("<font:palatino linotype:20>\c3" @ %c SPC "\c6|\c4" SPC %name);
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
	if(!$VCE::Server::ReplacerDictionaryCatagoryExists[%className]){
		$VCE::Server::ReplacerDictionaryCatagory[$VCE::Server::ReplacerDictionaryCatagoryCount++ - 1] = %className;
		$VCE::Server::ReplacerDictionaryCatagoryExists[%className] = 1;
	}
	if(!$VCE::Server::ReplacerDictionaryCatagoryEntryExists[%className,%name]){
		$VCE::Server::ReplacerDictionaryCatagoryEntry[%className,$VCE::Server::ReplacerDictionaryCatagoryEntryCount[%className]++ - 1] = %name;
		$VCE::Server::ReplacerDictionaryCatagoryEntryExists[%className,%name] = 1;
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