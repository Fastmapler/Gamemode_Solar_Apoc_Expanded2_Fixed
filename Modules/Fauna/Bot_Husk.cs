datablock PlayerData(HuskHoleBot : UnfleshedHoleBot)
{
	runforce			= 40 * 90;
	maxForwardSpeed		= 6;
	maxBackwardSpeed	= 3;
	maxSideSpeed		= 6;
	maxDamage			= 110;
	lavaImmune			= true;
	sunImmune			= true;

	//can have unique types, nazis will attack zombies but nazis will not attack other bots labeled nazi
	hName = "Husk";				//cannot contain spaces
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
		hAttackDamage = 24;				//Melee Damage
		hDamageType = "";
	hShoot = 1;
		hWep = "";
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

	hPlayerscale = "1.2 1.2 1.0";		//The size of the bot

	//Total Weight, % Chance to be gibbable on death
	//Note: Extra weight can be added to the loot table weight sum for a chance to drop nothing
	EOTWLootTableData = 3.0 TAB 0.4;
	//Weight, Min Loot * 3, Max Loot * 3, Material Name
	EOTWLootTable[0] = 1.0 TAB 64 TAB 64 TAB "Quartz";
	EOTWLootTable[1] = 1.0 TAB 64 TAB 64 TAB "Magnetite";
	EOTWLootTable[2] = 1.0 TAB 64 TAB 64 TAB "Coal";
};