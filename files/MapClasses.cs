using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Main{
	public abstract class Geral{
		public float vida_				= 100;

		public int id;
		public string nome				= "";
		public string dialog			= "";
		public int categoria			= 0;	
		public int imgHeight			= 0;	
		public int imgWidth				= 0;	
		public float adcX 				= 0;
		public float adcY 				= 0;
		public int zindex				= 1;	
		public int qtdObj				= 0;	
		public int frame				= 0;	
		public int fps					= 0;	
		public int mapa 				= 0;
		public int direcao				= 0;
		public float x					= 0;	
		public float y					= 0;	
		public float width				= 0;	
		public float height				= 0;
		public float vida				= 100;
		public float vidaLimite 		= 100;	
		public float vidaA				= 0;	
		public bool interior			= false;
		public bool activated 			= false;
		public bool pausado				= false;
		public bool interagindo			= false;
		public bool camera				= false;
		public Item item;
		public List<Item> itens			= new List<Item>();
		public List<Recurso> recursos 	= new List<Recurso>();
		public int[] troca;
		public int step;
		
		public abstract void Reset();
		public abstract void Destroy();
		public abstract void Draw();
		public abstract void Interagir(int y);
	}

	public abstract class Playable : Geral{
		public float energia_		= 100;

		public string alerta		= "";
		public int speed			= 0;
		public float energia 		= 100;
		public float energiaA 		= 100;
		public float xp				= 0;
		public float xpA			= 0;
		public float movimentar		= 0;
		public bool gameOver 		= false;
		public bool batalha			= false;
		public bool andando			= false;
		public bool menu			= false;
		public int 	menuType 		= 0;
		public int 	status			= -1;
		public int 	statusTurnos	= 0;
		public float minForca 		= 0;
		public float maxForca 		= 0;
		public List<Skill> skills	= new List<Skill>();

		public Playable(){
			
		}

		public abstract void OnDie();
		public override void Reset(){vida 	= vida_; energia = energia_;}
		public override void Destroy(){}
		public override void Draw(){}
		public override void Interagir(int y){}

		public void AddSkill(Skill skill){
			this.skills.Add(skill);
		}
	}

	public class NonPlayable : Geral{
		public int itemDestroi	= 0;
		public bool destruido	= false;

		public override void Reset(){vida 	= vida_;}
		public override void Destroy(){}
		public override void Draw(){}
		public override void Interagir(int y){}
	}
	
	public class Personagem : Playable{
		public int montaria;
		public int nivel;

		public Personagem(int id, string nome, float x, float y, float width, float height, int xp, int vida, bool interior, int mapa, List<Item> itens, List<Recurso> recursos){
			this.id 		= id;
			this.x 			= x;
			this.y 			= y;
			this.height 	= height;
			this.width 		= width;
			this.imgHeight 	= 0;
			this.imgWidth 	= 0;
			this.direcao 	= 0;
			this.zindex 	= 0;
			this.nivel 		= 0;
			this.qtdObj 	= 0;
			this.movimentar = 0;
			this.xp 		= xp;
			this.xpA 		= xp;
			this.vida 		= vida;
			this.vidaA 		= vida;
			this.vida_ 		= vida;
			this.speed 		= 4;
			this.andando 	= false;
			this.menu 		= false;
			this.pausado 	= false;
			this.batalha 	= false;
			this.interior 	= interior;
			this.camera 	= true;
			this.categoria 	= 1;
			this.nome 		= nome;
			this.alerta 	= "";
			this.mapa 		= mapa;
			this.frame 		= -1;

			this.itens 		= itens;
			this.recursos 	= recursos;

			V.objetos.Add(this);
		}

		public override void Draw(){
			qtdObj = 0;

			V.objetos.Sort((s, f) => s.zindex.CompareTo(f.zindex));
			foreach(Geral o in V.objetos.ToList()) if(o != this)
				F.ColideBloco(this, o, true);

			if(qtdObj == 0)
				zindex = 0;
				
			foreach(Recurso r in recursos.ToList()) if(r.quantidade <= 0)
				recursos.Remove(r);
				
			foreach(Item i in itens.ToList()) if(i.quantidade <= 0)
				itens.Remove(i);

			Efeitos.AtualizarVida(this);
			Efeitos.AtualizarEnergia(this);
			Efeitos.DesenharXp();
			Interacoes.Movimentar(this);
			
			int imgN = andando ? (direcao < 2 ? 1 : 2) : 0;
			V.img[0].Texture = V.IMG_CAT[0][imgN];

			if (andando){
				imgWidth 	= direcao > 1 ? 140 : (direcao == 0 ? 100 : 108);
				imgHeight 	= 140;
			}
			else{
				imgWidth 	= 96;
				imgHeight 	= 136;
			}

			if(!F.TeclaDesativada("frame_personagem")){
				frame += (frame >= (int)(V.IMG_CAT[0][imgN].Size.X / imgWidth)-1) ? -frame : 1;
				F.DesativarTecla("frame_personagem", fps);
			}

			int realHeight = direcao;
			realHeight -= direcao > 1 ? 2 : 0;
			V.img[0].Position 		= new Vector2f(x, y + height - imgHeight);
			V.img[0].TextureRect	= new IntRect(imgWidth*frame, imgHeight*realHeight, imgWidth, imgHeight);

			imgWidth 	= 96;
			imgHeight 	= 136;

			V.window.Draw(V.img[0]);
		}

		public override void Interagir(int y){}

		public override void OnDie(){
			gameOver = Efeitos.GameOver(this);
		}
	}

	public class Npc : Playable{
		public bool mover 		= false;
		public bool	batalhar 	= false;
		public int 	sprite 		= 0;
			
		public Npc(int id, int sprite, float x, float y, float width, float height, float adcX, float adcY, float v, float F, bool mover, bool interior, bool activated, string dialog, int[] troca, bool batalhar){
			this.id 		= id;
			this.sprite 	= sprite;
			this.x 			= x;
			this.y 			= y;
			this.width 		= width;
			this.height 	= height;
			this.adcX 		= 0;
			this.adcY 		= 0;
			this.vida 		= v;
			this.vidaA 		= v;
			this.vida_ 		= v;
			this.minForca	= F / 2;
			this.maxForca	= F;
			this.mover 		= mover;
			this.dialog 	= dialog;
			this.troca 		= troca;
			this.batalhar 	= batalhar;
			this.interior 	= interior;
			this.activated 	= activated;

			if(!V.qtdInimigos.ContainsKey(sprite))
				V.qtdInimigos.Add(sprite, 1);
			else
				V.qtdInimigos[sprite]++;


			if(troca.Count() >= 1 && troca[0] > 0)
				itens.Add(V.itens[troca[0]-1]);

			if(this.sprite > -1){
				V.img[0].Texture 	= V.IMG_CAT[1][sprite];
				this.imgWidth 		= (int)V.IMG_CAT[1][sprite].Size.X;
				this.imgHeight 		= (int)V.IMG_CAT[1][sprite].Size.Y;
			}
		}

		public override void OnDie(){
			V.tempObjects[id] = true;
		
			Interacoes.DarItem(this, this.troca[0]);
			Destroy();
		}

		public override void Destroy(){
			V.qtdInimigos[sprite]--;
			V.objetos.RemoveAt(V.objetos.IndexOf(this));
		}

		public override void Draw(){
			if(activated) V.tempObjects[id] = true;

			if(sprite != -1){
				if(vidaA <= 0 || (activated && batalhar))
					OnDie();
				else{
					if(mover && movimentar <= 0 && !pausado && Configuracoes.controle != this)
						Interacoes.Andar(this, V.random.Next(1, 5), V.random.Next(1, 200));
					
					if(Efeitos.Render(x+adcX, y+height-imgHeight, imgWidth, imgHeight)){
						Efeitos.AtualizarVida(this);
						Interacoes.Movimentar(this);
						
						V.img[0].Texture 		= V.IMG_CAT[1][sprite];
						V.img[0].TextureRect	= new IntRect(0, 0, (int)imgWidth, (int)imgHeight);
						V.img[0].Position 		= new Vector2f(x+adcX, y+height-imgHeight+adcY);
						V.window.Draw(V.img[0]);
					}

					foreach(Geral o in V.objetos.ToList())
						if(o != this && !(o is Personagem))
							F.AtualizarZindex(o, this);
				}
			}
		}

		public override void Interagir(int y){
			if(y == 0 && Type.GetType("Main.Eventos").GetMethod("NPC_"+this.id+"_OnTouch") != null)
				Type.GetType("Main.Eventos").GetMethod("NPC_"+this.id+"_OnTouch").Invoke(Type.GetType("Main.Eventos"), new object[]{this});

			if(y == 1 && Type.GetType("Main.Eventos").GetMethod("NPC_"+this.id+"_OnAction") != null)
				Type.GetType("Main.Eventos").GetMethod("NPC_"+this.id+"_OnAction").Invoke(Type.GetType("Main.Eventos"), new object[]{this});
		}
	}
	
	public class Materia : NonPlayable{
		public int sprite 	= 0;
		public int recurso 	= 0;
	
		public Materia(int id, int sprite, int recurso, int itemDestroi, float x, float y, float width, float height, float adcX, float adcY, bool activated){
			this.id 			= id;
			this.sprite 		= sprite;
			this.recurso		= recurso;
			this.x 				= x;
			this.y 				= y;
			this.width 			= width;
			this.height 		= height;
			this.adcX 			= adcX;
			this.adcY 			= adcY;
			this.imgWidth 		= (int)V.IMG_CAT[4][sprite].Size.X;
			this.imgHeight 		= (int)V.IMG_CAT[4][sprite].Size.Y;
			this.itemDestroi 	= itemDestroi;
			
			this.activated 		= activated;
		}
		
		public override void Destroy(){
			Geral p = Configuracoes.controle;
			V.tempObjects[id] = true;
			V.objetos.Remove(this);
		}
		
		public void Give(Geral p){
			string msg = "";

			switch(sprite){
				case 0:
					msg = "Heroi.madeira +=  5";
				break;

				case 1:
					msg = "Heroi.pedra +=  5";
				break;
			}

			V.temptexts.Add(new TempText(msg, Screen.width/2-(F.TxtWidth(msg, 22, true)/2), Screen.height-60, 22, 1250, "hud"));
			F.AddRecurso(p, recurso, 5);
		}
	
		public override void Draw(){
			if(activated) V.tempObjects[id] = true;
			if(this.vida <= 0 || activated) Destroy();
		
			if(Efeitos.Render(x+adcX, y+height-imgHeight, imgWidth, imgHeight)){
				V.img[0].Texture 		= V.IMG_CAT[4][sprite];
				V.img[0].TextureRect	= new IntRect(0, 0, imgWidth, imgHeight);
				V.img[0].Position 		= new Vector2f(x+adcX, y+height-imgHeight+adcY);
				V.img[0].Scale 			= new Vector2f(1, 1);
				V.window.Draw(V.img[0]);
			}

			foreach(Geral o in V.objetos.ToList())
				if(o != this && !(o is Personagem))
					F.AtualizarZindex(o, this);
		}
		
		public override void Interagir(int y){
			if(y == 0){
				
			}
			else{
				Interacoes.Danificar(this);
			}
		}
	}
	
	public class Bau : NonPlayable{
		public int tipo;
		public bool locked;

		public Bau(int id, int tipo, bool locked, float x, float y, int item, bool activated){
			this.id 		= id;
			this.tipo 		= tipo;
			this.locked 	= locked;
			this.x 			= x;
			this.y 			= y;
			this.height 	= 30;
			this.categoria 	= 4;
			this.activated 	= activated;

			if(tipo == 0){
				V.img[0].Texture = V.IMG_CAT[3][1];
				this.width 		= (float)V.IMG_CAT[3][1].Size.X / 2;
				this.imgWidth 	= (int)V.IMG_CAT[3][1].Size.X / 2;
				this.imgHeight 	= (int)V.IMG_CAT[3][1].Size.Y;

				this.locked = false;
			}
			else{
				V.img[0].Texture = V.IMG_CAT[3][2];
				this.width 		= (float)V.IMG_CAT[3][2].Size.X / 3;
				this.height 	= (float)100;
				this.imgWidth 	= (int)V.IMG_CAT[3][2].Size.X / 3;
				this.imgHeight 	= (int)V.IMG_CAT[3][2].Size.Y;
			}

			itens.Add(V.itens[item-1]);
		}

		public override void Draw(){
			int aberto = activated ? 1 : 0;

			if(activated) V.tempObjects[id] = true;
			if(!locked) V.lockedObjects[id] = true;
			
			if(Efeitos.Render(x, y+height-imgHeight, imgWidth, imgHeight)){
				if(tipo == 0){
					V.img[0].Texture 		= V.IMG_CAT[3][1];
					V.img[0].Position 		= new Vector2f(x, y - imgHeight + height);
					V.img[0].TextureRect 	= new IntRect(imgWidth*aberto, 0, imgWidth, imgHeight);
					V.window.Draw(V.img[0]);
				}
				else{
					int s = locked ? 0 : (activated ? 2 : 1);

					V.img[0].Texture 		= V.IMG_CAT[3][2];
					V.img[0].Position 		= new Vector2f(x, y - imgHeight + height);
					V.img[0].TextureRect 	= new IntRect(imgWidth*s, 0, imgWidth, imgHeight);
					V.window.Draw(V.img[0]);
				}
			}
			
			foreach(Geral o in V.objetos.ToList())
				if(o != this && !(o is Personagem))
					F.AtualizarZindex(o, this);
		}

		public override void Interagir(int y){
			if(y == 1 && !locked)
				Interacoes.DarItem(this, this.troca[0]);
		}
	}

	public class Estrutura : NonPlayable{
		public int sprite;

		public Estrutura(int sprite, float x, float y, float width, float height, float adcX, float adcY){
			this.sprite			= sprite;
			this.x 				= x;
			this.y 				= y;
			this.width 			= width;
			this.height 		= height;
			this.adcX 			= adcX;
			this.adcY 			= adcY;
			this.imgWidth 		= (int)V.IMG_CAT[7][sprite].Size.X;
			this.imgHeight 		= (int)V.IMG_CAT[7][sprite].Size.Y;
		}

		public override void Draw(){
			if(activated) V.tempObjects[id] = true;
				
			if(Efeitos.Render(x+adcX, y+height-imgHeight, imgWidth, imgHeight)){
				V.img[0].Texture 		= V.IMG_CAT[7][sprite];
				V.img[0].TextureRect	= new IntRect(0, 0, imgWidth, imgHeight);
				V.img[0].Position 		= new Vector2f(x+adcX, y+height-imgHeight+adcY);
				V.img[0].Scale 			= new Vector2f(1, 1);
				V.window.Draw(V.img[0]);
			}

			foreach(Geral o in V.objetos.ToList())
				if(o != this && !(o is Personagem))
					F.AtualizarZindex(o, this);
		}
		
		public override void Interagir(int y){
			
		}
	}

	public class Decoracao : NonPlayable{
		public int sprite 	= 0;
	
		public Decoracao(int id, int sprite, float x, float y, float width, float height, float adcX, float adcY){
			this.id 			= id;
			this.sprite 		= sprite;
			this.x 				= x;
			this.y 				= y;
			this.width 			= width;
			this.height 		= height;
			this.adcX 			= adcX;
			this.adcY 			= adcY;
			this.imgWidth 		= (int)V.IMG_CAT[3][sprite].Size.X;
			this.imgHeight 		= (int)V.IMG_CAT[3][sprite].Size.Y;
		}
		
		public override void Destroy(){
			
		}
	
		public override void Draw(){		
			if(Efeitos.Render(x+adcX, y+height-imgHeight, imgWidth, imgHeight)){
				V.img[0].Texture 		= V.IMG_CAT[3][sprite];
				V.img[0].TextureRect	= new IntRect(0, 0, imgWidth, imgHeight);
				V.img[0].Position 		= new Vector2f(x+adcX, y+height-imgHeight+adcY);
				V.img[0].Scale 			= new Vector2f(1, 1);
				V.window.Draw(V.img[0]);
			}

			foreach(Geral o in V.objetos.ToList())
				if(o != this && !(o is Personagem))
					F.AtualizarZindex(o, this);
		}
		
		public override void Interagir(int y){
			
		}
	}
	
	public class Limite : NonPlayable{
		public Limite(float x, float y, float width, float height){
			this.x 		= x;
			this.y 		= y;
			this.width 	= width;
			this.height = height;
		}

		public override void Draw(){
			if(Efeitos.Render(x, y, width, height))
				foreach(Geral o in V.objetos.ToList()) if(o is Playable)
					F.ColideBloco(o, this, false);
		}
		public override void Interagir(int y){

		}
	}

	public class Teleporte : NonPlayable{
		public string teleporteTipo;

		public Teleporte(float x, float y, float width, float height, int mapa, string teleporteTipo){
			this.x 				= x;
			this.y 				= y;
			this.width 			= width;
			this.height 		= height;
			this.mapa 			= mapa;
			this.teleporteTipo 	= teleporteTipo;
		}

		public override void Draw(){
			if(Efeitos.Render(x, y, width, height))
				foreach(Geral o in V.objetos.ToList()) if(o is Playable)
					F.ColideBloco(o, this, false);
		}

		public override void Interagir(int y){
			Interacoes.MudarMapa(this, this.teleporteTipo);
		}
	}

	public class SavePoint : NonPlayable{
		public SavePoint(int id, float x, float y, int mapa){
			this.id 			= id;
			this.x 				= x;
			this.y 				= y;
			this.mapa 			= mapa;

			this.width 			= 40;
			this.height 		= 40;
		}

		public override void Draw(){
			if(Efeitos.Render(x, y, width, height)){
				V.img[0].Position 		= new Vector2f(x, y);
				V.img[0].Texture 		= V.IMG_CAT[3][0];
				V.img[0].TextureRect	= new IntRect(0, 0, (int)width, (int)height);
				V.window.Draw(V.img[0]);

				foreach(Geral o in V.objetos.ToList()) if(o is Playable)
					F.ColideBloco(o, this, true);
			}
		}

		public override void Interagir(int y){
			if(y == 1)
				Interacoes.Salvar(this);
		}
	}
	
	public class Chao{
		public int sprite		= 0;
		public float x			= 0;
		public float y			= 0;
		public float width		= 0;
		public float height		= 0;
		public bool lCima		= false;
		public bool lBaixo		= false;
		public bool lEsquerda	= false;
		public bool lDireita	= false;
		public bool camera		= true; 
	
		public Chao(int t, int cX, int cY){
			this.sprite		= t;
			this.width 		= (float)V.IMG_CAT[2][sprite].Size.X;
			this.height 	= (float)V.IMG_CAT[2][sprite].Size.Y;
			this.x			= (float)cX*width;
			this.y			= (float)cY*height;

			if(x == 0)
				V.mapWidth += (int)V.IMG_CAT[2][sprite].Size.X;
			if(y == 0)
				V.mapHeight += (int)V.IMG_CAT[2][sprite].Size.Y;
		}

		public void Draw(){
			if(Efeitos.Render(x, y, width, height)){
				V.img[0].Position 		= new Vector2f(x, y);
				V.img[0].Texture 		= V.IMG_CAT[2][sprite];
				V.img[0].TextureRect	= new IntRect(0, 0, (int)width, (int)height);
				V.window.Draw(V.img[0]);
			}
		}

		public void SetLimits(){
			for (int n = 0;n < V.chao.Count;n++) {
				if(V.chao[n] != this)
					F.ChaoLimite(this, V.chao[n]);
				if(lCima && lBaixo && lEsquerda && lDireita)
					break;
			}
			
			if(!lCima)
				V.limites.Add(new Limite(x, y, width, 1));
			if(!lEsquerda)
				V.limites.Add(new Limite(x, y, 1, height));
			if(!lDireita)
				V.limites.Add(new Limite(x+width, y, 1, height));
			if(!lBaixo)
				V.limites.Add(new Limite(x, y+height, width, 1));
		}
	}
	
	public class TeclaDesativada{
		public string keyCode;
		public int ms;
		public DateTime inicio 	= DateTime.Now;

		public TeclaDesativada(string k, int mls){
			keyCode = k;
			ms 		= mls;
		}
	}
	
	public class Item{
		public int 		id;
		public int 		sprite;
		public string 	nome;
		public string 	descricao;
		public string 	categoria;
		public float 	efeito;
		public float 	quantidade;
		public bool 	battle;
		
		private string 	battleDescricao;
		
		public Item(int id, int sprite, string nome, string descricao, string categoria, int efeito){
			this.id 		= id;
			this.sprite 	= sprite;
			this.nome 		= nome;
			this.descricao 	= descricao;
			this.categoria 	= categoria;
			this.efeito 	= efeito;
			this.quantidade = 1;
			
			if(this.categoria == "heal"){
				this.battleDescricao = "#f92659public static #3da3efvoid #a6e22e"+this.nome+" #ffffff(){/n/n	Heroi.vida	   #f92659+= #ae81ff"+this.efeito+"#ffffff;/n/n}";
				battle = true;
			}
		}
		
		public void Use(List<Playable> x1, List<Playable> x2){
			if(this.categoria == "heal"){
				if(x1.Contains(x1[0]))
					x1[0].vida += efeito;

				this.quantidade -= 1;
				V.temptexts.Add(new TempText("+"+this.efeito, x1[0].x + x1[0].imgWidth + 10, x1[0].y + x1[0].height - x1[0].imgHeight + 15, 22, 500, "view"));
			}

			if(this.quantidade <= 0)
				this.Destroy();
		}
		
		public string Descricao(){
			return battleDescricao;
		}
		
		public string Nome(){
			return nome;
		}

		public void Destroy(){
			if(Configuracoes.controle.itens.Contains(this))
				Configuracoes.controle.itens.Remove(this);
		}
	}
	
	public class Recurso{
		public int 		id 			= 0;
		public string 	nome		= "";
		public string 	descricao	= "";
		public float 	quantidade;
		public int 		sprite;
		
		public Recurso(int id, string nome, string descricao, int sprite){
			this.id 		= id;
			this.nome 		= nome;
			this.descricao 	= descricao;
			this.sprite 	= sprite;
		}

		public void Add(float quantidade){
			this.quantidade += quantidade;
		}

		public object Clone(){
			return this.MemberwiseClone();
		}
	}

	public class IC{
		public Geral x1;
		public Geral x2;

		public IC(Geral x1, Geral x2){
			this.x1 	= x1;
			this.x2 	= x2;
		}
		public void Draw(){
			x1.Interagir(1);
		}
	}
	
	public class TxtBox{
		public string id;
		public string txt;
		
		public TxtBox(string id, string txt){
			this.id 	= id;
			this.txt 	= txt;
		}
	}
	
	public class CmbBox{
		public string 		id;
		public string 		valor;
		public int 			valorIndex;
		public List<string> valores;
		public bool 		open;
		
		public CmbBox(string id, List<string> valores){
			this.id 		= id;
			this.valores 	= valores;
			this.valor 		= valores[0];
			this.valorIndex = 0;
		}
	}

	public class Puzzle{
		public string 	forma;
		public string 	numero;
		public string 	cor;
		
		public Puzzle(string forma, string numero, string cor){
			this.forma 	= forma;
			this.numero = numero;
			this.cor 	= cor;
		}
	}

	public class Skill{
		private string 		nome;
		private string 		descricao;
		private float 		qtd;		//quantas pessoas vai atingir
		private string 		categoria; 	//heal, attack
		private float 		efeito; 	//aumenta/diminui vida/energia
		private float 		energia;	//energia necessaria
		private string[]	status = new string[]{"Envenenado", "Paralisado", "Fraqueza", "Confuso"};
		//private Animacao animacao;

		public Skill(string nome, string categoria, float qtd, float efeito, float energia){
			this.nome 	 	= nome;
			this.qtd 	 	= qtd;
			this.categoria 	= categoria;
			this.efeito  	= efeito;
			this.energia 	= energia;

			if(this.categoria == "attack")
				this.descricao = "#f92659public static #3da3efvoid #a6e22e"+this.nome+" #ffffff(#3da3efInimigo #fd971finimigo#ffffff){/n/n	Heroi.energia	#f92659-= #ae81ff"+this.energia+"#ffffff;/n	#ffffffinimigo.vida	 #f92659-= #ae81ff"+this.efeito+"#ffffff;/n/n}";
				
			if(this.categoria == "heal")
				this.descricao = "#f92659public static #3da3efvoid #a6e22e"+this.nome+" #ffffff(){/n/n	Heroi.energia	#f92659-= #ae81ff"+this.energia+"#ffffff;/n	Heroi.vida	   #f92659+= #ae81ff"+this.efeito+"#ffffff;/n/n}";
				
			if(this.categoria == "status")
				this.descricao = "#f92659public static #3da3efvoid #a6e22e"+this.nome+" #ffffff(#3da3efInimigo #fd971finimigo#ffffff){/n/n	Heroi.energia	#f92659-= #ae81ff"+this.energia+"#ffffff;/n	#ffffffinimigo.status   #f92659= #ffffff"+this.status[(int)this.efeito]+"#ffffff;/n/n}";
		}

		public bool Use(List<Playable> x1, List<Playable> x2){
			if(x1[0].energia >= this.energia){

				if(this.categoria == "attack"){
					for(int m=0;m<this.qtd;m++)
						if(x2.Contains(x2[m]))
							x2[m].vida -= efeito;
							
					x1[0].energia -= this.energia;
					V.temptexts.Add(new TempText("-"+this.efeito, x2[0].x + x2[0].imgWidth + 10, x2[0].y + x2[0].height - x2[0].imgHeight + 15, 22, 500, "view"));
				}

				if(this.categoria == "heal"){
					for(int m=0;m<this.qtd;m++)
						if(x1.Contains(x1[m]))
							x1[m].vida += efeito;
							
					x1[0].energia -= this.energia;
					V.temptexts.Add(new TempText("+"+this.efeito, x1[0].x + x1[0].imgWidth + 10, x1[0].y + x1[0].height - x1[0].imgHeight + 15, 22, 500, "view"));
				}

				if(this.categoria == "status"){
					string tmp;
					x2[0].statusTurnos = V.random.Next(0, 4);

					if(x2[0].statusTurnos > 0){
						x2[0].status = (int)this.efeito;
						tmp = this.status[(int)this.efeito]+"!";
					}
					else
						tmp = "Errou!";


					x1[0].energia -= this.energia;
					V.temptexts.Add(new TempText(tmp, x2[0].x + x2[0].imgWidth + 10, x2[0].y + x2[0].height - x2[0].imgHeight + 15, 22, 750, "view"));
				}
				
				return true;
			}
			else
				return false;
		}

		public string Nome(){
			return this.nome;
		}

		public float Efeito(){
			return this.efeito;
		}

		public string Descricao(){
			return this.descricao;
		}
	}

	public class TempText{
		private string 	txt;
		private string 	view;
		private float 	x;
		private float 	y;
		private float 	aumentarY;
		private byte 	opacidade;
		private uint 	fontsize;

		public TempText(string txt, float x, float y, uint fontsize, int ms, string view){
			this.txt 		= txt;
			this.view 		= view;
			this.x 			= x;
			this.y 			= y;
			this.aumentarY 	= 20;	
			this.opacidade	= 255;
			this.fontsize	= fontsize;

			F.DesativarTecla(txt, ms);
		}
		
		public void Destroy(){
			V.temptexts.Remove(this);
		}

		public void Draw(){
			if(view == "view")
				V.window.SetView(V.view);

			if(view == "hud")
				V.window.SetView(V.hud);


			if(!F.TeclaDesativada(txt) && aumentarY > 0){
				y 			-= 1;
				aumentarY 	-= 1;
				opacidade 	-= (byte)12.75f;
			}

			if(aumentarY <= 0)
				Destroy();

			F.Escrever(txt, true, x, y, fontsize, 255, 255, 255, opacidade, 0, 0, 0, opacidade, 2);

			V.window.SetView(V.hud);
		}
	}

	public class PopUp{
		public 	int 	tipo;
		public 	Item 	item;
		public 	Recurso recurso;
		private float 	aumentarY;
		private byte 	opacidade;

		public PopUp(Item item){
			this.item 		= item;
			this.tipo 		= 0;
			this.aumentarY 	= 50;
			this.opacidade 	= 170;

			V.popups.Add(this);
			F.DesativarTecla("popup", 750);
		}

		public PopUp(Recurso recurso){
			this.recurso 	= recurso;
			this.tipo 		= 1;
			this.aumentarY 	= 50;
			this.opacidade 	= 170;

			V.popups.Add(this);
			F.DesativarTecla("popup", 750);
		}

		public void Draw(){
			int sprite 	= (tipo == 0) ? item.sprite : recurso.sprite;
			string nome = (tipo == 0) ? item.nome : recurso.nome;

			F.DesenharShape(10, aumentarY+10, 100+F.TxtWidth(nome, 28, true), 70, 000, 000, 000, opacidade, 255, 255, 255, opacidade, 1);
			F.Escrever(nome, true, 90, aumentarY+22.5f, 28, 255, 255, 255, opacidade);

			V.img[1].Texture 		= V.IMG_CAT[5][sprite];
			V.img[1].Scale 			= new Vector2f((float)64/V.IMG_CAT[5][sprite].Size.X, (float)64/V.IMG_CAT[5][sprite].Size.Y);
			V.img[1].Position 		= new Vector2f(10, aumentarY+10);
			V.img[1].TextureRect	= new IntRect(0, 0, (int)V.IMG_CAT[5][sprite].Size.X, (int)V.IMG_CAT[5][sprite].Size.Y);
			V.window.Draw(V.img[1]);

			if(!F.TeclaDesativada("popup")){
				aumentarY 	-= 1;
				opacidade 	-= (byte)3.4f;

				if(aumentarY <= 0)
					this.Destroy();
			}
		}

		public void Destroy(){
			V.popups.Remove(this);
		}
	}
	
	public class SaveFile{
		public int id;
		public int savePoint;
		public string mapaNome;
	
		public SaveFile(int id, int savePoint, string mapaNome){
			this.id 		= id;
			this.savePoint 	= savePoint;
			this.mapaNome 	= mapaNome;
		}

		public bool Empty(){
			return savePoint == 0 ? true : false;
		}
	}

	public class Controle{
		public int id;
		public int keycode;
		public string nome;

		public Controle(int id, string nome, int keycode){
			this.id 		= id;
			this.nome 		= nome;
			this.keycode 	= keycode;
		}
	}

}