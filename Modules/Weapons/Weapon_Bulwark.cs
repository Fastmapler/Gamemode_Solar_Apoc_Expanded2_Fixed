$EOTW::ItemCrafting["BulwarkItem"] = (1 TAB "dog");
$EOTW::ItemDescription["BulwarkItem"] = "A quite massive shield.";

datablock itemData(BulwarkItem)
{
	uiName = "Bulwark Shield";
	iconName = "";
	doColorShift = false;
	colorShiftColor = "1.00 1.00 1.00 1.00";
	
	shapeFile = "./Shapes/Bulwark.dts";
	image = BulwarkImage;
	canDrop = true;
	
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;
	
	category = "Tools";
};

datablock shapeBaseImageData(BulwarkImage)
{
	shapeFile = "./Shapes/Bulwark.dts";
	item = BulwarkItem;
	
	mountPoint = 0;
	offset = "0 0 0";
	rotation = 0;
	
	eyeOffset = "0 1 -2";
	eyeRotation = 0;
	
	correctMuzzleVector = true;
	className = "WeaponImage";
	
	melee = false;
	armReady = true;

	doColorShift = BulwarkItem.doColorShift;
	colorShiftColor = BulwarkItem.colorShiftColor;
	
	stateName[0]					= "Start";
	stateTimeoutValue[0]			= 0.5;
	stateTransitionOnTimeout[0]	 	= "Ready";
	stateSound[0]					= weaponSwitchSound;
	
	stateName[1]					= "Ready";
	stateTransitionOnTriggerDown[1] = "Fire";
	stateAllowImageChange[1]		= true;
	
	stateName[2]					= "Fire";
	stateScript[2]					= "onFire";
	stateTimeoutValue[2]			= 0.1;
	stateAllowImageChange[2]		= false;
	stateTransitionOnTimeout[2]		= "checkFire";
	
	stateName[3]					= "checkFire";
	stateTransitionOnTriggerUp[3] 	= "Ready";
};

function BulwarkImage::onMount(%this, %obj, %slot)
{
	Parent::onMount(%this, %obj, %slot);

	%obj.playThread(1, armreadyboth);
}

$Shield::RiotCancelFalling = true;
$Shield::RiotReflects = true;

