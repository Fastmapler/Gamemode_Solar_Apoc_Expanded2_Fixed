/////////////////////////////////////////////////////////////
//             Support_AutoSaver - Core [Saving]           //
/////////////////////////////////////////////////////////////

function Autosaver_Schedule(%doReset)
{
	cancel($Server::EOTW_AS["Schedule"]);
	if($Pref::Server::EOTW_AS_["Interval"] < 1 || $Pref::Server::EOTW_AS_["Interval"] $= "")
		$Pref::Server::EOTW_AS_["Interval"] = 5;

	if(%doReset)
		$Server::EOTW_AS["TimeLeft"] = $Pref::Server::EOTW_AS_["Interval"] + 1;

	if(!$Pref::Server::EOTW_AS_["Enabled"])
	{
		$Server::EOTW_AS["Schedule"] = schedule(60 * 1000, 0, "Autosaver_Schedule");
		return;
	}

	$Server::EOTW_AS["TimeLeft"]--;
	//Autosaver_SetState("Schedule called with time left: " @ $Server::EOTW_AS["TimeLeft"]);
	if($Server::EOTW_AS["TimeLeft"] <= 0)
	{
		$Server::EOTW_AS["TimeLeft"] = $Pref::Server::EOTW_AS_["Interval"] + 1;
		Autosaver_Begin();
	}
	else
		$Server::EOTW_AS["Schedule"] = schedule(60 * 1000, 0, "Autosaver_Schedule");
}

