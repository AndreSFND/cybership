using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace Main{
	public class LocalMapState : Screen{

		public static bool menu = true;

		public override void Draw(){
			V.window.SetView(V.hud);
			if(V.window.Size.X < 800 || V.window.Size.Y < 600) V.window.Size = new Vector2u(800, 600);

			TelaPrincipal();
		}

		public override void OnExit(){

		}

		public override void OnStart(){
			SetEverything();

			Geral x = Configuracoes.camera;

			V.view.Center = new Vector2f(x.x+(x.imgWidth/2), x.y);
			V.cameraX = V.view.Center.X - ((Screen.width/V.Delta)/2);
			V.cameraY = V.view.Center.Y - ((Screen.height/V.Delta)/2);
		}

		public static void TelaPrincipal(){
			V.window.SetView(V.view);

				foreach(Chao x in V.chao.ToList())
					x.Draw();

				foreach(Limite x in V.limites.ToList())
					x.Draw();

				foreach(Teleporte x in V.teleportes.ToList())
					x.Draw();

				V.objetos.Sort((x, y) => x.zindex.CompareTo(y.zindex));
				foreach(Geral x in V.objetos.ToList())
					x.Draw();

			V.window.SetView(V.hud);

				Configuracoes.Draw();
				Efeitos.BackdropRun();
				if(menu)F.Menu();

				foreach(IC x in V.interacaoContinua.ToList())
					x.x1.Interagir(1);

				foreach(TempText x in V.temptexts.ToList())
					x.Draw();	

				foreach(PopUp x in V.popups.ToList())
					x.Draw();
		}

		public static void SetEverything(){
			foreach(Chao x in V.chao.ToList())
				x.SetLimits();
		}

		public static void Menu(bool x){
			menu = x;
		}
	}
}