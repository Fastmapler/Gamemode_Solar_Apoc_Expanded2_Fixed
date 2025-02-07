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
	if(%id $= "Global" || %id $= "Gl")
		return "Global";
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
		return getVariableGroupFromObject(%brick);

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
		return getVariableGroupFromObject(%brick);
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

$VCE::Server::ParserType1 = "Literal";
$VCE::Server::ParserType2 = "Function";
$VCE::Server::ParserTypeLiteral = 1;
$VCE::Server::ParserTypeFunction = 2;

$VCE::Server::ReplacerFunction["var"] = "VCE_ReplacerGetVariable";
$VCE::Server::ReplacerFunction["e"] = "VCE_ReplacerDoEvaluate";
$VCE::Server::ReplacerFunction["func"] = "VCE_ReplacerDoFunction";

$VCE::Server:CurrLiteral = -1;
function VCE_ReplacerDoLiteral(%parameter,%eventbrick,%target,%client,%player,%vehicle,%bot,%minigame)
{
	$VCE::Server:LiteralStack[$VCE::Server:CurrLiteral++] = %parameter;
	return "";
}

function VCE_ReplacerGetVariable(%parameter,%eventbrick,%target,%client,%player,%vehicle,%bot,%minigame)
{
	%name = NextToken(%parameter, "%type", ":");
	if(strpos(%type,"nb_") == 0)
	{
		return getVariableGroupFromObject(%eventbrick).getNamedBrickVariable(%name,getSubStr(%type,2,999999));
	}

	%brick = %eventbrick;
	if(%target.getClassName() $= "fxDtsBrick")
	{
		%brick = %target;
	}
	return getVariableGroupFromObject(%eventbrick).getVariable(%name,VCE_getObjectFromVarType(%type,%brick,%client,%player,%vehicle,%bot,%minigame));
}

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

function VCE_ReplacerDoFunction(%parameter,%eventbrick,%target,%client,%player,%vehicle,%bot,%minigame)
{
	%parameters = NextToken(%parameter, "%name", ":");

	if($VCE::Server::Function[%name] $= "")
	{
		return "";
	}

	return doVCEVarFunction(%name,%parameters);
}

$VCE::Server::OperatorNumber["+"] = 1;
	$VCE::Server::OperatorPrecedence["+"] = 11;
$VCE::Server::OperatorNumber["-"] = 2;
	$VCE::Server::OperatorPrecedence["-"] = 11;
$VCE::Server::OperatorNumber["*"] = 3;
	$VCE::Server::OperatorPrecedence["*"] = 12;
$VCE::Server::OperatorNumber["/"] = 4;
	$VCE::Server::OperatorPrecedence["/"] = 12;
$VCE::Server::OperatorNumber["%"] = 16;
	$VCE::Server::OperatorPrecedence["%"] = 12;
$VCE::Server::OperatorNumber["&&"] = 43;
	$VCE::Server::OperatorPrecedence["&&"] = 4;
$VCE::Server::OperatorNumber["||"] = 44;
	$VCE::Server::OperatorPrecedence["||"] = 3;
$VCE::Server::OperatorNumber["&"] = 45;
	$VCE::Server::OperatorPrecedence46 = 7;
$VCE::Server::OperatorNumber["|"] = 49;
	$VCE::Server::OperatorPrecedence["|"] = 5;
$VCE::Server::OperatorNumber["BSR"] = 47;
	$VCE::Server::OperatorPrecedence["BSR"] = 10;
$VCE::Server::OperatorNumber["BSL"] = 48;
	$VCE::Server::OperatorPrecedence["BSL"] = 10;
$VCE::Server::OperatorNumber["^"] = 49;
	$VCE::Server::OperatorPrecedence["^"] = 6;
$VCE::Server::OperatorNumber["=="] = 52;
	$VCE::Server::OperatorPrecedence["=="] = 8;
$VCE::Server::OperatorNumber["!="] = 53;
	$VCE::Server::OperatorPrecedence["!="] = 8;
$VCE::Server::OperatorNumber["gT"] = 54;
	$VCE::Server::OperatorPrecedence["gT"] = 9;
