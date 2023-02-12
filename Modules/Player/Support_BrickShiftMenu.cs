// Support_brickShiftMenu.cs by Oxy (260031)

$BSM::ROT = 0; // rotate id
$BSM::MOV = 1; // move id
$BSM::PLT = 2; // plant id
$BSM::CLR = 3; // clear id

function bsm()
{
	exec("./support_brickshiftmenu.cs");
}

function idxLoop(%id, %min, %max)
{
	if(%id < %min)
		%id = %max - 1;
	else if(%id >= %max)
		%id = %min;

	return %id;
}

function cleanup(%obj)
{
	while(isObject(%obj))
		%obj.delete();
}

package brickShiftMenuSupport
{
	function serverCmdPlantBrick(%cl, %idx)
	{
		if(%cl.brickShiftMenu !$= "")
		{
			%cl.brickShiftMenuPlant();
			return;
		}
		
		Parent::serverCmdPlantBrick(%cl, %idx);
	}

	function serverCmdCancelBrick(%cl, %idx)
	{
		if(%cl.brickShiftMenu !$= "")
		{
			%cl.brickShiftMenuClear();
			return;
		}
		
		Parent::serverCmdCancelBrick(%cl, %idx);
	}

	function serverCmdShiftBrick(%cl, %x, %y, %z)
	{
		if(%cl.brickShiftMenu !$= "")
		{
			%cl.brickShiftMenuMove(vectorNormalize(%y * -1 SPC %x SPC %z));
			return;
		}

		Parent::serverCmdShiftBrick(%cl, %x, %y, %z);
	}

	function serverCmdSuperShiftBrick(%cl, %x, %y, %z)
	{
		if(%cl.brickShiftMenu !$= "")
		{
			%cl.brickShiftMenuMove(vectorNormalize(%y * -1 SPC %x SPC %z));
			return;
		}

		Parent::serverCmdSuperShiftBrick(%cl, %x, %y, %z);
	}

	function serverCmdRotateBrick(%cl, %dir)
	{
		if(%cl.brickShiftMenu !$= "")
		{
			%cl.brickShiftMenuRotate(mClamp(%dir, -1, 1));
			return;
		}

		Parent::serverCmdRotateBrick(%cl, %dir);
	}
};
activatePackage(brickShiftMenuSupport);

function GameConnection::brickShiftMenuEnd(%cl)
{
	if(isObject(%bsm = %cl.brickShiftMenu) && %bsm.superClass $= "BSMObject")
		%bsm.onUserEnd(%cl);

	cancel(%cl.brickShiftLoop);
	%cl.brickShiftMenu = "";
	%cl.brickShiftLoop = "";
	%cl.centerprint("", 0);

	if (%bsm.deleteOnFinish)
		%bsm.delete();
}

function GameConnection::brickShiftMenuStart(%cl, %id)
{
	%cl.brickShiftMenu = %id;
	%cl.brickShiftLoop = %cl.schedule(0, brickShiftMenuLoop, %id);

	if(%id.superClass $= "BSMObject")
		%id.onUserStart(%cl);
}

function GameConnection::brickShiftMenuLoop(%cl)
{
	cancel(%cl.brickShiftLoop);

	if(%cl.brickShiftMenu $= "")
	{
		%cl.brickShiftMenuEnd();
		return;
	}

	if((%obj = %cl.brickShiftMenu).superClass $= "BSMObject")
	{
		%obj.onUserLoop(%cl);
	}
	
	%cl.brickShiftLoop = %cl.schedule(100, brickShiftMenuLoop);
}

function GameConnection::brickShiftMenuClear(%cl)
{
	if((%obj = %cl.brickShiftMenu).superClass $= "BSMObject")
	{
		%obj.onUserMove(%cl, getField(%obj.entry[%cl.selid], 1), $BSM::CLR);
		// talk("clr");
	}
}

function GameConnection::brickShiftMenuPlant(%cl)
{
	if((%obj = %cl.brickShiftMenu).superClass $= "BSMObject")
	{
		%obj.onUserMove(%cl, getField(%obj.entry[%cl.selid], 1), $BSM::PLT);
		// talk("plant");
	}
}

function GameConnection::brickShiftMenuMove(%cl, %dir)
{
	if((%obj = %cl.brickShiftMenu).superClass $= "BSMObject")
	{
		%obj.onUserMove(%cl, getField(%obj.entry[%cl.selid], 1), $BSM::MOV, %dir);
		// talk("shift" SPC %dir);
	}
}

function GameConnection::brickShiftMenuRotate(%cl, %dir)
{
	if((%obj = %cl.brickShiftMenu).superClass $= "BSMObject")
	{
		%obj.onUserMove(%cl, getField(%obj.entry[%cl.selid], 1), $BSM::ROT, %dir);
		// talk("rotate" SPC %dir);
	}
}

