//////////////////////////////////////////////////////////////
//              Support_AutoSaver - Packaging               //
//////////////////////////////////////////////////////////////

//If we make any kind of modification (not events), then we will make it autosave. There is no easy way to tell if an event is changed, so I am not going to bother.
package Server_Autosaver
{
	//If you re-exec this add-on this function can possibly break (sometimes changing the gamemode causes the problem)
	//I have no idea why this happens, not on my side
	function ServerLoadSaveFile_End()
	{
		Parent::ServerLoadSaveFile_End();
		Autosaver_BootUp();
	}

	//Restarting the server could break the init prefs - This should prevent that
	function onServerDestroyed()
	{
		Parent::onServerDestroyed();
		$Server::EOTW_AS::Init = 0;
		$Server::EOTW_AS::HasAutoLoaded = 0;
	}

	function fxDtsBrick::setColor(%this, %id)
	{
		$Server::EOTW_AS["BrickChanged"] = 1;
		Parent::setColor(%this, %id);
	}

	function fxDtsBrick::setColorFX(%this, %id)
	{
		$Server::EOTW_AS["BrickChanged"] = 1;
		Parent::setColorFX(%this, %id);
	}

	function fxDtsBrick::setItem(%this, %id, %c)
	{
		$Server::EOTW_AS["BrickChanged"] = 1;
		return Parent::setItem(%this, %id, %c);
	}

	function fxDtsBrick::setLight(%this, %id, %c)
	{
		$Server::EOTW_AS["BrickChanged"] = 1;
		return Parent::setLight(%this, %id, %c);
	}

	function fxDtsBrick::setEmitter(%this, %id, %c)
	{
		$Server::EOTW_AS["BrickChanged"] = 1;
		return Parent::setEmitter(%this, %id, %c);
	}

	function fxDtsBrick::setItemDirection(%this, %id, %c)
	{
		$Server::EOTW_AS["BrickChanged"] = 1;
		return Parent::setItemDirection(%this, %id, %c);
	}

	function fxDtsBrick::setItemPosition(%this, %id, %c)
	{
		$Server::EOTW_AS["BrickChanged"] = 1;
		return Parent::setItemPosition(%this, %id, %c);
	}

	function fxDtsBrick::setItemRespawnTime(%this, %time, %c)
	{
		$Server::EOTW_AS["BrickChanged"] = 1;
		return Parent::setItemRespawnTime(%this, %time, %c);
	}

	function fxDtsBrick::setNTObjectName(%this, %name)
	{
		$Server::EOTW_AS["BrickChanged"] = 1;
		return Parent::setNTObjectName(%this, %name);
	}

	function fxDtsBrick::setVehicle(%this, %vehicle, %c)
	{
		$Server::EOTW_AS["BrickChanged"] = 1;
		return Parent::setVehicle(%this, %vehicle, %c);
	}


	function serverDirectSaveFileLoad (%filename, %colorMethod, %dirName, %ownership, %silent)
	{
		echo ("Direct load " @ %filename @ ", " @ %colorMethod @ ", " @ %dirName @ ", " @ %ownership @ ", " @ %silent);
		if (!isFile (%filename))
		{
			MessageAll ('', "ERROR: File \"" @ %filename @ "\" not found.  If you are seeing this, you broke something.");
			return;
		}
		$LoadingBricks_ColorMethod = %colorMethod;
		$LoadingBricks_DirName = %dirName;
		$LoadingBricks_Ownership = %ownership;
		if ($LoadingBricks_Ownership $= "")
		{
			$LoadingBricks_Ownership = 1;
		}
		calcSaveOffset ();
		if ($LoadingBricks_Client && $LoadingBricks_Client != 1)
		{
			MessageAll ('', "Load interrupted by host.");
			if (isObject ($LoadBrick_FileObj))
			{
				$LoadBrick_FileObj.close ();
				$LoadBrick_FileObj.delete ();
			}
		}
		$LoadingBricks_Client = findLocalClient ();
		if ($LoadingBricks_Client)
		{
			if ($LoadingBricks_Ownership == 2)
			{
				$LoadingBricks_BrickGroup = "BrickGroup_888888";
			}
			else 
			{
				$LoadingBricks_BrickGroup = $LoadingBricks_Client.brickGroup;
			}
		}
		else 
		{
			if ($Server::LAN)
			{
				if ($LoadingBricks_Ownership == 2)
				{
					$LoadingBricks_BrickGroup = "BrickGroup_888888";
				}
				else 
				{
					$LoadingBricks_BrickGroup = "BrickGroup_999999";
				}
			}
			else if ($LoadingBricks_Ownership == 2)
			{
				$LoadingBricks_BrickGroup = "BrickGroup_888888";
			}
			else 
			{
				$LoadingBricks_BrickGroup = "BrickGroup_" @ getMyBLID ();
			}
			$LoadingBricks_BrickGroup.isAdmin = 1;
			$LoadingBricks_BrickGroup.brickGroup = $LoadingBricks_BrickGroup;
			$LoadingBricks_Client = $LoadingBricks_BrickGroup;
		}
		$LoadingBricks_Silent = %silent;
		if (!$LoadingBricks_Silent)
		{
			MessageAll ('MsgUploadStart', "Loading bricks. Please wait.");
		}
		$LoadingBricks_StartTime = getSimTime ();

		//Fallback check if we don't have a brick group.
		if (!isObject($LoadingBricks_BrickGroup))
			$LoadingBricks_BrickGroup = "BrickGroup_888888";
		if (!isObject($LoadingBricks_Client))
			$LoadingBricks_Client = "BrickGroup_888888";

		ServerLoadSaveFile_Start (%filename);
	}

};
activatePackage("Server_Autosaver");