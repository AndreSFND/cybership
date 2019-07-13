using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Text.RegularExpressions;

namespace Main{
	public static class Configuracoes{
		public static Playable 	camera;
		public static Playable 	controle;
		public static int   	letraTempo		= 10;
		public static int 		defaultSpeed	= 2;
		public static bool 		vsync			= false;
		public static string 	textBoxId 		= "";

		public static void Draw(){
			Efeitos.AjustarCamera(camera);
			if(!controle.pausado)	F.MovePersonagem(controle);
			if(controle.vidaA <= 0)	controle.gameOver = Efeitos.GameOver(controle);
		}
		
		public static void Set(Playable x){
			camera 		= x;
			controle 	= x;
		}

		public static int DefaultSpeed(){
			return defaultSpeed;
		}
		
		public static void VSync(bool b, bool db){
			if(db)
				Conexao.ExecuteQuery("update configuracoes set valor = '"+b+"' where id = '2'");
				
			V.window.SetFramerateLimit(b ? (uint)0 : (uint)120);
			V.window.SetVerticalSyncEnabled(b);
		
			vsync = b;
		}
	}
}