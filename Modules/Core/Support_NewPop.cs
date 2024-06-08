//schedule pop rewrite so it isn't so schedule stupid
//this should only ever use 1 schedule :) so nice
if(!isObject($ItemPopSet))
{
	$ItemPopSet = new SimSet();
}

function Item::schedulePop(%obj)
{
	%obj.popTime = getSimTime() + $Game::Item::PopTime - 1000;
	$ItemPopSet.add(%obj);

	if($ItemPopSet.isLooping)
	{
		return;
	}

	//makes sure no quota objects delete the schedule
	%oldQuotaObject = getCurrentQuotaObject();
	if (isObject(%oldQuotaObject))
	{
		clearCurrentQuotaObject();
	}
	$ItemPopSet.isLooping = true;
	PopSet_Loop();
	if (isObject(%oldQuotaObject))
	{
		setCurrentQuotaObject(%oldQuotaObject);
	}
}

function Item::cancelPop(%obj)
{
	if(!$ItemPopSet.isMember(%obj))
	{
		return;
	}

	%obj.fading = false;
	%obj.setNodColor("ALL","0 0 0 1");
	%obj.startFade(0,0,0);
	$ItemPopSet.remove(%obj);
}

function PopSet_Loop()
{
	%set = $ItemPopSet;
	%count = %set.getCount();

	if(%count == 0)
	{
		%set.isLooping = false;
		return;
	}
	
	%time = getSimTime();
	for(%i = 0; %i < %count; %i++)
	{
		%item = %set.getObject(%i);
		
		if((%item.popTime - %time) > 0)
		{
			
			continue;
		}

		if(!%item.fading)
		{
			%item.startFade(1000,0,1);
			%item.fading = true;
		}
		
		%opacity = 1 + (%item.popTime - %time) / 1000;
		if(%opacity <= 0)
		{
			%item.schedule(0,"delete");
			continue;
		}
		
		%item.setNodeColor("ALL","0 0 0" SPC %opacity);
	}
	$ItemPopSet.scheduleloop = schedule(100,%set,"PopSet_Loop");
}