$VCE::Server::OperatorNumber["lT"] = 55;
	$VCE::Server::OperatorPrecedence["lT"] = 9;
$VCE::Server::OperatorNumber["gT="] = 56;
	$VCE::Server::OperatorPrecedence["gT="] = 9;
$VCE::Server::OperatorNumber["lT="] = 57;
	$VCE::Server::OperatorPrecedence["lT="] = 9;
$VCE::Server::OperatorNumber["s="] = 58;
	$VCE::Server::OperatorPrecedence["s="] = 9;

$VCE::Server::IsNumericLiteral[0] = true;
$VCE::Server::IsNumericLiteral[1] = true;
$VCE::Server::IsNumericLiteral[2] = true;
$VCE::Server::IsNumericLiteral[3] = true;
$VCE::Server::IsNumericLiteral[4] = true;
$VCE::Server::IsNumericLiteral[5] = true;
$VCE::Server::IsNumericLiteral[6] = true;
$VCE::Server::IsNumericLiteral[7] = true;
$VCE::Server::IsNumericLiteral[8] = true;
$VCE::Server::IsNumericLiteral[9] = true;
$VCE::Server::IsNumericLiteral["."] = true;
$VCE::Server::IsNumericLiteral["-"] = true;

$VCE::Server::IsWhiteSpace[" "] = true;

$VCE::Server::IsOperatorSymbol["+"] = true;
$VCE::Server::IsOperatorSymbol["-"] = true;
$VCE::Server::IsOperatorSymbol["*"] = true;
$VCE::Server::IsOperatorSymbol["/"] = true;
$VCE::Server::IsOperatorSymbol["%"] = true;
$VCE::Server::IsOperatorSymbol["&"] = true;
$VCE::Server::IsOperatorSymbol["|"] = true;
$VCE::Server::IsOperatorSymbol["B"] = true;
$VCE::Server::IsOperatorSymbol["S"] = true;
$VCE::Server::IsOperatorSymbol["R"] = true;
$VCE::Server::IsOperatorSymbol["L"] = true;
$VCE::Server::IsOperatorSymbol["^"] = true;
$VCE::Server::IsOperatorSymbol["="] = true;
$VCE::Server::IsOperatorSymbol["!"] = true;
$VCE::Server::IsOperatorSymbol["g"] = true;
$VCE::Server::IsOperatorSymbol["l"] = true;
$VCE::Server::IsOperatorSymbol["t"] = true;
$VCE::Server::IsOperatorSymbol["s="] = true;

