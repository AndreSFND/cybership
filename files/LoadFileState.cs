using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using MySql.Data.MySqlClient;
using System.Data.SQLite;

namespace Main{
	public class LoadFileState : Screen{
	
		public static List<SaveFile> saveFiles = new List<SaveFile>();

		public override void Draw(){
			V.window.SetView(V.hud);
			if(V.window.Size.X < 800 || V.window.Size.Y < 600) V.window.Size = new Vector2u(800, 600);
			
			TelaPrincipal();
		}

		public override void OnExit(){

		}

		public override void OnStart(){
			Idk.windowColor = new Color(000, 000, 000);
			LoadSaveFiles();
		}

		public static void TelaPrincipal(){
			if(F.Key("esc") || F.Key("c"))
				CurrentScreen.Change("mainmenu");
		
			F.DesenharShape(0, 0, Screen.width, Screen.height, 19, 19, 19, 255);
			F.Escrever("v0.5.2 Alpha", false, Screen.width-F.TxtWidth("v0.5.2 Alpha", 32, false)-30, Screen.height-50, 32, 255, 255, 255, 255);

			F.Escrever("Selecione um arquivo", true, Screen.width-F.TxtWidth("Selecione um arquivo", 36, true)-30, 20, 36, 255, 255, 255, 255);

			for(int m=0;m<4;m++) {
				byte shapeColor = (m == option && !editing) ? (byte)180 : (byte)35; 
				byte fontColor 	= (m == option && !editing) ? (byte)255 : (byte)112;
				string txt 		= !LoadFileState.saveFiles[m].Empty() ? LoadFileState.saveFiles[m].mapaNome : "-Empty-";
				
				F.DesenharShape(Screen.width-330, 110+(105*m), 310, 80, shapeColor, shapeColor, shapeColor, 255);
				F.Escrever(txt, true, Screen.width-175-(F.TxtWidth(txt, 31, true)/2), 125+(105*m), 31, fontColor, fontColor, fontColor, 255);
				
				if(!LoadFileState.saveFiles[m].Empty()) for(int n=0;n<2;n++){
					shapeColor = (m == option && n == editingOption && editing) ? (byte)180 : (byte)35; 
					F.DesenharShape(Screen.width-380, 110+(45*n)+(105*m), 40, 35, shapeColor, shapeColor, shapeColor, 255);
					
					V.img[0].Texture 		= V.IMG_CAT[8][3+n];
					V.img[0].TextureRect	= new IntRect(0, 0, 26, 24);
					V.img[0].Position 		= new Vector2f(Screen.width-373, 114+(45*n)+(105*m));
					V.window.Draw(V.img[0]);
				}
			}
				
			if(F.Key("up") && !F.TeclaDesativada("option")){
				if(editing && editingOption > 0)
					editingOption--;
				else if(!editing && option > 0)
					option--;
					
				F.DesativarTecla("option", 175);
			}
			else if(F.Key("down") && !F.TeclaDesativada("option")){
				if(editing && editingOption < 1)
					editingOption++;
				else if(!editing && option < 3)
					option++;
				
				F.DesativarTecla("option", 175);
			}
			else if(F.Key("left") && !F.TeclaDesativada("option") && !editing && !LoadFileState.saveFiles[option].Empty() && !copying){
				editing = true;
				F.DesativarTecla("option", 175);
			}
			else if(F.Key("right") && !F.TeclaDesativada("option") && editing){
				editing = false;
				F.DesativarTecla("option", 175);
			}
			else if(F.Key("x") && !F.TeclaDesativada("x")){
				if(editing){
					if(editingOption == 0)
						DeleteFile(option+1);
					if(editingOption == 1){
						copyingOption 	= option+1;
						editing 		= false;
						copying 		= true;
					}
				}
				else if(copying){
					CopyFile(copyingOption, option+1);

					copying = false;
				}
				else
					LoadFile(option+1);

				F.DesativarTecla("x", 175);
			}
		}

