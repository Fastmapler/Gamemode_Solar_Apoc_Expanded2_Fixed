package EOTW_ItemCrafting
{
    function Armor::onCollision(%data,%this,%col,%vec,%vel)
	{
		if(%col.getClassName() $= "Item")
		{
            if (isObject(%col.spawnBrick) && $EOTW::ItemCrafting[%col.getDatablock().getName()] !$= "")
            {
                if(!%this.client.messagedAboutCTPU)
                {
                    messageClient(%this.client,'',"This item must be crafted. Click it with an empty hand to craft it.");
                    %this.client.messagedAboutCTPU = 1;
                }
                return 0;
            }
            else
            {
                %this.pickup(%col);
                return 0;
            }
                
		}
		return Parent::onCollision(%data,%this,%col,%vec,%vel);
	}
    function Armor::onTrigger(%data, %obj, %trig, %tog)
	{
		if(isObject(%client = %obj.client))
		{
			if(%trig == 0 && %tog && !isObject(%obj.getMountedImage(0)))
			{
				%eye = %obj.getEyePoint();
				%dir = %obj.getEyeVector();
				%for = %obj.getForwardVector();
				%face = getWords(vectorScale(getWords(%for, 0, 1), vectorLen(getWords(%dir, 0, 1))), 0, 1) SPC getWord(%dir, 2);
				%mask = $Typemasks::FxBrickObjectType | $Typemasks::TerrainObjectType | $TypeMasks::ItemObjectType;
				%ray = containerRaycast(%eye, vectorAdd(%eye, vectorScale(%face, 5)), %mask, %obj);
				if(isObject(%hit = firstWord(%ray)) && %hit.getClassName() $= "Item")
				{
                    %itemData = %hit.getDataBlock();
					for(%x = 0; %x < %data.maxTools; %x++)
                    {
                        if(isObject(%obj.tool[%x]) && %obj.tool[%x].getID() == %itemData.getID())
						{
							%copy = 1;
							break;
						}
                    }

                    if(%hit.canPickup && !%copy && isObject(%hit.spawnBrick) && $EOTW::ItemCrafting[%itemData.getName()] !$= "")
                    {
                        %obj.CraftItemPrompt(%itemData);
                    }
                    //else
                        //%obj.pickup(%hit);
				}
			}
		}
		Parent::onTrigger(%data, %obj, %trig, %tog);
	}
};
activatePackage("EOTW_ItemCrafting");

function Player::CraftItemPrompt(%obj, %item)
{
    if (!isObject(%item))
        return;

    %costData = $EOTW::ItemCrafting[%item.getName()];
    %obj.selectedCraftingItemData = %item;

    for (%i = 0; %i < getFieldCount(%costData); %i += 2)
        %text = %text @ %obj.GetMatterCount(getField(%costData, %i + 1)) @ "/" @ getField(%costData, %i) SPC getField(%costData, %i + 1) @ "<br>";

    commandToClient(%obj.client,'messageBoxYesNo',"Crafting", "[" @ %item.uiName @ "] " @ $EOTW::ItemDescription[%item.getName()] @ "<br>crafting cost:<br>---<br>" @ %text @ "---<br>Craft this item?", 'CraftItemAccept','CraftItemCancel');
}

function ServerCmdCraftItemAccept(%client)
{
    if (!isObject(%player = %client.player))
        return;

    if (!$Pref::Server::SAEX2::DevMode)
    {
        %costData = $EOTW::ItemCrafting[%player.selectedCraftingItemData.getName()];

        for (%i = 0; %i < getFieldCount(%costData); %i += 2)
        {
            if (%player.GetMatterCount(getField(%costData, %i + 1)) < getField(%costData, %i))
            {
                %client.chatMessage("You need more " @ getField(%costData, %i + 1) @ "!");
                ServerCmdCraftItemCancel(%client);
                return;
            }
        }

        for (%i = 0; %i < getFieldCount(%costData); %i += 2)
            %player.ChangeMatterCount(getField(%costData, %i + 1), getField(%costData, %i) * -1);
    }
    
    %item = new Item()
    {
        datablock = %player.selectedCraftingItemData;
        static    = "0";
        position  = vectorAdd(%player.getPosition(),"0 0 1");
        craftedItem = true;
    };

    ServerCmdCraftItemCancel(%client);
}

function ServerCmdCraftItemCancel(%client)
{
    if (!isObject(%player = %client.player))
        return;

    %player.selectedCraftingItemData = "";
    %player.selectedCraftingItemBrick = "";
}