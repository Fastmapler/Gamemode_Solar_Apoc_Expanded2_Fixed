function SetupUGVeinData()
{
	if (isObject(UGVeinData))
	{
		UGVeinData.deleteAll();
		UGVeinData.delete();
	}

    //Note: Due to the limits of floating points, going above 99 maxSize can result in infinite veins.
	new SimSet(UGVeinData)
	{
        new ScriptObject(UGVeinType) { matter="Brimstone"; weight=100; minSize=32; maxSize=64; countPerArea=64.0; };
        new ScriptObject(UGVeinType) { matter="Uraninite"; weight=50; minSize=32; maxSize=32; countPerArea=128.0; };
        new ScriptObject(UGVeinType) { matter="Fluorspar"; weight=50; minSize=64; maxSize=64; countPerArea=64.0; };
        
        new ScriptObject(UGVeinType) { matter="Crude Oil"; weight=100; minSize=32; maxSize=64; countPerArea=256.0; };
        new ScriptObject(UGVeinType) { matter="Light Oil"; weight=75; minSize=16; maxSize=32; countPerArea=128.0; };
        new ScriptObject(UGVeinType) { matter="Heavy Oil"; weight=75; minSize=16; maxSize=32; countPerArea=128.0; };
        new ScriptObject(UGVeinType) { matter="Sludge"; weight=50; minSize=16; maxSize=32; countPerArea=16.0; };
        new ScriptObject(UGVeinType) { matter="Water"; weight=50; minSize=16; maxSize=32; countPerArea=1024.0; };
        
    };

    $EOTW::UGVeinWeight = 0;
    for (%i = 0; %i < UGVeinData.getCount(); %i++)
        $EOTW::UGVeinWeight += UGVeinData.getObject(%i).weight;

}
SetupUGVeinData();

$EOTW::UGMinVeinSize = 0.5;
function spawnUndergroundVein(%veinType, %position)
{
    if (!isObject(UGVeinSet))
        $EOTW::UGVeinSet = new SimSet(UGVeinSet);

    if (vectorDist(%position, "0 0") < 1)
        return;
    
    %vein = new ScriptObject()
    {
        matter = %veinType.matter;
        position = getWords(%position, 0, 1);
        size = %veinType.minSize / 2;
        maxSize = getRandom(%veinType.minSize, %veinType.maxSize);
        richness = %veinType.countPerArea * (0.85 + (getRandom() * 0.3));
        ready = false;
    };

    UGVeinSet.add(%vein);

    return %vein;
}

function createNewRandomUGVein()
{
    if (!isObject(UGVeinSet))
        $EOTW::UGVeinSet = new SimSet(UGVeinSet);

    %position = getRandom(getWord($EOTW::WorldBounds, 0), getWord($EOTW::WorldBounds, 2)) SPC getRandom(getWord($EOTW::WorldBounds, 1), getWord($EOTW::WorldBounds, 3));
    %roll = getRandom() * $EOTW::UGVeinWeight;
    for (%i = 0; %i < UGVeinData.getCount() && %roll > 0; %i++)
    {
        %veinType = UGVeinData.getObject(%i);
        %roll -= %veinType.weight;
    }

    if (isObject(%veinType))
        return spawnUndergroundVein(%veinType, %position);

    return -1;
}

$EOTW::MaxUGVeins = 100;
$EOTW::AvgTimeToTickUGVein = 60000;
$EOTW::TicksToGrowUGVein = 120;
$EOTW::TicksToDeleteUGVein = 960;
function tickRandomUGVein()
{
    cancel($EOTW::UGVeinLoop);

    if (!isObject(UGVeinSet))
        $EOTW::UGVeinSet = new SimSet(UGVeinSet);

    if (UGVeinSet.getCount() < $EOTW::MaxUGVeins)
        createNewRandomUGVein();
    else
        tickUGVein(UGVeinSet.getObject(getRandom(0, UGVeinSet.getCount() - 1)));

    $EOTW::UGVeinLoop = schedule($EOTW::AvgTimeToTickUGVein / $EOTW::MaxUGVeins, UGVeinSet, "tickRandomUGVein");
}
tickRandomUGVein();

function tickUGVein(%vein)
{
    if (%vein.ready)
    {
        return;
        %vein.size -= %vein.maxSize / $EOTW::TicksToDeleteUGVein;
        if (%vein.size < $EOTW::UGMinVeinSize)
            %vein.delete();
    }
    else
    {
        %vein.size += %vein.maxSize / $EOTW::TicksToGrowUGVein;
        if (%vein.size >= %vein.maxSize)
            %vein.ready = true;
    }
}

//How much matter stuff is in a part of the circle
function getUGVeinComp(%vein, %position)
{
    %position = getWords(%position, 0, 1);
    %distAway = vectorDist(%vein.position, %position);
    %fullComp = $pi * mPow(%vein.size, 2) * %vein.richness;
    return getMax(0, %fullComp * (1 - (%distAway / %vein.size)));
}

function removeUGVeinOre(%vein, %amount)
{
    if (!%vein.ready|| getRandom() > 1 / (%vein.richness / %amount))
        return 0;

    %area = $pi * mPow(%vein.size, 2);
    %radius = %vein.size;

    %newRadius = mSqrt(getMax((%area - %amount) / $pi, 0));
    %vein.size = %newRadius;

    if (%vein.size < $EOTW::UGMinVeinSize)
            %vein.delete();

    return %radius - %newRadius;
}

function getUGVeins(%position)
{
    %position = getWords(%position, 0, 1);
    %veinList = "";
    //Is there a better way to scan our list of veins? Instead of iterating over every single one.
    for (%i = 0; %i < UGVeinSet.getCount(); %i++)
    {
        %vein = UGVeinSet.getObject(%i);
        if (vectorDist(%vein.position, %position) < %vein.size)
            %veinList = trim(%veinList TAB %vein.getID());
    }

    return %veinList;
}

function ServerCmdCurrentVeins(%client)
{
    for (%i = 0; %i < UGVeinSet.getCount(); %i++)
    {
        %vein = UGVeinSet.getObject(%i);
        %veinComp = getUGVeinComp(%vein, %vein.position);

        %client.chatMessage("<color:ffffff>" @ %vein.matter SPC "Vein, ~" @ %veinComp SPC "Units.");
    }
}