$EOTW::ClimbCost = 12; //Energy cost per climb boost
$EOTW::MininumRunEnergy = 10; //Mininum energy to start running
$EOTW::RunSpeedMultiplier = 1.25; //How much faster running is
$EOTW::RunCost = 60; //Consumption per second

datablock PlayerData(PlayerSolarApoc : PlayerStandardArmor)
{
	minJetEnergy = 1;
	jetEnergyDrain = 1;
	canJet = 0;
	
	maxDamage = 100;
	maxEnergy = 100;
	repairRate = 0.1;
	rechargeRate = 0.6;
	runForce = 48 * 90 * 2;
	
	jumpDelay = 7;
	jumpEnergyDrain = 25;
	minJumpEnergy = 25;
	
	airControl = 0.25;
	speedDamageScale = 1.0;
	
	maxTools = 5;
	maxWeapons = 5;

	uiName = "Solar Apoc Player";
	showEnergyBar = true;

	kitDatablock = DefaultPlayerKitItem;
	protectType = "Standard";
};



package Player_SolarApoc
{
	function Armor::onTrigger(%this,%player,%slot,%val,%a,%b,%c)
	{
		Parent::onTrigger(%this,%player,%slot,%val,%a,%b,%c);
		%client = %player.client;
		//Climbing code
		if(%slot == 4 && %val && mAbs(getWord(%player.getVelocity(), 2)) > 0)
		{
			%ray = containerRayCast(%player.getEyePoint(),vectorAdd(%player.getEyePoint(),vectorScale(%player.getEyeVector(),5 * getWord(%player.getScale(),2))),$TypeMasks::FxBrickObjectType,%player);
			%target = firstWord(%ray);
			if(isObject(%target))
			{
				%level = %player.getEnergyLevel();
				if(%level >= $EOTW::ClimbCost)
				{
					//Play climb sound
					%player.playThread(3, "shiftUp");
					%player.addVelocity("0 0 3.3");

					%cost = %target.material $= "Asphalt" ? $EOTW::ClimbCost / 2 : $EOTW::ClimbCost;
					%newlevel = %level <= $EOTW::ClimbCost ? 0 : %level - %cost;
					%player.setEnergyLevel(%newlevel);
				}
			}
		}
		if (%slot == 4)
		{
			if (%val && !%player.moveRunning && vectorLen(setWord(%player.getVelocity(), 2, "0")) > 1)
			{
				%level = %player.getEnergyLevel();
				if(%level >= $EOTW::MininumRunEnergy)
				{
					//Play sprint sound
					%player.StartRunMove();
				}
			}
			else if (!%val && %player.moveRunning)
			{
				%player.StopRunMove();
			}
		}
	}
};
activatePackage("Player_SolarApoc");

function Player::SetSpeedMulti(%player, %multi)
{
	//If no multi input, just update our player's speed.
	if (%multi $= "")
	{
		if (%player.MoveSpeedMult $= "")
			%player.MoveSpeedMult = 1;
		
		%multi = %player.MoveSpeedMult;
	}
	
	%player.MoveSpeedMult = %multi;
	
	%data = %player.getDatablock();
	
	//Probably over complicated
	//copied from rallypack code
	%forwardSpeed = %data.maxForwardSpeed;
	%backwardSpeed = %data.maxBackwardSpeed;
	%strafeSpeed = %data.maxSideSpeed;
	%forwardSpeedCrouching = %data.maxForwardCrouchSpeed;
	%backwardSpeedCrouching = %data.maxBackwardCrouchSpeed;
	%strafeSpeedCrouching = %data.maxSideCrouchSpeed;
	
	%speedMaul = %player.MoveSpeedMult;
	
	%forwardSpeed *= %speedMaul;
	%backwardSpeed *= %speedMaul;
	%strafeSpeed *= %speedMaul;
	%forwardSpeedCrouching *= %speedMaul;
	%backwardSpeedCrouching *= %speedMaul;
	%strafeSpeedCrouching *= %speedMaul;
	
	%player.setMaxBackwardSpeed(%backwardSpeed);
	%player.setMaxCrouchBackwardSpeed(%backwardSpeedCrouching);
	%player.setMaxCrouchForwardSpeed(%forwardSpeedCrouching);
	%player.setMaxCrouchSideSpeed(%strafeSpeedCrouching);
	%player.setMaxForwardSpeed(%forwardSpeed);
	%player.setMaxSideSpeed(%strafeSpeed);
	
	//Don't forget to auto update on datablock change
}

function Player::GetSpeedMulti(%player)
{
	if (%player.MoveSpeedMult $= "")
		%player.MoveSpeedMult = 1;
		
	return %player.MoveSpeedMult;
}

function Player::ChangeSpeedMulti(%player, %change)
{
	%player.setSpeedMulti(%player.GetSpeedMulti() + %change);
}

function Player::StartRunMove(%player)
{
	%boost = getMax(1.0, %player.getDatablock().runBoost);
	%change = %boost * $EOTW::RunSpeedMultiplier;
	%player.ChangeSpeedMulti(%change);
	%player.runBoostAmount = %change;
	%player.moveRunning = true;
	%player.RunMoveLoop();
}

function Player::StopRunMove(%player)
{
	%player.ChangeSpeedMulti(%player.runBoostAmount * -1);
	%player.moveRunning = false;
	cancel(%player.RunMoveLoop);
}

$EOTW::RunTickRate = 30;
function Player::RunMoveLoop(%player)
{
	cancel(%player.RunMoveLoop);
	
	%level = %player.getEnergyLevel();
	%cost = ($EOTW::RunCost / $EOTW::RunTickRate);
	%newlevel = %level <= %cost ? 0 : %level - %cost;
	
	%player.setEnergyLevel(%newlevel);
	
	if(%newlevel < 1)
		%player.StopRunMove();
	else
		%player.RunMoveLoop = %player.schedule(1000 / $EOTW::RunTickRate, "RunMoveLoop");
}

