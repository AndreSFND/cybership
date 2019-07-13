using System;
using System.IO;
using System.Linq;
using System.Drawing;
using System.Threading;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using Gma.QrCodeNet.Encoding;
using Gma.QrCodeNet.Encoding.Windows.Render;


namespace Main{
	public class ControlesState : Screen{

		public override void Draw(){
			V.window.SetView(V.hud);
			if(V.window.Size.X < 800 || V.window.Size.Y < 600) V.window.Size = new Vector2u(800, 600);

			TelaPrincipal();
		}

		public override void OnExit(){
			if(save) foreach(Controle c in V.controles.ToList())
				Conexao.ExecuteQuery("update controle set keycode = '"+c.keycode+"' where id = '"+c.id+"'");
				
			save = false;
		}

		public override void OnStart(){
			
		}

		public static void TelaPrincipal(){
			if((F.Key("esc") || F.Key("c")) && !F.TeclaDesativada("voltar") && !F.TeclaDesativada("change") && !changing){
				CurrentScreen.Change("config");
				F.DesativarTecla("voltar", 200);
			}

			F.DesenharShape(0, 0, Screen.width, Screen.height, 19, 19, 19, 255);
		
			F.Escrever("Configurar Controles", true, Screen.width-F.TxtWidth("Configurar Controles", 50, true)-30, 20, 50, 255, 255, 255, 255);
			F.Escrever("v0.5.2 Alpha", false, Screen.width-F.TxtWidth("v0.5.2 Alpha", 32, false)-30, Screen.height-50, 32, 255, 255, 255, 255);
			
			int m = 0;
			foreach(Controle c in V.controles){
				string s = (changing && m == option) ? "-" : ((Keyboard.Key)c.keycode).ToString();
			
				F.Escrever(c.nome, false, 315, 120+40*m, 32, 255, 255, 255, 255);
				F.Escrever(s, false, Screen.width-F.TxtWidth(s, 32, false)-30, 120+40*m, 32, 255, 255, 255, 255);
				m++;
			}
			
			V.img[0].Texture 		= V.IMG_CAT[8][0];
			V.img[0].TextureRect	= new IntRect(0, 0, 27, 15);
			V.img[0].Position 		= new Vector2f(280, 135+(40*option));
			V.window.Draw(V.img[0]);
			
			if(F.Key("up") && !F.TeclaDesativada("option") && option > 0 && !changing  && !F.TeclaDesativada("change")){
				option--;
				F.DesativarTecla("option", 175);
			}
			else if((F.Key("down") && !F.TeclaDesativada("option")) && option < V.controles.Count()-1 && !changing && !F.TeclaDesativada("change")){
				option++;
				F.DesativarTecla("option", 175);
			}
			else if(F.Key("x") && !F.TeclaDesativada("x") && !F.TeclaDesativada("change")){
				changing = true;
				F.DesativarTecla("x", 175);
			}
		}
		
		public static void Change(int keycode){
			if(changing && !F.TeclaDesativada("change")){
				V.controles[option].keycode = keycode;	
				if(option < V.controles.Count()-1)option++;
				changing 	= false;
				save 		= true;
				
				F.DesativarTecla("change", 300);
			}
		}

		public static int option = 0;
		public static int keycode;
		public static bool changing;
		public static bool save;
	}
}