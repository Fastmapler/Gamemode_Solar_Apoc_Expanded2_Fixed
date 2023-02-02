function miniGameCanDamage (%client, %victimObject)
{
	%miniGame1 = getMiniGameFromObject (%client);
	%miniGame2 = getMiniGameFromObject (%victimObject);
    talk(%client SPC %victimObject);
	if (%client.isBot)
	{
		if (isObject (%client.spawnBrick))
		{
			%minigameHost1 = %miniGame1.owner;
			%isHost1 = %client.spawnBrick.getGroup ().client == %minigameHost1;
			%isIncluded1 = %miniGame1.UseAllPlayersBricks;
			%botNum1 = 1;
			%botCount += 1;
		}
		else 
		{
			%otherBotCount += 1;
		}
	}
	if (%victimObject.isBot)
	{
		if (isObject (%victimObject.spawnBrick))
		{
			%minigameHost2 = %miniGame2.owner;
			%isHost2 = %victimObject.spawnBrick.getGroup ().client == %minigameHost2;
			%isIncluded2 = %miniGame2.UseAllPlayersBricks;
			%botNum2 = 1;
			%botCount += 1;
		}
		else 
		{
			%otherBotCount += 1;
		}
	}
	%type = %victimObject.getType ();
	if (%miniGame2 != %miniGame1 && getBL_IDFromObject (%client) == getBL_IDFromObject (%victimObject))
	{
		%doHack = 1;
		if (%victimObject.getType () & $TypeMasks::PlayerObjectType)
		{
			if (%victimObject.getControllingClient () > 0)
			{
				%doHack = 0;
			}
		}
		if (%doHack)
		{
			%miniGame2 = %miniGame1;
		}
	}
	if ($Server::LAN)
	{
		if (!isObject (%miniGame1))
		{
			return 1;
		}
		if (%type & $TypeMasks::PlayerObjectType)
		{
			if (%victimObject.isBot || %client.isBot)
			{
				if (%miniGame1 != %miniGame2)
				{
					return 0;
				}
				if (%botCount == 2)
				{
					if ((%isHost1 && %isHost2) || %isIncluded1 || %isIncluded2)
					{
						return 1;
					}
					else 
					{
						return 0;
					}
				}
				if (%miniGame1.BotDamage)
				{
					%cIsPlayerVehicle = %client.getClassName () $= "AIPlayer" && !%client.isBot;
					if (%cIsPlayerVehicle && !%miniGame1.VehicleDamage)
					{
						return 0;
					}
					if (%otherBotCount)
					{
						return 1;
					}
					if (%botCount == 2)
					{
						if ((%isHost1 && %isHost2) || %isIncluded1 || %isIncluded2)
						{
							return 1;
						}
						else 
						{
							return 0;
						}
					}
					if (%botNum1)
					{
						%a = 1;
						if (%isHost[%a] || %isIncluded[%a])
						{
							return 1;
						}
						else 
						{
							return 0;
						}
					}
					if (%botNum2)
					{
						%a = 2;
						if (%isHost[%a] || %isIncluded[%a])
						{
							return 1;
						}
						else 
						{
							return 0;
						}
					}
					return 1;
				}
				else 
				{
					return 0;
				}
			}
			else if (isObject (%victimObject.client))
			{
				if (%miniGame1 != %miniGame2)
				{
					return 0;
				}
				if (%miniGame1.WeaponDamage)
				{
					return 0; //Changed for No PvP
				}
			}
			else if (%miniGame1.VehicleDamage)
			{
				return 0; //Changed for No PvP
			}
		}
		else if (%type & $TypeMasks::VehicleObjectType)
		{
			if (%miniGame1.VehicleDamage)
			{
				return 0;  //Changed for No PvP and no cycles of eternal fire damage
			}
		}
		else if (%type & $TypeMasks::FxBrickAlwaysObjectType)
		{
			if (%miniGame1.BrickDamage)
			{
				return 1;
			}
		}
		else if (%miniGame1.WeaponDamage)
		{
			return 1;
		}
		return 0;
	}
	if (!isObject (%miniGame1) && !isObject (%miniGame2))
	{
		return -1;
	}
	if (%miniGame1 != %miniGame2)
	{
		return 0;
	}
	if (!isObject (%miniGame1))
	{
		return 0;
	}
	%ruleDamage = 0;
	if (%type & $TypeMasks::PlayerObjectType)
	{
		if (%victimObject.isBot || %client.isBot)
		{
			if (%botCount == 2)
			{
				if ((%isHost1 && %isHost2) || %isIncluded1 || %isIncluded2)
				{
					return 1;
				}
				else 
				{
					return 0;
				}
			}
			if (%miniGame1.BotDamage)
			{
				%cIsPlayerVehicle = %client.getClassName () $= "AIPlayer" && !%client.isBot;
				if (%cIsPlayerVehicle && !%miniGame1.VehicleDamage)
				{
					return 0;
				}
				if (%otherBotCount)
				{
					return 1;
				}
				if (%botCount == 2)
				{
					if ((%isHost1 && %isHost2) || %isIncluded1 || %isIncluded2)
					{
						return 1;
					}
					else 
					{
						return 0;
					}
				}
				if (%botNum1)
				{
					%a = 1;
					if (%isHost[%a] || %isIncluded[%a])
					{
						return 1;
					}
					else 
					{
						return 0;
					}
				}
				if (%botNum2)
				{
					%a = 2;
					if (%isHost[%a] || %isIncluded[%a])
					{
						return 1;
					}
					else 
					{
						return 0;
					}
				}
				return 1;
			}
			else 
			{
				return 0;
			}
		}
		else if (isObject (%victimObject.client))
		{
			if (%miniGame1.WeaponDamage)
			{
				if (%victimObject.client == %client)
				{
					if (%miniGame1.SelfDamage)
					{
						return 1;
					}
					else 
					{
						return 0;
					}
				}
				else 
				{
					return 1;
				}
			}
			else 
			{
				return 0;
			}
		}
		else if (%miniGame1.VehicleDamage)
		{
			%ruleDamage = 1;
		}
	}
	else if (%type & $TypeMasks::VehicleObjectType)
	{
		if (%miniGame1.VehicleDamage)
		{
			%ruleDamage = 1;
		}
	}
	else if (%type & $TypeMasks::FxBrickAlwaysObjectType)
	{
		if (%miniGame1.BrickDamage)
		{
			%ruleDamage = 1;
		}
	}
	else if (%miniGame1.WeaponDamage)
	{
		%ruleDamage = 1;
	}
	if (%ruleDamage == 0)
	{
		return 0;
	}
	if (%miniGame1.UseAllPlayersBricks)
	{
		return 1;
	}
	else 
	{
		%victimBL_ID = getBL_IDFromObject (%victimObject);
		if (%victimBL_ID == %miniGame1.owner.getBLID ())
		{
			return 1;
		}
	}
	return 0;
}