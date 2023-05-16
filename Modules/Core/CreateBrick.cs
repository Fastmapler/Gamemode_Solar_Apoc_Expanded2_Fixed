function CreateBrick(%cl, %data, %pos, %color, %angleID, %checkSpace)
{
	if(!isObject(%data) || %data.getClassName() !$= "fxDTSBrickData")
		return -1;
	if(getWordCount(%pos) != 3)
		return -1;
	if (%checkSpace)
	{
		%boxsize = vectorScale((0.5 * %data.bricksizeX) SPC (0.5 * %data.bricksizeY) SPC (0.2 * %data.bricksizeZ), 0.5);
		initContainerBoxSearch(%pos, %boxsize, $TypeMasks::FxBrickAlwaysObjectType);
		
		%hit = containerSearchNext();
		if (isObject(%hit))
		{
			//createBoxMarker(%pos, '1 0 0 0.5', %boxsize).schedule(2000, "delete");
			//talk("pinis " @ %hit);
			return -1;
		}
			
	}
	if(%angleID $= "")
		%angleID = 0;
	if(isObject(%cl) && (%cl.getClassName() $= "GameConnection" | %cl.getClassName() $= "AIConnection"))
	{
		%blid = %cl.bl_id;
		if(%blid $= "")
			%blid = -1;
		%flag = 1;
	}
	else
	{
		if(isObject(%cl) && %cl.getClassName() $= "SimGroup" && MainBrickgroup.isMember(%cl))
			%group = %cl;
		%cl = 0;
	}
	switch(%angleID)
	{
		case 0:
			%rot = "1 0 0 0";
		case 1:
			%rot = "0 0 1 90";
		case 2:
			%rot = "0 0 1 180";
		case 3:
			%rot = "0 0 -1 90";
	}
	(%brick = new fxDtsBrick()
	{
		client = %cl;
		colorFxID = 0;
		colorID = %color;
		datablock = %data;
		isPlanted = 1;
		position = getWord(%pos, 0) SPC getWord(%pos, 1) SPC getWord(%pos, 2);
		rotation = %rot;
		shapeFxID = 0;
		stackBL_ID = %blid;
	}).angleID = %angleID;
	%err = %brick.plant();
	%brick.setTrusted(1);
	if(%flag && isObject(%cl.brickgroup))
		%cl.brickgroup.add(%brick);
	else if(isObject(%group))
		%group.add(%brick);
	return %brick TAB %err;
}

function getClosestColor(%color)
{
	for(%i=0;%i<getWordCount(%color);%i++)
		if(getWord(%color, %i) > 1)
			%flag = 1;
	if(%flag)
	{
		for(%i=0;%i<getWordCount(%color);%i++)
			%newCol = %newCol SPC getWord(%color, %i) / 255;
		%color = %newCol;
	}
	%color = trim(%color);
	if(getWordCount(%color) == 3)
		%color = %color @ " 1.000000";
	%lowDiff = 10000;
	for(%i=0;%i<64;%i++)
	{
		%flag = 0;
		%test = getColorIDTable(%i);
		for(%j=0;%j<getWordCount(%test);%j++)
			if(getWord(%test, %j) > 1)
				%flag = 1;
		if(%flag)
		{
			for(%j=0;%j<getWordCount(%test);%j++)
				%newCol = %newCol SPC getWord(%test) / 255;
			%test = %newCol;
		}
		trim(%test);
		%diff = mAbs(getWord(%color,0)-getWord(%test,0));
		%diff += mAbs(getWord(%color,1)-getWord(%test,1));
		%diff += mAbs(getWord(%color,2)-getWord(%test,2));
		%diff += mAbs(getWord(%color,3)-getWord(%test,3))*3;
		if(%diff < %lowDiff)
		{
			%lowDiff = %diff;
			%lowColor = %i;
		}
	}
	return %lowColor;
}