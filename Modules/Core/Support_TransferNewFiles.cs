if(!isObject(ActiveDownloadSet))
	new SimSet(ActiveDownloadSet);

function transmitNewFiles()
{
	if(ActiveDownloadSet.getCount() != 0)
	{
		messageAll('', "\c6Downloads are already in progress! Cannot start again until finished.");
		return;
	}

	messageAll('', "\c6Starting download of new files...");

	setManifestDirty();
	%hash = snapshotGameAssets();

	for(%i = 0; %i < ClientGroup.getCount(); %i++)
	{
		%client = ClientGroup.getObject(%i);

		if(!%client.hasSpawnedOnce)
		{
			commandToClient(%client, 'GameModeChange');
			%client.schedule(10, delete, "Please rejoin!");
		}
		else
		{
			%client.sendManifest(%hash);
			ActiveDownloadSet.add(%client);
		}
	}
}

package ReDownload
{
	function serverCmdBlobDownloadFinished(%client)
	{
		parent::serverCmdBlobDownloadFinished(%client);

		if(ActiveDownloadSet.isMember(%client))
		{
			ActiveDownloadSet.remove(%client);

			if(ActiveDownloadSet.getCount() == 0)
				schedule(1000, 0, messageAll, '', "\c6All clients finished downloading the new files!");
		}
	}

	function GameConnection::onClientLeaveGame(%client)
	{
		if(ActiveDownloadSet.isMember(%client))
		{
			ActiveDownloadSet.remove(%client);

			if(ActiveDownloadSet.getCount() == 0)
				schedule(1000, 0, messageAll, '', "\c6All clients finished downloading the new files!");
		}

		parent::onClientLeaveGame(%client);
	}
};
activatePackage(ReDownload);