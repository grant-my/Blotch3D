﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using Blotch;

namespace BlotchExample
{
	/// <summary>
	/// The 3D window. This must inherit from BlWindow3D. See BlWindow3D for details.
	/// </summary>
	public class Example : BlWindow3D
	{
		/// <summary>
		/// This will be the torus model we draw in the window
		/// </summary>
		BlSprite Surface;

		/// <summary>
		/// This will be the font for the help menu we draw in the window
		/// </summary>
		SpriteFont Font;

		/// <summary>
		/// The help menu text that we draw in the window
		/// </summary>
		string Help = @"
Camera controls:
Dolly  -  Wheel
Zoom   -  Left-CTRL-wheel
Truck  -  Left-drag 
Rotate -  Right-drag
Pan    -  Left-CTRL-left-drag
Reset  -  Esc
Fine control  -  Left-Shift
";


		/// <summary>
		/// See BlWindow3D for details.
		/// </summary>
		protected override void Setup()
		{
			Graphics.Lights.Clear();
			var light = new BlGraphicsDeviceManager.Light();
			light.LightDiffuseColor = new Vector3(1, 1, .5f);
			light.LightDirection = new Vector3(1, 0, 0);
			Graphics.Lights.Add(light);

			// We need to create one ContentManager object for each top-level content folder we'll be
			// loading things from. Here "Content" is the most senior folder name of the content tree.
			// (Content [models, fonts, etc.] are added to the project with the Content utility. Double-click
			// 'Content.mgcb' in solution explorer.). You can create multiple content managers if content
			// is spread over diverse folders.
			var MyContent = new ContentManager(Services, "Content");

			// The font we will use to draw the menu on the screen.
			// "Arial14" is the pathname to the font file
			Font = MyContent.Load<SpriteFont>("Arial14");

			// load the terrain image
			var terrain = Graphics.LoadFromImageFile("terrain.png", true);

			// The vertices of the surface
			var SurfaceArray = BlGeometry.CreatePlanarSurface(terrain);

			// convert to vertex buffer
			var vertexBuf = BlGeometry.TrianglesToVertexBuffer(Graphics.GraphicsDevice, SurfaceArray);

			// The sprite we draw in this window
			Surface = new BlSprite(Graphics, "Surface");
			Surface.Mipmap = terrain;
			Surface.LODs.Add(vertexBuf);
			Surface.BoundSphere = new BoundingSphere(Vector3.Zero, 1);
			Surface.SetAllMaterialBlack();
			Surface.Color = new Vector3(1, 1, 1);
		}

		/// <summary>
		/// See BlWindow3D for details.
		/// </summary>
		/// <param name="timeInfo"></param>
		protected override void FrameProc(GameTime timeInfo)
		{
			//
			// Put your periodic code here
			//

			// Handle the standard mouse and keystroke functions. (Don't call this if you want some other behavior
			// of mouse and keys.)
			Graphics.DoDefaultGui();
		}
		/// <summary>
		/// See BlWindow3D for details.
		/// </summary>
		/// <param name="timeInfo">Provides a snapshot of timing values.</param>
		protected override void FrameDraw(GameTime timeInfo)
		{
			//
			// Draw things here using BlSprite.Draw(), graphics.DrawText(), etc.
			//

			Surface.Draw();

			var MyMenuText = String.Format("{0}\nEye: {1}\nLookAt: {2}\nMaxDistance: {3}\nMinDistance: {4}\nViewAngle: {5}\nModelLod: {6}\nModelApparentSize: {7}",
				Help,
				Graphics.Eye,
				Graphics.LookAt,
				Graphics.MaxCamDistance,
				Graphics.MinCamDistance,
				Graphics.Zoom,
				Surface.LodTarget,
				Surface.ApparentSize
			);

			// handle undrawable characters for the specified font(like the infinity symbol)
			try
			{
				Graphics.DrawText(MyMenuText, Font, new Vector2(50, 50));
			}
			catch { }
		}
	}
}