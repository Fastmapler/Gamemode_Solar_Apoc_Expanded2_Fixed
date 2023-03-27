datablock PlayerData(UnfleshedHoleBot : PlayerStandardArmor)
{
	uiName 				= "";
	minJetEnergy 		= 0;
	jetEnergyDrain 		= 0;
	canJet 				= 0;
	maxItems			= 0;
	maxWeapons			= 0;
	maxTools			= 0;
	runforce			= 60 * 90;
	maxForwardSpeed		= 4;
	maxBackwardSpeed	= 2;
	maxSideSpeed		= 4;
	rideable			= false;
	canRide				= false;
	maxDamage			= 80;
	jumpSound			= "";
	lavaImmune			= false;
	
	//Hole Attributes
	isHoleBot = 1;
	
	//Spawning option
	hSpawnTooClose = 0;					//Doesn't spawn when player is too close and can see it
		hSpawnTCRange = 8;				//above range, set in brick units
	hSpawnClose = 0;					//Only spawn when close to a player, can be used with above function as long as hSCRange is higher than hSpawnTCRange
		hSpawnCRange = 64;				//above range, set in brick units

	hType = enemy;						//Enemy,Friendly, Neutral
		hNeutralAttackChance = 100;

	//can have unique types, nazis will attack zombies but nazis will not attack other bots labeled nazi
	hName = "Unfleshed";				//cannot contain spaces
	hTickRate = 3000;
	
	//Wander Options
	hWander = 1;						//Enables random walking
		hSmoothWander = 1;
		hReturnToSpawn = 1;				//Returns to spawn when too far
		hSpawnDist = 10000;				//Defines the distance bot can travel away from spawnbrick
		hGridWander = 256;
	
	//Searching options
	hSearch	= 1;						//Search for Players
		hSearchRadius = 256;			//in brick units
		hSight = 1;						//Require bot to see player before pursuing
		hStrafe = 1;					//Randomly strafe while following player
	hSearchFOV = 1;						//if enabled disables normal hSearch
		hFOVRadius = 32;				//max 10

	hAlertOtherBots = 1;				//Alerts other bots when he sees a player, or gets attacked

	//Attack Options
	hMelee = 1;							//Melee
		hAttackDamage = 15;				//Melee Damage
		hDamageType = "";
	hShoot = 0;
		hWep = "gunImage";
		hShootTimes = 4;				//Number of times the bot will shoot between each tick
		hMaxShootRange = 30;			//The range in which the bot will shoot the player
		hAvoidCloseRange = 1;
			hTooCloseRange = 7;			//in brick units
		isChargeWeapon = 0;				//If weapons should be charged to fire (ie spears)

	hHerding = 0;
	hSound = 1;
	hHearing = 1;
	hSpawnDetect = 0;					//Will not spawn when user is too close and can see spawn

	//Misc options
	hAvoidObstacles = 1;
	hSuperStacker = 1;
	hSpazJump = 1;						//Makes bot jump when the user their following is higher than them

	hAFKOmeter = 1;						//Determines how often the bot will wander or do other idle actions, higher it is the less often he does things
	hIdle = 1;							//Enables use of idle actions, actions which are done when the bot is not doing anything else
		hIdleAnimation = 0;				//Plays random animations/emotes, sit, click, love/hate/etc
		hIdleLookAtOthers = 1;			//Randomly looks at other players/bots when not doing anything else
			hIdleSpam = 0;				//Makes them spam click and spam hammer/spraycan
		hSpasticLook = 1;				//Makes them look around their environment a bit more.
	hEmote = 1;

	hPlayerscale = "1.0 1.0 1.0";		//The size of the bot

	//Total Weight, % Chance to be gibbable on death
	//Note: Extra weight can be added to the loot table weight sum for a chance to drop nothing
	EOTWLootTableData = 2.0 TAB 0.2;
	//Weight, Min Loot * 3, Max Loot * 3, Material Name
	EOTWLootTable[0] = 1.0 TAB 128 TAB 128 TAB "Wood";
	EOTWLootTable[1] = 1.0 TAB 64 TAB 64 TAB "Granite";
};