function Autosaver_Begin(%name, %bl_id)
{
	%saveStartTag = '';
	%saveDoneTag = '';
	if($Pref::Server::EOTW_AS_["Sounds"])
	{
		%saveStartTag = 'MsgUploadStart';
		%saveDoneTag = 'MsgUploadEnd';
	}

	cancel($Server::EOTW_AS["Schedule"]);
	if(getBrickCount() <= 0)
	{
		//~Debug code goes here~
		if($Pref::Server::EOTW_AS_["Announce"])
			messageAll(%saveDoneTag, ($Pref::Server::EOTW_AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c6There are no bricks to autosave.");

		echo("[" @ getWord(getDateTime(), 1) @ "] [Autosaver]\n  - There are no bricks to autosave.");
		Autosaver_Schedule(1);
		return;
	}

	//Autosaver_SetState("Init");

	if(%bl_id !$= "")
		%bl_id = mFloor(%bl_id);

	%date = getDateTime();
	if(%name $= "")
	{
		%save_date = strReplace(getWord(%date, 0), "/", "-");
		%save_time = stripChars(getWord(%date, 1), ":");

		$Server::EOTW_AS["isCustomName"] = 0;
		%name = "Autosave - " @ %save_date @ " at " @ %save_time;
	}
	else //Used someday
		$Server::EOTW_AS["isCustomName"] = 1;

	$Server::EOTW_AS["SaveName"] = %name;
	//If this is more than 0, it will only save that BL_ID
	$Server::EOTW_AS["SaveBLID"] = %bl_id;

	if(isObject(%file = $Server::EOTW_AS["FileObject"]))
		%file.close();
	else
		$Server::EOTW_AS["FileObject"] = new FileObject();

	if(!isObject($Server::EOTW_AS["List"]))
		$Server::EOTW_AS["List"] = new GuiTextListCtrl("AutosaverList");
	else
		$Server::EOTW_AS["List"].clear();

	$Server::EOTW_AS["Cooling"] = 0;
	$Server::EOTW_AS["InUse"] = 1;
	$Server::EOTW_AS["Init"] = getRealTime();
	$Server::EOTW_AS["SaveWarn"] = 0;
	$Server::TempAS["NeatSaving"] = $Pref::Server::EOTW_AS_["NeatSaving"]; //If someone changes prefs in the middle of saving this should not break
	if($Pref::Server::EOTW_AS_["NeatSaveProtect"] && getBrickCount() > 75000) //After a certain amount of bricks the server will lag when new bricks are added to the list, blame torque for that
		$Server::TempAS["NeatSaving"] = 0;

	if($Pref::Server::EOTW_AS_["ShowProgress"])
		CenterPrintAll("<just:right>\c6Initiating Autosaver\n<font:arial bold:20>\c7...", 1);

	if($Pref::Server::EOTW_AS_["Announce"])
	{
		%autosaveMsg = ($Pref::Server::EOTW_AS_["SaveEvents"] ? ($Pref::Server::EOTW_AS_["SaveOwnership"] ? "\c3Events and ownership" : "\c3Events") : ($Pref::Server::EOTW_AS_["SaveOwnership"] ? "\c3Ownership" : ""));
		messageAll(%saveStartTag, ($Pref::Server::EOTW_AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c6" @ ($Pref::Server::EOTW_AS_["Enabled"] ? "Autosaving" : "Saving") @ " bricks... " @ (%autosaveMsg !$= "" ? "\c6(" @ %autosaveMsg @ "\c6)" : ""));
	}

	%autosaveMsgEcho = ($Pref::Server::EOTW_AS_["SaveEvents"] ? ($Pref::Server::EOTW_AS_["SaveOwnership"] ? "Events and ownership" : "Events") : ($Pref::Server::EOTW_AS_["SaveOwnership"] ? "Ownership" : ""));
	echo("[" @ getWord(getDateTime(), 1) @ "] [Autosaver]\n  - " @ ($Pref::Server::EOTW_AS_["Enabled"] ? "Autosaving" : "Saving") @ " bricks... " @ %autosaveMsgEcho);

	deleteVariables("$Server::EOTW_ASGroup*");
	deleteVariables("$Server::EOTW_ASBrickIdx*");

	$Server::EOTW_AS["Brickcount"] = 0;
	$Server::EOTW_AS["BricksSaved"] = 0;
	$Server::EOTW_ASGroupCount = 0;

	//Solar Apoc - Blacklist Public bricks and gatherable bricks to greatly reduce brix to save
	%blacklist = "888888 1337";
	for(%i = 0; %i < MainBrickGroup.getCount(); %i++)
	{
		%g = MainBrickGroup.getObject(%i);
		%b = %g.getCount();
		if(%b > 0 && !hasWord(%blacklist, %g.bl_id) && ((%bl_id >= 0 && %bl_id == %g.bl_id) || %bl_id $= ""))
		{
			$Server::EOTW_ASGroup[$Server::EOTW_ASGroupCount] = %g;
			$Server::EOTW_ASGroupCount++;
		}
	}

	//Rely on these global vars
	if(isObject(%group = $Server::EOTW_ASGroup[0]))
	{
		//Autosaver_SetState("Gathering groups");
		$Server::EOTW_ASGroup_Current = 0;
		Autosaver_InitGroups();
	}
	else
	{
		//Autosaver_SetState("Gathering groups failed");
		if($Pref::Server::EOTW_AS_["Announce"])
			messageAll(%saveDoneTag, ($Pref::Server::EOTW_AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c6There are no bricks to autosave.");

		echo("[" @ getWord(%date, 1) @ "] [Autosaver]\n  - There are no bricks to autosave.");
		Autosaver_Schedule(1);
		$Server::EOTW_AS["InUse"] = 0;
	}
}

function Autosaver_InitGroups()
{
	if(!$Server::EOTW_AS["InUse"])
		return;

	//Autosaver_SetState("Gathering groups (tick " @ $Server::EOTW_ASGroup_Current @ ")");
	if(!isObject($Server::EOTW_ASGroup[$Server::EOTW_ASGroup_Current]))
	{
		if($Pref::Server::EOTW_AS_["ShowProgress"])
		{
			if($Pref::Server::EOTW_AS_["TimeElapsed"])
			{
				%time = mCeil((getRealTime() - $Server::EOTW_AS["Init"]) / 1000);
				%timeStr = "\n\c6Time elapsed: \c3" @ getTimeString(%time) @ " ";
			}

			CenterPrintAll("<just:right>\c6Gathering groups...\n<font:arial bold:20>\c2Complete " @ %timeStr, 2);
		}

		//Autosaver_SetState("Gathering groups DONE");

		if($Server::TempAS["NeatSaving"])
			$Server::EOTW_AS["List"].sortNumerical(0, 1);
		
		Autosaver_SaveInit();

		return;
	}

	Autosave_GroupTick($Server::EOTW_ASGroup[$Server::EOTW_ASGroup_Current], 0);
}

function Autosave_GroupTick(%group, %count)
{
	if(!$Server::EOTW_AS["InUse"] || !isObject(%list = $Server::EOTW_AS["List"]) || !isObject(%group))
	{
		if($Pref::Server::EOTW_AS_["Announce"])
		{
			%saveErrorTag = '';
			if($Pref::Server::EOTW_AS_["Sounds"])
				%saveErrorTag = 'MsgClearBricks';

			%date = getDateTime();
			%diff = (getRealTime() - $Server::EOTW_AS["Init"]);
			%time = %diff / 1000;

			if(%time > 60)
			{
				%timeString = getTimeString(mFloor(%time));
				%TimeElapsed = ($Pref::Server::EOTW_AS_["TimeElapsed"] ? "Time elapsed: \c3" @ %time @ " minute" @ (%time != 1 ? "s" : "") : "");
				%TimeElapsedEcho = ($Pref::Server::EOTW_AS_["TimeElapsed"] ? "Time elapsed: " @ %time @ " minute" @ (%time != 1 ? "s" : ""): "");
			}
			else
			{
				%time = mFloatLength(%time, 2);
				if(%time < 1)
					%time = 0;

				%TimeElapsed = ($Pref::Server::EOTW_AS_["TimeElapsed"] ? "Time elapsed: \c3" @ %time @ " second" @ (%time != 1 ? "s" : "") : "");
				%TimeElapsedEcho = ($Pref::Server::EOTW_AS_["TimeElapsed"] ? "Time elapsed: " @ %time @ " second" @ (%time != 1 ? "s" : ""): "");
			}

			messageAll(%saveErrorTag, ($Pref::Server::EOTW_AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c6" @ ($Pref::Server::EOTW_AS_["Enabled"] ? "Autosave" : "Save") @ " failed. Missing resources. " @ %TimeElapsed);
			error("Autosave_GroupTick() - Missing resources");
		}

		if($Pref::Server::EOTW_AS_["ShowProgress"])
		{
			if($Pref::Server::EOTW_AS_["TimeElapsed"])
			{
				%time = mCeil((getRealTime() - $Server::EOTW_AS["Init"]) / 1000);
				%timeStr = "\n\c6Time elapsed: \c3" @ getTimeString(%time) @ " ";
			}

			CenterPrintAll("<just:right>\c6Gathering groups...\n<font:arial bold:20>\c0Failed " @ %timeStr, 1);
		}

		return;
	}

	%gc = %group.getCount();
	if(%count >= %gc)
	{
		if($Pref::Server::EOTW_AS_["ShowProgress"])
		{
			if($Pref::Server::EOTW_AS_["TimeElapsed"])
			{
				%time = mCeil((getRealTime() - $Server::EOTW_AS["Init"]) / 1000);
				%timeStr = "\n\c6Time elapsed: \c3" @ getTimeString(%time) @ " ";
			}

			CenterPrintAll("<just:right>\c6Gathering groups...\n<font:arial bold:20>\c2Complete " @ %timeStr, 1);
		}

		//Autosaver_SetState("Gathering group " @ %group.bl_id @ " DONE - Overloaded");
		$Server::EOTW_ASGroup_Current++;
		Autosaver_InitGroups();
		return;
	}

	if($Server::TempAS["NeatSaving"] && !$Server::EOTW_AS["SaveWarn"] && $Server::EOTW_AS["Brickcount"] > 75000)
	{
		$Server::EOTW_AS["SaveWarn"] = 1;
		messageAll('', ($Pref::Server::EOTW_AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c0Warning\c6: Many bricks detected, there may be lag.");
	}

	for(%i = %count; %i <= %count + $Pref::Server::EOTW_AS_["ChunkCount"]; %i++)
	{
		if(%i >= %gc)
		{
			if($Pref::Server::EOTW_AS_["ShowProgress"])
			{
				if($Pref::Server::EOTW_AS_["TimeElapsed"])
				{
					%time = mCeil((getRealTime() - $Server::EOTW_AS["Init"]) / 1000);
					%timeStr = "\n\c6Time elapsed: \c3" @ getTimeString(%time) @ " ";
				}

				CenterPrintAll("<just:right>\c6Gathering groups...\n<font:arial bold:20>\c2Complete " @ %timeStr, 1);
			}

			//Autosaver_SetState("Gathering group " @ %group.bl_id @ " DONE");
			$Server::EOTW_ASGroup_Current++;
			Autosaver_InitGroups();
			return;
		}

		%brick = nameToID(%group.getObject(%i)); //Just to be sure
		if(%brick.isPlanted)
		{
			if($Server::TempAS["NeatSaving"] && %list.getRowNumByID(%brick) == -1)
				%list.addRow(%brick, %brick.getDistanceFromGround());

			$Server::EOTW_ASBrickIdx[$Server::EOTW_AS["Brickcount"]] = %brick;
			$Server::EOTW_AS["Brickcount"]++;
		}
		else if($Pref::Server::EOTW_AS_["RemoveUnwantedTempbricks"] && isObject(%brick))
		{
			%del = 1;
			for(%i = 0; %i < ClientGroup.getCount(); %i++)
			{
				if(ClientGroup.getObject(%i).player.tempBrick == %brick)
					%del = 0;
			}
		
			if(%del) //This helps delete unwanted temp bricks that don't belong to anyone
				%brick.schedule(0, "delete");
		}
	}

	if($Pref::Server::EOTW_AS_["ShowProgress"])
	{
		if($Pref::Server::EOTW_AS_["TimeElapsed"])
		{
			%time = mCeil((getRealTime() - $Server::EOTW_AS["Init"]) / 1000);
			%timeStr = "\n\c6Time elapsed: \c3" @ getTimeString(%time) @ " ";
		}

		%progress = mFloatLength((($Server::EOTW_ASGroup_Current / $Server::EOTW_ASGroupCount)) * 100, 1);
		CenterPrintAll("<just:right>\c6Gathering groups... \n<font:arial bold:20>\c3" @ %progress @ "\c6% " @ %timeStr, 1);
	}

	schedule(33, 0, "Autosave_GroupTick", %group, %count + $Pref::Server::EOTW_AS_["ChunkCount"] + 1);
}

function Autosaver_SaveInit()
{
	//Autosaver_SetState("Save init");
	%dir = $Pref::Server::EOTW_AS_["Directory"];
	if(isObject($Server::EOTW_AS["TempB"]))
	{
		$Server::EOTW_AS["TempB"].close();
		$Server::EOTW_AS["TempB"].delete();
	}

	if(!$Server::EOTW_AS["InUse"])
		return;

	$Server::EOTW_AS["TempB"] = new FileObject(){path = %dir @ "SAVETEMP.bls";};
	$Server::EOTW_AS["TempB"].openForWrite($Server::EOTW_AS["TempB"].path);

	$Server::EOTW_AS["TempB"].writeLine("This is a Blockland save file.  You probably shouldn't modify it cause you'll mess it up.");
	$Server::EOTW_AS["TempB"].writeLine("1");
	$Server::EOTW_AS["TempB"].writeLine(%desc);

	for(%i = 0; %i < 64; %i++)
		$Server::EOTW_AS["TempB"].writeLine(getColorIDTable(%i));

	$Server::EOTW_AS["TempB"].writeLine("Linecount " @ $Server::EOTW_AS["Brickcount"]);

	$Server::EOTW_AS["BricksSaved"] = 0;
	$Server::EOTW_AS["Eventcount"] = 0;
	Autosaver_SaveTick($Server::EOTW_AS["TempB"], 0);
}

function Autosaver_SaveTick(%file, %count)
{
	if(!$Server::EOTW_AS["InUse"])
	{
		if(isObject(%file))
		{
			%file.close();
			%file.delete();
		}

		return 0;
	}

	%events = $Pref::Server::EOTW_AS_["SaveEvents"];
	%ownership = $Pref::Server::EOTW_AS_["SaveOwnership"];
	%rCount = $Server::EOTW_AS["Brickcount"];

	if($Pref::Server::EOTW_AS_["ShowProgress"])
	{
		if($Pref::Server::EOTW_AS_["TimeElapsed"])
		{
			%time = mCeil((getRealTime() - $Server::EOTW_AS["Init"]) / 1000);
			%timeStr = "\n\c6Time elapsed: \c3" @ getTimeString(%time) @ " ";
		}

		%progress = mFloatLength((%count / %rCount) * 100, 1);
		CenterPrintAll("<just:right>\c6Saving... \n<font:arial bold:20>\c3" @ %progress @ "\c6% " @ %timeStr, 1);
	}

	for(%i = %count; %i <= %count + $Pref::Server::EOTW_AS_["ChunkCount"]; %i++)
	{
		if(%i >= %rCount)
		{
			//Autosaver_SetState("Saved bricks: " @ %i);
			Autosaver_Save(%file);
			return;
		}

		%brick = ($Server::TempAS["NeatSaving"] ? $Server::EOTW_AS["List"].getRowID(%i) : $Server::EOTW_ASBrickIdx[%i]);
		if(isObject(%brick))
		{
			$Server::EOTW_AS["EventCount"] += %brick.saveToFile(%events, %ownership, %file);
			$Server::EOTW_AS["BricksSaved"]++;
		}
	}

	schedule($Pref::Server::EOTW_AS_["Tick"], 0, "Autosaver_SaveTick", %file, %count + $Pref::Server::EOTW_AS_["ChunkCount"] + 1);
}

function Autosaver_Save(%file)
{
	%date = getDateTime();
	%diff = (getRealTime() - $Server::EOTW_AS["Init"]);
	%time = %diff / 1000;

	if(%time > 60)
	{
		%timeString = getTimeString(mFloor(%time));
		%TimeElapsed = ($Pref::Server::EOTW_AS_["TimeElapsed"] ? "Time elapsed: \c3" @ %time @ " minute" @ (%time != 1 ? "s" : "") : "");
		%TimeElapsedEcho = ($Pref::Server::EOTW_AS_["TimeElapsed"] ? "Time elapsed: " @ %time @ " minute" @ (%time != 1 ? "s" : ""): "");

		%msg = "in \c6"@ %timeString @" \c6minute" @ (%time != 1 ? "s" : "");
	}
	else
	{
		%time = mFloatLength(%time, 2);
		if(%time < 1)
			%time = 0;

		%TimeElapsed = ($Pref::Server::EOTW_AS_["TimeElapsed"] ? "Time elapsed: \c3" @ %time @ " second" @ (%time != 1 ? "s" : "") : "");
		%TimeElapsedEcho = ($Pref::Server::EOTW_AS_["TimeElapsed"] ? "Time elapsed: " @ %time @ " second" @ (%time != 1 ? "s" : ""): "");

		%msg = "in \c6"@ %time @" \c6second" @ (%time != 1 ? "s" : "");
	}

	if(%time < 1)
		%msg = "\c6instantly";

	%saveStartTag = '';
	%saveDoneTag = '';
	%saveErrorTag = '';
	if($Pref::Server::EOTW_AS_["Sounds"])
	{
		%saveStartTag = 'MsgUploadStart';
		%saveDoneTag = 'MsgUploadEnd';
		%saveErrorTag = 'MsgClearBricks';
	}

	//Autosaver_SetState("Polishing save");
	if(!isObject(%file))
	{
		$Server::EOTW_AS["InUse"] = 0;
		//Autosaver_SetState("Polishing save FAILED");
		error("Autosaver_Save() - Invalid file object");

		if($Pref::Server::EOTW_AS_["Announce"])
			messageAll(%saveErrorTag, ($Pref::Server::EOTW_AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c6" @ ($Pref::Server::EOTW_AS_["Enabled"] ? "Autosave" : "Save") @ " failed. Missing file object to save bricks. " @ %TimeElapsed);

		if($Pref::Server::EOTW_AS_["ShowProgress"])
			CenterPrintAll("<just:right>\c6Saving... \n<font:arial bold:20>\c0Failed ", 1);

		Autosaver_Schedule(1);
		return;
	}

	%file.close();
	%dir = $Pref::Server::EOTW_AS_["Directory"];

	if(!$Pref::Server::EOTW_AS_["OverwriteOnChange"])
		$Server::EOTW_AS["BrickChanged"] = 0;

	$Server::EOTW_AS["RelatedBrickCount"] = 0;
	if($Autosaver::Pref["LastBrickCount"] == $Server::EOTW_AS["BricksSaved"] && $Autosaver::Pref["LastEventCount"] == $Server::EOTW_AS["EventCount"] && $Server::EOTW_AS["BrickChanged"] == 0)
		$Server::EOTW_AS["RelatedBrickCount"] = 1;
	else
	{
		$Autosaver::Pref["LastEventCount"] = $Server::EOTW_AS["EventCount"];
		$Autosaver::Pref["LastBrickCount"] = $Server::EOTW_AS["BricksSaved"];
	}

	$Server::EOTW_AS["BrickChanged"] = 0;
	if((!$Server::EOTW_AS["RelatedBrickCount"] && !$Pref::Server::EOTW_AS_["SaveRelatedBrickcount"]) || $Pref::Server::EOTW_AS_["SaveRelatedBrickcount"])
	{
		if($Server::EOTW_AS["SaveName"] !$= "")
			%direc = %dir @ $Server::EOTW_AS["SaveName"] @ ".bls";
		else
		{
			%saveError = 1;
			%direc = %dir @ "Autosave.bls";
		}

		if(isFile(%direc)) //Overwrite it, just delete it for sure
			fileDelete(%direc);

		if(isFile(%file.path))
		{
			//This is a hack to update the resource files. fileCopy does not seem to update it properly; not sure why.
			//If fileCopy causes any other issue I'll just make another function to move files.
			%fTemp = new FileObject();
			%fTemp.openForWrite(%direc);
			%fTemp.writeLine("This is a temp file. If you see this, report this immediately.");
			%fTemp.close();
			%fTemp.delete();

			fileCopy(%file.path, %direc);
		}
	}

	if(isFile(%file.path))
		fileDelete(%file.path);
	%file.delete();

	if((!$Server::EOTW_AS["RelatedBrickCount"] && !$Pref::Server::EOTW_AS_["SaveRelatedBrickcount"]) || $Pref::Server::EOTW_AS_["SaveRelatedBrickcount"])
	{
		$Autosaver::Pref["LastAutoSave"] = fileBase(%direc);

		if($Pref::Server::EOTW_AS["OverwriteSave"] && isFile(%file.path))
			fileCopy(%file.path, "base/server/temp/temp.bls");
	}

	export("$Autosaver::Pref*", "config/server/AutosavePrefs.cs");

	if($Pref::Server::EOTW_AS_["ShowProgress"])
		CenterPrintAll("<just:right>\c6Saving... \n<font:arial bold:20>\c2Complete ", 1);

	%bGroups = $Server::EOTW_ASGroupCount;
	if($Pref::Server::EOTW_AS_["Announce"])
	{
		if($Server::EOTW_AS["RelatedBrickCount"] && !$Pref::Server::EOTW_AS_["SaveRelatedBrickcount"])
			messageAll(%saveDoneTag, ($Pref::Server::EOTW_AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c6Canceled " @ ($Pref::Server::EOTW_AS_["Enabled"] ? "autosave" : "save") @ " due to lack of changes. " @ %TimeElapsed);
		else
		{
			if($Pref::Server::EOTW_AS_["AnnounceSaveName"])
				%saveMsg = " Saved as \c3" @ $Autosaver::Pref["LastAutoSave"] @ "\c6.";

			if($Pref::Server::EOTW_AS_["Report"])
				%reportMsg = " \c6Saved \c3" @ $Server::EOTW_AS["EventCount"] @ " event" @ ($Server::EOTW_AS["EventCount"] == 1 ? "" : "s") @
					" \c6and \c3" @ %bGroups @ " group" @ (%bGroups == 1 ? "" : "s") @ "\c6.";

			messageAll(%saveDoneTag, ($Pref::Server::EOTW_AS_["TimeStamp"] ? "\c6[\c3" @ getWord(getDateTime(), 1) @ "\c6] " : "") @ "\c6[\c0!\c6] \c6" @ ($Pref::Server::EOTW_AS_["Enabled"] ? "Autosaved" : "Saved") @ " \c6" @ $Server::EOTW_AS["BricksSaved"] @" \c6brick" @ ($Server::EOTW_AS["BricksSaved"] == 1 ? "" : "s")
				@ " " @ %msg @ "\c6." @ %saveMsg @ %reportMsg);
		}
	}

	if($Server::EOTW_AS["RelatedBrickCount"] && !$Pref::Server::EOTW_AS_["SaveRelatedBrickcount"])
		echo("[" @ getWord(%date, 1) @ "] [Autosaver]\n  - Canceled " @ ($Pref::Server::EOTW_AS_["Enabled"] ? "autosave" : "save") @ " due to lack of changes. " @ %TimeElapsedEcho);
	else
		echo("[" @ getWord(%date, 1) @ "] [Autosaver]\n  - " @ ($Pref::Server::EOTW_AS_["Enabled"] ? "Autosaved" : "Saved") @ " " @ $Server::EOTW_AS["BricksSaved"] @ " bricks " @ %msg @ ". Saved as " @ $Autosaver::Pref["LastAutoSave"] @ ".");

	if(!$Server::EOTW_AS["RelatedBrickCount"])
		echo("  - Saved " @ $Server::EOTW_AS["EventCount"] @ " event" @ ($Server::EOTW_AS["EventCount"] == 1 ? "" : "s") @
			" and " @ %bGroups @ " group" @ (%bGroups == 1 ? "" : "s") @ ".");

	//Autosaver_SetState("Polishing save DONE, clearing list");
	Autosaver_ClearList();
	Autosaver_RemoveFiles();

	//Solar Apoc - Save all Solar Apoc related data
	EOTW_SaveData();
}

function Autosaver_ClearList()
{
	cancel($Server::EOTW_AS::ClearSch);
	if(isObject(%brickList = $Server::EOTW_AS["List"]))
	{
		$Server::EOTW_AS["InUse"] = 1;
		$Server::EOTW_AS["Cooling"] = 1;
		for(%i = 0; %i < $Pref::Server::EOTW_AS_["ChunkCount"]; %i++)
		{
			if(%brickList.rowCount() == 0)
			{
				%brickList.delete();
				$Server::EOTW_AS::ClearSch = schedule(1, 0, "Autosaver_ClearList");
				return;
			}
			else
				%brickList.removeRow(0);
		}
	}
	else
	{
		$Server::EOTW_AS["InUse"] = 0;

		//Autosaver_SetState("Clearing list DONE, scheduling autosaver");
		//If you manually call the save function (console), then this will be overwritten
		Autosaver_Schedule(1);
		$Server::EOTW_AS["Cooling"] = 0;
		return;
	}

	$Server::EOTW_AS::ClearSch = schedule(1, 0, "Autosaver_ClearList");
}

if(isFile("config/server/AutosavePrefs.cs"))
	exec("config/server/AutosavePrefs.cs");

$AutosaverSch = schedule(3000, 0, Autosaver_Schedule, 1);

///

function Autosaver_RemoveFiles()
{
	//Autosaver_SetState("Beginning to remove autosaves");
	%max = $Pref::Server::EOTW_AS_["MaxSaves"];
	if(%max <= 0 || %max $= "") //No limit
		return;

	if(isObject(Autosaver_DelList))
		Autosaver_DelList.delete();

	%list = new GuiTextListCtrl("Autosaver_DelList");

	%dir = $Pref::Server::EOTW_AS_["Directory"] @ "Autosave - *.bls";
	//setModPaths(getModPaths()); //For some reason files don't update
	%count = getFileCount(%dir);
	if(%count <= 1)
		return;

	%c = 0;
	for(%file = findFirstFile(%dir); %file !$= ""; %file = findNextFile(%dir))
	{
		%time = stripChars(getFileModifiedTime(%file), " :/-");
		%list.addRow(%c, %time TAB %file);
		%c++;
	}

	%list.sort(0, 1);
	if(%count > %max)
	{
		%savesToDelete = %count - %max;
		echo("[" @ getWord(getDateTime(), 1) @ "] [Autosaver] - Found " @ %savesToDelete @ " old autosave" @ (%savesToDelete != 1 ? "s" : "") @ " to delete (max capacity)");
		for(%i = 0; %i < %savesToDelete; %i++)
		{
			%file = getField(%list.getRowText(%i), 1);
			schedule(0, 0, "fileDelete", %file);
			//Autosaver_SetState("Removing file: " @ %file);
			echo("  - Deleting: " @ fileName(%file));
		}
	}

	//%list.delete();
}

/////////////////////////////////////////////////////////////
//            Core function to split autosaves             //
//   This function should ONLY be used for the autosaver   //
//  The default save system does not save all ownership!   //
/////////////////////////////////////////////////////////////

if(!isObject(Autosaver_ActiveSet))
	new ScriptGroup(Autosaver_ActiveSet);

//Splits all BL_IDs detected into multiple saves inside a folder
//  ~TODO~  \\
function Autosaver_Split(%fileName, %toFilePath)
{
	%toFilePath = filePath(%toFilePath);
}