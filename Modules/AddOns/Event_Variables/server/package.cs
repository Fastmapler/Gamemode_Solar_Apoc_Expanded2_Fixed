//---
//	@package VCE
//	@title Package
//	@author Zack0Wack0/www.zack0wack0.com
//	@auther Monoblaster/46426
//	@time 5:30 PM 16/04/2011
//---
$VCEisEventParameterType["int"] = 1;
$VCEisEventParameterType["float"] = 1;
$VCEisEventParameterType["list"] = 1;
$VCEisEventParameterType["bool"] = 1;
$VCEisEventParameterType["intList"] = 1;
$VCEisEventParameterType["datablock"] = 1;
$VCEisEventParameterType["string"] = 1;
$VCEisEventParameterType["vector"] = 1;
$VCEisEventParameterType["paintColor"] = 1;
package VCE_Main
{
	//packages for figuring out special variable examples
	function GameConnection::OnAdd(%client)
	{
		$VCE::Server::SpecialVariableObject[%client,GLOBAL] = %client;
		$VCE::Server::SpecialVariableObject[%client,CLIENT] = %client;
	}
	function Armor::onAdd(%this, %obj)
	{
		if(isObject(%obj.client) && %obj.client.getClassName() $= "GameConnection")
			$VCE::Server::SpecialVariableObject[%obj.client,PLAYER] = %obj;
		else if (isObject(%obj.spawnBrick))
			$VCE::Server::SpecialVariableObject[%obj.spawnBrick.getGroup().client,BOT] = %obj;

		Parent::onAdd(%this, %obj);
	}
	function fxDtsBrick::spawnVehicle(%brick)
	{
		Parent::spawnVehicle(%brick);

		$VCE::Server::SpecialVariableObject[%brick.getGroup().client,VEHICLE] = %brick.vehicle;
	}
	function fxDtsBrick::onPlant(%brick)
	{
		VCE_createVariableGroup(%brick);
		$VCE::Server::SpecialVariableObject[%brick.getGroup().client,BRICK] = %brick;

		return Parent::onPlant(%brick);
	}
	function fxDtsBrick::onLoadPlant(%brick)
	{
		VCE_createVariableGroup(%brick);
		$VCE::Server::SpecialVariableObject[%brick.getGroup().client,BRICK] = %brick;
		
		return Parent::onLoadPlant(%brick);
	}
	function MinigameSO::AddMember(%mg,%client)
	{
		Parent::AddMember(%mg,%client);

		$VCE::Server::SpecialVariableObject[%client,MINIGAME] = %mg;
	}
	function servercmdMessageSent(%client,%message)
	{
		%client.lastMessage = %message;
		%mini = getMinigameFromObject(%client);
		if(isObject(%mini))
			%mini.lastMessage = %message;
		$VCE::Other::LastMessage = %message;
		return Parent::servercmdMessageSent(%client,%message);
	}
	function servercmdteamMessageSent(%client,%message)
	{
		%client.lastTeamMessage = %message;
		return Parent::servercmdteamMessageSent(%client,%message);
	}
	function Armor::onTrigger(%this, %obj, %triggerNum, %val){
		%obj.VCETrigger[%triggerNum] = %val;
		Parent::onTrigger(%this, %obj, %triggerNum, %val);
	}
	function serverCmdSit(%client)
	{
		%client.VCESitting = !%client.player.vceSitting;
		Parent::serverCmdSit(%client);
	}
	function GameConnection::onDeath(%client,%source,%sourceClient,%type,%area)
	{
		%client.vceDeaths++;
		if(%client != %sourceClient && isObject(%sourceClient))
			%sourceClient.vceKills++;
		return Parent::onDeath(%client,%source,%sourceClient,%type,%area);
	}
	function serverCmdAddEvent (%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4){
		%brick = %client.wrenchBrick;
		%targetClass = inputEvent_GetTargetClass("fxDTSBrick", %inputEventIdx, %targetIdx);

		%outputName = $OutputEvent_Name[%targetClass, %outputEventIdx];
		%inputName = $InputEvent_Name[%targetClass, %inputEventIdx];
		%i = mFloor (%brick.numEvents);
		
		
		%parameterWords = verifyOutputParameterList(%targetClass, outputEvent_GetOutputEventIdx(%targetClass, %outputName));
		%parameterWordCount = getWordCount(%parameterWords);
		%c = 1;
		//go thorugh parameters and filter replacers for them to make eval strings for later computaion
		if(%i == 0)
		{
			//spaghetti code because i don't feel like making an initlizing function
			$VCE[RFC,%brick] = 0;
			$VCE[RLC,%brick] = 0;
		}

		//remove previous reference strings
		deleteVariables("$VCE_ReferenceString"@%obj@"_"@%i@"_*");
		
		for(%j = 0; %j < %parameterWordCount; %j++)
		{
			%word = getWord(%parameterWords, %j);
			if(%word $= "string"){
				//cleansing strings because you can crash by self referencing
				%par[%c] = strReplace(%par[%c], "RF_", "");
				%par[%c] = strReplace(%par[%c], "RL_", "");
				//filtering and creating a reference string
				$VCE_ReferenceString[%brick,%i,%c] = trim(%brick.filterVCEString(%par[%c],%client,%client.player,%brick.vehicle,%brick.hbot,%client.minigame));
			}
			if($VCEisEventParameterType[%word])
			{
				%c++;
			}	

		}
		%brick.VCE_Parsed = true;
		//startfunction setup
		if(%outputName $= "VCE_StartFunction"){
			%brick.VCE_startFunction(%par1,%par2,%par3,%client);
		}	
		//loop checking
		if((%inputName $= "onVariableTrue" || %inputName $= "onVariableFalse") && (%outputName $= "VCE_ifVariable" || %outputName $= "VCE_ifValue") || (%inputName $= "onVariableUpdate" && (%outputName $= "VCE_ifValue" || %outputName $= "VCE_ifVariable")) || (%inputName $= "onVariableUpdate" && %outputName $= "VCE_modVariable")){
			if (%delay < $Pref::VCE::LoopDelay)
				%delay = $Pref::VCE::LoopDelay;
		}
		Parent::serverCmdAddEvent(%client, %enabled, %inputEventIdx, %delay, %targetIdx, %NTNameIdx, %outputEventIdx, %par1, %par2, %par3, %par4);
	}
	function AIPlayer::startHoleLoop(%bot){
		%bot.hIsEnabled = true;
		Parent::startHoleLoop(%bot);
	}
	function AIPlayer::stopHoleLoop(%bot){
		%bot.hIsEnabled = false;
		Parent::stopHoleLoop(%bot);
	}
	//Repackaging this function to include a brick when giving this to specific output events
	function SimObject::processInputEvent(%obj, %EventName, %client)
	{
		if (%obj.numEvents <= 0.0)
		{
			return;
		}
		%foundOne = 0;
		%i = 0;
		while(%i < %obj.numEvents)
		{
			if (%obj.eventInput[%i] !$= %EventName)
			{
			}
			else
			{
				if (!%obj.eventEnabled[%i])
				{
				}
				else
				{
					%foundOne = 1;
					break;
				}
			}
			%i = %i + 1.0;
		}
		if (!%foundOne)
		{
			return;
		}
		if (isObject(%client))
		{
			%quotaObject = getQuotaObjectFromClient(%client);
		}
		else
		{
			if (%obj.getType() & $TypeMasks::FxBrickAlwaysObjectType)
			{
				%quotaObject = getQuotaObjectFromBrick(%obj);
			}
			else
			{
				if (getBuildString() !$= "Ship")
				{
					error("ERROR: SimObject::ProcessInputEvent() - could not get quota object for event " @ %EventName @ " on object " @ %obj);
				}
				return;
			}
		}
		if (!isObject(%quotaObject))
		{
			error("ERROR: SimObject::ProcessInputEvent() - new quota object creation failed!");
		}
		setCurrentQuotaObject(%quotaObject);
		if (%EventName $= "OnRelay")
		{
			if (%obj.implicitCancelEvents)
			{
				%obj.cancelEvents();
			}
		}
		%i = 0;
		while(%i < %obj.numEvents)
		{
			if (!%obj.eventEnabled[%i])
			{
			}
			else
			{
				if (%obj.eventInput[%i] !$= %EventName)
				{
				}
				else
				{
					if (%obj.eventOutput[%i] !$= "CancelEvents")
					{
					}
					else
					{
						if (%obj.eventDelay[%i] > 0.0)
						{
						}
						else
						{
							if (%obj.eventTarget[%i] == -1.0)
							{
								%name = %obj.eventNT[%i];
								%group = %obj.getGroup();
								%j = 0;
								while(%j < %group.NTObjectCount[%name])
								{
									%target = %group.NTObject[%name,%j];
									if (!isObject(%target))
									{
									}
									else
									{
										%target.cancelEvents();
									}
									%j = %j + 1.0;
								}
							}
							else
							{
								%target = $InputTarget_[%obj.eventTarget[%i]];
								if (!isObject(%target))
								{
								}
								else
								{
									%target.cancelEvents();
								}
							}
						}
					}
				}
			}
			%i = %i + 1.0;
		}
		%eventCount = 0;
		%i = 0;
		while(%i < %obj.numEvents)
		{
			if (%obj.eventInput[%i] !$= %EventName)
			{
			}
			else
			{
				if (!%obj.eventEnabled[%i])
				{
				}
				else
				{
					if (%obj.eventOutput[%i] $= "CancelEvents" && %obj.eventDelay[%i] == 0.0)
					{
					}
					else
					{
						if (%obj.eventTarget[%i] == -1.0)
						{
							%name = %obj.eventNT[%i];
							%group = %obj.getGroup();
							%j = 0;
							while(%j < %group.NTObjectCount[%name])
							{
								%target = %group.NTObject[%name,%j];
								if (!isObject(%target))
								{
								}
								else
								{
									%eventCount = %eventCount + 1.0;
								}
								%j = %j + 1.0;
							}
						}
						else
						{
							%eventCount = %eventCount + 1.0;
						}
					}
				}
			}
			%i = %i + 1.0;
		}
		if (%eventCount == 0.0)
		{
			return;
		}
		%currTime = getSimTime();
		if (%eventCount > %quotaObject.getAllocs_Schedules())
		{
			commandToClient(%client, 'CenterPrint', "<color:FFFFFF>Too many events at once!\n(" @ %EventName @ ")", 1);
			if (%client.SQH_StartTime <= 0.0)
			{
				%client.SQH_StartTime = %currTime;
			}
			else
			{
				if (%currTime - %client.SQH_LastTime < 2000.0)
				{
					%client.SQH_HitCount = %client.SQH_HitCount + 1.0;
				}
				if (%client.SQH_HitCount > 5.0)
				{
					%client.ClearEventSchedules();
					%client.resetVehicles();
					%mask = $TypeMasks::PlayerObjectType | $TypeMasks::ProjectileObjectType | $TypeMasks::VehicleObjectType | $TypeMasks::CorpseObjectType;
					%client.ClearEventObjects(%mask);
				}
			}
			%client.SQH_LastTime = %currTime;
			return;
		}
		if (%currTime - %client.SQH_LastTime > 1000.0)
		{
			%client.SQH_StartTime = 0;
			%client.SQH_HitCount = 0;
		}
		%i = 0;
		while(%i < %obj.numEvents)
		{
			if (%obj.eventInput[%i] !$= %EventName)
			{
			}
			else
			{
				if (!%obj.eventEnabled[%i])
				{
				}
				else
				{
					if (%obj.eventOutput[%i] $= "CancelEvents" && %obj.eventDelay[%i] == 0.0)
					{
					}
					else
					{
						%delay = %obj.eventDelay[%i];
						%outputEvent = %obj.eventOutput[%i];
						%par1 = %obj.eventOutputParameter[%i,1];
						%par2 = %obj.eventOutputParameter[%i,2];
						%par3 = %obj.eventOutputParameter[%i,3];
						%par4 = %obj.eventOutputParameter[%i,4];
						%outputEventIdx = %obj.eventOutputIdx[%i];
						if (%obj.eventTarget[%i] == -1.0)
						{
							%name = %obj.eventNT[%i];
							%group = %obj.getGroup();
							%j = 0;
							while(%j < %group.NTObjectCount[%name])
							{
								%target = %group.NTObject[%name,%j];
								if (!isObject(%target))
								{
								}
								else
								{
									%targetClass = "fxDTSBrick";
									%numParameters = outputEvent_GetNumParametersFromIdx(%targetClass, %outputEventIdx);
									if (%numParameters >= 0 && %numParameter <= 4)
									{
										%scheduleID = %target.schedule(%delay,"VCECallEvent", %outputEvent, %obj, %client,%client.player,%obj.vehicle,%obj.hbot,getMinigameFromObject(%obj), %obj.eventOutputAppendClient[%i],%i, %par1, %par2, %par3, %par4);
									}
									else
									{
										error("ERROR: SimObject::ProcessInputEvent() - bad number of parameters on event \'" @ %outputEvent @ "\' (" @ numParameters @ ")");
									}
									if (%delay > 0.0)
									{
										%obj.addScheduledEvent(%scheduleID);
									}
								}
								%j = %j + 1.0;
							}
						}
						else
						{
							%target = $InputTarget_[%obj.eventTarget[%i]];
							if (!isObject(%target))
							{
							}
							else
							{
								%targetClass = inputEvent_GetTargetClass("fxDTSBrick", %obj.eventInputIdx[%i], %obj.eventTargetIdx[%i]);
								%numParameters = outputEvent_GetNumParametersFromIdx(%targetClass, %outputEventIdx);
								if (%numParameters >= 0 && %numParameter <= 4)
								{
									%scheduleID = %target.schedule(%delay,"VCECallEvent", %outputEvent, %obj, %client,%client.player,%obj.vehicle,%obj.hbot,getMinigameFromObject(%obj), %obj.eventOutputAppendClient[%i],%i, %par1, %par2, %par3, %par4);
								}
								else
								{
									error("ERROR: SimObject::ProcessInputEvent() - bad number of parameters on event \'" @ %outputEvent @ "\' (" @ numParameters @ ")");
								}
								
								if (%delay > 0.0 && %EventName !$= "onToolBreak")
								{
									%obj.addScheduledEvent(%scheduleID);
								}
							}
						}
					}
				}
			}
			%i = %i + 1.0;
		}
	}

};


package VCE_FireRelayNumFix 
{
	function SimObject::ProcessFireRelay(%obj, %process, %client) 
	{
			
		// Onhly check for those events we are interested in
		%count = getWordCount(%process);
		for (%j = 0; %j < %count; %j++)
		{
			%i = getWord(%process, %j);
			
			// Already processed
			if (%tempEvent[%i])
				continue;

			// Enabled event
			if (!%obj.eventEnabled[%i])
				continue;
			
			// Not onRelay
			if (%obj.eventInput[%i] !$= "onRelay")
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
			
			// Append client
			if (%obj.eventOutputAppendClient[%i] && isObject(%client))
			{
				%p[%n] = %client;
				%numParams++;
			}
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
};

