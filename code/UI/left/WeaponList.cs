﻿using Sandbox;
using Sandbox.UI;
using Sandbox.UI.Construct;
using Sandbox.UI.Tests;
using System.Linq;

[Library]
public partial class WeaponList : Panel
{
	VirtualScrollPanel Canvas;

	public WeaponList()
	{
		AddClass( "spawnpage" );
		AddChild( out Canvas, "canvas" );

		Canvas.Layout.AutoColumns = true;
		Canvas.Layout.ItemWidth = 100;
		Canvas.Layout.ItemHeight = 100;
		Canvas.OnCreateCell = ( cell, data ) =>
		{
			if ( data is TypeDescription type )
			{
				var btn = cell.Add.Button( type.Title );
				btn.AddClass( "icon" );
				btn.AddEventListener( "onclick", () => ConsoleSystem.Run( "spawn_entity", type.ClassName ) );
				btn.Style.BackgroundImage = Texture.Load( FileSystem.Mounted, $"/entity/{type.ClassName}.png", false );
			}
		};

		LoadAllItem( false );
	}

	private void LoadAllItem( bool isreload )
	{
		if ( isreload )
			Canvas.Data.Clear();

		var ents = TypeLibrary.GetDescriptions<Carriable>()
									.Where( x => x.HasTag( "spawnable" ) )
									.OrderBy( x => x.Title )
									.ToArray();

		foreach ( var entry in ents )
		{
			Canvas.AddItem( entry );
		}
	}

	public override void OnHotloaded()
	{
		base.OnHotloaded();

		LoadAllItem( true );
	}
}
