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
	public class V{
		public static RenderWindow window 	= new RenderWindow(new VideoMode(Screen.width, Screen.height), "Cybership v0.5.2");
		public static View view 			= new View(new Vector2f(Screen.width/2, Screen.height/2), new Vector2f(Screen.width, Screen.height));
		public static View hud 				= new View(new Vector2f(Screen.width/2, Screen.height/2), new Vector2f(Screen.width, Screen.height));
		public static View menuCreator 		= new View(new Vector2f(Screen.width/2, ((float)Screen.height-170)/2), new Vector2f(Screen.width, (float)Screen.height-170));
		public static Font eBit				= new Font("res/fonts/PixelOperator.ttf");
		public static Font eBitBold			= new Font("res/fonts/PixelOperatorMono-Bold.ttf");
		public static Event evento;
		public static string tecla			= "";
		public static string textEntered 	= "";
		public static Random random			= new Random();

		public static int mapWidth			= 0;
		public static int mapHeight			= 0;
		public static int chaoWidth			= mapWidth / 115;
		public static int chaoHeight		= mapHeight / 90;

		public static int mouseX			= 0;
		public static int mouseY			= 0;
		public static int mouseWheel 		= 0;
		public static int smoothScroll 		= 0;
		public static float Delta 			= 1;
		public static string mouseButton	= "";

		public static float letraTempo;
		public static float animS = 0;
		public static float cameraX;
		public static float cameraY;

		public static List<Geral> 					objetos 			= new List<Geral>();

		public static List<Limite> 					limites 			= new List<Limite>();
		public static List<Teleporte> 				teleportes 			= new List<Teleporte>();
		public static List<Chao> 					chao 				= new List<Chao>();
		public static List<TempText>				temptexts 			= new List<TempText>();	
		public static List<PopUp>					popups 				= new List<PopUp>();
		public static List<Controle> 				controles 			= new List<Controle>();

		public static List<float> 					nivelArray			= new List<float>(){0, 200, 500, 1000, 2500, 5000};
		public static List<IC> 						interacaoContinua	= new List<IC>();
		public static List<TeclaDesativada>			teclasDesativadas 	= new List<TeclaDesativada>();
		public static List<Texture>[] 				IMG_CAT 			= new List<Texture>[14];
		public static List<TxtBox> 					txtBoxes			= new List<TxtBox>();
		public static List<CmbBox> 					comboBoxes			= new List<CmbBox>();
		public static List<Skill>					skills 				= new List<Skill>();
		public static List<Item> 					itens				= new List<Item>();
		public static List<Recurso> 				recursos			= new List<Recurso>();
		public static List<Puzzle> 					puzzleObjects		= new List<Puzzle>();
		public static List<Puzzle> 					puzzle2				= new List<Puzzle>();

		public static Dictionary<string, bool> mapCreatorTabs 	= new Dictionary<string, bool>();
		public static Dictionary<int, bool> tempObjects			= new Dictionary<int, bool>();
		public static Dictionary<int, bool> lockedObjects		= new Dictionary<int, bool>();

		public static Dictionary<int, int> qtdInimigos 	= new Dictionary<int, int>();
		public static Sprite[] img 						= new Sprite[2];

		public static void ImgLoad(){
			int m;
			SQLiteDataReader reader;
			
			reader = Conexao.LoadData("select * from categoria");
			while(reader.Read())
	        	mapCreatorTabs[Convert.ToString(reader["nome"])] = Convert.ToBoolean(reader["mappable"]);

			string[] buttons = {"up", "down", "left", "right", "x", "y", "a", "b"};
			foreach(string x in buttons) Joystick.Button[x] = false;


			menuCreator.Viewport = new FloatRect(((float)Screen.width-250)/(float)Screen.width, 60/(float)Screen.height, 1, ((float)Screen.height-170)/(float)Screen.height);
			
			img[0]		= new Sprite();
			img[1]		= new Sprite();

			string[] diretorios = new string[]{
				"personagem", 
				"npc", 
				"localmap", 
				"misc", 
				"materia",
				"item",
				"recurso",
				"estrutura",
				"icone",
				"puzzle",
				"cursor",
				"mar",
				"worldmap",
				"video"
			};

			for(m=0;m<IMG_CAT.Count();m++)
				IMG_CAT[m] 	= new List<Texture>();

			for(int d=0;d<diretorios.Count();d++){
				m = 0;
				while(System.IO.File.Exists(@"res\img\"+diretorios[d]+@"\"+m+".png")){ 
					IMG_CAT[d].Add(new Texture("res/img/"+diretorios[d]+"/"+m+".png"));
					m++;
				}
			}

			Conexao.sql_con.Close(); 
		}

		public static void DbLoad(){
			SQLiteDataReader reader;
			
			reader = Conexao.LoadData("select * from configuracoes");
			while(reader.Read()){
				switch(Convert.ToInt32(reader["id"])){
					case 1:
						if(Convert.ToBoolean(reader["valor"]))
							Screen.Fullscreen(true, false);
					break;
					
					case 2:
						Configuracoes.VSync(Convert.ToBoolean(reader["valor"]), false);
					break;
				}
			}
			
			reader = Conexao.LoadData("select * from item");
			while(reader.Read())
	        	V.itens.Add(new Item(Convert.ToInt32(reader["id"]), Convert.ToInt32(reader["sprite"]), Convert.ToString(reader["nome"]), Convert.ToString(reader["descricao"]), Convert.ToString(reader["categoria"]), Convert.ToInt32(reader["efeito"])));

	       	reader = Conexao.LoadData("select * from recurso");
			while(reader.Read())
	        	V.recursos.Add(new Recurso(Convert.ToInt32(reader["id"]), Convert.ToString(reader["nome"]), Convert.ToString(reader["descricao"]), Convert.ToInt32(reader["sprite"])));
			
			reader = Conexao.LoadData("select * from mapa");
			while(reader.Read())
	        	 LoadFileState.mapas.Add(Convert.ToString(reader["nome"]));


	       	skills.Add(new Skill("Attack", 		"attack", 	1, 30, 20));
			skills.Add(new Skill("Heal", 		"heal", 	1, 30, 20));
			skills.Add(new Skill("Envenenado", 	"status", 	1, 0, 20));
			skills.Add(new Skill("Paralisado", 	"status", 	1, 1, 20));
			skills.Add(new Skill("Fraqueza", 	"status", 	1, 2, 20));
			skills.Add(new Skill("Confuso",		"status", 	1, 3, 20));

	       	puzzleObjects.Add(new Puzzle("Estrela", 	"-", 	"Vermelho"));
	       	puzzleObjects.Add(new Puzzle("-",			"2", 	"Azul"));
	       	puzzleObjects.Add(new Puzzle("-", 			"1", 	"-"));
	       	puzzleObjects.Add(new Puzzle("-", 			"-", 	"Verde"));
	       	puzzleObjects.Add(new Puzzle("-", 			"1", 	"Amarelo"));
	       	puzzleObjects.Add(new Puzzle("Quadrado", 	"-", 	"-"));
	       	puzzleObjects.Add(new Puzzle("Losango", 	"4", 	"-"));
	       	puzzleObjects.Add(new Puzzle("Triângulo",	"-",	"-"));

	       	puzzle2.Add(new Puzzle("Estrela", 	"1", 	"Vermelho"));
	       	puzzle2.Add(new Puzzle("Quadrado", 	"2", 	"Azul"));
	       	puzzle2.Add(new Puzzle("Triângulo", "3", 	"Verde"));
	       	puzzle2.Add(new Puzzle("Losango", 	"4", 	"Verde"));
	       	puzzle2.Add(new Puzzle("Estrela", 	"3", 	"Vermelho"));
	       	puzzle2.Add(new Puzzle("Quadrado", 	"1", 	"Amarelo"));
	       	puzzle2.Add(new Puzzle("Triângulo", "2", 	"Azul"));
	       	puzzle2.Add(new Puzzle("Losango", 	"4",	"Amarelo"));
		}

		public static Personagem personagem;
	}

	public partial class F{
		public static string txtAlerta 	= "";
		public static float cursorX		= 0;
		public static float cursorY		= 0;
		public static int letraAtual	= 0;
		public static int cursorIcon	= -1;
		public static string barra 		= "";

		public static void AddItem(Geral p, Item i){
			if(p.itens.Contains(i))
				p.itens[p.itens.IndexOf(i)].quantidade++;
			else
				p.itens.Add(i);
		}
		
		public static void AddRecurso(Geral x, int id, int qtd){
			Recurso it = x.recursos.Find(item => item.id == id);
			if (it != null) it.quantidade += qtd;
			else{
				Recurso r = (Recurso)V.recursos[id-1].Clone();
			
				x.recursos.Add(r);
				x.recursos[x.recursos.IndexOf(r)].Add(qtd);
			}
		}

		public static void Cursor(int value, float x, float y){
			cursorIcon 	= value;
			cursorX 	= x;
			cursorY 	= y;
		}

		public static void CursorDraw(){
			if(cursorIcon == -1)
				V.window.SetMouseCursorVisible(true);
			else{
				V.window.SetMouseCursorVisible(false);
				V.img[0].Texture 		= V.IMG_CAT[10][cursorIcon];
				V.img[0].Position 		= new Vector2f(V.mouseX-(int)(V.IMG_CAT[10][cursorIcon].Size.X*cursorX), V.mouseY-(int)(V.IMG_CAT[10][cursorIcon].Size.Y*cursorY));
				V.img[0].TextureRect	= new IntRect(0, 0, (int)V.IMG_CAT[10][cursorIcon].Size.X, (int)V.IMG_CAT[10][cursorIcon].Size.Y);
				V.window.Draw(V.img[0]);
			}
		}

		public static void AtivarTecla(string tecla){
			TeclaDesativada it = V.teclasDesativadas.Find(item => item.keyCode == tecla);
			if (it != null)
				V.teclasDesativadas.Remove(it);	
		}

		public static void DesativarTecla(string tecla, int mls){
			TeclaDesativada it = V.teclasDesativadas.Find(item => item.keyCode == tecla);
			if (it == null)
				V.teclasDesativadas.Add(new TeclaDesativada(tecla, mls));		
		}

		public static bool TeclaDesativada(string tecla){
			TeclaDesativada it = V.teclasDesativadas.Find(item => item.keyCode == tecla);

			return it != null ? true : false;
		}

		public static void TeclasDesativadasF(){
			int l = V.teclasDesativadas.Count;
			for(int x=0;x<l;x++){
				TimeSpan time = DateTime.Now - V.teclasDesativadas[x].inicio;

				if(time.TotalMilliseconds > V.teclasDesativadas[x].ms){
					V.teclasDesativadas.RemoveAt(x);
					break;
				}
			}
		}

		public static void DesenharShape(float x, float y, float width, float height, byte r, byte g, byte b, float op){
			RectangleShape shape 	= new RectangleShape();
			shape.Position 			= new Vector2f(x, y);
			shape.Size 				= new Vector2f(width, height);
			shape.FillColor 		= new Color(r, g, b, (byte)op);
			V.window.Draw(shape);
		}

		public static void DesenharShape(float x, float y, float width, float height, byte r, byte g, byte b, float op, byte r_, byte g_, byte b_, byte op_, float thickness){
			RectangleShape shape 	= new RectangleShape();
			shape.Position 			= new Vector2f(x, y);
			shape.Size 				= new Vector2f(width, height);
			shape.OutlineColor 		= new Color(r_, g_, b_, op_);
			shape.OutlineThickness 	= thickness;
			shape.FillColor 		= new Color(r, g, b, (byte)op);
			V.window.Draw(shape);
		}
		
		public static void DesenharTriangulo(float x, float y, float size, byte r, byte g, byte b, byte op){
			CircleShape triangle	= new CircleShape(size, 3);
			triangle.Position 		= new Vector2f(x, y);
			triangle.FillColor 		= new Color(r, g, b, (byte)op);
			triangle.Rotation		= 180;
			V.window.Draw(triangle);
		}

		public static void DesenharCirculo(float x, float y, float raio, float points, byte r, byte g, byte b, float op){
			CircleShape shape 		= new CircleShape();
			shape.FillColor 		= new Color(r,g,b,(byte)op);
			shape.Position 			= new Vector2f(x, y);
			shape.Radius 			= raio;
			shape.SetPointCount((uint)points);
			V.window.Draw(shape);
		}

		public static void Escrever(string text, bool bold, float x, float y, uint fontsize, byte r, byte g, byte b, byte op){
			Font font = bold ? V.eBitBold : V.eBit;

			Text texto 				= new Text(text, font, fontsize);
            texto.Position 			= new Vector2f((int)x, (int)y);
            texto.FillColor 		= new Color(r, g, b, op);
			
			V.window.Draw(texto);
		}

		public static void Escrever(string text, bool bold, float x, float y, uint fontsize, byte r, byte g, byte b, byte op, byte r_, byte g_, byte b_, byte op_, float t){
			Font font = bold ? V.eBitBold : V.eBit;

			Text texto 				= new Text(text, font, fontsize);
            texto.Position 			= new Vector2f((int)x, (int)y);
            texto.FillColor 		= new Color(r, g, b, op);
            texto.OutlineColor 		= new Color(r_, g_, b_, op_);
            texto.OutlineThickness 	= t;
			
			V.window.Draw(texto);
		}

		public static float TxtWidth(string text, uint fontsize, bool bold){
			Text texto 				= new Text();
			texto.Font 				= bold ? V.eBitBold : V.eBit;
			texto.DisplayedString 	= text;
			texto.CharacterSize 	= fontsize;
			return texto.GetGlobalBounds().Width;
		}

		public static float TxtHeight(string text, uint fontsize, bool bold){
			Text texto 				= new Text();
			texto.Font 				= bold ? V.eBitBold : V.eBit;
			texto.DisplayedString 	= text;
			texto.CharacterSize 	= fontsize;
			return texto.GetGlobalBounds().Height;
		}

		public static void BloquearMov(Geral x, bool y){
			x.pausado = y;

			if(x is Playable){
				Playable p = (Playable)x;
				p.andando 	= false;
			}
		}

		public static void Alerta(string txt){
			letraAtual = 1;
			txtAlerta = txt;
		}

		public static void AlertaF(){
			if(txtAlerta != ""){
				Playable x = Configuracoes.controle;

				if(!x.pausado && !TeclaDesativada("x")){
					x.interagindo = true;
					letraAtual = 1;
					BloquearMov(x, true);
					DesativarTecla("x", 500);
				}
				else if(x.pausado && F.Key("x") && !F.TeclaDesativada("x") && letraAtual >= txtAlerta.Length && Configuracoes.letraTempo == 50){
					x.interagindo = false;
					txtAlerta 	= "";
					letraAtual 	= 1;
					BloquearMov(x, false);
					DesativarTecla("x", 500);
					return;
				}
				
				if(!TeclaDesativada("letraAtual") && letraAtual < txtAlerta.Length){
					letraAtual++;
					DesativarTecla("letraAtual", Configuracoes.letraTempo);
				}

				if(txtAlerta.Substring(letraAtual-1, 1) == "#")
					letraAtual += 7;

				Configuracoes.letraTempo = Key("x") ? 10 : 50;
				
				DesenharShape(10, Screen.height-210, Screen.width-20, 180, 000, 000, 000, 200);
				new RichText(20, Screen.height-210, 22, true, txtAlerta.Substring(0, letraAtual));
				Escrever("Aperte X para continuar", true, Screen.width - TxtWidth("Aperte X para continuar", 20, true)-20, Screen.height-60, 20, 255, 255, 255, 255);
			}
		}

		public static bool MouseIn(float x, float y, float w, float h){
			float catX = (x + (w/2)) - (V.mouseX + 0.5f);
			float catY = (y + (h/2)) - (V.mouseY + 0.5f);
									
			float sumHalfWidth 	= w/2 + 0.5f;
			float sumHalfHeight = h/2 + 0.5f;
									
			return (Math.Abs(catX) < sumHalfWidth && Math.Abs(catY) < sumHalfHeight) ? true : false;
		}
		
		public static void Limpar(string[] textBoxes, string[] comboBoxes){
			for(int m=0;m<textBoxes.Count();m++){
				TxtBox it = V.txtBoxes.Find(item => item.id == textBoxes[m]);
				if(it != null)	it.txt = "";
			}
			for(int m=0;m<comboBoxes.Count();m++){
				CmbBox it = V.comboBoxes.Find(item => item.id == comboBoxes[m]);
				if(it != null){
					it.valor = it.valores[0];
					it.valorIndex = 0;
				}	
			}
		}

		public static bool Button(string txt, uint fontsize, float x, float y, float width, float height, byte r, byte g, byte b, float op){
			DesenharShape(x, y, width, height, r, g, b, op);
			Escrever(txt, true, x+(width/2)-(TxtWidth(txt, fontsize, true)/2), y-(TxtHeight(txt, fontsize, true)/2)+(height/2)-(fontsize/2), fontsize, 255, 255, 255, 255);
		
			bool click = false;
			if(MouseIn(x, y, width, height)){
				Cursor(1, 0.2f, 0);

				if(V.mouseButton == "Left")
					click = true;
			}

			return click;
		}
		
		public static void TextBox(string id, string filtro, string txt, float x, float y, float width, float height, byte r, byte g, byte b, float op){
			string barra_ 	= barra;
		
			if(MouseIn(x, y, width, height)){
				Cursor(0, 0, 0.5f);
				if(V.mouseButton == "Left")
					Configuracoes.textBoxId = id; 
			}
		
			TxtBox it = V.txtBoxes.Find(item => item.id == id);
			if(it == null) 	V.txtBoxes.Add(new TxtBox(id, txt));

			else{
				if(id == Configuracoes.textBoxId){
					if(Key("backspace") && !TeclaDesativada("backspace") && it.txt.Length > 0){
						it.txt = it.txt.Substring(0, it.txt.Length - 1);
						DesativarTecla("backspace", 100);
					}
					else{
						if(filtro == "Numeros"){
							Regex word = new Regex(@"^[0-9]*$");
							Match m = word.Match(V.textEntered);
							it.txt += m;
						}
						else
							it.txt += V.textEntered;
					}
					

					V.textEntered = "";
				}
				else
					barra_ = "";

				View txtBoxView 	= new View(new FloatRect(0, 0, width, height));
				txtBoxView.Viewport = new FloatRect(x/Screen.width, y/Screen.height, width/Screen.width, height/Screen.height);

				if(TxtWidth(it.txt, 24, false) > width)
					txtBoxView.Move(new Vector2f(TxtWidth(it.txt, 24, false)-width+5, 0));

				V.window.SetView(txtBoxView);

					float addTxtWidth;
					string textBoxValue;

					textBoxValue 	= filtro == "password" ? new Regex(".").Replace(it.txt, "*") : it.txt;
					addTxtWidth 	= TxtWidth(textBoxValue, 24, false) > width ? TxtWidth(textBoxValue, 24, false) - width : 0;

					DesenharShape(0, 0, width+addTxtWidth+5, height, r, g, b, op, 185, 185, 185, 255, 1);
					Escrever(textBoxValue+barra_, false, 2, -5, 24, 000, 000, 000, 255);
			}

			V.window.SetView(V.view);
		}

		public static void TextArea(string id, string txt, float x, float y, float width, float height, byte r, byte g, byte b, float op){
			string barra_ = barra;
		
			if(MouseIn(x, y, width, height)){
				Cursor(0, 0, 0.5f);
				if(V.mouseButton == "Left")
					Configuracoes.textBoxId = id; 
			}

			DesenharShape(x, y, width, height, r, g, b, op, 185, 185, 185, 255, 1);
		
			TxtBox it = V.txtBoxes.Find(item => item.id == id);
			if(it == null) 	V.txtBoxes.Add(new TxtBox(id, txt));

			else{
				if(id == Configuracoes.textBoxId){
					if(Key("backspace") && !TeclaDesativada("backspace") && it.txt.Length > 0){
						it.txt = it.txt.Substring(0, it.txt.Length - 1);
						DesativarTecla("backspace", 100);
					}
					else if(TxtWidth(it.txt+V.textEntered, 24, false) < width*(height/25)){
						it.txt += V.textEntered;
					}

					V.textEntered = "";
				}
				else 
					barra_ = "";

				float 	o 			= 0;
				float 	sumHeight 	= 0;
				int 	br 			= 0;
				
				int n = it.txt.Length;
				for(int m=0;m<n;m++){
					o = TxtWidth(it.txt.Substring(br,(m+1)-br), 24, false);
					
					if(o >= width){
						o = 0;
						br = m;
						sumHeight++;
					}
					
					barra_ = (m != n-1 || id != Configuracoes.textBoxId) ? "" : barra;
					Escrever(it.txt.Substring(br, (m+1)-br)+barra_, false, x+2, y-5+(25*sumHeight), 24, 000, 000, 000, 255);
				}
				if(n == 0)
					Escrever(barra_, false, x+2, y-5+(25*sumHeight), 24, 000, 000, 000, 255);
			}
		}
		
		public static void ComboBox(string id, List<string> valores, float x, float y, float width, float height, byte r, byte g, byte b, float op){
			DesenharShape(x, y, width, height, r, g, b, op);
			
			CmbBox it = V.comboBoxes.Find(item => item.id == id);
			if(it == null) 	V.comboBoxes.Add(new CmbBox(id, valores));
			else{
				Escrever(it.valor, false, x+2, y-5, 24, 000, 000, 000, 255);
			
				if(MouseIn(x, y, width, height) && V.mouseButton == "Left" && !TeclaDesativada("mouseLeft")){
					it.open = !it.open;
					DesativarTecla("mouseLeft", 500);
				}
					
				byte color = 255;

				if(it.open){
					int n = it.valores.Count();
					for(int m=0;m<n;m++){					
						if(MouseIn(x, y+(25*(m+1)), width, height)){
							DesenharShape(x, y+(25*(m+1)), width, height, 120, 120, 120, op);

							if(V.mouseButton == "Left"){
								it.valorIndex 	= m;
								it.valor 		= it.valores[m];
								it.open 		= false;
							}
						}
						else
						DesenharShape(x, y+(25*(m+1)), width, height, color, color, color, op, 000, 000, 000, 255, 1);
						Escrever(it.valores[m], false, x+2, y-5+(25*(m+1)), 24, 000, 000, 000, 255);

						if(color == 255)
							color = 220;
						else
							color = 255;
					}
				}
			}

			DesenharShape(x+width-25, y, 25, height-1, r, g, b, op);
			DesenharTriangulo(x+width-2, y+height-3, 10, 000, 000, 000, 255);
		}
	}
}