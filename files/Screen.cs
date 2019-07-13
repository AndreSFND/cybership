using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Text.RegularExpressions;

namespace Main{
	public abstract class Screen{
		public static uint width		= 500;
		public static uint height		= 400;
		public static bool fullscreen 	= false;

		public abstract void Draw();
		public abstract void OnExit();
		public abstract void OnStart();
		
		public static void Fullscreen(bool v, bool db){
			if(db)
				Conexao.ExecuteQuery("update configuracoes set valor = '"+v+"' where id = '1'");
			
			Screen.fullscreen = v;
			V.window.Close();
			V.window = null;
			
			if(v){
				Screen.width 	= VideoMode.DesktopMode.Width;
				Screen.height 	= VideoMode.DesktopMode.Height;

				V.hud.Size 				= new Vector2f(Screen.width, Screen.height);
				V.view.Size 			= new Vector2f(Screen.width, Screen.height);
				V.menuCreator.Size 		= new Vector2f(Screen.width, Screen.height-170);

				V.hud.Center 			= new Vector2f(Screen.width/2, Screen.height/2);
				V.menuCreator.Center	= new Vector2f(Screen.width/2, (float)(Screen.height-170)/2);

				V.menuCreator.Viewport 	= new FloatRect(((float)Screen.width-250)/(float)Screen.width, 60/(float)Screen.height, 1, ((float)Screen.height-170)/(float)Screen.height);

				V.window = new RenderWindow(new VideoMode(Screen.width, Screen.height), "Cybership v0.5.2", Styles.Fullscreen);
			}
			else{
				Screen.width 	= 800;
				Screen.height 	= 600;

				V.hud.Size 				= new Vector2f(Screen.width, Screen.height);
				V.view.Size 			= new Vector2f(Screen.width, Screen.height);
				V.menuCreator.Size 		= new Vector2f(Screen.width, Screen.height-170);

				V.hud.Center 			= new Vector2f(Screen.width/2, Screen.height/2);
				V.menuCreator.Center	= new Vector2f(Screen.width/2, (float)(Screen.height-170)/2);

				V.menuCreator.Viewport 	= new FloatRect(((float)Screen.width-250)/(float)Screen.width, 60/(float)Screen.height, 1, ((float)Screen.height-170)/(float)Screen.height);

				V.window = new RenderWindow(new VideoMode(Screen.width, Screen.height), "Cybership v0.5.2");
			}
			
			V.window.SetFramerateLimit(Configuracoes.vsync ? (uint)0 : (uint)120);
			V.window.SetVerticalSyncEnabled(Configuracoes.vsync);
			
			V.window.Closed 				+= new EventHandler(Idk.OnClose);
			V.window.Resized 				+= new EventHandler<SizeEventArgs>(Idk.OnResize);
			V.window.MouseMoved 			+= new EventHandler<MouseMoveEventArgs>(Idk.OnMouseMoved);
			V.window.MouseButtonPressed 	+= new EventHandler<MouseButtonEventArgs>(Idk.OnMouseClicked);
			V.window.MouseButtonReleased 	+= new EventHandler<MouseButtonEventArgs>(Idk.OnMouseReleased);
			V.window.MouseWheelScrolled 	+= new EventHandler<MouseWheelScrollEventArgs>(Idk.OnMouseWheel);
			V.window.TextEntered 			+= new EventHandler<TextEventArgs>(Idk.OnTextEntered);
			V.window.KeyPressed				+= new EventHandler<SFML.Window.KeyEventArgs>(Idk.OnKeyPressed);
		}
	}
}