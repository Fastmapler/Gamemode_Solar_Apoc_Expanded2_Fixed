//Requires the respective DLL!!!!!!!!!!!
//    https://gitlab.com/Eagle517/move-handler    //

function onPlayerProcessTick(%player, %move)
{
    $MoveHandlerEnabled = true;
    %player.moveHandler = %move;
}