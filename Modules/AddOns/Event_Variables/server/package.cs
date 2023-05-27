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
//MIM between proccessing and actual event calling
function SimObject::VCECallEvent(%obj, %outputEvent, %brick, %client,%player,%vehicle,%bot,%minigame, %passClient,%targetClass, %par1, %par2, %par3, %par4)
{
	%classname = %obj.getClassName();

	%parameterWords = verifyOutputParameterList(%targetClass, outputEvent_GetOutputEventIdx(%targetClass, %outputEvent));
	%parameterWordCount = getWordCount(%parameterWords);
	%c = 1;

	//filter all string parameters
	for(%i = 0; %i < %parameterWordCount; %i++)
	{
		%word = getWord(%parameterWords, %i);

		
		
		if(%word $= "string")
			%par[%c] = %brick.filterVCEString(%par[%c],%client,%player,%vehicle,%bot,%minigame);
		
		if($VCEisEventParameterType[%word])
		{
			
			%c++;
		}	

	}

	%parCount = %c - 1;

	%vargroup = %brick.getGroup().vargroup;

	//there's some special vce functions we want to call within this scope so they have access to needed references
	if(%outPutEvent $= "VCE_modVariable")
	{
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

		%newValue = doVCEVarFunction(%logic, %oldValue, %newValue);

		%vargroup.setVariable(%varName,%newValue,%obj);

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
				%obj.call(%outputEvent,%client);
		if(%parCount == 1)
				%obj.call(%outputEvent,%par1,%client);
		if(%parCount == 2)
				%obj.call(%outputEvent,%par1,%par2,%client);
		if(%parCount == 3)
				%obj.call(%outputEvent,%par1,%par2,%par3,%client);
		if(%parCount == 4)
				%obj.call(%outputEvent,%par1,%par2,%par3,%par4,%client);	
	}
	else
	{
		if(%parCount == 0)
			%obj.call(%outputEvent);
		if(%parCount == 1)
			%obj.call(%outputEvent,%par1);
		if(%parCount == 2)
			%obj.call(%outputEvent,%par1,%par2);
		if(%parCount == 3)
			%obj.call(%outputEvent,%par1,%par2,%par3);
		if(%parCount == 4)
			%obj.call(%outputEvent,%par1,%par2,%par3,%par4);	
	}
}
package VCE_Main
{
	function fxDtsBrick::onPlant(%brick)
	{
		VCE_createVariableGroup(%brick);
		
		return Parent::onPlant(%brick);
	}
	function fxDtsBrick::onLoadPlant(%brick)
	{
		VCE_createVariableGroup(%brick);
		
		return Parent::onLoadPlant(%brick);
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
		%outputName = $OutputEvent_Name[%brick.getClassName(), %outputEventIdx];
		%inputName = $InputEvent_Name[%brick.getClassName(), %inputEventIdx];
		%i = mFloor (%brick.numEvents);
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
										%scheduleID = %target.schedule(%delay,"VCECallEvent", %outputEvent, %obj, %client,%client.player,%obj.vehicle,%obj.hbot,getMinigameFromObject(%obj), %obj.eventOutputAppendClient[%i],%targetClass, %par1, %par2, %par3, %par4);
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
									%scheduleID = %target.schedule(%delay,"VCECallEvent", %outputEvent, %obj, %client,%client.player,%obj.vehicle,%obj.hbot,getMinigameFromObject(%obj), %obj.eventOutputAppendClient[%i],%targetClass, %par1, %par2, %par3, %par4);
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
					%event = %next.schedule(%eventDelay,"VCECallEvent",%eventOutput, %obj, %client,%client.player,%obj.vehicle,%obj.hbot,getMinigameFromObject(%obj), %obj.eventOutputAppendClient[%i],%type, %p1, %p2, %p3, %p4);
				
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

