//---
//	@package VCE
//	@title Package
//	@author Zack0Wack0/www.zack0wack0.com
//	@auther Monoblaster/46426
//	@time 5:30 PM 16/04/2011
//---
package VCE_Main
{
	//packages for figuring out special variable examples
	function GameConnection::OnAdd(%client)
	{
		$VCE::Server::SpecialVariableObject[%client,GLOBAL] = "GLOBAL";
		$VCE::Server::SpecialVariableObject[%client,CLIENT] = %client;
	}
	function Armor::onAdd(%this, %obj)
	{
		if(isObject(%obj.client) && %obj.client.getClassName() $= "GameConnection")
			$VCE::Server::SpecialVariableObject[%obj.client,PLAYER] = %obj;
		else
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

		%brick.VCE_Dirty = true;
		
		//startfunction setup
		if(%outputName $= "VCE_StartFunction"){
			%brick.VCE_startFunction(%par1,%par2,%par3,%client);
		}	
		//loop checking
		if((%inputName $= "onVariableTrue" || %inputName $= "onVariableFalse") && (%outputName $= "VCE_ifVariable" || %outputName $= "VCE_ifValue") || (%inputName $= "onVariableUpdate" && (%outputName $= "VCE_ifValue" || %outputName $= "VCE_ifVariable")) || (%inputName $= "onVariableUpdate" && %outputName $= "VCE_modVariable")){
			if (%delay < $Pref::VCE::LoopDelay)
				%delay = $Pref::VCE::LoopDelay;
		}

		//OVERWRITE to fix strmlcontrolchars
		%clientIsAdmin = 1;
		if (isObject (%client))
		{
			%clientIsAdmin = %client.isAdmin;
		}
		else 
		{
			%client.isAdmin = 1;
		}
		if ($Server::WrenchEventsAdminOnly == 1)
		{
			if (!%clientIsAdmin)
			{
				return;
			}
		}
		%brick = %client.wrenchBrick;
		if (!isObject (%brick))
		{
			messageClient (%client, '', 'Wrench Error - AddEvent: Brick no longer exists!');
			return;
		}
		if (getTrustLevel (%client, %brick) < $TrustLevel::WrenchEvents && %brick != $LastLoadedBrick)
		{
			%client.sendTrustFailureMessage (%brick.getGroup ());
			return;
		}
		if (%brick.numEvents >= $Game::MaxEventsPerBrick)
		{
			return;
		}
		%brickClass = %brick.getClassName ();
		%inputEventName = $InputEvent_Name[%brickClass, %inputEventIdx];
		if (%inputEventName $= "onPlayerEnterZone" || %inputEventName $= "onPlayerleaveZone" || %inputEventName $= "onInZone")
		{
			if (!%clientIsAdmin)
			{
				messageClient (%client, '', "The event \'" @ %inputEventName @ "\' is admin only!");
				return;
			}
		}
		if ($InputEvent_AdminOnly[%brickClass, %inputEventIdx])
		{
			if (!%clientIsAdmin)
			{
				messageClient (%client, '', "The event \'" @ %inputEventName @ "\' is admin only!");
				return;
			}
		}
		%enabled = mClamp (mFloor (%enabled), 0, 1);
		%delay = mClamp (%delay, 0, 30000);
		if (%inputEventIdx == -1)
		{
			%i = mFloor (%brick.numEvents);
			%brick.eventEnabled[%i] = %enabled;
			%brick.eventDelay[%i] = %delay;
			%brick.eventInputIdx[%i] = %inputEventIdx;
			%brick.numEvents += 1;
			return;
		}
		%inputEventIdx = mClamp (%inputEventIdx, 0, $InputEvent_Count[%brickClass]);
		%targetIdx = mClamp (%targetIdx, -1, getFieldCount ($InputEvent_TargetList[%brickClass, %inputEventIdx]));
		%NTNameIdx = mClamp (%NTNameIdx, 0, %brick.getGroup ().NTNameCount - 1);
		if (%targetIdx == -1)
		{
			%targetClass = "fxDTSBrick";
		}
		else 
		{
			%targetClass = getWord (getField ($InputEvent_TargetList[%brickClass, %inputEventIdx], %targetIdx), 1);
		}
		if (%targetClass $= "")
		{
			error ("ERROR: serverCmdAddEvent() - invalid target class.  %inputEventIdx:" @ %inputEventIdx @ " %targetIdx:" @ %targetIdx);
			return;
		}
		%outputEventIdx = mClamp (%outputEventIdx, 0, $OutputEvent_Count[%targetClass]);
		%verifiedPar[1] = "";
		%verifiedPar[2] = "";
		%verifiedPar[3] = "";
		%verifiedPar[4] = "";
		%parameterCount = getFieldCount ($OutputEvent_parameterList[%targetClass, %outputEventIdx]);
		%i = 1;
		while (%i < %parameterCount + 1)
		{
			%field = getField ($OutputEvent_parameterList[%targetClass, %outputEventIdx], %i - 1);
			%type = getWord (%field, 0);
			if (%type $= "int")
			{
				%min = mFloor (getWord (%field, 1));
				%max = mFloor (getWord (%field, 2));
				%default = mFloor (getWord (%field, 3));
				%val = %par[%i];
				if (%val $= "")
				{
					%val = %default;
				}
				%verifiedPar[%i] = mClamp (%val, %min, %max);
			}
			else if (%type $= "intList")
			{
				%wordCount = getWordCount (%par[%i]);
				if (%par[%i] $= "ALL")
				{
					%verifiedPar[%i] = "ALL";
				}
				else 
				{
					%verifiedPar[%i] = "";
					%w = 0;
					while (%w < %wordCount)
					{
						%word = atoi (getWord (%par[%i], %w));
						if (%w == 0)
						{
							%verifiedPar[%i] = %word;
						}
						else 
						{
							%verifiedPar[%i] = %verifiedPar[%i] SPC %word;
						}
						%w += 1;
					}
				}
			}
			else if (%type $= "float")
			{
				%min = atof (getWord (%field, 1));
				%max = atof (getWord (%field, 2));
				%step = mAbs (getWord (%field, 3));
				%default = atof (getWord (%field, 4));
				%val = %par[%i];
				if (%val $= "")
				{
					%val = %default;
				}
				%val = mClampF (%val, %min, %max);
				%numSteps = mFloor ((%val - %min) / %step);
				%val = %min + (%numSteps * %step);
				%verifiedPar[%i] = %val;
			}
			else if (%type $= "bool")
			{
				if (%par[%i])
				{
					%verifiedPar[%i] = 1;
				}
				else 
				{
					%verifiedPar[%i] = 0;
				}
			}
			else if (%type $= "string")
			{
				%maxLength = mFloor (getWord (%field, 1));
				%width = mFloor (getWord (%field, 2));
				%par[%i] = strreplace (%par[%i], "<font:", "&A01");
				%par[%i] = strreplace (%par[%i], "<color:", "&A02");
				%par[%i] = strreplace (%par[%i], "<bitmap:", "&A03");
				%par[%i] = strreplace (%par[%i], "<shadow:", "&A04");
				%par[%i] = strreplace (%par[%i], "<shadowcolor:", "&A05");
				%par[%i] = strreplace (%par[%i], "<linkcolor:", "&A06");
				%par[%i] = strreplace (%par[%i], "<linkcolorHL:", "&A07");
				%par[%i] = strreplace (%par[%i], "<a:", "&A08");
				%par[%i] = strreplace (%par[%i], "</a>", "&A09");
				%par[%i] = strreplace (%par[%i], "<br>", "&A10");

				%par[%i] = strreplace (%par[%i], "<just:", ""); // replacement for stripping tml start
				%par[%i] = strreplace (%par[%i], "<clip:", "");
				%par[%i] = strreplace (%par[%i], "<lmargin%:", "");
				%par[%i] = strreplace (%par[%i], "<rmargin%:", "");
				%par[%i] = strreplace (%par[%i], "<lmargin:", "");
				%par[%i] = strreplace (%par[%i], "<rmargin:", "");
				%par[%i] = strreplace (%par[%i], "<tab:", "");
				%par[%i] = strreplace (%par[%i], "<spop>", "");
				%par[%i] = strreplace (%par[%i], "<spush>", "");
				%par[%i] = strreplace (%par[%i], "<sbreak>", "");
				%par[%i] = strreplace (%par[%i], "<div:", "");
				%par[%i] = strreplace (%par[%i], "<tag:", ""); //replacement for stripping tml end
				
				%par[%i] = strreplace (%par[%i], "&A01", "<font:");
				%par[%i] = strreplace (%par[%i], "&A02", "<color:");
				%par[%i] = strreplace (%par[%i], "&A03", "<bitmap:");
				%par[%i] = strreplace (%par[%i], "&A04", "<shadow:");
				%par[%i] = strreplace (%par[%i], "&A05", "<shadowcolor:");
				%par[%i] = strreplace (%par[%i], "&A06", "<linkcolor:");
				%par[%i] = strreplace (%par[%i], "&A07", "<linkcolorHL:");
				%par[%i] = strreplace (%par[%i], "&A08", "<a:");
				%par[%i] = strreplace (%par[%i], "&A09", "</a>");
				%par[%i] = strreplace (%par[%i], "&A10", "<br>");
				%verifiedPar[%i] = getSubStr (%par[%i], 0, %maxLength);
				%verifiedPar[%i] = chatWhiteListFilter (%verifiedPar[%i]);
				%verifiedPar[%i] = strreplace (%verifiedPar[%i], ";", "");
			}
			else if (%type $= "datablock")
			{
				%dbClassName = getWord (%field, 1);
				if (isObject (%par[%i]))
				{
					%newDB = %par[%i].getId ();
				}
				else if (%par[%i] $= "NONE" || %par[%i] $= -1)
				{
					%newDB = -1;
				}
				else 
				{
					if (%dbClassName $= "FxLightData")
					{
						%newDB = "PlayerLight";
					}
					else if (%dbClassName $= "ItemData")
					{
						%newDB = "hammerItem";
					}
					else if (%dbClassName $= "ProjectileData")
					{
						if (isObject (gunProjectile))
						{
							%newDB = "gunProjectile";
						}
						else 
						{
							%newDB = "deathProjectile";
						}
					}
					else if (%dbClassName $= "ParticleEmitterData")
					{
						%newDB = "PlayerFoamEmitter";
					}
					else if (%dbClassName $= "Music")
					{
						%newDB = "musicData_After_School_Special";
					}
					else if (%dbClassName $= "Sound")
					{
						%newDB = "lightOnSound";
					}
					else if (%dbClassName $= "Vehicle")
					{
						%newDB = "JeepVehicle";
					}
					else if (%dbClassName $= "PlayerData")
					{
						%newDB = "PlayerStandardArmor";
					}
					else 
					{
						%newDB = -1;
					}
					if (isObject (%newDB))
					{
						%newDB = %newDB.getId ();
					}
					else 
					{
						%newDB = -1;
					}
				}
				if (!isObject (%newDB))
				{
					%newDB = -1;
				}
				if (%newDB != -1)
				{
					if (%dbClassName $= "Music")
					{
						if (%newDB.getClassName () !$= "AudioProfile")
						{
							return;
						}
						if (%newDB.uiName $= "")
						{
							return;
						}
					}
					else if (%dbClassName $= "Sound")
					{
						if (%newDB.getClassName () !$= "AudioProfile")
						{
							return;
						}
						if (%newDB.uiName !$= "")
						{
							return;
						}
						if (%newDB.getDescription ().isLooping == 1)
						{
							return;
						}
						if (!%newDB.getDescription ().is3D)
						{
							return;
						}
					}
					else if (%dbClassName $= "Vehicle")
					{
						%dbClass = %newDB.getClassName ();
						if (%newDB.uiName $= "")
						{
							return;
						}
						if (%dbClass !$= "WheeledVehicleData" && %dbClass !$= "HoverVehicleData" && %dbClass !$= "FlyingVehicleData" && !(%dbClass $= "PlayerData" && %newDB.rideAble))
						{
							return;
						}
					}
					else 
					{
						if (%newDB.getClassName () !$= %dbClassName)
						{
							return;
						}
						if (%newDB.uiName $= "")
						{
							return;
						}
					}
				}
				%verifiedPar[%i] = %newDB;
			}
			else if (%type $= "vector")
			{
				%x = atof (getWord (%par[%i], 0));
				%y = atof (getWord (%par[%i], 1));
				%z = atof (getWord (%par[%i], 2));
				%mag = atoi (getWord (%field, 1));
				if (%mag == 0)
				{
					%mag = 200;
				}
				%vec = %x SPC %y SPC %z;
				if (VectorLen (%vec) > %mag)
				{
					%vec = VectorNormalize (%vec);
					%vec = VectorScale (%vec, %mag);
					%x = atoi (getWord (%vec, 0));
					%y = atoi (getWord (%vec, 1));
					%z = atoi (getWord (%vec, 2));
				}
				%verifiedPar[%i] = %x SPC %y SPC %z;
			}
			else if (%type $= "list")
			{
				%val = mFloor (%par[%i]);
				%itemCount = (getWordCount (%field) - 1) / 2;
				%foundMatch = 0;
				%j = 0;
				while (%j < %itemCount)
				{
					%idx = (%j * 2) + 1;
					%name = getWord (%field, %idx);
					%id = getWord (%field, %idx + 1);
					if (%val == %id)
					{
						%foundMatch = 1;
						break;
					}
					%j += 1;
				}
				if (!%foundMatch)
				{
					return;
				}
				%verifiedPar[%i] = %val;
			}
			else if (%type $= "paintColor")
			{
				%color = %par[%i];
				if (%client == $LoadingBricks_Client && $LoadingBricks_ColorMethod == 3)
				{
					%color = $colorTranslation[%color];
				}
				%verifiedPar[%i] = mClamp (%color, 0, $maxSprayColors);
			}
			else 
			{
				error ("ERROR: serverCmdAddEvent() - default type validation for type \"" @ %type @ "\"");
				%verifiedPar[%i] = strreplace (%par[%i], ";", "");
			}
			%i += 1;
		}
		%i = mFloor (%brick.numEvents);
		%brick.eventInputIdx[%i] = %inputEventIdx;
		%brick.eventTargetIdx[%i] = %targetIdx;
		%brick.eventOutputIdx[%i] = %outputEventIdx;
		%brick.eventEnabled[%i] = %enabled;
		%brick.eventInput[%i] = $InputEvent_Name[%brickClass, %inputEventIdx];
		%brick.eventDelay[%i] = %delay;
		if (%targetIdx == -1)
		{
			%targetClass = "FxDTSBrick";
			%brick.eventTarget[%i] = -1;
			%brick.eventNT[%i] = %brick.getGroup ().NTName[%NTNameIdx];
		}
		else 
		{
			%brick.eventTarget[%i] = getWord (getField ($InputEvent_TargetList[%brickClass, %inputEventIdx], %targetIdx), 0);
			%brick.eventNT[%i] = "";
		}
		%brick.eventOutput[%i] = $OutputEvent_Name[%targetClass, %outputEventIdx];
		%brick.eventOutputParameter[%i, 1] = %verifiedPar[1];
		%brick.eventOutputParameter[%i, 2] = %verifiedPar[2];
		%brick.eventOutputParameter[%i, 3] = %verifiedPar[3];
		%brick.eventOutputParameter[%i, 4] = %verifiedPar[4];
		if (%brick.eventOutput[%i] $= "FireRelay")
		{
			if (%brick.eventDelay[%i] < 33)
			{
				%brick.eventDelay[%i] = 33;
			}
		}
		%brick.eventOutputAppendClient[%i] = $OutputEvent_AppendClient[%targetClass, %outputEventIdx];
		%brick.numEvents += 1;
		if (!%brick.implicitCancelEvents)
		{
			%obj = %brick;
			%i = 0;
			while (%i < %obj.numEvents)
			{
				if (%obj.eventInput[%i] !$= "OnRelay")
				{
					
				}
				else if (%obj.eventTarget[%i] == -1)
				{
					
				}
				else if (%obj.eventTarget[%i] !$= "Self")
				{
					
				}
				else 
				{
					%outputEvent = %obj.eventOutput[%i];
					if (%outputEvent !$= "fireRelay")
					{
						
					}
					else 
					{
						%obj.implicitCancelEvents = 1;
						break;
					}
				}
				%i += 1;
			}
		}
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

