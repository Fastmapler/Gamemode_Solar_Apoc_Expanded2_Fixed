$EOTW::TutorialDialouge[0] = "Welcome to Solar Apocalypse Expanded 2! You must gather materials and build a shelter from the dangerous sun and monsters.<br><br>You can skip this tutorial by pressing 'No' or using the /skiptutorial command.";
$EOTW::TutorialDialouge[1] = "Your first step to not dying is gathering some building materials. Gather the 1x1f bricks found scattered throughout the world.<br>Left click a gatherable brick to start gathering it.";
$EOTW::TutorialDialouge[2] = "[Left click] the beige colored deposit of granite to collect it.";
$EOTW::TutorialDialouge[3] = "Great! You have collected some granite to build a shelter. You now need to build a shelter to shield you against the sun's deadly, invisible rays.";
$EOTW::TutorialDialouge[4] = "Take out your brick tool and build a roof blocking you from the sky. Once you have placed a ghost brick, right click to scroll between materials to the 'Granite' option.";
$EOTW::TutorialDialouge[5] = "You will additionally need walls to protect yourself as the sun rises and sets. You will take damage from being in line of sight of the sun, not directly from above.";
$EOTW::TutorialDialouge[6] = "You must place an essential brick: the Checkpoint! This will allow you to respawn at your home incase you die.<br>Place and step on the checkpoint to continue the tutorial.";
$EOTW::TutorialDialouge[7] = "Before we finish, lets craft a tool. Lets make a basic pickaxe to gather materials faster.";
$EOTW::TutorialDialouge[8] = "First, place any basic brick. Then, using the wrench tool, spawn the \"Pickaxe 0\" tool on the brick. [Primary Fire] the item with an empty hand to craft it.";
$EOTW::TutorialDialouge[9] = "This is the end of the tutorial, but you will encounter many greater treasures, trials, and danger down the road. If you are not dying, then you are doing it right.";

function RunTutorialStep(%client) { %client.RunTutorialStep(); }
function GameConnection::RunTutorialStep(%client)
{
    %client.canSkipTutorial = false;
    %client.SetProtectionTime(12 * 60 * 1000, false);
    if (%client.tutorialStep > 9)
        return;

    switch (%client.tutorialStep + 0) {
        case 0: //Introduction dialouge
            %client.messageBoxYesNoCallback = "ServerCmdSkipTutorial";
            commandToClient(%client,'messageBoxYesNo',"Tutorial", $EOTW::TutorialDialouge[0], 'TutorialStepCheck');
        case 1: //Gathering dialouge
            %client.messageBoxYesNoCallback = "ServerCmdSkipTutorial";
            commandToClient(%client,'messageBoxYesNo',"Tutorial", $EOTW::TutorialDialouge[1], 'TutorialStepCheck');
        case 2: //Gathering example
            GatheringTutorialLoop(%client, "");
        case 3: //Building dialouge
            %client.messageBoxYesNoCallback = "ServerCmdSkipTutorial";
            commandToClient(%client,'messageBoxYesNo',"Tutorial", $EOTW::TutorialDialouge[3], 'TutorialStepCheck');
        case 4: //Building example
            BuildingTutorialLoop(%client);
        case 5: //Checkpoint dialouge
            %client.messageBoxYesNoCallback = "ServerCmdSkipTutorial";
            commandToClient(%client,'messageBoxYesNo',"Tutorial", $EOTW::TutorialDialouge[5], 'TutorialStepCheck');
        case 6: //Checkpoint example
            CheckpointTutorialLoop(%client);
        case 7: //Crafting dialouge
            %client.messageBoxYesNoCallback = "ServerCmdSkipTutorial";
            commandToClient(%client,'messageBoxYesNo',"Tutorial", $EOTW::TutorialDialouge[7], 'TutorialStepCheck');
        case 8: //Crafting example
            CraftingTutorialLoop(%client);
        case 9: //Outtro
            %client.messageBoxYesNoCallback = "ServerCmdTutorialStepCheck";
            commandToClient(%client,'messageBoxYesNo',"Tutorial", $EOTW::TutorialDialouge[9], 'TutorialStepCheck');
    }
}