function VCE_ReplacerDoEvaluate(%parameter,%eventbrick,%target,%client,%player,%vehicle,%bot,%minigame)
{
	%count = strlen(%parameter);
	%currtoken = -1;
	%curroperator = -1;
	%i = 0;
	%char = getSubStr(%parameter,%i,1);
	while(%i < %count)
	{	

		if($VCE::Server::IsWhiteSpace[%char])
		{
			for(%i = %i; %i < %count; %i++)
			{
				%char = getSubStr(%parameter,%i,1);
				if(!$VCE::Server::IsWhiteSpace[%char])
				{
					break;
				}
			}
		}

		if(%char $= "(")
		{
			%operatorStack[%curroperator++] = "(";
			%char = getSubStr(%parameter,%i++,1);
		}

		if($VCE::Server::IsWhiteSpace[%char])
		{
			for(%i = %i; %i < %count; %i++)
			{
				%char = getSubStr(%parameter,%i,1);
				if(!$VCE::Server::IsWhiteSpace[%char])
				{
					break;
				}
			}
		}

		if($VCE::Server::IsNumericLiteral[%char])
		{
			%token = %char;
			%i++;
			for(%i = %i; %i < %count; %i++)
			{
				%char = getSubStr(%parameter,%i,1);
				if(!$VCE::Server::IsNumericLiteral[%char])
				{
					break;
				}
				%token = %token @ %char;
			}
			%tokenstack[%currToken++] = %token;
		}
		else if(!$VCE::Server::IsOperatorSymbol[%char])
		{
			return %parameter SPC "\c0INVALID CHARACTER \"" @ %char @ "\" @" SPC %i; //invalid character error
		}

		if($VCE::Server::IsWhiteSpace[%char])
		{
			for(%i = %i; %i < %count; %i++)
			{
				%char = getSubStr(%parameter,%i,1);
				if(!$VCE::Server::IsWhiteSpace[%char])
				{
					break;
				}
			}
		}

		if(%char $= ")")
		{
			for(%j = %curroperator; %j > -1; %j--)
			{
				if(%operatorStack[%j] $= "(")
				{
					break;
				}
				%tokenstack[%currToken++] = %operatorStack[%j];
			}
			%curroperator = %j - 1;
			%char = getSubStr(%parameter,%i++,1);
		}

		if($VCE::Server::IsWhiteSpace[%char])
		{
			for(%i = %i; %i < %count; %i++)
			{
				%char = getSubStr(%parameter,%i,1);
				if(!$VCE::Server::IsWhiteSpace[%char])
				{
					break;
				}
			}
		}

		if(%i >= %count)
		{
			break;
		}

		if($VCE::Server::IsOperatorSymbol[%char])
		{
			%token = %char;
			%i++;
			for(%i = %i; %i < %count; %i++)
			{
				%char = getSubStr(%parameter,%i,1);
				if(!$VCE::Server::IsOperatorSymbol[%char])
				{
					break;
				}
				%token = %token @ %char;
			}
			if($VCE::Server::OperatorNumber[%token] $= "")
			{
				return %parameter SPC "\c0INVALID OPERATOR \"" @ %token @ "\" @" SPC %i; //invalid operator error
			}

			if(%curroperator != -1 && $VCE::Server::OperatorPrecedence[%operatorStack[%curroperator]] >= $VCE::Server::OperatorPrecedence[%token])
			{
				for(%j = %curroperator; %j > -1; %j--)
				{
					%tokenstack[%currToken++] = %operatorStack[%j];
				}
				%curroperator = -1;
			}
			
			%operatorStack[%curroperator++] = %token;
		}
		else if(!$VCE::Server::IsNumericLiteral[%char] && !$VCE::Server::IsWhiteSpace[%char])
		{
			return %parameter SPC "\c0INVALID CHARACTER \"" @ %char @ "\" @" SPC %i; //invalid character error
		}
	}

	for(%j = %curroperator; %j > -1; %j--)
	{
		%tokenstack[%currToken++] = %operatorStack[%j];
	}

	%currsoltuion = -1;
	for(%i = 0; %i < %currToken + 1; %i++)
	{
		if($VCE::Server::OperatorNumber[%tokenstack[%i]] $= "")
		{
			%solutionstack[%currsoltuion++] = %tokenstack[%i];
			continue;
		}

		%solutionstack[%currsoltuion--] = doVCEVarFunction($VCE::Server::OperatorNumber[%tokenstack[%i]], %solutionstack[%currsoltuion - 1] TAB %solutionstack[%currsoltuion]);
	}

	return %solutionstack0;
}

