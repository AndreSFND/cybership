using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Text.RegularExpressions;

namespace Main{
	public class CurrentScreen{
		public static Screen CScreen;
		public static Dictionary<string, Screen> Screens = new Dictionary<string, Screen>();

		public static void Add(string nome, Screen Screen){
			Screens[nome] = Screen;
		}

		public static void Set(string nome){
			CScreen = Screens[nome];
	        CScreen.OnStart();
		}

		public static void Change(string nome){
			CScreen.OnExit();
	        CScreen = Screens[nome];
	        CScreen.OnStart();
		}

		public static void Draw(){
			CScreen.Draw();
		}

		public static Screen Screen(){
			return CScreen;
		}
	}
}