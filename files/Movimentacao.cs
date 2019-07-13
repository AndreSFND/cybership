using System;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Main{
	public partial class F{
		public static List<string> 	tecla 		= new List<string>();
		public static int 			duplo 		= 0;
		public static string 		btn;
		public static DateTime 		n1;


		public static bool Key(string x){
			int keyIndex = tecla.IndexOf(x);

			return keyIndex == -1 ? false : true;
		}

		public static void Andar(int direcao, Playable x){
			if(!x.andando || tecla.Count < 2)
				x.direcao 	= direcao;
				
			if(!x.andando && x.movimentar <= 0){
				F.AtivarTecla("frame_personagem");
				x.frame = 0;
			}

			x.andando = true;
		}

		public static void MovePersonagem(Playable x) {
			float oldX 	= x.x;
			float oldY 	= x.y;
			int direcao = x.direcao;

			if(Key("c")){
				x.speed = Configuracoes.defaultSpeed*2;
				x.fps = 75;
			}
			else{
				x.speed = Configuracoes.defaultSpeed;
				x.fps = 150;
			}

			if (Key("up") && !(x.direcao == 0 && x.andando)){
				Andar(1, x);
				
				x.y -= x.speed;
			}
			if (Key("down") && !(x.direcao == 1 && x.andando)){
				Andar(0, x);

				x.y += x.speed;
			}
			if (Key("left") && !(x.direcao == 2 && x.andando)){
				Andar(3, x);

				x.x -= x.speed;
			}
			if (Key("right") && !(x.direcao == 3 && x.andando)){
				Andar(2, x);

				x.x += x.speed;
			}

			if(x.direcao != direcao){
				F.AtivarTecla("frame_personagem");
				x.frame = 0;
			}

			if (Key("no-movement") || (oldX - x.x == 0 && oldY - x.y == 0)){
				if(x.andando)
					x.frame = 0;

				x.andando 	= false;
				x.speed 	= Configuracoes.defaultSpeed;
				x.fps 		= 300;
			}

			if(x.andando && tecla.Count > 1 && ((!Key("up") && x.direcao == 1) || (!Key("down") && x.direcao == 0) || (!Key("left") && x.direcao == 3) || (!Key("right") && x.direcao == 2))){
				x.frame 	= 0;
				x.andando 	= false;
			}
		}

		public static void AtualizarTecla(){
			tecla.Clear();

			if (Keyboard.IsKeyPressed((Keyboard.Key)V.controles[0].keycode)		|| Joystick.Button["up"])
			   	tecla.Add("up");
			if (Keyboard.IsKeyPressed((Keyboard.Key)V.controles[1].keycode)		|| Joystick.Button["down"])
			   	tecla.Add("down");	
			if (Keyboard.IsKeyPressed((Keyboard.Key)V.controles[2].keycode)		|| Joystick.Button["left"])
			   	tecla.Add("left");	
			if (Keyboard.IsKeyPressed((Keyboard.Key)V.controles[3].keycode)		|| Joystick.Button["right"])
			   	tecla.Add("right");
			if (Keyboard.IsKeyPressed((Keyboard.Key)V.controles[4].keycode)		|| Joystick.Button["b"])
			   	tecla.Add("x");
			if (Keyboard.IsKeyPressed((Keyboard.Key)V.controles[5].keycode)		|| Joystick.Button["y"])
			   	tecla.Add("c");
			if (Keyboard.IsKeyPressed((Keyboard.Key)V.controles[6].keycode)		|| Joystick.Button["a"])
			   	tecla.Add("enter");
			if (Keyboard.IsKeyPressed((Keyboard.Key)V.controles[7].keycode)		|| Joystick.Button["x"])
				tecla.Add("esc");
			if (Keyboard.IsKeyPressed(Keyboard.Key.BackSpace))
				tecla.Add("backspace");
			if (Keyboard.IsKeyPressed(Keyboard.Key.LControl))
				tecla.Add("lcontrol");


			if(!Key("up") && !Key("down") && !Key("left") && !Key("right"))
				tecla.Add("no-movement");
		}
	}
}