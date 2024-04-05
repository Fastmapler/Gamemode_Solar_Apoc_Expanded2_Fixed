package Server_DropInventoryOnDeath
{
	function gameConnection::onDeath(%client,%source,%killer,%type,%location)
	{
		if(isObject(%client.minigame))
		{
			if(isObject(%client.player))
			{
				%client.implantList = ""; //I dont want to make a whole new package just for this one line.
                %blacklist = "hammerItem WrenchItem PrintGun";
				for(%i=0;%i<%client.GetMaxInvSlots();%i++)
				{
					%item = %client.player.tool[%i];
					if(isObject(%item) && !hasWord(%blacklist, %item.getName()))
					{
						%pos = %client.player.getPosition();
						%vec = %client.player.getVelocity();
						%item = new Item()
						{
							dataBlock = %item;
							position = vectorAdd(%pos, "0 0 1");
						};
						%itemVec = vectorAdd(%vec,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
						%item.BL_ID = %client.BL_ID;
						%item.deathItem = true;
						%item.minigame = %client.minigame;
						%item.spawnBrick = -1;
						%item.setVelocity(%itemVec);
						%item.schedulePop();
					}
					%client.player.tool[%i] = "";
			
				}
			}
		}
		Parent::onDeath(%client,%source,%killer,%type,%location);
	}
};
activatePackage(Server_DropInventoryOnDeath);