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
	public class FimDemoState : Screen{

		public static float stringOp = 0;

		public override void Draw(){
			V.window.SetView(V.hud);

			TelaPrincipal();
		}

		public override void OnExit(){
			
		}

		public override void OnStart(){
			Efeitos.opacidade = 0;
			Efeitos.Backdrop("fade-in", 255, 2.55f);
		}

		public static void TelaPrincipal(){
			if(Efeitos.opacidade < 255)
				LocalMapDraw();
			else{
				stringOp += stringOp < 255 ? 2.55f : 0;
				F.Escrever("Fim da Demonstração", true, (Screen.width/2)-(F.TxtWidth("Fim da Demonstração", 36, true)/2), (Screen.height/2)-18, 36, 255, 255, 255, (byte)stringOp);
			}
		}

		public static void LocalMapDraw(){
			V.window.SetView(V.view);

				foreach(Chao x in V.chao.ToList())
					x.Draw();

				foreach(Limite x in V.limites.ToList())
					x.Draw();

				V.objetos.Sort((x, y) => x.zindex.CompareTo(y.zindex));
				foreach(Geral x in V.objetos.ToList())
					x.Draw();

			V.window.SetView(V.hud);

				foreach(TempText x in V.temptexts.ToList())
					x.Draw();	

				Efeitos.BackdropRun();
		}
	}
}