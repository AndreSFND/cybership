using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Data.SQLite;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;

namespace Main{
	public partial class Idk{
		public static Color windowColor;

		[STAThread]
		static void Main() {
			V.window.SetMouseCursorVisible(false);

			V.window.Closed 				+= new EventHandler(OnClose);
			V.window.Resized 				+= new EventHandler<SizeEventArgs>(OnResize);
			V.window.MouseMoved 			+= new EventHandler<MouseMoveEventArgs>(OnMouseMoved);
			V.window.MouseButtonPressed 	+= new EventHandler<MouseButtonEventArgs>(OnMouseClicked);
			V.window.MouseButtonReleased 	+= new EventHandler<MouseButtonEventArgs>(OnMouseReleased);
			V.window.MouseWheelScrolled 	+= new EventHandler<MouseWheelScrollEventArgs>(OnMouseWheel);
			V.window.TextEntered 			+= new EventHandler<TextEventArgs>(OnTextEntered);
			V.window.KeyPressed				+= new EventHandler<SFML.Window.KeyEventArgs>(OnKeyPressed);

			V.ImgLoad();
			windowColor = new Color(255, 255, 255);

			CurrentScreen.Add("login",   	new LoginState());
			CurrentScreen.Add("intro",   	new IntroState());
			CurrentScreen.Add("mainmenu",   new MainMenuState());
			CurrentScreen.Add("loadfile", 	new LoadFileState());
			CurrentScreen.Add("config", 	new ConfigState());
			CurrentScreen.Add("controles", 	new ControlesState());
			CurrentScreen.Add("joystick", 	new JoystickState());
			CurrentScreen.Add("createmap", 	new CreateMapState());
			CurrentScreen.Add("localmap", 	new LocalMapState());
			CurrentScreen.Add("worldmap", 	new WorldMapState());
			CurrentScreen.Add("battle", 	new BattleState());
			CurrentScreen.Add("video", 		new VideoState());
			CurrentScreen.Add("fimdemo", 	new FimDemoState());

			CurrentScreen.Set("intro");

			Image image = new Image("res/icon.png");
			V.window.SetIcon(image.Size.X, image.Size.Y, image.Pixels);
			
			while (V.window.IsOpen) {
				V.window.DispatchEvents();
				V.window.Clear(windowColor);

					try{
						Draw();
						CurrentScreen.Draw();
						DrawHUD();
					}
					catch(Exception e){
						Console.WriteLine(e);
					}

				V.window.Display();
			}
		}
		
		public static void OnClose(object sender, EventArgs e) {
			RenderWindow window = (RenderWindow)sender;
			window.Close();
		}

		public static void OnResize(object sender, SizeEventArgs e){
			Screen.width 	= e.Width;
			Screen.height 	= e.Height;

			V.hud.Size 				= new Vector2f(e.Width, e.Height);
			V.view.Size 			= new Vector2f(e.Width, e.Height);
			V.menuCreator.Size 		= new Vector2f(e.Width, e.Height-170);

			V.hud.Center 			= new Vector2f(e.Width/2, e.Height/2);
			V.menuCreator.Center	= new Vector2f(e.Width/2, (float)(e.Height-170)/2);

			V.menuCreator.Viewport 	= new FloatRect(((float)Screen.width-250)/(float)Screen.width, 60/(float)Screen.height, 1, ((float)Screen.height-170)/(float)Screen.height);
		}
		public static void OnMouseMoved(object sender, MouseMoveEventArgs e){
			V.mouseX = e.X;
			V.mouseY = e.Y;
		}
		public static void OnMouseClicked(object sender, MouseButtonEventArgs e){
			if(e.Button == Mouse.Button.Left)
				V.mouseButton = "Left";
			if(e.Button == Mouse.Button.Right)
				V.mouseButton = "Right";
			if(e.Button == Mouse.Button.Middle)
				V.mouseButton = "Middle";
		}
		public static void OnMouseReleased(object sender, MouseButtonEventArgs e){
			V.mouseButton = "";
		}
		public static void OnMouseWheel(object sender, MouseWheelScrollEventArgs e){
			V.mouseWheel = (int)e.Delta;
		}
		public static void OnTextEntered(object sender, TextEventArgs e){
			Regex word = new Regex(@"^[a-zA-Z0-9_+-.,!@#$%^&*();\/|<>""'´^~\u00C0-\u00FF ]*$");
			Match m = word.Match(e.Unicode);
			V.textEntered = m.Value;
		}
		public static void OnKeyPressed(object sender, SFML.Window.KeyEventArgs e){
			ControlesState.Change((int)e.Code);
		}
	}
}