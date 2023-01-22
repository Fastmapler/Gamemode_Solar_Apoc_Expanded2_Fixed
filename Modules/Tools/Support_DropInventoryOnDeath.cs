package Server_DropInventoryOnDeath
{
	function gameConnection::onDeath(%client,%source,%killer,%type,%location)
	{
		if(isObject(%client.minigame))
		{
			if(isObject(%client.player))
			{
                %blacklist = "hammerItem WrenchItem PrintGun SurvivalKnifeItem RecurveBowItem";
				for(%i=0;%i<%client.player.getDatablock().maxTools;%i++)
				{
					%item = %client.player.tool[%i];
					if(isObject(%item) && !hasWord(%blacklist, %item.getName()))
					{
						%pos = %client.player.getPosition();
						%posX = getWord(%pos,0);
						%posY = getWord(%pos,1);
						%posZ = getWord(%pos,2);
						%vec = %client.player.getVelocity();
						%vecX = getWord(%vec,0);
						%vecY = getWord(%vec,1);
						%vecZ = getWord(%vec,2);
						%item = new Item()
						{
							dataBlock = %item;
							position = %pos;
						};
						%itemVec = %vec;
						%itemVec = vectorAdd(%itemVec,getRandom(-8,8) SPC getRandom(-8,8) SPC 10);
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