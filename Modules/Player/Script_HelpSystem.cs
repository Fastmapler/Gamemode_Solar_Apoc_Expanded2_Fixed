$EOTW::TutorialDialouge[0] = "Welcome to Solar Apocalypse Expanded 2! In this gamemode, you must gather materials and build a shelter from the dangerous sun and monsters.";
$EOTW::TutorialDialouge[1] = "Your first step to not dying is gathering some building materials. The 1x1f sized bricks found scattered throughout the world can be gathered. Left click a gatherable brick to start gathering it.";
$EOTW::TutorialDialouge[2] = "[Left click] the beige colored deposit of granite to collect it.";
$EOTW::TutorialDialouge[3] = "Great! You now have some supplies to build a house. You now need to build a shelter to protect you against the sun's deadly, invisible rays.";
$EOTW::TutorialDialouge[4] = "Take out your brick tool and build a roof blocking you from the sky. There are many materials to build from, press [Right Click] to scroll to the Granite option.";
$EOTW::TutorialDialouge[5] = "You will additionally need walls to protect yourself as the sun rises and sets. However, you must place an essential brick: the Checkpoint! This will allow you to respawn at your home incase you die.";
$EOTW::TutorialDialouge[6] = "Place and step on the checkpoint to complete the tutorial.";
$EOTW::TutorialDialouge[7] = "Congratulations! You now have the basic needs to survive. Make sure you build walls to further protect yourself from the sun.";
$EOTW::TutorialDialouge[8] = "This is the end of the tutorial, but you will encounter many greater treasures, trials, and danger down the road. If you are not dying, then you are doing it right.";
$EOTW::TutorialDialouge[9] = "Good luck.";
function GameConnection::RunTutorialStep(%client)
{
    if (%client.tutorialStep > 9)
        return;
    
    %client.permaProtection = true;
    switch (%client.tutorialStep + 0) {
        case 0: //Introduction dialouge
            commandToClient(%client,'messageBoxYesNo',"Tutorial", $EOTW::TutorialDialouge[0], 'TutorialStepCheck', 'TutorialStepCheck');
        case 1: //Gathering dialouge
            commandToClient(%client,'messageBoxYesNo',"Tutorial", $EOTW::TutorialDialouge[1], 'TutorialStepCheck', 'TutorialStepCheck');
        case 2: //Gathering example
            GatheringTutorialLoop(%client, "");
        case 3: //Building dialouge
            commandToClient(%client,'messageBoxYesNo',"Tutorial", $EOTW::TutorialDialouge[3], 'TutorialStepCheck', 'TutorialStepCheck');
        case 4: //Building example
            BuildingTutorialLoop(%client);
        case 5: //Checkpoint dialouge
            commandToClient(%client,'messageBoxYesNo',"Tutorial", $EOTW::TutorialDialouge[5], 'TutorialStepCheck', 'TutorialStepCheck');
        case 6: //Checkpoint example
            CheckpointTutorialLoop(%client);
        case 7: //Outro 1
            commandToClient(%client,'messageBoxYesNo',"Tutorial", $EOTW::TutorialDialouge[7], 'TutorialStepCheck', 'TutorialStepCheck');
        case 8: //Outro 2
            commandToClient(%client,'messageBoxYesNo',"Tutorial", $EOTW::TutorialDialouge[8], 'TutorialStepCheck', 'TutorialStepCheck');
        case 9: //End.
            %client.tutorialStep++;
            %client.chatMessage("\c6" @ $EOTW::TutorialDialouge[9]);
            %client.SetProtectionTime(20 * 60 * 1000);
    }
}