		public static void LoadFile(int file){
			SQLiteDataReader reader;
			int mapa = 1, x = 400, y = 500;

			V.objetos.Clear();
			V.tempObjects.Clear();
			V.lockedObjects.Clear();
			
			List<Item> a				= new List<Item>();
	        List<Recurso> b				= new List<Recurso>();
			
			reader = Conexao.LoadData("select x, y, mapa from saveFile A inner join savePoint B where A.savePoint = B.id and A.id = '"+file+"'");
			while(reader.Read()){
				x 		= Convert.ToInt32(reader["x"]);
				y 		= Convert.ToInt32(reader["y"])+50;
				mapa 	= Convert.ToInt32(reader["mapa"]);
			}

			reader = Conexao.LoadData("select * from objeto_item where objeto_id = 1 and saveFile_id = '"+file+"'");
			while(reader.Read())
				a.Add(V.itens[Convert.ToInt32(reader["item_id"])-1]);

			reader = Conexao.LoadData("select * from objeto_recurso where objeto_id = 1 and saveFile_id = '"+file+"'");
			while(reader.Read()){
				Recurso r = V.recursos[Convert.ToInt32(reader["recurso_id"])-1];
				Recurso n = (Recurso)r.Clone();
				b.Add(n);
				
				b[b.IndexOf(n)].Add(Convert.ToInt32(reader["quantidade"]));
			}

			reader = Conexao.LoadData("select B.id, A.width, A.height, A.xp, A.vida, C.interior from objeto A inner join objeto_mapa B inner join mapa C on A.id = B.objeto_id and B.mapa = C.id where categoria = 1 and playable = 'true'");
			while(reader.Read()){
				V.personagem = null;
				V.personagem = new Personagem(Convert.ToInt32(reader["id"]), Data.Nick(), x, y, Convert.ToInt32(reader["width"]), Convert.ToInt32(reader["height"]), Convert.ToInt32(reader["xp"]), Convert.ToInt32(reader["vida"]), Convert.ToBoolean(reader["interior"]), mapa, a, b);
				Configuracoes.Set(V.personagem);
			}

			reader = Conexao.LoadData("select * from objeto_savefile where saveFile_id = '"+file+"' and activated = 'true'");
			while(reader.Read())
				V.tempObjects[Convert.ToInt32(reader["objeto_id"])] = Convert.ToBoolean(reader["activated"]);

			reader = Conexao.LoadData("select * from objeto_locked where saveFile_id = '"+file+"' and locked = 'false'");
			while(reader.Read())
				V.lockedObjects[Convert.ToInt32(reader["objeto_id"])] = Convert.ToBoolean(reader["locked"]);

			V.personagem.AddSkill(V.skills[0]);
			V.personagem.AddSkill(V.skills[1]);
			V.personagem.AddSkill(V.skills[3]);
			V.personagem.AddSkill(V.skills[2]);
			
			V.personagem.AddSkill(V.skills[4]);
			V.personagem.AddSkill(V.skills[5]);

			Idk.LoadMap(mapa, false);
			CurrentScreen.Change("localmap");
		}
		
		public static void DeleteFile(int file){
			Conexao.ExecuteQuery("update saveFile set savePoint = '' where id = '"+file+"'");
			Conexao.ExecuteQuery("delete from objeto_item 	  where saveFile_id = '"+file+"'");
			Conexao.ExecuteQuery("delete from objeto_recurso  where saveFile_id = '"+file+"'");
			Conexao.ExecuteQuery("delete from objeto_savefile where saveFile_id = '"+file+"'");
			Conexao.ExecuteQuery("delete from objeto_locked   where saveFile_id = '"+file+"'");

			LoadSaveFiles();
			editing = false;
		}

		public static void CopyFile(int file, int secondFile){
			DeleteFile(secondFile);

			Conexao.ExecuteQuery("update saveFile set savePoint = (select savePoint from saveFile where id = '"+file+"') where id = '"+secondFile+"'");
			Conexao.ExecuteQuery("insert into objeto_item (item_id, objeto_id, quantidade, saveFile_id) select item_id, objeto_id, quantidade, '"+secondFile+"' from objeto_item where saveFile_id = '"+file+"'");
			Conexao.ExecuteQuery("insert into objeto_recurso (recurso_id, objeto_id, quantidade, saveFile_id) select recurso_id, objeto_id, quantidade, '"+secondFile+"' from objeto_recurso where saveFile_id = '"+file+"'");
			Conexao.ExecuteQuery("insert into objeto_savefile (objeto_id, saveFile_id, activated) select objeto_id, '"+secondFile+"', activated from objeto_savefile where saveFile_id = '"+file+"'");
			Conexao.ExecuteQuery("insert into objeto_locked (objeto_id, saveFile_id, locked) select objeto_id, '"+secondFile+"', locked from objeto_locked where saveFile_id = '"+file+"'");

			LoadSaveFiles();
		}
		
		public static void LoadSaveFiles(){
			saveFiles.Clear();
			
			SQLiteDataReader reader = Conexao.LoadData("select A.*, C.nome from saveFile A left join savePoint B on A.savePoint = B.mapa left join mapa C on A.savePoint = C.id");
			while(reader.Read())
				saveFiles.Add(new SaveFile(Convert.ToInt32(reader["id"]), Convert.ToInt32(reader["savePoint"]), Convert.ToString(reader["nome"])));
		}

		public static int copyingOption = 0;

		public static int option 		= 0;
		public static int editingOption = 0;
		public static bool editing 		= false;
		public static bool copying 		= false;
		public static List<string> mapas = new List<string>();
	}
}