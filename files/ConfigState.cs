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
	public class ConfigState : Screen{

		public override void Draw(){
			V.window.SetView(V.hud);
			if(V.window.Size.X < 800 || V.window.Size.Y < 600) V.window.Size = new Vector2u(800, 600);

			TelaPrincipal();
		}

		public override void OnExit(){

		}

		public override void OnStart(){
			
		}

		public static void TelaPrincipal(){
			if((F.Key("esc") || F.Key("c")) && !F.TeclaDesativada("voltar")){
				CurrentScreen.Change("mainmenu");
				F.DesativarTecla("voltar", 200);
			}

			F.DesenharShape(0, 0, Screen.width, Screen.height, 19, 19, 19, 255);
		
			F.Escrever("Configurações", true, Screen.width-F.TxtWidth("Configurações", 50, true)-30, 20, 50, 255, 255, 255, 255);
			F.Escrever("v0.5.2 Alpha", false, Screen.width-F.TxtWidth("v0.5.2 Alpha", 32, false)-30, Screen.height-50, 32, 255, 255, 255, 255);
			
			string[] opcoes = {"Tela Cheia", "VSync", "Controles", "Conectar Joystick"};

			for(int m=0;m<opcoes.Count();m++){
				byte c = (JoystickState.connected && m == 3) ? (byte)125 : (byte)255;
				F.Escrever(opcoes[m], false, Screen.width-350, Screen.height/2-40+40*m, 32, c, c, c, 255);
			}

			F.Escrever(Screen.fullscreen ? "ON" : "OFF", false, (Screen.fullscreen ? -52 : -58)+Screen.width-F.TxtWidth("OFF", 32, false), Screen.height/2-40, 32, 255, 255, 255, 255);
			F.Escrever(Configuracoes.vsync ? "ON" : "OFF", false, (Configuracoes.vsync ? -52 : -58)+Screen.width-F.TxtWidth("OFF", 32, false), Screen.height/2, 32, 255, 255, 255, 255);

			if(option < 2){
				V.img[0].Texture 		= V.IMG_CAT[8][1];
				V.img[0].TextureRect	= new IntRect(0, 0, 18, 15);
				V.img[0].Position 		= new Vector2f(Screen.width-51, (Screen.height/2)-25+(40*option));
				V.window.Draw(V.img[0]);

				V.img[0].Texture 		= V.IMG_CAT[8][2];
				V.img[0].TextureRect	= new IntRect(0, 0, 18, 15);
				V.img[0].Position 		= new Vector2f(Screen.width-80-F.TxtWidth("OFF", 32, false), (Screen.height/2)-25+(40*option));
				V.window.Draw(V.img[0]);	
			}

			V.img[0].Texture 		= V.IMG_CAT[8][0];
			V.img[0].TextureRect	= new IntRect(0, 0, 27, 15);
			V.img[0].Position 		= new Vector2f(Screen.width-387, (Screen.height/2)-25+(40*option));
			V.window.Draw(V.img[0]);

			
			if((F.Key("up") && !F.TeclaDesativada("option"))  && option > 0){
				option--;
				F.DesativarTecla("option", 175);
			}
			else if((F.Key("down") && !F.TeclaDesativada("option")) && option < opcoes.Length-1){
				option++;
				F.DesativarTecla("option", 175);
			}
			else if((F.Key("left") && !F.TeclaDesativada("option"))){
				SetValue(option, false);
				F.DesativarTecla("option", 175);
			}
			else if((F.Key("right") && !F.TeclaDesativada("option"))){
				SetValue(option, true);
				F.DesativarTecla("option", 175);
			}
			else if(F.Key("x") && !F.TeclaDesativada("x")){
				ChangeState(option);
				F.DesativarTecla("x", 175);
			}
		}

		public static void SetValue(int opcao, bool valor){
			switch(opcao){
				case 0:
					if(!Screen.fullscreen && valor)
						Screen.Fullscreen(true, true);
					else if(Screen.fullscreen && !valor)
						Screen.Fullscreen(false, true);
				break;

				case 1:
					if(!Configuracoes.vsync && valor)
						Configuracoes.VSync(true, true);
					else if(Configuracoes.vsync && !valor)
						Configuracoes.VSync(false, true);
				break;
			}
		}
		
		public static void ChangeState(int opcao){
			switch(opcao){
				case 2:
					CurrentScreen.Change("controles");
				break;

				case 3:
					if(!JoystickState.connected)
						CurrentScreen.Change("joystick");
				break;
			}
		}

		public static int option = 0;	
	}
}