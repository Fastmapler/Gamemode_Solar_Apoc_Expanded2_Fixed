datablock PlayerData(BlobHoleBot : UnfleshedHoleBot)
{
	runforce			= 40 * 90 * 0.2;
	maxForwardSpeed		= 8;
	maxBackwardSpeed	= 0;
	maxSideSpeed		= 0;
	maxDamage			= 300;
	lavaImmune			= false;

	boundingBox				= VectorScale("1.25 1.25 1.33", 4);
    crouchBoundingBox		= VectorScale("1.25 1.25 0.05", 4);
    proneBoundingBox		= VectorScale("1.25 1.25 0.05", 4);

	//can have unique types, nazis will attack zombies but nazis will not attack other bots labeled nazi
	hName = "Blob";				//cannot contain spaces
	hTickRate = 3000;

	//Searching options
	hSearch	= 1;						//Search for Players
		hSearchRadius = 256;			//in brick units
		hSight = 0;						//Require bot to see player before pursuing
		hStrafe = 1;					//Randomly strafe while following player
	hSearchFOV = 1;						//if enabled disables normal hSearch
		hFOVRadius = 64;				//max 10

	//Attack Options
	hMelee = 1;							//Melee
		hAttackDamage = 20;				//Melee Damage
		hDamageType = "";
	hShoot = 0;
		hWep = "gunImage";
		hShootTimes = 4;				//Number of times the bot will shoot between each tick
		hMaxShootRange = 30;			//The range in which the bot will shoot the player
		hAvoidCloseRange = 1;
			hTooCloseRange = 7;			//in brick units
		isChargeWeapon = 0;				//If weapons should be charged to fire (ie spears)

	//Misc options
	hAvoidObstacles = 0;
	hSuperStacker = 1;
	hSpazJump = 1;						//Makes bot jump when the user their following is higher than them

	hAFKOmeter = 0.1;					//Determines how often the bot will wander or do other idle actions, higher it is the less often he does things
	hIdle = 1;							//Enables use of idle actions, actions which are done when the bot is not doing anything else
		hIdleAnimation = 0;				//Plays random animations/emotes, sit, click, love/hate/etc
		hIdleLookAtOthers = 1;			//Randomly looks at other players/bots when not doing anything else
			hIdleSpam = 0;				//Makes them spam click and spam hammer/spraycan
		hSpasticLook = 1;				//Makes them look around their environment a bit more.
	hEmote = 1;

	hPlayerscale = "2.0 2.0 2.0";		//The size of the bot

	//Total Weight, % Chance to be gibbable on death
	//Note: Extra weight can be added to the loot table weight sum for a chance to drop nothing
	EOTWLootTableData = 0.0 TAB 0.0;
};

datablock PlayerData(BlobChildHoleBot : UnfleshedHoleBot)
{
	runforce			= 40 * 90;
	maxForwardSpeed		= 8;
	maxBackwardSpeed	= 0;
	maxSideSpeed		= 0;
	maxDamage			= 55;
	lavaImmune			= false;

	//can have unique types, nazis will attack zombies but nazis will not attack other bots labeled nazi
	hName = "Blob";				//cannot contain spaces
	hTickRate = 1500;

	//Searching options
	hSearch	= 1;						//Search for Players
		hSearchRadius = 256;			//in brick units
		hSight = 0;						//Require bot to see player before pursuing
		hStrafe = 1;					//Randomly strafe while following player
	hSearchFOV = 1;						//if enabled disables normal hSearch
		hFOVRadius = 64;				//max 10

	//Attack Options
	hMelee = 1;							//Melee
		hAttackDamage = 20;				//Melee Damage
		hDamageType = "";
	hShoot = 0;
		hWep = "gunImage";
		hShootTimes = 4;				//Number of times the bot will shoot between each tick
		hMaxShootRange = 30;			//The range in which the bot will shoot the player
		hAvoidCloseRange = 1;
			hTooCloseRange = 7;			//in brick units
		isChargeWeapon = 0;				//If weapons should be charged to fire (ie spears)

	//Misc options
	hAvoidObstacles = 1;
	hSuperStacker = 1;
	hSpazJump = 1;						//Makes bot jump when the user their following is higher than them

	hAFKOmeter = 0.1;					//Determines how often the bot will wander or do other idle actions, higher it is the less often he does things
	hIdle = 1;							//Enables use of idle actions, actions which are done when the bot is not doing anything else
		hIdleAnimation = 0;				//Plays random animations/emotes, sit, click, love/hate/etc
		hIdleLookAtOthers = 1;			//Randomly looks at other players/bots when not doing anything else
			hIdleSpam = 0;				//Makes them spam click and spam hammer/spraycan
		hSpasticLook = 1;				//Makes them look around their environment a bit more.
	hEmote = 1;

	hPlayerscale = "0.8 0.8 0.8";		//The size of the bot

	//Total Weight, % Chance to be gibbable on death
	//Note: Extra weight can be added to the loot table weight sum for a chance to drop nothing
	EOTWLootTableData = 3.0 TAB 0.0;
	//Weight, Min Loot * 3, Max Loot * 3, Material Name
	EOTWLootTable[0] = 1 TAB 8 TAB 16 TAB "Coal";
	EOTWLootTable[1] = 0.5 TAB 8 TAB 16 TAB "Crude Oil";
	EOTWLootTable[2] = 0.3 TAB 8 TAB 16 TAB "Gold";
	EOTWLootTable[3] = 0.1 TAB 1 TAB 3 TAB "Diamond";
	EOTWLootTable[4] = 0.5 TAB "ITEM" TAB BossKeyItem;
};

package BlobSplit
{
	function Armor::damage(%this, %obj, %sourceObj, %position, %damage, %damageType)
	{
		%toReturn = parent::damage(%this, %obj, %sourceObj, %position, %damage, %damageType);
		
		if (%obj.getState() $= "DEAD" && %obj.getClassName() $= "AIPlayer")
		{
			if (%obj.getDatablock().getName() $= "BlobHoleBot" && !%obj.summonedChildren)
			{
				%obj.summonedChildren = true;
				%x = getWord(%obj.getPosition(), 0);
				%y = getWord(%obj.getPosition(), 1);
				%z = getWord(%obj.getPosition(), 2);

				for (%i = 0; %i < 3; %i++)
					spawnNewFauna((%x + getRandom(-1,1)) SPC (%y + getRandom(-1,1)) SPC %z,"BlobChildHoleBot");

				%obj.schedule(100,"delete");
			}
		}
		
		return %toReturn;
	}
};
activatePackage("BlobSplit");