function BSMObject::getSource(%obj)
{
	%src = %obj;

	for(%i = 0; %i < 256; %i++)
	{
		if(!isObject(%src.parent))
			return %src;
		
		%src = %src.parent;
	}
}

function BSMObject::printToClient(%obj, %cl)
{
	if(getFieldCount(%obj.title) != 1 || getFieldCount(%obj.format) != 5)
	{
		warn("BSMObject::printToClient() - invalid BSM title or format field count");
		return;
	}

	%str = %obj.title @ "<font:" @ getField(%obj.format, 0) @ ">";

	%idx = %cl.selid;
	%act = %cl.actid;

	if(%act $= "")
		%act = -1;

	if (%obj.cut !$= "")
		%cut = %obj.cut;
	else
		%cut = 3;

	for(%i = 0; %i < %obj.entryCount; %i++)
	{
		%entry = trim(getField(%obj.entry[%i], 0));
		if(%entry $= "")
		{
			warn("BSMObject::printToClient() - empty entry " @ %i);
			continue;
		}

		%entid = trim(getField(%obj.entry[%i], 1));

		// man I'm really good at making unreadable code
		if(%cl.selid < %obj.entryCount - %cut)
		{
			if(mAbs(%cl.selid - %i) > %cut && %cl.selid > %cut || %i > %cut * 2 && %cl.selid <= %cut)
				continue;
		}
		else
		{
			if(%i < %obj.entryCount - %cut * 2 - 1)
				continue;
		}

		%form = getField(%obj.format, (%act == %i || %obj.highlight[%entid] ? (%idx == %i ? 3 : 1) : (%idx == %i ? 2 : 4)));
		%str = %str @ "<br>" @ %form;
		%str = %str @ %entry;
	}

	%cl.centerprint(%str, 1);
}

function BSMObject::onUserMove(%obj, %cl, %id, %move, %val)
{
	switch(%move)
	{
		case ($BSM::PLT):
			if(isObject(%next = %obj.submenu[%id]))
			{
				if(!%next.hideOnDeath || isObject(%cl.Player))
				{
					%obj.onUserEnd(%cl);
					%cl.brickShiftMenu = %next;
					%next.onUserStart(%cl);
				}
			}
			else
			{
				if(%id $= "closeMenu")
				{
					%cl.brickShiftMenuEnd();
				}
				else if(!%obj.blockSelect[%id] && !%obj.disableSelect)
				{
					if(%cl.actid != %cl.selid)
					{
						%pre = %cl.actid;
						%cl.actid = %cl.selid;
						%obj.onUserSelect(%cl, %pre, %cl.actid);
					}
					else
					{
						%pre = %cl.actid;
						%cl.actid = -1;
						%obj.onUserSelect(%cl, %pre, %cl.actid);
					}
				}
			}

		case ($BSM::MOV):
			%dir = getWord(%val, 1);

			%id = %cl.selid;
			%id -= %dir;
			%id = idxLoop(%id, 0, %obj.entryCount);

			%cl.selid = %id;

		case ($BSM::ROT):

		case ($BSM::CLR):
			%src = %obj.getSource();

			if(%src.getID() == %obj.getID())
			{
				%cl.brickShiftMenuEnd();
			}
			else
			{
				%obj.onUserEnd(%cl);
				%cl.brickShiftMenu = %obj.parent;
				%obj.parent.onUserStart(%cl);
			}
	}

	if(isObject(%next = %cl.brickShiftMenu))
		%next.onUserLoop(%cl);
}

function BSMObject::onUserSelect(%obj, %cl, %pre, %post)
{
	if(%pre > 0)
	{
		%cl.actid = -1;
	}
}

function BSMObject::onUserStart(%obj, %cl)
{
	%cl.selid = 0;
	%cl.actid = -1;
	%obj.onUserLoop(%cl);
}

function BSMObject::onUserEnd(%obj, %cl)
{
	%cl.selid = "";
	%cl.actid = "";
}

function BSMObject::onUserLoop(%obj, %cl)
{
	if(%obj.hideOnDeath && !isObject(%cl.Player))
	{
		%src = %obj.getSource();

		if(%src.getID() == %obj.getID() || %obj.parent.hideOnDeath)
		{
			%obj.onUserEnd(%cl);
			%cl.brickShiftMenuEnd();
		}
		else
		{
			%cl.brickShiftMenu = %obj.parent;
			%obj.parent.onUserStart(%cl);
		}
		return;
	}
	
	%obj.printToClient(%cl);
}

function getTempBSM(%name)
{
	%bsm = new ScriptObject()
	{
		superClass = "BSMObject";
        class = %name;
        
		title = "Loading...";
		format = "arial:24" TAB "\c2" TAB "\c6" TAB "\c2" TAB "\c7";

		entryCount = 0;

        hideOnDeath = true;
        deleteOnFinish = true;
	};

	return %bsm;
}