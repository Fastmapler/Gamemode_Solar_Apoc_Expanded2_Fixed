$EOTW::PowerLevel[0] = 64; //LV
$EOTW::PowerLevel[1] = 256; //MV
$EOTW::PowerLevel[2] = 1024; //HV

exec("./Support_PowerNet.cs");
exec("./Brick_PowerGen.cs");
exec("./Brick_PowerUnits.cs");
exec("./Brick_Debug.cs");