function VCE_CompiledParameter_Create(%brick,%eventidx,%paridx)
{
	%brick.VCE_CompiledParameter[%eventidx,%paridx] = true;
	%input = %brick.eventOutputParameter[%eventidx,%paridx];
	%oldopencount = strlen(%input) -  strlen(stripChars(%input, "<"));
	%oldclosecount = strlen(%input) -  strlen(stripChars(%input, ">"));

	if(%oldopencount != %oldclosecount)
	{
		VCE_Literal(%obj,%input);
		return; // there is not an equal number of open and close 
	}

	//loop through dilemented by < and >
	//keep track of scope by counting change in the number of >
	
	%inmarkdown = false; //true if markup is detected
	%enteringreplacer = stripos(%input,"<") == 0; //true if we've entered a <
	if(%enteringreplacer)
	{
		%oldopencount--;
	}

	%donotmergenextliteral = false; //forces literals not to merge
	%currdepth = %enteringreplacer;
	%currtoken = -1;
	%curroperator = -1;
	while(true)
	{
		%input = NextToken(%input, "%token", "<>");
		if(%token $= "")
		{
			break;
		}

		%newopencount = strlen(%input) -  strlen(stripChars(%input, "<"));
		%newclosecount = strlen(%input) -  strlen(stripChars(%input, ">"));
		%openchange = %oldopencount - %newopencount;
		%closechange = %oldclosecount - %newclosecount;
		%oldopencount = %newopencount;
		%oldclosecount = %newclosecount;

		if(%enteringreplacer && %currdepth != 0) //entered a potential replacer
		{
			%remainder = NextToken(%token, "%type", ":");
			if($VCE::Server::ReplacerFunction[%type] $= "")
			{
				%inmarkdown = true;
				if(%brick.VCE_tokentype[%eventidx,%paridx,%currtoken] != $VCE::Server::ParserTypeLiteral || %donotmergenextliteral)
				{
					%brick.VCE_tokenStack[%eventidx,%paridx,%currToken++] = "<" @ %token;
					%brick.VCE_tokentype[%eventidx,%paridx,%currtoken] = $VCE::Server::ParserTypeLiteral;
					%donotmergenextliteral = false;
				}
				else
				{
					%brick.VCE_tokenStack[%eventidx,%paridx,%currtoken] = %brick.VCE_tokenStack[%eventidx,%paridx,%currtoken] @ "<" @ %token;
				}
			}
			else
			{
				if(%brick.VCE_tokenStack[%eventidx,%paridx,%currtoken] !$= "")
				{
					%brick.VCE_tokenStack[%eventidx,%paridx,%currtoken++] = "VCE_ReplacerDoLiteral";
					%brick.VCE_tokentype[%eventidx,%paridx,%currtoken] = $VCE::Server::ParserTypeFunction;
				}

				%operatorStack[%curroperator++] = $VCE::Server::ReplacerFunction[%type];
				
				if(%remainder !$= "")
				{
					%brick.VCE_tokenStack[%eventidx,%paridx,%currtoken++] = %remainder;
					%brick.VCE_tokentype[%eventidx,%paridx,%currtoken] = $VCE::Server::ParserTypeLiteral;
				}
			}
		}
		else
		{
			if(%brick.VCE_tokentype[%eventidx,%paridx,%currtoken] != $VCE::Server::ParserTypeLiteral || %donotmergenextliteral)
			{
				%brick.VCE_tokenStack[%eventidx,%paridx,%currtoken++] = %token;
				%brick.VCE_tokentype[%eventidx,%paridx,%currtoken] = $VCE::Server::ParserTypeLiteral;
				%donotmergenextliteral = false;
				
			}
			else
			{
				%brick.VCE_tokenStack[%eventidx,%paridx,%currtoken] = %brick.VCE_tokenStack[%eventidx,%paridx,%currtoken] @ %token;
			}
		}

		if(%closechange > 0) //closing for a replacer or markdown
		{
			if(%inmarkdown)
			{
				if(%closechange > 1)
				{
					%donotmergenextliteral = true;
					%closechange--;	
					for(%i = 0; %i < %closechange; %i++)
					{
						%brick.VCE_tokenStack[%eventidx,%paridx,%currToken++] = %operatorStack[%curroperator-- + 1];
						%brick.VCE_tokentype[%eventidx,%paridx,%currtoken] = $VCE::Server::ParserTypeFunction;
					}
					%brick.VCE_tokentype[%eventidx,%paridx,%currtoken] = $VCE::Server::ParserTypeFunction;
				}
				
				if(%brick.VCE_tokentype[%eventidx,%paridx,%currtoken] != $VCE::Server::ParserTypeLiteral)
				{
					%currtoken++;
					%brick.VCE_tokentype[%eventidx,%paridx,%currtoken] = $VCE::Server::ParserTypeLiteral;
				}
				%brick.VCE_tokenStack[%eventidx,%paridx,%currtoken] = %brick.VCE_tokenStack[%eventidx,%paridx,%currtoken] @ ">";
				%inmarkdown = false;
			}
			else
			{
				%donotmergenextliteral = true;
				for(%i = 0; %i < %closechange; %i++)
				{
					%brick.VCE_tokenStack[%eventidx,%paridx,%currToken++] = %operatorStack[%curroperator-- + 1];
					%brick.VCE_tokentype[%eventidx,%paridx,%currtoken] = $VCE::Server::ParserTypeFunction;
				}
			}
		}

		%enteringreplacer = %openchange > 0;
		%currdepth += %openchange - %closechange;
	}

	for(%i = %curroperator; %i > -1; %i--)
	{
		%brick.VCE_tokenStack[%eventidx,%paridx,%currtoken++] = %operatorStack[%i];
		%brick.VCE_tokentype[%eventidx,%paridx,%currtoken] = $VCE::Server::ParserTypeFunction;
	}
	%brick.VCE_tokenStackSize[%eventidx,%paridx] = %currtoken + 1;
}

