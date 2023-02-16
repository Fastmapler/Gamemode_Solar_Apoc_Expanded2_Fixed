function Projectile::spawnHomingProjectile(%this)
{
	if(!isObject(%this.client) && %this.sourceObject.getClassName() !$= "AIPlayer")
		return;
	
	if(!isObject(%this) || vectorLen(%this.getVelocity()) == 0)
		return;
		
	%client = %this.client;
	%muzzle = vectorLen(%this.getVelocity());
	
	if(!isObject(%this.target) || %this.target.getState() $= "Dead" || %this.target.getMountedImage(0) == adminWandImage.getID() || vectorDist(%this.getPosition(),%this.target.getHackPosition()) > 30)
	{
		if (%this.sourceObject.getClassName() $= "AIPlayer")
		{
			%pos = %this.getPosition();
			%radius = 100;
			%searchMasks = $TypeMasks::PlayerObjectType;
			InitContainerRadiusSearch(%pos, %radius, %searchMasks);
			%minDist = 1000;
			while ((%searchObj = containerSearchNext()) != 0 )
			{
				if(isObject(%searchObj.client) && miniGameCanDamage(%searchObj.client,%this.sourceObject))
				{
					if(%searchObj.getState() $= "Dead")
						continue;
						
					if(%this.sourceObject == %searchObj)
						continue;
					
					if(%searchObj.getClassName() $= "AIPlayer")
						continue;
					
					if(%searchObj.getMountedImage(0) == adminWandImage.getID())
						continue;
					
					if(%searchObj.isCloaked)
						continue;
					
					%d = vectorDist(%pos,%searchObj.getPosition());
					if(%d < %minDist)
					{
						%minDist = %d;
						%found = %searchObj;
					}
				}
			}
		}
		else
		{
			%pos = %this.getPosition();
			%radius = 100;
			%searchMasks = $TypeMasks::PlayerObjectType;
			InitContainerRadiusSearch(%pos, %radius, %searchMasks);
			%minDist = 1000;
			while ((%searchObj = containerSearchNext()) != 0 )
			{
				if((miniGameCanDamage(%client,%searchObj)) == 1)
				{
					if(%searchObj.getState() $= "Dead")
						continue;
					
					//if(%client == %searchObj.client)
					if(%this.sourceObject == %searchObj)
						continue;
					
					if(%searchObj.getMountedImage(0) == adminWandImage.getID())
						continue;
					
					if(%searchObj.isCloaked)
						continue;
					
					%d = vectorDist(%pos,%searchObj.getPosition());
					if(%d < %minDist)
					{
						%minDist = %d;
						%found = %searchObj;
					}
				}
			}
		}
		
		if(isObject(%found))
			%this.target = %found;
		else
		{
			%this.schedule(300,spawnHomingRocket);
			return;
		}
	}
	
	%found = %this.target;
	
	%pos = %this.getPosition();
	%start = %pos;
	%end = %found.getHackPosition();
	%enemypos = %end;
	%vec = vectorNormalize(vectorSub(%end,%start));
	for(%i=0;%i<5;%i++)
	{
		%t = vectorDist(%start,%end) / vectorLen(vectorScale(getWord(%vec,0) SPC getWord(%vec,1),%muzzle));
		%velaccl = vectorScale(%accl,%t);
		
		%x = getword(%velaccl,0);
		%y = getword(%velaccl,1);
		%z = getWord(%velaccl,2);
		
		%x = (%x < 0 ? 0 : %x);
		%y = (%y < 0 ? 0 : %y);
		%z = (%z < 0 ? 0 : %z);
		
		%vel = vectorAdd(vectorScale(%found.getVelocity(),%t),%x SPC %y SPC %z);
		%end = vectorAdd(%enemypos,%vel);
		%vec = vectorNormalize(vectorSub(%end,%start));
	}
	
	%addVec = vectorAdd(%this.getVelocity(),vectorScale(%vec,180/vectorDist(%pos,%end)*(%muzzle*%this.getDataBlock().homingTurn)));
	%vec = vectorNormalize(%addVec);
	
    if (%this.initalSpawnTime == 0)
        %this.initalSpawnTime = getSimTime();
        
    if ((getSimTime() - %this.initalSpawnTime) < (%this.getDatablock().lifetime * 32))
    {
        %p = new Projectile()
        {
            dataBlock = %this.getDatablock();
            initialPosition = %pos;
            initialVelocity = vectorScale(%vec,%muzzle);
            sourceObject = %this.sourceObject;
            client = %this.client;
            sourceSlot = 0;
            originPoint = %this.originPoint;
            doneHomingDelay = 1;
            target = %this.target;
            reflectTime = %this.reflectTime;
            initalSpawnTime = %this.initalSpawnTime;
        };
        
        if(isObject(%p))
        {
            MissionCleanup.add(%p);
            %p.setScale(%this.getScale());
            %this.delete();
        }
    }
    else
        %this.delete();
	
}

package HomingProjectiles
{
	function Projectile::onAdd(%obj,%a,%b)
	{
		Parent::onAdd(%obj,%a,%b);
		if(%obj.getDatablock().isHoming)
		{
			if(!%obj.doneHomingDelay)
				%obj.schedule(300,spawnHomingProjectile);
			else
				%obj.schedule(75,spawnHomingProjectile);
		}
	}
};activatePackage(HomingProjectiles);