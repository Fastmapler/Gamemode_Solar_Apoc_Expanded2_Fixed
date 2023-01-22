// client.cs contains all clientside stuff for bots, wrench gui and such

exec( "./wrenchBotDlg.gui" );
exec( "./wrenchBotDlg.cs" );
// function wrenchBotDlg::send( %this, %a )
// {

// }

if( $pref::server::isDebugCrap )
	echo( "add-ons/bot_hole/client.cs executed" );
	
function wrenchBotDlg::respawn( %this )
{
	commandToServer( 'RespawnCurrentHoleBot' );
}

function wrenchBotDlg::botOff( %this )
{
	commandToServer( 'CurrentHoleBotOff' );
}

function wrenchBotDlg::botOn( %this )
{
	commandToServer( 'CurrentHoleBotOn' );
}


function clientCmdOpenWrenchBotHack()
{
	// echo( "wrench bot hack called" );
	// $client::isWrenchBotHack = 1;
	
			
	// echo( "opening bot wrench dlg" );
	// if( $client::isWrenchBotHack )
	// {
		// echo( "clearing wrenchBotHack" );
		// $client::isWrenchBotHack = 0;
	
	%dlg = wrenchBotDlg;
	canvas.popDialog( wrenchDlg );
	clientCmdOpenwrenchBotDlg(%dlg.Hid, %dlg.HallowNamedTargets, %dlg.HadminOverride, %dlg.HadminOnlyEvents);
	// }
}

package ClientBotHolePackage
{
	// packeged so bot wrench dlg closes when event dialog send is called
	function wrenchEventsDlg::send()
	{
		canvas.popDialog( wrenchBotDlg );
		parent::send();
	}

	function clientCmdSetWrenchData( %data )
	{
		parent::clientCmdSetWrenchData( %data );
	}
	
	function clientCmdWrenchLoadingDone()
	{
		wrenchBot_LoadingWindow.setVisible(false);
		
		parent::clientCmdWrenchLoadingDone();
	}
	
	function clientCmdOpenWrenchDlg(%id, %allowNamedTargets, %adminOverride, %adminOnlyEvents)
	{
		wrenchBotDlg.Hid = %id;
		wrenchBotDlg.HallowNamedTargets = %allowNamedTargets;
		wrenchBotDlg.HadminOverride = %adminOverride;
		wrenchBotDlg.HadminOnlyEvents = %adminOnlyEvents;
		
		parent::clientCmdOpenWrenchDlg(%id, %allowNamedTargets, %adminOverride, %adminOnlyEvents);
	}

	function clientCmdSetwrenchData(%data)
	{
		parent::clientCmdSetwrenchData(%data);
		
		clientCmdSetwrenchBotData(%data);
	}
	
	function clientCmdwrench_LoadMenus()
	{
		parent::clientCmdwrench_LoadMenus();
	
		wrenchBotDlg.loadDatablocks();
	}
};
activatePackage( ClientBotHolePackage );