function VCE_CompiledParameter_Run(%eventbrick,%eventidx,%paridx,%target,%client,%player,%vehicle,%bot,%minigame)
{
	%parameter = "";
	%count = %eventbrick.VCE_tokenStackSize[%eventidx,%paridx];
	$VCE::Server:CurrLiteral = -1;
	for(%i = 0; %i < %count; %i++)
	{
		if(%eventbrick.VCE_tokentype[%eventidx,%paridx,%i] == $VCE::Server::ParserTypeLiteral)
		{
			%parameter = %parameter @ %eventbrick.VCE_tokenStack[%eventidx,%paridx,%i];
			continue;
		}

		%parameter = getSubStr(call(%eventbrick.VCE_tokenStack[%eventidx,%paridx,%i],getSubStr(%parameter,0,1000),%eventbrick,%target,%client,%player,%vehicle,%bot,%minigame),0,1000); //clamp parameter input and output
		if(%eventbrick.VCE_tokenStack[%eventidx,%paridx,%i] !$= "VCE_ReplacerDoLiteral" && $VCE::Server:CurrLiteral > -1)
		{
			%parameter = $VCE::Server:LiteralStack[$VCE::Server:CurrLiteral-- + 1] @ %parameter;
		}
	}

	for(%i = $VCE::Server:CurrLiteral; %i > -1; %i++)
	{
		%parameter = $VCE::Server:LiteralStack[%i] @ %parameter;
	}

	return %parameter;
}

function fxDTSBrick::VCE_CompileBrick(%brick)
{
	if(%brick.VCE_Compiled)
	{
		return;
	}
	%brick.VCE_Compiled = true;

	%count = %brick.numEvents;
	for(%i = 0; %i < %count; %i++)
	{
		%targetIDX = %brick.eventTargetIdx[%i];
		if(%targetIDX <= 0)
		{
			%targetClass = inputEvent_GetTargetClass("fxDTSBrick", %brick.eventInputIdx[%i], inputEvent_GetTargetIndex("fxDTSBrick",%brick.eventInputIdx[%i],"Self"));
		}
		else
		{
			%targetClass = inputEvent_GetTargetClass("fxDTSBrick", %brick.eventInputIdx[%i], %targetIDX);
		}

		%parameterWords = verifyOutputParameterList(%targetClass, %brick.eventOutputIdx[%i]);
	 	%parameterWordCount = getFieldCount(%parameterWords);
		for(%j = 1; %j <= %parameterWordCount; %j++)
		{
			%word = getWord(getField(%parameterWords, %j-1),0);
			if(%brick.VCE_CompiledParameter[%i,%j])
			{
				%brick.VCE_CompiledParameter[%i,%j] = false;
			}

			if(%word !$= "String")
			{
				continue;	
			}
			VCE_CompiledParameter_Create(%brick,%i,%j); // parameters are 1-4 (WHY BADSPOT)
		}
	}
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
		%vargroup = getVariableGroupFromObject(%group.getObject(%i));
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
	if(!($Pref::VCE::canEditSpecialVars && %client.isAdmin && getVariableGroupFromObject(%client)))
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
			%client.chatMessage("<font:palatino linotype:20>\c3" @ %c SPC "\c6|\c4" SPC %name SPC "(ex:" SPC trim(getVariableGroupFromObject(%client).getVariable(%name,$VCE::Server::SpecialVariableObject[%client,$VCE::Server::ObjectToReplacer[%catagoryName]])) @ ")");
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