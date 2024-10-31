//--------------------------------------------------------------------------------------
//		Blockland Glass Prefs:
//--------------------------------------------------------------------------------------


// Register the Addon
registerPreferenceAddon("Script_PeggFootsteps", "Peggy Footsteps", "user_pirate");

// Control whether or not PeggyFootsteps is enabled
new ScriptObject(Preference)
{
	className      = "EnablePeggyFootstepsPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "Enabled FX";
	title          = "Enable Peggy Footsteps";

	type           = "bool";
	params         = "0 1";

	variable       = "$Pref::Server::PF::footstepsEnabled"; //global variable (optional)

	defaultValue   = "1";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};

// Enable BrickFX custom sounds: Chrome or Glow bricks = different footsteps
new ScriptObject(Preference)
{
	className      = "EnableBrickFXPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "Enabled FX";
	title          = "Enable BrickFX custom SoundFX";

	type           = "bool";
	params         = "0 1";

	variable       = "$Pref::Server::PF::brickFXSounds::enabled"; //global variable (optional)

	defaultValue   = "1";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};

// Enable Swimming Sounds
new ScriptObject(Preference)
{
	className      = "EnableSwimmingFXPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "Enabled FX";
	title          = "Enable Swimming SoundFX";

	type           = "bool";
	params         = "0 1";

	variable       = "$Pref::Server::PF::waterSFX"; //global variable (optional)

	defaultValue   = "1";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};


// Enable Landing FX
new ScriptObject(Preference)
{
	className      = "EnableLandingFXPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "Enabled FX";
	title          = "Enable Landing SoundFX";

	type           = "bool";
	params         = "0 1";

	variable       = "$Pref::Server::PF::landingFX"; //global variable (optional)

	defaultValue   = "1";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};

// Threshold speed for landing soundFX
new ScriptObject(Preference)
{
	className      = "LandingFXThresholdPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "Thresholds";
	title          = "Landing Speed Threshold for Sound";

	type           = "int";
	params         = "0 20";

	variable       = "$Pref::Server::PF::minLandSpeed"; //global variable (optional)

	defaultValue   = "12";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};

// Threshold speed for running soundFX
new ScriptObject(Preference)
{
	className      = "RunningFXThresholdPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "Thresholds";
	title          = "Running Speed Threshold for Sound";

	type           = "int";
	params         = "0 20";

	variable       = "$Pref::Server::PF::runningMinSpeed"; //global variable (optional)

	defaultValue   = "2.8";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};

// Default SoundFX
new ScriptObject(Preference)
{
	className      = "DefaultFootstepPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "Footsteps";
	title          = "Default Footstep";

	type           = "dropdown";
	params         = "Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9";

	variable       = "$Pref::Server::PF::defaultStep"; //global variable (optional)

	defaultValue   = "1";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};

// Terrain Footstep SoudFX
new ScriptObject(Preference)
{
	className      = "TerrainFootstepPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "Footsteps";
	title          = "Terrain Footstep SoundFX";

	type           = "dropdown";
	params         = "Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9";

	variable       = "$Pref::Server::PF::terrainStep"; //global variable (optional)

	defaultValue   = "1";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};

// Vehicle Footstep SoundFX
new ScriptObject(Preference)
{
	className      = "VehicleFootstepPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "Footsteps";
	title          = "Steps on Vehicles SoundFX";

	type           = "dropdown";
	params         = "Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9";

	variable       = "$Pref::Server::PF::vehicleStep"; //global variable (optional)

	defaultValue   = "1";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};

// Pearl Brick Footstep SoundFX
new ScriptObject(Preference)
{
	className      = "pearlStepPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "BrickFX Footsteps";
	title          = "Pearl Brick Step SoundFX";

	type           = "dropdown";
	params         = "Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9";

	variable       = "$Pref::Server::PF::pearlStep"; //global variable (optional)

	defaultValue   = "4";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};

// Chrome Brick Footstep SoundFX
new ScriptObject(Preference)
{
	className      = "chromeStepPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "BrickFX Footsteps";
	title          = "Chrome Brick Step SoundFX";

	type           = "dropdown";
	params         = "Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9";

	variable       = "$Pref::Server::PF::chromeStep"; //global variable (optional)

	defaultValue   = "4";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};

// Glow Brick Footstep SoundFX
new ScriptObject(Preference)
{
	className      = "glowStepPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "BrickFX Footsteps";
	title          = "Glow Brick Step SoundFX";

	type           = "dropdown";
	params         = "Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9";

	variable       = "$Pref::Server::PF::glowStep"; //global variable (optional)

	defaultValue   = "0";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};

// Glow Brick Footstep SoundFX
new ScriptObject(Preference)
{
	className      = "glowStepPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "BrickFX Footsteps";
	title          = "Glow Brick Step SoundFX";

	type           = "dropdown";
	params         = "Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9";

	variable       = "$Pref::Server::PF::glowStep"; //global variable (optional)

	defaultValue   = "0";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};

// Blink Brick Footstep SoundFX
new ScriptObject(Preference)
{
	className      = "blinkStepPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "BrickFX Footsteps";
	title          = "Blink Brick Step SoundFX";

	type           = "dropdown";
	params         = "Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9";

	variable       = "$Pref::Server::PF::blinkStep"; //global variable (optional)

	defaultValue   = "0";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};


// Rainbow Brick Footstep SoundFX
new ScriptObject(Preference)
{
	className      = "rainbowStepPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "BrickFX Footsteps";
	title          = "Rainbow Brick Step SoundFX";

	type           = "dropdown";
	params         = "Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9";

	variable       = "$Pref::Server::PF::rainbowStep"; //global variable (optional)

	defaultValue   = "0";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};


// Glow Brick Footstep SoundFX
new ScriptObject(Preference)
{
	className      = "unduloStepPref"; //namespace

	addon          = "Script_PeggFootsteps"; //add-on filename
	category       = "BrickFX Footsteps";
	title          = "Undulo Brick Step SoundFX";

	type           = "dropdown";
	params         = "Default 0 Basic 1 Dirt 2 Grass 3 Metal 4 Sand 5 Snow 6 Stone 7 Water 8 Wood 9";

	variable       = "$Pref::Server::PF::unduloStep"; //global variable (optional)

	defaultValue   = "8";

	updateCallback = ""; //to call after ::onUpdate (optional)
	loadCallback   = ""; //to call after ::onLoad (optional)

	hostOnly       = false; //default false (optional)
	secret         = false; //whether to tell clients the value was updated (optional)

	loadNow        = false; // load value on creation instead of with pool (optional)
	noSave         = false; // do not save (optional)
	requireRestart = false; // denotes a restart is required (optional)
};

// Set a global init value to true so we don't clutter the BLG Preference GUI
$PFGlassInit = true;
