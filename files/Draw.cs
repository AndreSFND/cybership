using System;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Data.SQLite;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;


namespace Main{
	public partial class Idk{
		public static int fps;
	
		public static void Fps(){
			if(!F.TeclaDesativada("fps")){
				Console.Clear();
				Console.WriteLine("FPS: "+fps);
				fps = 0;
				F.DesativarTecla("fps", 1000);
			}

			fps++;
		}

		public static void DeletarMapa(){
			V.chao.Clear();
			V.limites.Clear();
			V.teleportes.Clear();

			V.mapWidth			= 0;
			V.mapHeight			= 0;
			
			foreach(Geral o in V.objetos.ToList()) if(!(o is Personagem))
				V.objetos.Remove(o);
		}
		
		public static void LoadMap(float mapa, bool changeMap){
			DeletarMapa();
			SQLiteDataReader reader;

			if(changeMap){
				reader = Conexao.LoadData("select * from teleporte where id = '"+mapa+"'");	
				while(reader.Read()){
					float addY = 0, addX = 0;

					if(Convert.ToInt32(reader["ladoSaida"]) < 2)
						addY = Convert.ToInt32(reader["ladoSaida"]) == 0 ? (Configuracoes.controle.height/2)+(Convert.ToInt32(reader["height"])/2)+5 : -(Convert.ToInt32(reader["height"])/2)-5-(Configuracoes.controle.height*1.5f);
					else
						addX = Convert.ToInt32(reader["ladoSaida"]) == 2 ? (Configuracoes.controle.width/2)+(Convert.ToInt32(reader["width"])/2)+5 : -(Convert.ToInt32(reader["width"])/2)-5-(Configuracoes.controle.width*1.5f);

					Configuracoes.controle.x 		= Convert.ToInt32(reader["x"])+(Convert.ToInt32(reader["width"])/2)-(Configuracoes.controle.width/2)+addX;
        			Configuracoes.controle.y 		= Convert.ToInt32(reader["y"])+(Convert.ToInt32(reader["height"])/2)-(Configuracoes.controle.height/2)+addY;
        			Configuracoes.controle.mapa 	= Convert.ToInt32(reader["mapa"]);
        			mapa 							= Convert.ToInt32(reader["mapa"]);
				}
			}

			reader = Conexao.LoadData("select A.nome, A.categoria, A.sprite, A.width, A.height, A.adcX, A.adcY, A.vida, A.xp, A.fps, A.forca, A.playable, A.dialog, A.troca, A.batalhar, A.mover, A.recurso, A.itemDestroi, B.*, C.interior from objeto A inner join objeto_mapa B inner join mapa C on A.id = B.objeto_id and B.mapa = C.id where A.playable = 'true' and B.mapa = '"+mapa+"'");
			while(reader.Read()){
				if(Convert.ToInt32(reader["categoria"]) == 2){
					int[] troca 	= reader["troca"] is DBNull ? new int[]{} : Array.ConvertAll<string,int>(Convert.ToString(reader["troca"]).TrimStart('-').Split('-'), int.Parse);
					bool activated 	= V.tempObjects.ContainsKey(Convert.ToInt32(reader["id"])) ? true : false;

					V.objetos.Add(new Npc(Convert.ToInt32(reader["id"]), Convert.ToInt32(reader["sprite"]), Convert.ToSingle(reader["x"]), Convert.ToSingle(reader["y"]), Convert.ToInt32(reader["width"]), Convert.ToInt32(reader["height"]), 0, 0, Convert.ToInt32(reader["vida"]), Convert.ToInt32(reader["forca"]), Convert.ToBoolean(reader["mover"]), Convert.ToBoolean(reader["interior"]), activated, Convert.ToString(reader["dialog"]), troca, Convert.ToBoolean(reader["batalhar"])));
				}
			}

			reader = Conexao.LoadData("select * from chao where mapa = '"+mapa+"'");
			while(reader.Read())
	        	V.chao.Add(new Chao(Convert.ToInt32(reader["sprite"]), Convert.ToInt32(reader["x"]), Convert.ToInt32(reader["y"])));
			
			reader = Conexao.LoadData("select * from limite where mapa = '"+mapa+"'");
			while(reader.Read())
				V.limites.Add(new Limite(Convert.ToInt32(reader["x"]), Convert.ToInt32(reader["y"]), Convert.ToInt32(reader["width"]), Convert.ToInt32(reader["height"])));

			reader = Conexao.LoadData("select * from teleporte where mapa = '"+mapa+"'");
			while(reader.Read())
				V.teleportes.Add(new Teleporte(Convert.ToInt32(reader["x"]), Convert.ToInt32(reader["y"]), Convert.ToInt32(reader["width"]), Convert.ToInt32(reader["height"]), Convert.ToInt32(reader["destino"]), Convert.ToString(reader["tipo"])));

			reader = Conexao.LoadData("select B.*, A.nome, A.categoria, A.sprite, A.width, A.height, A.adcX, A.adcY, A.vida, A.xp, A.fps, A.forca, A.playable, A.dialog, A.troca, A.batalhar, A.mover, A.recurso, A.itemDestroi from objeto A inner join objeto_mapa B on A.id = B.objeto_id where mapa = '"+mapa+"'");
			while(reader.Read())
				switch(Convert.ToInt32(reader["categoria"])){
					case 4:
						bool activated 		= V.tempObjects.ContainsKey(Convert.ToInt32(reader["id"])) ? true : false;
						bool locked 		= V.lockedObjects.ContainsKey(Convert.ToInt32(reader["id"])) ? false : true;

						V.objetos.Add(new Bau(Convert.ToInt32(reader["id"]), Convert.ToInt32(reader["sprite"]), locked, Convert.ToInt32(reader["x"]), Convert.ToInt32(reader["y"]), Convert.ToInt32(reader["troca"]), activated));
					break;

					case 7:
						V.objetos.Add(new Estrutura(Convert.ToInt32(reader["sprite"]), Convert.ToInt32(reader["x"]), Convert.ToInt32(reader["y"]), Convert.ToInt32(reader["width"]), Convert.ToInt32(reader["height"]), Convert.ToInt32(reader["adcX"]), Convert.ToInt32(reader["adcY"])));
					break;

					case 9:
						activated = V.tempObjects.ContainsKey(Convert.ToInt32(reader["id"])) ? true : false;
						V.objetos.Add(new Materia(Convert.ToInt32(reader["id"]), Convert.ToInt32(reader["sprite"]), Convert.ToInt32(reader["recurso"]), Convert.ToInt32(reader["itemDestroi"]), Convert.ToInt32(reader["x"]), Convert.ToInt32(reader["y"]), Convert.ToInt32(reader["width"]), Convert.ToInt32(reader["height"]), Convert.ToInt32(reader["adcX"]), Convert.ToInt32(reader["adcY"]), activated));
					break;

					case 10:
						V.objetos.Add(new Decoracao(Convert.ToInt32(reader["id"]), Convert.ToInt32(reader["sprite"]), Convert.ToInt32(reader["x"]), Convert.ToInt32(reader["y"]), Convert.ToInt32(reader["width"]), Convert.ToInt32(reader["height"]), Convert.ToInt32(reader["adcX"]), Convert.ToInt32(reader["adcY"])));
					break;
				}
				
			reader = Conexao.LoadData("select * from savePoint where mapa = '"+mapa+"'");
			while(reader.Read())
				V.objetos.Add(new SavePoint(Convert.ToInt32(reader["id"]), Convert.ToInt32(reader["x"]), Convert.ToInt32(reader["y"]), Convert.ToInt32(reader["mapa"])));


			LocalMapState.SetEverything();
			Conexao.sql_con.Close();
		}

		public static void Draw(){
			//Fps();
			V.window.SetView(V.view);
			F.cursorIcon 	= -1;

			if(!F.TeclaDesativada("barra")){
				F.barra = F.barra == "" ? "|" : "";
				F.DesativarTecla("barra", 500);
			}
		
			F.AtualizarTecla();
			F.TeclasDesativadasF();
		}

		public static void DrawHUD(){	
			V.window.SetView(V.hud);

			F.AlertaF();	
			F.CursorDraw();

			V.mouseWheel = 0;
			V.textEntered = "";
		}
	}
}