function ServerCmdTutorialStepCheck(%client)
{
    if (!isObject(%player = %client.player))
        return false;

    switch (%client.tutorialStep + 0)
    {
        case 0:
            %client.tutorialStep++;
            %client.RunTutorialStep();
        case 1:
            %client.tutorialStep++;
            %client.RunTutorialStep();
        case 2:
            if ($EOTW::Material[%client.bl_id, "Granite"] > 0)
            {
                $EOTW::Material[%client.bl_id, "Granite"] = 2048;
                %client.tutorialStep++;
                %client.RunTutorialStep();
            }
        case 3:
            %client.tutorialStep++;
            %client.RunTutorialStep();
        case 4:
            %origin = vectorAdd(%player.getPosition(), "0 0 128");
            %offset = "0 0 0";
            %eye = vectorAdd(%origin, %offset);
            %dir = "0 0 -1";
            %for = "0 1 0";
            %face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
            %mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
            %ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 500)), %mask, %this);
            %pos = getWord(%ray,1) SPC getWord(%ray,2) SPC (getWord(%ray,3) + 0.1);
            if(isObject(%hit = firstWord(%ray)) && (getWord(%pos, 2) > $EOTW::LavaHeight + 2))
            {
                if (%hit.getGroup().bl_id == %client.bl_id)
                {
                    %client.tutorialStep++;
                    %client.RunTutorialStep();
                }
            }
        case 5:
            %client.tutorialStep++;
            %client.RunTutorialStep();
        case 6:
            if (isObject(%brick = %client.checkpointBrick) && %brick.getGroup().bl_id == %client.bl_id)
            {
                %client.tutorialStep++;
                %client.RunTutorialStep();
            }
        case 7:
            %client.tutorialStep++;
            %client.RunTutorialStep();
        case 8:
            %client.tutorialStep++;
            %client.RunTutorialStep();
    }
}

function GatheringTutorialLoop(%client, %brick)
{
    if (!isObject(%player = %client.player))
    {
        if (isObject(%brick))
            %brick.delete();

        return;
    }
    
    while (!isObject(%brick) && %attempt < 100)
    {
        ServerCmdtutorialStepCheck(%client);

        if (%client.tutorialStep != 2)
            return;

        %attempt++;
        %veinRange = 16;
        %origin = vectorAdd(%player.getPosition(), "0 0 128");
        %offset = (getRandom(-1 * %veinRange, %veinRange) / 2) SPC (getRandom(-1 * %veinRange, %veinRange) / 2) SPC "0";
		%eye = vectorAdd(%origin, %offset);
		%dir = "0 0 -1";
		%for = "0 1 0";
		%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
		%mask = $Typemasks::fxBrickAlwaysObjectType | $Typemasks::TerrainObjectType;
		%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 500)), %mask, %this);
		%pos = getWord(%ray,1) SPC getWord(%ray,2) SPC (getWord(%ray,3) + 0.1);
		if(isObject(%hit = firstWord(%ray)) && (getWord(%pos, 2) > $EOTW::LavaHeight + 2))
		{
			if (%hit.getClassName() !$= "FxPlane" && strPos(%hit.getDatablock().uiName,"Ramp") > -1)
				%pos = vectorAdd(%pos,"0 0 0.4");

            if (!%hit.isCollectable)
                %brick = SpawnGatherable(%pos, GetMatterType("Granite"), 999999);
		}
    }

    if (getSimTime() - %client.lastTutorialMessage > 15000)
    {
        %client.lastTutorialMessage = getSimTime();
        %client.chatMessage("\c6" @ $EOTW::TutorialDialouge[2]);
    }

    if (getSimTime() - %brick.lastScanTime > 1200)
    {
        %brick.lastScanTime = getSimTime();
        %brick.TempColorFX(3, 600, true);
    }

    schedule(100, 0, "GatheringTutorialLoop", %client, %brick);
}

function BuildingTutorialLoop(%client)
{
    if (!isObject(%player = %client.player))
        return;
    
    if (%client.tutorialStep != 4)
            return;

    if (getSimTime() - %client.lastTutorialMessage > 15000)
    {
        %client.lastTutorialMessage = getSimTime();
        %client.chatMessage("\c6" @ $EOTW::TutorialDialouge[4]);
    }

    ServerCmdtutorialStepCheck(%client);

    schedule(100, 0, "BuildingTutorialLoop", %client, %brick);
}

function CheckpointTutorialLoop(%client)
{
    if (!isObject(%player = %client.player))
        return;

    if (%client.tutorialStep != 6)
        return;

    if (getSimTime() - %client.lastTutorialMessage > 15000)
    {
        %client.lastTutorialMessage = getSimTime();
        %client.chatMessage("\c6" @ $EOTW::TutorialDialouge[6]);
    }

    ServerCmdtutorialStepCheck(%client);

    schedule(100, 0, "CheckpointTutorialLoop", %client, %brick);
}