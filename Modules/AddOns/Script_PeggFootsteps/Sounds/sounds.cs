//--------------------------------------------------------------------------------------
//		Sound Datablocks:
//--------------------------------------------------------------------------------------

//++ Stone
datablock AudioProfile(StepStone1R_Sound) { 	fileName = "./stone/STONER01.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepStone2R_Sound) { 	fileName = "./stone/STONER02.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepStone3R_Sound) { 	fileName = "./stone/STONER03.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepStone4R_Sound) { 	fileName = "./stone/STONER04.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepStone5R_Sound) { 	fileName = "./stone/STONER05.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepStone6R_Sound) { 	fileName = "./stone/STONER06.wav"; 	description = AudioClose3d; 	preload = true; };
%a = 0;
$StepStoneR[%a++] = StepStone1R_Sound;
$StepStoneR[%a++] = StepStone2R_Sound;
$StepStoneR[%a++] = StepStone3R_Sound;
$StepStoneR[%a++] = StepStone4R_Sound;
$StepStoneR[%a++] = StepStone5R_Sound;
$StepStoneR[%a++] = StepStone6R_Sound;

datablock AudioProfile(StepStone1W_Sound) { 	fileName = "./stone/STONEW01.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepStone2W_Sound) { 	fileName = "./stone/STONEW02.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepStone3W_Sound) { 	fileName = "./stone/STONEW03.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepStone4W_Sound) { 	fileName = "./stone/STONEW04.wav"; 	description = AudioClosest3d; 	preload = true; };
%a = 0;
$StepStoneW[%a++] = StepStone1W_Sound;
$StepStoneW[%a++] = StepStone2W_Sound;
$StepStoneW[%a++] = StepStone3W_Sound;
$StepStoneW[%a++] = StepStone4W_Sound;

//++ wood
datablock AudioProfile(StepWood1R_Sound) { 	fileName = "./wood/woodR01.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepWood2R_Sound) { 	fileName = "./wood/woodR02.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepWood3R_Sound) { 	fileName = "./wood/woodR03.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepWood4R_Sound) { 	fileName = "./wood/woodR04.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepWood5R_Sound) { 	fileName = "./wood/woodR05.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepWood6R_Sound) { 	fileName = "./wood/woodR06.wav"; 	description = AudioClose3d; 	preload = true; };
%a = 0;
$StepWoodR[%a++] = StepWood1R_Sound;
$StepWoodR[%a++] = StepWood2R_Sound;
$StepWoodR[%a++] = StepWood3R_Sound;
$StepWoodR[%a++] = StepWood4R_Sound;
$StepWoodR[%a++] = StepWood5R_Sound;
$StepWoodR[%a++] = StepWood6R_Sound;

datablock AudioProfile(StepWood1W_Sound) { 	fileName = "./wood/woodW01.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepWood2W_Sound) { 	fileName = "./wood/woodW02.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepWood3W_Sound) { 	fileName = "./wood/woodW03.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepWood4W_Sound) { 	fileName = "./wood/woodW04.wav"; 	description = AudioClosest3d; 	preload = true; };
%a = 0;
$StepWoodW[%a++] = StepWood1W_Sound;
$StepWoodW[%a++] = StepWood2W_Sound;
$StepWoodW[%a++] = StepWood3W_Sound;
$StepWoodW[%a++] = StepWood4W_Sound;

//++ dirt
datablock AudioProfile(StepDirt1R_Sound) { 	fileName = "./dirt/dirtR01.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepDirt2R_Sound) { 	fileName = "./dirt/dirtR02.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepDirt3R_Sound) { 	fileName = "./dirt/dirtR03.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepDirt4R_Sound) { 	fileName = "./dirt/dirtR04.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepDirt5R_Sound) { 	fileName = "./dirt/dirtR05.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepDirt6R_Sound) { 	fileName = "./dirt/dirtR06.wav"; 	description = AudioClose3d; 	preload = true; };
%a = 0;
$StepDirtR[%a++] = StepDirt1R_Sound;
$StepDirtR[%a++] = StepDirt2R_Sound;
$StepDirtR[%a++] = StepDirt3R_Sound;
$StepDirtR[%a++] = StepDirt4R_Sound;
$StepDirtR[%a++] = StepDirt5R_Sound;
$StepDirtR[%a++] = StepDirt6R_Sound;

datablock AudioProfile(StepDirt1W_Sound) { 	fileName = "./dirt/dirtW01.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepDirt2W_Sound) { 	fileName = "./dirt/dirtW02.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepDirt3W_Sound) { 	fileName = "./dirt/dirtW03.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepDirt4W_Sound) { 	fileName = "./dirt/dirtW04.wav"; 	description = AudioClosest3d; 	preload = true; };
%a = 0;
$StepDirtW[%a++] = StepDirt1W_Sound;
$StepDirtW[%a++] = StepDirt2W_Sound;
$StepDirtW[%a++] = StepDirt3W_Sound;
$StepDirtW[%a++] = StepDirt4W_Sound;

//++ grass
datablock AudioProfile(StepGrass1R_Sound) { 	fileName = "./grass/grassR01.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepGrass2R_Sound) { 	fileName = "./grass/grassR02.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepGrass3R_Sound) { 	fileName = "./grass/grassR03.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepGrass4R_Sound) { 	fileName = "./grass/grassR04.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepGrass5R_Sound) { 	fileName = "./grass/grassR05.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepGrass6R_Sound) { 	fileName = "./grass/grassR06.wav"; 	description = AudioClose3d; 	preload = true; };
%a = 0;
$StepGrassR[%a++] = StepGrass1R_Sound;
$StepGrassR[%a++] = StepGrass2R_Sound;
$StepGrassR[%a++] = StepGrass3R_Sound;
$StepGrassR[%a++] = StepGrass4R_Sound;
$StepGrassR[%a++] = StepGrass5R_Sound;
$StepGrassR[%a++] = StepGrass6R_Sound;

datablock AudioProfile(StepGrass1W_Sound) { 	fileName = "./grass/grassW01.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepGrass2W_Sound) { 	fileName = "./grass/grassW02.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepGrass3W_Sound) { 	fileName = "./grass/grassW03.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepGrass4W_Sound) { 	fileName = "./grass/grassW04.wav"; 	description = AudioClosest3d; 	preload = true; };
%a = 0;
$StepGrassW[%a++] = StepGrass1W_Sound;
$StepGrassW[%a++] = StepGrass2W_Sound;
$StepGrassW[%a++] = StepGrass3W_Sound;
$StepGrassW[%a++] = StepGrass4W_Sound;

//++ water
datablock AudioProfile(StepWater1R_Sound) { 	fileName = "./water/waterR01.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepWater2R_Sound) { 	fileName = "./water/waterR02.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepWater3R_Sound) { 	fileName = "./water/waterR03.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepWater4R_Sound) { 	fileName = "./water/waterR04.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepWater5R_Sound) { 	fileName = "./water/waterR05.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepWater6R_Sound) { 	fileName = "./water/waterR06.wav"; 	description = AudioClose3d; 	preload = true; };
%a = 0;
$StepWaterR[%a++] = StepWater1R_Sound;
$StepWaterR[%a++] = StepWater2R_Sound;
$StepWaterR[%a++] = StepWater3R_Sound;
$StepWaterR[%a++] = StepWater4R_Sound;
$StepWaterR[%a++] = StepWater5R_Sound;
$StepWaterR[%a++] = StepWater6R_Sound;

datablock AudioProfile(StepWater1W_Sound) { 	fileName = "./water/waterW01.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepWater2W_Sound) { 	fileName = "./water/waterW02.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepWater3W_Sound) { 	fileName = "./water/waterW03.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepWater4W_Sound) { 	fileName = "./water/waterW04.wav"; 	description = AudioClosest3d; 	preload = true; };
%a = 0;
$StepWaterW[%a++] = StepWater1W_Sound;
$StepWaterW[%a++] = StepWater2W_Sound;
$StepWaterW[%a++] = StepWater3W_Sound;
$StepWaterW[%a++] = StepWater4W_Sound;

//++ sand
datablock AudioProfile(StepSand1R_Sound) { 	fileName = "./sand/SandRun01.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepSand2R_Sound) { 	fileName = "./sand/SandRun02.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepSand3R_Sound) { 	fileName = "./sand/SandRun03.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepSand4R_Sound) { 	fileName = "./sand/SandRun04.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepSand5R_Sound) { 	fileName = "./sand/SandRun05.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepSand6R_Sound) { 	fileName = "./sand/SandRun06.wav"; 	description = AudioClose3d; 	preload = true; };
%a = 0;
$StepSandR[%a++] = StepSand1R_Sound;
$StepSandR[%a++] = StepSand2R_Sound;
$StepSandR[%a++] = StepSand3R_Sound;
$StepSandR[%a++] = StepSand4R_Sound;
$StepSandR[%a++] = StepSand5R_Sound;
$StepSandR[%a++] = StepSand6R_Sound;

datablock AudioProfile(StepSand1W_Sound) { 	fileName = "./sand/sandW01.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepSand2W_Sound) { 	fileName = "./sand/sandW02.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepSand3W_Sound) { 	fileName = "./sand/sandW03.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepSand4W_Sound) { 	fileName = "./sand/sandW04.wav"; 	description = AudioClosest3d; 	preload = true; };
%a = 0;
$StepSandW[%a++] = StepSand1W_Sound;
$StepSandW[%a++] = StepSand2W_Sound;
$StepSandW[%a++] = StepSand3W_Sound;
$StepSandW[%a++] = StepSand4W_Sound;

//++ swimming
datablock AudioProfile(StepSwimming1_Sound) { 	fileName = "./swimming/swimmingR01.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepSwimming2_Sound) { 	fileName = "./swimming/swimmingR02.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepSwimming3_Sound) { 	fileName = "./swimming/swimmingR03.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepSwimming4_Sound) { 	fileName = "./swimming/swimmingR04.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepSwimming5_Sound) { 	fileName = "./swimming/swimmingR05.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepSwimming6_Sound) { 	fileName = "./swimming/swimmingR06.wav"; 	description = AudioClose3d; 	preload = true; };
%a = 0;
$StepSwimming[%a++] = StepSwimming1_Sound;
$StepSwimming[%a++] = StepSwimming2_Sound;
$StepSwimming[%a++] = StepSwimming3_Sound;
$StepSwimming[%a++] = StepSwimming4_Sound;
$StepSwimming[%a++] = StepSwimming5_Sound;
$StepSwimming[%a++] = StepSwimming6_Sound;

//++ snow
datablock AudioProfile(StepSnow1R_Sound) { 	fileName = "./snow/snowR01.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepSnow2R_Sound) { 	fileName = "./snow/snowR02.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepSnow3R_Sound) { 	fileName = "./snow/snowR03.wav"; 	description = AudioClose3d; 	preload = true; };
%a = 0;
$StepSnowR[%a++] = StepSnow1R_Sound;
$StepSnowR[%a++] = StepSnow2R_Sound;
$StepSnowR[%a++] = StepSnow3R_Sound;

//++ metal
datablock AudioProfile(StepMetal1R_Sound) { 	fileName = "./metal/metalR01.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepMetal2R_Sound) { 	fileName = "./metal/metalR02.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepMetal3R_Sound) { 	fileName = "./metal/metalR03.wav"; 	description = AudioClose3d; 	preload = true; };
%a = 0;
$StepMetalR[%a++] = StepMetal1R_Sound;
$StepMetalR[%a++] = StepMetal2R_Sound;
$StepMetalR[%a++] = StepMetal3R_Sound;

datablock AudioProfile(StepMetal1W_Sound) { 	fileName = "./metal/metalW01.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepMetal2W_Sound) { 	fileName = "./metal/metalW02.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepMetal3W_Sound) { 	fileName = "./metal/metalW03.wav"; 	description = AudioClosest3d; 	preload = true; };
%a = 0;
$StepMetalW[%a++] = StepMetal1W_Sound;
$StepMetalW[%a++] = StepMetal2W_Sound;
$StepMetalW[%a++] = StepMetal3W_Sound;

//++ basic
datablock AudioProfile(StepBasic1R_Sound) { 	fileName = "./basic/sn_walk_conc_1.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepBasic2R_Sound) { 	fileName = "./basic/sn_walk_conc_2.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepBasic3R_Sound) { 	fileName = "./basic/sn_walk_conc_3.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(StepBasic4R_Sound) { 	fileName = "./basic/sn_walk_conc_4.wav"; 	description = AudioClose3d; 	preload = true; };
%a = 0;
$StepBasicR[%a++] = StepBasic1R_Sound;
$StepBasicR[%a++] = StepBasic2R_Sound;
$StepBasicR[%a++] = StepBasic3R_Sound;
$StepBasicR[%a++] = StepBasic4R_Sound;

datablock AudioProfile(StepBasic1W_Sound) { 	fileName = "./basic/walkQuiet1.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepBasic2W_Sound) { 	fileName = "./basic/walkQuiet2.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepBasic3W_Sound) { 	fileName = "./basic/walkQuiet3.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(StepBasic4W_Sound) { 	fileName = "./basic/walkQuiet4.wav"; 	description = AudioClosest3d; 	preload = true; };
%a = 0;
$StepBasicW[%a++] = StepBasic1W_Sound;
$StepBasicW[%a++] = StepBasic2W_Sound;
$StepBasicW[%a++] = StepBasic3W_Sound;
$StepBasicW[%a++] = StepBasic4W_Sound;

//++ landing
datablock AudioProfile(LandHeavy_Sound) { 	fileName = "./landing/LandHeavy.wav"; 	description = AudioDefault3d; 	preload = true; };

datablock AudioProfile(LandMedium1_Sound) { 	fileName = "./landing/LandMedium_01.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(LandMedium2_Sound) { 	fileName = "./landing/LandMedium_02.wav"; 	description = AudioClose3d; 	preload = true; };
datablock AudioProfile(LandMedium3_Sound) { 	fileName = "./landing/LandMedium_03.wav"; 	description = AudioClose3d; 	preload = true; };
%a = 0;
$LandMedium[%a++] = LandMedium1_Sound;
$LandMedium[%a++] = LandMedium2_Sound;
$LandMedium[%a++] = LandMedium3_Sound;

datablock AudioProfile(LandLite1_Sound) { 	fileName = "./landing/LandLite_01.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(LandLite2_Sound) { 	fileName = "./landing/LandLite_02.wav"; 	description = AudioClosest3d; 	preload = true; };
datablock AudioProfile(LandLite3_Sound) { 	fileName = "./landing/LandLite_03.wav"; 	description = AudioClosest3d; 	preload = true; };
%a = 0;
$LandLite[%a++] = LandLite1_Sound;
$LandLite[%a++] = LandLite2_Sound;
$LandLite[%a++] = LandLite3_Sound;
