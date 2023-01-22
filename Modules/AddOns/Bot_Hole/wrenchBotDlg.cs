//wrenchBotDlg.cs

function clientCmdOpenwrenchBotDlg(%id, %allowNamedTargets, %adminOverride, %adminOnlyEvents)
{
   ServerConnection.allowNamedTargets = %allowNamedTargets;
	wrenchBot_Window.setText("wrenchBot - " @ %id);
	canvas.pushDialog(wrenchBotDlg);

   wrenchBot_SendBlocker.setVisible(%adminOverride);    

   if($IamAdmin || serverConnection.isLocal())
      wrenchBot_EventsBlocker.setVisible(false);
   else
      wrenchBot_EventsBlocker.setVisible(%adminOnlyEvents);
}


//the server is sending us data about what's already set for this brick
function clientCmdSetwrenchBotData(%data)
{
	%fieldCount = getFieldCount(%data);

	for(%j = 0; %j < %fieldCount; %j++)
	{
		%field = getField(%data, %j);
		%type = getWord(%field, 0);

		switch$ (%type)
		{
			case "N":		//name
				%name = trim(getSubStr(%field, 2+1, strLen(%field) - 2)); //the + 1 is to remove the _ character

				if(!wrenchBotLock_Name.getValue())
					wrenchBot_Name.setText(%name);
				
				// if(!wrenchBotSoundLock_Name.getValue())
					// wrenchBotSound_Name.setText(%name);

				// if(!wrenchBotVehicleSpawnLock_Name.getValue())
					// wrenchBotVehicleSpawn_Name.setText(%name);

			case "LDB":		//light datablock
				if(!wrenchBotLock_Lights.getValue())
				{
					%db = getWord(%field, 1);
					wrenchBot_Lights.setSelected(%db);
				}
			case "EDB":		//emitter datablock
				if(!wrenchBotLock_Emitters.getValue())
				{
					%db = getWord(%field, 1);
					wrenchBot_Emitters.setSelected(%db);
				}
			case "EDIR":	//emitter direction
				if(!wrenchBotLock_EmitterDir.getValue())
				{
					%idx = getWord(%field, 1);
					%obj = "wrenchBot_EmitterDir" @ %idx;
					if(isObject(%obj))
					{
						for(%i = 0; %i < 6; %i++)
						{
							%clearObj = "wrenchBot_EmitterDir" @ %i;
							%clearObj.setValue(0);
						}
						%obj.setValue(1);
					}
					else
						error("ERROR: clientCmdSetwrenchBotData() - Bad emitter direction \"" @ %idx @ "\""); 
				}
			case "IDB":		//item datablock
				if(!wrenchBotLock_Items.getValue())
				{
					%db = getWord(%field, 1);
					wrenchBot_Items.setSelected(%db);
				}
			case "IPOS":	//item Position
				if(!wrenchBotLock_ItemPos.getValue())
				{
					%idx = getWord(%field, 1);
					%obj = "wrenchBot_ItemPos" @ %idx;
					if(isObject(%obj))
					{
						for(%i = 0; %i < 6; %i++)
						{
							%clearObj = "wrenchBot_ItemPos" @ %i;
							%clearObj.setValue(0);
						}
						%obj.setValue(1);
					}
					else
						error("ERROR: clientCmdSetwrenchBotData() - Bad item position \"" @ %idx @ "\""); 
				}
			case "IDIR":	//item direction
				if(!wrenchBotLock_ItemDir.getValue())
				{
					%idx = getWord(%field, 1);
					%obj = "wrenchBot_ItemDir" @ %idx;
					//echo("wrenchBot item direction obj = ", %obj);
					if(isObject(%obj))
					{
						for(%i = 2; %i < 6; %i++)
						{
							%clearObj = "wrenchBot_ItemDir" @ %i;
							%clearObj.setValue(0);
						}
						%obj.setValue(1);
					}
					else
						error("ERROR: clientCmdSetwrenchBotData() - Bad item direction \"" @ %idx @ "\""); 
				}
			case "IRT":		//item respawn time
				if(!wrenchBotLock_ItemRespawnTime.getValue())
				{
					wrenchBot_ItemRespawnTime.setText(getWord(%field, 1));
				}


			case "SDB":		//sound datablock
				// if(!wrenchBotSoundLock_Sounds.getValue())
				// {
					// %db = getWord(%field, 1);
					// wrenchBotSound_Sounds.setSelected(%db);
				// }
			case "VDB":		//vehicle datablock
				// if(!wrenchBotVehicleSpawnLock_Vehicles.getValue())
				// {
					// %db = getWord(%field, 1);
					// wrenchBotVehicleSpawn_Vehicles.setSelected(%db);
				// }
			case "RCV":		//re-color vehicle
				// if(!wrenchBotVehicleSpawnLock_ReColorVehicle.getValue())
				// {
					// %val = getWord(%field, 1);
					// wrenchBotVehicleSpawn_ReColorVehicle.setValue(%val);
				// }
         case "RC": //raycasting
            %val = getWord(%field, 1);
            if(!wrenchBotLock_RayCasting.getValue())
					wrenchBot_RayCasting.setValue(%val);
            // if(!wrenchBotVehicleSpawnLock_RayCasting.getValue())
					// wrenchBotVehicleSpawn_RayCasting.setValue(%val);

         case "C": //collision
            %val = getWord(%field, 1);
            if(!wrenchBotLock_Collision.getValue())
					wrenchBot_Collision.setValue(%val);
            // if(!wrenchBotVehicleSpawnLock_Collision.getValue())
					// wrenchBotVehicleSpawn_Collision.setValue(%val);

         case "R": //rendering
            %val = getWord(%field, 1);
            if(!wrenchBotLock_Rendering.getValue())
					wrenchBot_Rendering.setValue(%val);
            // if(!wrenchBotVehicleSpawnLock_Rendering.getValue())
					// wrenchBotVehicleSpawn_Rendering.setValue(%val);

			default:		//unknown
				error("ERROR: clientCmdSetwrenchBotData() - unknown field type \""@ %field @"\"");
		}
	}
}

