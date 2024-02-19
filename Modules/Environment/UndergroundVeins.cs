$EOTW::UGMinVeinSize = 1;
function spawnUndergroundVein(%matter, %position, %size)
{
    if (!isObject(UGVeinSet))
        $EOTW::UGVeinSet = new SimSet(UGVeinSet);
    
    %vein = new ScriptObject()
    {
        matter = getMatterType(%matter).name;
        position = getWords(%position, 0, 2);
        size = $EOTW::UGMinVeinSize;
        maxSize = %size;
        richness = 0.85 + (getRandom() * 0.3);
        ready = false;
    }

    UGVeinSet.add(%vein);
}

$EOTW::MaxUGVeins = 100;
$EOTW::AvgTimeToTickUGVein = 60000;
$EOTW::TicksToGrowUGVein = 100;
$EOTW::TicksToDeleteUGVein = 300;
function tickRandomUndergroundVein()
{
    cancel($EOTW::UGVeinLoop);

    if (!isObject(UGVeinSet))
        $EOTW::UGVeinSet = new SimSet(UGVeinSet);

    if (UGVeinSet.getCount() < $EOTW::MaxUGVeins)
    {
        //New vein
    }
    else
    {
        tickUndergroundVein(UGVeinSet.getObject(getRandom(0, UGVeinSet.getCount() - 1)));
    }

    $EOTW::UGVeinLoop = schedule($EOTW::AvgTimeToTickUGVein / $EOTW::MaxUGVeins, UGVeinSet, "tickRandomUndergroundVein");
}

function tickUndergroundVein(%vein)
{
    if (%vein.ready)
    {
        %vein.size -= %vein.maxSize / $EOTW::TicksToDeleteUGVein;
        if (%vein.size < $EOTW::UGMinVeinSize)
            %vein.delete();
    }
    else
    {
        %vein.size += %vein.maxSize / $EOTW::TicksToGrowUGVein;
        if (%vein.size >= %vein.maxSize)
            ready = true;
    }
}

function getUndergroundVeins(%position)
{
    %position = getWords(%position, 0, 2);
    %veinList = "";
    //Is there a better way to scan our list of veins? Instead of iterating over every single one.
    for (%i = 0; %i < UGVeinSet.getCount(); %i++)
    {
        %vein = UGVeinSet.getObject(%i);
        if (vectorDist(%vein.position, %position) < %vein.size)
            %veinList = trim(%veinList TAB %vein.getID());
    }
}