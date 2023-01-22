$Pref::Server::SAEX2::DevMode = false;

//Verify we are using the actual gamemode and not a custom mode.
if ($GameModeArg !$= "Add-Ons/Gamemode_Solar_Apoc_Expanded2_Fixed/Gamemode.txt")
{
	error("ERROR: Gamemode_Solar_Apoc_Expanded2_Fixed should not be used in custom games.");
	return;
}

//World Size.
function getMapArea()
{
	return (getWord($EOTW::WorldBounds, 2) - getWord($EOTW::WorldBounds, 0)) * (getWord($EOTW::WorldBounds, 3) - getWord($EOTW::WorldBounds, 1));
}

//Select which modules we wish to execute.
$EOTW::Modules = "Core Environment Matter Player AddOns Fauna Tools Weapons";

function LoadModules()
{
	for (%i = 0; %i < getWordCount($EOTW::Modules); %i++)
		exec("./Modules/" @ getWord($EOTW::Modules, %i) @ "/" @ getWord($EOTW::Modules, %i) @ ".cs");
}
LoadModules();

//One of our goals is to keep the code as organized as possible. We split each section of code into their own
//folders. Even though loading more files may take longer, the better code organization and readability is worth it.

//We also should try to keep external add-on requirements to a mininum, barring default blockland add-ons. This will make
//Solar apoc much easier to run, especially when setting up new servers.
//External add-ons are fine if they are not required to run the server (ie new bricks), but try to keep modification to a minimal.