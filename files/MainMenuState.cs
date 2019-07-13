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
	public class MainMenuState : Screen{

		public override void Draw(){
			V.window.SetView(V.hud);
			if(V.window.Size.X < 800 || V.window.Size.Y < 600) V.window.Size = new Vector2u(800, 600);

			switch(TelaPrincipal()){
				case 0:
					CurrentScreen.Change("loadfile");
				break;

				case 1:
					CurrentScreen.Change("config");
				break;

				case 2:
					CurrentScreen.Change("createmap");
				break;

				case 3:
				break;

				case 4:
					V.window.Close();
				break;
			}
		}

		public override void OnExit(){

		}

		public override void OnStart(){
			Efeitos.Backdrop("fade-out", 0, 2.5f);
		}

		public static int TelaPrincipal(){
			string[] opcoesMenu = {"Jogar", "Configurações", "Criar Mapa", "Créditos", "Sair"};

			F.DesenharShape(0, 0, Screen.width, Screen.height, 19, 19, 19, 255);
		
			F.Escrever("Cybership", true, Screen.width-F.TxtWidth("Cybership", 50, true)-30, 20, 50, 255, 255, 255, 255);
						
			for(int m=0;m<opcoesMenu.Count();m++)
				F.Escrever(opcoesMenu[m], false, Screen.width-F.TxtWidth(opcoesMenu[m], 32, false)-30, Screen.height/2-40+40*m, 32, 255, 255, 255, 255);

			F.Escrever("v0.5.2 Alpha", false, Screen.width-F.TxtWidth("v0.5.2 Alpha", 32, false)-30, Screen.height-50, 32, 255, 255, 255, 255);

			if(!F.TeclaDesativada("frameMainMenu")){
				frame += frame > 6 ? -frame : 1;
				F.DesativarTecla("frameMainMenu", 300);
			}

			V.img[0].Texture 		= V.IMG_CAT[8][0];
			V.img[0].TextureRect	= new IntRect(0, 0, 27, 15);
			V.img[0].Position 		= new Vector2f(Screen.width-F.TxtWidth(opcoesMenu[option], 32, false)-67, (Screen.height/2)-25+(40*option));
			V.window.Draw(V.img[0]);

			V.img[0].Texture 		= V.IMG_CAT[0][0];
			V.img[0].Position 		= new Vector2f(30, Screen.height-294);
			V.img[0].TextureRect	= new IntRect(96*frame, 0, 96, 136);
			V.img[0].Scale 			= new Vector2f(1.5f, 1.5f);

			V.window.Draw(V.img[0]);

			V.img[0].Scale 			= new Vector2f(1, 1);
			
			if((F.Key("down") && !F.TeclaDesativada("option")) && option < opcoesMenu.Length-1){
				option++;
				F.DesativarTecla("option", 175);
			}
			else if((F.Key("up") && !F.TeclaDesativada("option"))  && option > 0){
				option--;
				F.DesativarTecla("option", 175);
			}
			else if(F.Key("x") && !F.TeclaDesativada("x")){
				F.DesativarTecla("x", 175);
				F.DesativarTecla("mouseLeft", 175);
				return option;
			}

			Efeitos.BackdropRun();

			return -1;
		}

		public static int option 	= 0;	
		public static int frame 	= 0;
	}
}