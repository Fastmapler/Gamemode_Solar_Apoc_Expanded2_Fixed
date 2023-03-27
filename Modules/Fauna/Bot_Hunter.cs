datablock PlayerData(HunterHoleBot : UnfleshedHoleBot)
{
	shapeFile = "Add-Ons/Gamemode_Inferno/EnemyShapes/hydralisk.dts";

	runforce			= 40 * 90;
	maxForwardSpeed		= 7;
	maxBackwardSpeed	= 7;
	maxSideSpeed		= 7;
	maxDamage			= 200;
	lavaImmune			= true;

	boundingBox			= vectorScale("2.2 2.2 4.2", 4);
	crouchBoundingBox	= vectorScale("2.2 2.2 4", 4);
	proneBoundingBox    = vectorScale("2.2 2.2 4.2", 4);

	//can have unique types, nazis will attack zombies but nazis will not attack other bots labeled nazi
	hName = "Hunter";				//cannot contain spaces
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
		hAttackDamage = 28;				//Melee Damage
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

	hPlayerscale = "1.0 1.0 1.0";		//The size of the bot

	//Total Weight, % Chance to be gibbable on death
	//Note: Extra weight can be added to the loot table weight sum for a chance to drop nothing
	EOTWLootTableData = 2.2 TAB 0.6;
	//Weight, Min Loot * 3, Max Loot * 3, Material Name
	EOTWLootTable[1] = 1.0 TAB 64 TAB 64 TAB "Diamond";
	EOTWLootTable[2] = 1.0 TAB 64 TAB 64 TAB "Sturdium";
	EOTWLootTable[3] = 0.2 TAB "ITEM" TAB BossKeyItem;
};

datablock TSShapeConstructor(HunterHoleBotDts)
{
	baseShape  = "Add-Ons/Gamemode_Inferno/EnemyShapes/hydralisk.dts";
	sequence0  = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq root";

	sequence1  = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_move.dsq run";
	sequence2  = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_move.dsq walk";
	sequence3  = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_back.dsq back";
	sequence4  = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_side.dsq side";

	sequence5  = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_crouch.dsq crouch";
	sequence6  = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_crouchMove.dsq crouchRun";
	sequence7  = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_crouch.dsq crouchBack";
	sequence8  = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_crouchSide.dsq crouchSide";

	sequence9  = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq look";
	sequence10 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq headside";
	sequence11 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq headUp";

	sequence12 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_fall.dsq jump";
	sequence13 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq standjump";
	sequence14 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_fall.dsq fall";
	sequence15 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq land";

	sequence16 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq armAttack";
	sequence17 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq armReadyLeft";
	sequence18 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq armReadyRight";
	sequence19 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq armReadyBoth";
	sequence20 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq spearready";  
	sequence21 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq spearThrow";

	sequence22 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq talk";  

	sequence23 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_death.dsq death1"; 

	sequence24 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq shiftUp";
	sequence25 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq shiftDown";
	sequence26 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_acid_attack.dsq shiftAway";
	sequence27 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq shiftTo";
	sequence28 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq shiftLeft";
	sequence29 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq shiftRight";
	sequence30 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq rotCW";
	sequence31 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq rotCCW";

	sequence32 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq undo";
	sequence33 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq plant";

	sequence34 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_sit.dsq sit";

	sequence35 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq wrench";

	sequence36 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_attack.dsq activate";
	sequence37 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_attack.dsq activate2";

	sequence38 = "Add-Ons/Gamemode_Inferno/EnemyShapes/hyd_root.dsq leftrecoil";
};

function HunterHoleBot::onAdd(%this,%obj)
{
	parent::onAdd(%this,%obj);
	%obj.schedule(1200,"InfernalInvisCheck");
}

function AIPlayer::InfernalInvisCheck(%obj)
{
	if (%obj.getState() $= "DEAD")
	{
		%obj.unHideNode("ALL");
		return;
	}
	
	initContainerRadiusSearch(%obj.getPosition(), 4, $TypeMasks::PlayerObjectType);
	
	for (%i = 0; isObject(%target = containerSearchNext()); %i++)
	{
		if (%target.getClassName() $= "Player")
		{
			if (%obj.isInvisible)
			{
				%obj.burn(0);
				%obj.unHideNode("ALL");
				schedule(1200,%obj,"eval",%obj @ ".hAttackDamage = " @ %obj @ ".getDatablock().hAttackDamage;");
				
				%obj.isInvisible = false;
			
				%obj.setMaxForwardSpeed(%obj.getMaxForwardSpeed() / 4);
				%obj.setMaxSideSpeed(%obj.getMaxSideSpeed() / 4);
				%obj.setMaxBackwardSpeed(%obj.getMaxBackwardSpeed() / 4);
				
				GameConnection::ApplyBodyParts(%obj);
				GameConnection::ApplyBodyColors(%obj);
			}
			%obj.InvernalInvisCount = 0;
			break;
		}
	}
	
	%obj.InvernalInvisCount++;
	
	if (%obj.InvernalInvisCount > 4 && !%obj.isInvisible)
	{
		%obj.burn(0);
		%obj.hideNode("ALL");
		%obj.hAttackDamage = 0;
		
		%obj.isInvisible = true;
			
		%obj.setMaxForwardSpeed(%obj.getMaxForwardSpeed() * 4);
		%obj.setMaxSideSpeed(%obj.getMaxSideSpeed() * 4);
		%obj.setMaxBackwardSpeed(%obj.getMaxBackwardSpeed() * 4);
	}
	%obj.schedule(1200,"InfernalInvisCheck");
}