function ServerCmdSkipTutorial(%client)
{
    if (%client.tutorialStep >= 10)
        return;
        
    %client.messageBoxYesNoCallback = "RunTutorialStep";
    %client.canSkipTutorial = true;
    commandToClient(%client,'messageBoxYesNo',"Skip Tutorial", "Do you want to skip the tutorial? If you have played Solar Apoc before, you should be safe to skip.", 'ActuallySkipTutorial');
}

function ServerCmdActuallySkipTutorial(%client)
{
    if (!%client.canSkipTutorial)
        return;

    %client.canSkipTutorial = false;
    %client.tutorialStep = 10;

    if ($EOTW::Material[%client.bl_id, "Granite"] == 0)
        $EOTW::Material[%client.bl_id, "Granite"] += 2048;

    if ($EOTW::Material[%client.bl_id, "Wood"] == 0)
        $EOTW::Material[%client.bl_id, "Wood"] += 2048;
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
        %client.lastTutorialMessage = 0;
            %client.tutorialStep++;
            %client.RunTutorialStep();
        case 2:
            if ($EOTW::Material[%client.bl_id, "Granite"] > 0)
            {
                %client.chatMessage("\c5You have been gifted a lot of Granite to build!");
                $EOTW::Material[%client.bl_id, "Granite"] += 2048;
                %client.tutorialStep++;
                %client.RunTutorialStep();
            }
        case 3:
            %client.lastTutorialMessage = 0;
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
            %ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 126)), %mask, %this);
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
            %client.lastTutorialMessage = 0;
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
            $EOTW::Material[%client.bl_id, "Wood"] += 2048;
            %client.chatMessage("\c5You have been gifted a lot of Wood to craft stuff!");
        case 8:
            if (%player.hasTool("EOTWPickaxe0Item"))
            {
                %client.tutorialStep++;
                %client.RunTutorialStep();
            }
        case 9:
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

    if ($EOTW::Material[%client.bl_id, "Granite"] > 0 && isObject(%brick))
        %brick.delete();
    
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
            {
                %brick = SpawnGatherable(%pos, GetMatterType("Granite"), 999999);
                %player.setVelocity("0 0 0");
                %player.setTransform(vectorAdd(%brick.getPosition(), "0 0 1"));
            }
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

        if (vectorDist(%player.getPosition(), %brick.getPosition()) > 8)
        {
            %player.setVelocity("0 0 0");
            %player.setTransform(vectorAdd(%brick.getPosition(), "0 0 1"));
        }
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

    schedule(100, 0, "CheckpointTutorialLoop", %client);
}

function CraftingTutorialLoop(%client)
{
    if (!isObject(%player = %client.player))
        return;

    if (%client.tutorialStep != 8)
        return;

    if (getSimTime() - %client.lastTutorialMessage > 15000)
    {
        %client.lastTutorialMessage = getSimTime();
        %client.chatMessage("\c6" @ $EOTW::TutorialDialouge[8]);
    }

    ServerCmdtutorialStepCheck(%client);

    schedule(100, 0, "CraftingTutorialLoop", %client);
}

package MessageBoxCallback
{
    function serverCmdMessageBoxNo(%client)
    {
        if(isFunction(%client.messageBoxYesNoCallback))
        {
            %callback = %client.messageBoxYesNoCallback;
            %client.messageBoxYesNoCallback = "";
            call(%callback, %client);
            
        }

        return parent::serverCmdMessageBoxYesNo(%client);
    }

    function serverCmdMessageBoxCancel(%client)
    {
        if(isFunction(%client.messageBoxYesCancelCallback))
        {
            %callback = %client.messageBoxYesCancelCallback;
            %client.messageBoxYesCancelCallback = "";
            call(%callback, %client);
            
        }

        return parent::serverCmdMessageBoxCancel(%client);
    }
};
activatePackage("MessageBoxCallback");
