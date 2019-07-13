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
	public class WorldMapState : Screen{

		public static int spriteMar 	= 0;
		public static int spriteMapa 	= 0;

		public static bool firstFrame	= true;
		public static bool arrow 		= true;

		public override void Draw(){
			V.window.SetView(V.hud);

			TelaPrincipal();
		}

		public override void OnExit(){

		}

		public override void OnStart(){
			firstFrame 	= true;
			spriteMar 	= 0;
		}

		public static void TelaPrincipal(){
			int sw = (int)(Screen.width/128)+1;
			int sh = (int)(Screen.height/128)+1;

			for(int x=0;x<sw;x++)
				for(int y=0;y<sh;y++){
					V.img[0].TextureRect 	= new IntRect(0, 0, 128, 128);
					V.img[0].Texture 		= V.IMG_CAT[11][spriteMar];
					V.img[0].Position 		= new Vector2f(128*x, 128*y);
					V.window.Draw(V.img[0]);
				}

			V.img[0].TextureRect 	= new IntRect(0, 0, 702, 738);
			V.img[0].Texture 		= V.IMG_CAT[12][spriteMapa];
			V.img[0].Position 		= new Vector2f((int)(Screen.width/2)-351, (int)(Screen.height/2)-369);
			V.window.Draw(V.img[0]);

			int arrowY = arrow ? 0 : -10;

			V.img[0].TextureRect 	= new IntRect(0, 0, 18, 27);
			V.img[0].Texture 		= V.IMG_CAT[8][6];
			V.img[0].Position 		= new Vector2f((int)(Screen.width/2)-351+138, (int)(Screen.height/2)-369+353+arrowY);
			V.window.Draw(V.img[0]);

			if(firstFrame){
				F.DesativarTecla("worldmap_mar", 300);
				firstFrame = false;
			}

			if(!F.TeclaDesativada("worldmap_mar")){
				spriteMar = (spriteMar > 4) ? 0 : spriteMar+1;
				F.DesativarTecla("worldmap_mar", 300);
			}

			if(!F.TeclaDesativada("worldmap_arrow")){
				arrow = !arrow;
				F.DesativarTecla("worldmap_arrow", 500);
			}

			if((F.Key("esc") || F.Key("c")) && !F.TeclaDesativada("esc")){
				CurrentScreen.Change("localmap");
				F.DesativarTecla("esc", 200);
			}
		}
	}
}