//the server is telling us that it's done sending the wrenchBot data
function clientCmdwrenchBotLoadingDone()
{
	wrenchBot_LoadingWindow.setVisible(false);
}



//this is called once after we finish loading datablocks
function clientCmdwrenchBot_LoadMenus()
{
	wrenchBotDlg.loadDatablocks();
}

//populate the light datablock list
function wrenchBotDlg::loadDatablocks()
{
	//remember the old value we had selected so we can put it back after we re-populate
	%oldLight = wrenchBot_Lights.getText();
	%oldEmitter = wrenchBot_Emitters.getText();
	%oldItem = wrenchBot_Items.getText();
	// %oldSound = wrenchBotSound_Sounds.getText();


	wrenchBot_Lights.clear();
	wrenchBot_Emitters.clear();
	wrenchBot_Items.clear();
	// wrenchBotSound_Sounds.clear();	
	// wrenchBotVehicleSpawn_Vehicles.clear();

	//create a list of the categories and sub categories
	wrenchBot_Lights.add(" NONE", 0);
	wrenchBot_Emitters.add(" NONE", 0);
	wrenchBot_Items.add(" NONE", 0);
	// wrenchBotSound_Sounds.add(" NONE", 0);
	// wrenchBotVehicleSpawn_Vehicles.add(" NONE", 0);

	%dbCount = getDataBlockGroupSize();

	for(%i = 0; %i < %dbCount; %i++)
	{
		%db = getDataBlock(%i);
		%dbClass = %db.getClassName();
		
		if(%db.uiName !$= "")
		{
         //echo("db class = ", %dbclass);
			switch$(%dbClass)
			{
				case "FxLightData":
					wrenchBot_Lights.add(%db.uiName, %db);
				case "ParticleEmitterData":
					wrenchBot_Emitters.add(%db.uiName, %db);
				case "ItemData":
					wrenchBot_Items.add(%db.uiName, %db);
				// case "AudioProfile":
					// if(%db.getDescription().isLooping) //only allow looping "music" 
						// wrenchBotSound_Sounds.add(%db.uiName, %db);		

				// case "PlayerData":
					//echo("playerdata ", %db, " ", %db.uiname);
					// if(%db.uiName !$= "" && %db.rideable)
						// wrenchBotVehicleSpawn_Vehicles.add(%db.uiName, %db);
				// case "WheeledVehicleData":
               //echo("wheeled vehicle db = ", %db);
					// if(%db.uiName !$= "" && %db.rideable)
						// wrenchBotVehicleSpawn_Vehicles.add(%db.uiName, %db);
				// case "FlyingVehicleData":
					// if(%db.uiName !$= "" && %db.rideable)
						// wrenchBotVehicleSpawn_Vehicles.add(%db.uiName, %db);
				// case "HoverVehicleData":
					// if(%db.uiName !$= "" && %db.rideable)
						// wrenchBotVehicleSpawn_Vehicles.add(%db.uiName, %db);
			}
		}
	}

	//wrenchBot_Lights.sort();
	wrenchBot_Emitters.sort();
	wrenchBot_Items.sort();
	// wrenchBotSound_Sounds.sort();
   // wrenchBotVehicleSpawn_Vehicles.sort();

	//put old values back
	wrenchBot_Lights.setSelected(wrenchBot_Lights.findText(%oldLight));
	wrenchBot_Emitters.setSelected(wrenchBot_Emitters.findText(%oldEmitter));
	wrenchBot_Items.setSelected(wrenchBot_Items.findText(%oldItem));
	// wrenchBotSound_Sounds.setSelected(wrenchBotSound_Sounds.findText(%oldSound));

}



