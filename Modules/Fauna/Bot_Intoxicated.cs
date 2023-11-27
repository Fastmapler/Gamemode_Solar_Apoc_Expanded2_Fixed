datablock PlayerData(IntoxicatedHoleBot : UnfleshedHoleBot)
{
	runforce			= 40 * 90 * 0.5;
	maxForwardSpeed		= 3;
	maxBackwardSpeed	= 1;
	maxSideSpeed		= 1;
	maxDamage			= 350;
	lavaImmune			= true;

	//can have unique types, nazis will attack zombies but nazis will not attack other bots labeled nazi
	hName = "Intoxicated";				//cannot contain spaces
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
		hAttackDamage = 35;				//Melee Damage
		hDamageType = "";
	hShoot = 1;
		hWep = "acidImage";
		hShootTimes = 2;				//Number of times the bot will shoot between each tick
		hMaxShootRange = 16;			//The range in which the bot will shoot the player
		hAvoidCloseRange = 0;
			hTooCloseRange = 0;			//in brick units
		isChargeWeapon = 1;				//If weapons should be charged to fire (ie spears)
			

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

	hPlayerscale = "2.0 2.0 1.5";		//The size of the bot

	//Total Weight, % Chance to be gibbable on death
	//Note: Extra weight can be added to the loot table weight sum for a chance to drop nothing
	EOTWLootTableData = 2.2 TAB 0.8;
	//Weight, Min Loot * 3, Max Loot * 3, Material Name
	EOTWLootTable[0] = 1.0 TAB 64 TAB 64 TAB "Anglesite";
	EOTWLootTable[1] = 1.0 TAB 64 TAB 64 TAB "Native Gold";
	EOTWLootTable[2] = 0.2 TAB "ITEM" TAB BossKeyItem;
};