package Bulwark
{  
   function ProjectileData::damage(%this,%obj,%col,%fade,%pos,%normal) 
   {
      %shielded = 0;
      if(%col.getType() & $TypeMasks::PlayerObjectType)
      {
         %image0 = %col.getMountedImage(0);
         %image1 = %col.getMountedImage(1);
         %image2 = %col.getMountedImage(2);
         %state0 = %col.getImageState(0);
         
         %scale = getWord(%col.getScale(),2);
         
         %fvec = %col.getForwardVector();
         %fX = getWord(%fvec,0);
         %fY = getWord(%fvec,1);
         
         %evec = %col.getEyeVector();
         %eX = getWord(%evec,0);
         %eY = getWord(%evec,1);
         %eZ = getWord(%evec,2);
         
         %eXY = mSqrt(%eX*%eX+%eY*%eY);
         
         %aimVec = %fX*%eXY SPC %fY*%eXY SPC %eZ;
         
         if(%image0 == BulwarkImage.getID() && %state0 $= "Ready")
         {
            if(%eZ > 0.75)
               %shielded = (getword(%pos, 2) > getword(%col.getWorldBoxCenter(),2) - 3.3*%scale);
            else if(%ez < -0.75)
               %shielded = (getword(%pos, 2) < getword(%col.getWorldBoxCenter(),2) - 4.4*%scale);
            else
               %shielded = (vectorDot(vectorNormalize(%obj.getVelocity()),%aimVec) < 0);
            
            %damageScale = 0;
            %reflect = $Shield::RiotReflects;
            %reflectVector = %aimVec;
            %reflectPoint = vectorAdd(%col.getHackPosition(),vectorScale(%reflectVector,vectorLen(%col.getVelocity())/5+1));
            %impulseScale = 0.3;
            if(%shielded)
               %col.spawnExplosion(hammerProjectile,getWord(%col.getScale(),2));
         }
      }
      
      if(getSimTime() - %obj.reflectTime < 500) %reflect = 0;
      
      if(%shielded)
      {
         //cancel radius damage on %obj
         %obj.damageCancel[%col] = 1;
         %obj.impulseScale[%col] = %impulseScale;
         serverPlay3d(wrenchMissSound,%pos);
         
         if(%reflect)
         {
            %scaleFactor = getWord(%obj.getScale(), 2);
            %pos = %reflectPoint;
            %vec = vectorScale(%reflectVector,vectorLen(%obj.getVelocity()));
            %vel = vectorAdd(%vec,vectorScale(%col.getVelocity(),%obj.dataBlock.velInheritFactor));
            %p = new Projectile()
            {
               dataBlock = %obj.dataBlock;
               initialPosition = %pos;
               initialVelocity = %vel;
               sourceObject = %obj;
               client = %col.client;
               sourceSlot = 0;
               originPoint = %pos;
               reflectTime = getSimTime();
            };
            MissionCleanup.add(%p);
            %p.setScale(%scaleFactor SPC %scaleFactor SPC %scaleFactor);
         }
         
         %obj.schedule(10,delete);
         
         //Special effect weapons like the Horse Ray will still affect you from the back
         if(%damageScale > 0)
         {
            %this.directDamage *= %damageScale;
            %ret = Parent::damage(%this,%obj,%col,%fade,%pos,%normal);
            %this.directDamage /= %damageScale;
         }
         
         return %ret;
      }
      else
      {
         return Parent::damage(%this,%obj,%col,%fade,%pos,%normal);
      }
   }
   
   function ProjectileData::radiusImpulse(%this, %obj, %col, %a, %pos, %b, %c)
   {
      if(%obj.damageCancel[%col])
      {
         %b = %b * %obj.impulseScale[%col];
      }
      
      return Parent::radiusImpulse(%this, %obj, %col, %a, %pos, %b, %c);
   }
   
   function ProjectileData::impactImpulse(%this, %obj, %col, %a)
   {
      if(%obj.damageCancel[%col])
      {
         %this.impactImpulse *= %obj.impulseScale[%col];
         %val = Parent::impactImpulse(%this, %obj, %col, %a);
         %this.impactImpulse /= %obj.impulseScale[%col];
         return %val;
      }
      
      return Parent::impactImpulse(%this, %obj, %col, %a);
   }
   
   function ShapeBase::damage(%this, %sourceObject, %pos, %directDamage, %damageType)
   {
      %image0 = %this.getMountedImage(0);
      %image1 = %this.getMountedImage(1);
      %image2 = %this.getMountedImage(2);
      %state0 = %this.getImageState(0);
      
      %fvec = %this.getForwardVector();
      %fX = getWord(%fvec,0);
      %fY = getWord(%fvec,1);
      
      %evec = %this.getEyeVector();
      %eX = getWord(%evec,0);
      %eY = getWord(%evec,1);
      %eZ = getWord(%evec,2);
      
      %eXY = mSqrt(%eX*%eX+%eY*%eY);
      
      %aimVec = %fX*%eXY SPC %fY*%eXY SPC %eZ;
      
      if(%damageType == $DamageType::Fall || %damageType == $DamageType::Impact)
      {
         %quit = 1;
         
         if(%damageType == $DamageType::Fall)
            %attackvec = "0 0 -1";
         else
            %attackvec = vectorNormalize(vectorSub(%this.getHackPosition(),%pos));
         
         %fvec = %this.getForwardVector();
         %fX = getWord(%fvec,0);
         %fY = getWord(%fvec,1);
         
         %evec = %this.getEyeVector();
         %eX = getWord(%evec,0);
         %eY = getWord(%evec,1);
         %eZ = getWord(%evec,2);
         
         %eXY = mSqrt(%eX*%eX+%eY*%eY);
         
         %aimVec = %fX*%eXY SPC %fY*%eXY SPC %eZ;
         if(%image0 == BulwarkImage.getID() && %state0 $= "Ready")
            %shielded = (vectorDot(%attackVec,%aimVec) > 0);
         
         if(%shielded && $Shield::RiotCancelFalling)
         {
            %this.spawnExplosion(hammerProjectile,getWord(%this.getScale(),2)*2);
            %directDamage = %directDamage/8;
         }
      }
      else if(!%quit && (%this.getType() & $TypeMasks::PlayerObjectType) && !%sourceObject.damageCancel[%obj])
      {
         %scale = getWord(%this.getScale(),2);
         
         %attackvec = vectorNormalize(vectorSub(%this.getHackPosition(),%pos));
         
         if(%image0 == BulwarkImage.getID() && %state0 $= "Ready")
         {
            if(%eZ > 0.75)
               %shielded = (getword(%pos, 2) > getword(%this.getWorldBoxCenter(),2) - 3.3*%scale);
            else if(%ez < -0.75)
               %shielded = (getword(%pos, 2) < getword(%this.getWorldBoxCenter(),2) - 4.4*%scale);
            else
               %shielded = (vectorDot(%attackvec,%aimVec) < 0);
            
            %damageScale = 0;
            if(vectorDist(%pos,"0 0 0") < 0.1) %shielded = 0;
            if(vectorDist(%pos,%this.getPosition()) < 0.1) %shielded = 0;
            if(%directDamage > 5000) %shielded = 0;
            
            if(%shielded)
               %this.spawnExplosion(hammerProjectile,getWord(%this.getScale(),2));
         }
         
         if(%shielded)
            %directDamage = %directDamage * %damageScale;
      }
      return Parent::damage(%this, %sourceObject, %pos, %directDamage, %damageType);
   }
};
activatepackage("Bulwark");