function wrenchBotDlg::onWake(%this)
{	
   //un bind the shift key
   if(!isObject(NoShiftMoveMap))
   {
      new ActionMap(NoShiftMoveMap);
      NoShiftMoveMap.bind("keyboard0", "lshift", "");
   }
   NoShiftMoveMap.push();

	//make the "loading" dialog visible
	wrenchBot_LoadingWindow.setVisible(true);
}
function wrenchBotDlg::onSleep(%this)
{
   //re-bind the shift key
   NoShiftMoveMap.pop();
}



function wrenchBotDlg::Send(%this)
{
	//send wrenchBot update to server (may take multiple packets)
	//try to only send the data that we have changed

	%data = "";

	//NAME
		%name = trim(wrenchBot_Name.getValue());
		
		if(%name !$= "")
			%data = %data TAB "N" SPC %name;
		else
			%data = %data TAB "N" SPC " ";
	
	//LIGHT
		%lightDB = wrenchBot_Lights.getSelected();
		%data = %data TAB "LDB" SPC %lightDB;

	//PARTICLE EMITTER
		%emitterDB = wrenchBot_Emitters.getSelected();
		%data = %data TAB "EDB" SPC %emitterDB;

	//EMITTER DIRECTION
		%emitterDir = 0;
		for(%i = 0; %i < 6; %i++)
		{
			%obj = "wrenchBot_EmitterDir" @ %i;
			if(%obj.getValue()) 
			{
				%emitterDir = %i;
				break;
			}
		}
		%data = %data TAB "EDIR" SPC %emitterDir;
	
	//ITEM
		%itemDB = wrenchBot_Items.getSelected();
		%data = %data TAB "IDB" SPC %itemDB;

	//ITEM POSITION
		%itemPos = 0;
		for(%i = 0; %i < 6; %i++)
		{
			%obj = "wrenchBot_ItemPos" @ %i;
			if(%obj.getValue()) 
			{
				%itemPos = %i;
				break;
			}
		}
		%data = %data TAB "IPOS" SPC %itemPos;

	//ITEM DIRECTION
		%itemDir = 0;
		for(%i = 2; %i < 6; %i++)
		{
			%obj = "wrenchBot_ItemDir" @ %i;
			if(%obj.getValue()) 
			{
				%itemDir = %i;
				break;
			}
		}
		%data = %data TAB "IDIR" SPC %itemDir;

	//ITEM RESPAWN TIME
		%val = mFloor(wrenchBot_ItemRespawnTime.getValue());
		%data = %data TAB "IRT" SPC %val;
	
   //Raycasting
      %data = %data TAB "RC" SPC wrenchBot_RayCasting.getValue();

   //collision
      %data = %data TAB "C" SPC wrenchBot_Collision.getValue();

   //rendering
      %data = %data TAB "R" SPC wrenchBot_Rendering.getValue();



	%data = trim(%data);

	//check length of data, break into seperate fields if necessary
	commandToserver('SetWrenchData', %data);

	canvas.popDialog(wrenchBotDlg);
}

