using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Data.SQLite;
using System.Text.RegularExpressions;

namespace Main{
	public class LoginState : Screen{

		public override void Draw(){
			if(TelaPrincipal())
				CurrentScreen.Set("intro");
		}

		public override void OnExit(){
			
		}

		public override void OnStart(){
			Screen.width		= 500;
			Screen.height		= 300;

			V.window.Position =  new Vector2i((int)(VideoMode.DesktopMode.Width/2)-(int)(Screen.width/2), (int)(VideoMode.DesktopMode.Height/2)-(int)(Screen.height/2));
			
			SQLiteDataReader reader = Conexao.LoadData("select * from controle");
			while(reader.Read())
	        	V.controles.Add(new Controle(Convert.ToInt32(reader["id"]), Convert.ToString(reader["nome"]), Convert.ToInt32(reader["keycode"])));
		}

		public static bool TelaPrincipal(){
			V.window.SetView(V.hud);

			if(V.window.Size.X != Screen.width || V.window.Size.Y != Screen.height) V.window.Size = new Vector2u(Screen.width, Screen.height);
			
			F.DesenharShape(0, 0, Screen.width, Screen.height, 19, 19, 19, 255);
			F.Escrever("Cybership - Login", true, (Screen.width/2)-(F.TxtWidth("Cybership - Login", 32, true)/2), 10, 32, 255, 255, 255, 255);

			F.Escrever("Email:", false, (Screen.width/2)-F.TxtWidth("Email:", 24, false)-10, 105, 24, 255, 255, 255, 255);
			F.TextBox("user_email", "", "", (Screen.width/2), 110, 240, 25, 255, 255, 255, 255);
			V.window.SetView(V.hud);

			F.Escrever("Senha:", false, (Screen.width/2)-F.TxtWidth("Senha:", 24, false)-10, 145, 24, 255, 255, 255, 255);
			F.TextBox("user_pswd", "password", "", (Screen.width/2), 150, 240, 25, 255, 255, 255, 255);
			V.window.SetView(V.hud);
			
			V.img[0].TextureRect 	= new IntRect(0, 0, 100, 100);
			V.img[0].Texture 		= new Texture("res/logobranco.png");;
			V.img[0].Position 		= new Vector2f((Screen.width/2)-220, (Screen.height/2)-55);
			V.window.Draw(V.img[0]);
			
			F.Escrever("Nicode - Todos os Direitos Reservados 2017", false, (Screen.width/2)-(F.TxtWidth("Nicode® - Todos os Direitos Reservados 2017", 16, false)/2), Screen.height-20, 16, 255, 255, 255, 255);
			
			if(F.Button("Entrar", 24, (Screen.width/2)+100, 190, 140, 30, 221, 66, 82, 255) && !F.TeclaDesativada("mouseLeft")){
				string email = V.txtBoxes.Find(item => item.id == "user_email").txt;
				string senha = V.txtBoxes.Find(item => item.id == "user_pswd").txt;

				if(Conexao.Login(email, senha))
					return true;
				else
					loginTry++;

				F.DesativarTecla("mouseLeft", 1000);
			}
			if(loginTry > 0)
				F.Escrever("Email ou senha incorretos!", false, (Screen.width/2)-(F.TxtWidth("Email ou senha incorretos!", 24, false)/2), 300, 24, 221, 66, 82, 255);

			return false;
		}

		public static int loginTry = 0;
	}
}