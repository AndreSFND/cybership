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
	public class BattleState : Screen{
		public static List<Playable> x1;
		public static List<Playable> x2;
		public static int option	= 0;
		public static int option2	= 0;
		public static bool menu = false, submenu = false, finish = false, turno = false, skillEmpty = false, itemEmpty = false;
		public static string b;
		
		public static int delayTurno = 750;

		public override void Draw(){
			if(V.window.Size.X < 800 || V.window.Size.Y < 600) V.window.Size = new Vector2u(800, 600);

			LocalMapDraw();
			
			if(!x1[0].andando && !finish)
				TelaPrincipal();
			if(finish)
				Fim();
		}

		public override void OnExit(){
			
		}

		public override void OnStart(){
			menu = true;
			submenu = turno = finish = false;

	        foreach (Playable x in x1.ToList()){
	        	F.BloquearMov(x, true);
	        	Interacoes.Andar(x, 2, 200);
	        }

	        foreach (Playable x in x2.ToList()){
	        	F.BloquearMov(x, true);
	        	Interacoes.Andar(x, 3, 200);
	        }
		}

		public static void Set(List<Playable> x1, List<Playable> x2){
			BattleState.x1 = x1;
			BattleState.x2 = x2;
		}

		public static void TelaPrincipal(){
			V.window.SetView(V.view);
				foreach (Playable x in x1.ToList()){
		        	F.DesenharShape(x.x - 50 + (x.imgWidth / 2), x.y - x.imgHeight + x.height - 50, 100, 10, 19, 19, 19, 230);
					F.DesenharShape(x.x - 50 + (x.imgWidth / 2), x.y - x.imgHeight + x.height - 50, (100/x.vida_)*x.vidaA, 10, 000, 255, 000, 230);
					
					F.DesenharShape(x.x - 50 + (x.imgWidth / 2), x.y - x.imgHeight + x.height - 65, 100, 10, 19, 19, 19, 230);
					F.DesenharShape(x.x - 50 + (x.imgWidth / 2), x.y - x.imgHeight + x.height - 65, (100/x.energia_)*x.energiaA, 10, 255, 255, 000, 230);
		        }

		        foreach (Playable x in x2.ToList()){
		        	F.DesenharShape(x.x - 50 + (x.imgWidth / 2), x.y - x.imgHeight + x.height - 30, 100, 15, 19, 19, 19, 230);
					F.DesenharShape(x.x - 50 + (x.imgWidth / 2), x.y - x.imgHeight + x.height - 30, (100/x.vida_)*x.vidaA, 15, 000, 255, 000, 230);
		        }


			V.window.SetView(V.hud);

				F.DesenharShape(10, Screen.height-200, Screen.width-20, 190, 210, 210, 210, 230);
				F.DesenharShape(230, Screen.height-190, Screen.width-250, 170, 19, 19, 19, 255);
				
				HeroiAtacar();
				InimigoAtacar();

				int qtdX1=0, qtdX2=0;

				foreach (Playable x in x1.ToList())
		        	if(x.vidaA <= 0) qtdX1++;

		        foreach (Playable x in x2.ToList())
		        	if(x.vidaA <= 0){
						x.OnDie();
						x2.Remove(x);
						qtdX2++;
					}

		        if(qtdX1 >= x1.Count() && !finish)
					finish = true;

		        if(qtdX2 >= x2.Count() && !finish){
					finish = true;
		        	foreach (Playable x in x1.ToList())
						Interacoes.Andar(x, 3, 200);
				}
		}

		public static void HeroiAtacar(){
			int n;

			if(menu){
				for(n=0;n<3;n++){
					byte m = (option == n) ? (byte)35 : (byte)19;
					F.DesenharShape(20, Screen.height-190+(60*n), 200, 50, m, m, m, 255);
				}

				F.Escrever("Selecione uma opção", true, 240, Screen.height-190, 22, 255, 255, 255, 255);

				byte c = (!turno && !F.TeclaDesativada("delayTurno")) ? (byte)255 : (byte)135;
				byte g;
				
				n = 0;
				foreach(Item x in Configuracoes.controle.itens.ToList())  if (n < 3 && x.battle) n++;
				
				skillEmpty 	= (Configuracoes.controle.skills.ToList().Count() <= 0) ? true : false;
				itemEmpty 	= (n <= 0) ? true : false;
				

				F.Escrever("Atacar", true, 120-(F.TxtWidth("Atacar", 28, true)/2), Screen.height-187.5f, 28, c, c, c, 255);
				
				g = skillEmpty ? (byte)135 : (byte)255;
				F.Escrever("Skills", true, 120-(F.TxtWidth("Skills", 28, true)/2), Screen.height-127.5f, 28, g, g, g, 255);
				
				g = itemEmpty ? (byte)135 : (byte)255;
				F.Escrever("Itens", true, 120-(F.TxtWidth("Itens", 28, true)/2), Screen.height-67.5f, 28, g, g, g, 255);
				
				if(F.Key("up") && !F.TeclaDesativada("up") && option > 0){
					option--;
					F.DesativarTecla("up", 200);
				}
				if(F.Key("down") && !F.TeclaDesativada("down") && option < 2){
					option++;
					F.DesativarTecla("down", 200);
				}
				if(F.Key("x") && !F.TeclaDesativada("x")){
					menu = !menu;
					F.DesativarTecla("x", 200);
				}
			}

			if(!menu){
				skillEmpty = itemEmpty = false;
			
				if(F.Key("c") && !F.TeclaDesativada("c")){
					option2 = 0;
					menu = !menu;
					F.DesativarTecla("c", 200);
				}

				byte c = (!turno && !F.TeclaDesativada("delayTurno")) ? (byte)255 : (byte)135;
			
				switch(option){
					case 0:
						if(!turno && !F.TeclaDesativada("delayTurno")){
							x2[0].vida -= 20;
							turno = !turno;
							V.temptexts.Add(new TempText("-20", x2[0].x + x2[0].imgWidth + 10, x2[0].y + x2[0].height - x2[0].imgHeight + 15, 22, 500, "view"));
							F.DesativarTecla("delayTurno", delayTurno);
						}

						menu = !menu;
					break;
					
					case 1:
						n = 0;
						
						foreach(Skill x in Configuracoes.controle.skills.ToList())  if (n < 3){
							byte m = (option2 == n) ? (byte)35 : (byte)19;
							
							F.DesenharShape(20, Screen.height-190+(60*n), 200, 50, m, m, m, 255);
							F.Escrever(x.Nome(), true, 120-(F.TxtWidth(x.Nome(), 28, true)/2), Screen.height-187.5f+(60*n), 28, c, c, c, 255);
					
							if(option2 == n)
								new RichText(240, Screen.height-190, 24, true, x.Descricao());
							
							if(F.Key("x") && !F.TeclaDesativada("x") && !F.TeclaDesativada("delayTurno") && option2 == n && !turno){
								if(Configuracoes.controle.skills[n].Use(x1, x2)){
									turno = !turno;
									F.DesativarTecla("delayTurno", delayTurno);
								}
								else
									V.temptexts.Add(new TempText("Energia Insuficiente!", Screen.width/2-(F.TxtWidth("Energia Insuficiente!", 22, true)/2), Screen.height-235, 22, 500, "hud"));
							}

							n++;
						}
						if(n <= 0){
							menu = !menu;
							break;
						}

						if(F.Key("up") && !F.TeclaDesativada("up") && option2 > 0){
							option2--;
							F.DesativarTecla("up", 200);
						}
						if(F.Key("down") && !F.TeclaDesativada("down") && option2 < n-1){
							option2++;
							F.DesativarTecla("down", 200);
						}
					break;
					
					case 2:
						n = 0;

						foreach(Item x in Configuracoes.controle.itens.ToList())  if (n < 3 && x.battle){
							byte m = (option2 == n) ? (byte)35 : (byte)19;
							
							F.DesenharShape(20, Screen.height-190+(60*n), 200, 50, m, m, m, 255);
							F.Escrever(x.Nome(), true, 120-(F.TxtWidth(x.Nome(), 28, true)/2), Screen.height-187.5f+(60*n), 28, c, c, c, 255);
					
							if(option2 == n)
								new RichText( 240, Screen.height-190, 24, true, x.Descricao());
							
							if(F.Key("x") && !F.TeclaDesativada("x") && !F.TeclaDesativada("delayTurno") && option2 == n && !turno){
								x.Use(x1, x2);
								turno = !turno;
								F.DesativarTecla("delayTurno", delayTurno);
							}

							n++;
						}
						
						if(n <= 0){
							menu = !menu;
							break;
						}

						if(F.Key("up") && !F.TeclaDesativada("up") && option2 > 0){
							option2--;
							F.DesativarTecla("up", 200);
						}
						if(F.Key("down") && !F.TeclaDesativada("down") && option2 < n-1){
							option2++;
							F.DesativarTecla("down", 200);
						}
					break;
				}
			}
		}

		public static void InimigoAtacar(){
			float dano;
			bool atacar;
			double fraqueza = 1;

			if(turno && !F.TeclaDesativada("delayTurno")){
				switch(x2[0].status){
					case 0:
						dano = V.random.Next(10, 30);
						x2[0].vida -= dano;
						V.temptexts.Add(new TempText("-"+dano, x2[0].x + x2[0].imgWidth + 10, x2[0].y + x2[0].height - x2[0].imgHeight + 15, 22, 500, "view"));

						atacar = true;
					break;

					case 1:
						atacar = false;
						turno = !turno;
					break;

					case 2:
						fraqueza = Math.Round(V.random.NextDouble(), 2);
						atacar = true;
					break;

					case 3:
						if(V.random.Next(0, 4) != 0){
							dano = (float)(V.random.Next((int)x2[0].minForca, (int)x2[0].maxForca)*fraqueza);
							int alvo = V.random.Next(0, x2.Count()-1);

							x2[alvo].vida -= dano;
							turno = !turno;
							F.DesativarTecla("delayTurno", delayTurno);
							V.temptexts.Add(new TempText("-"+dano, x2[alvo].x + x2[alvo].imgWidth + 10, x2[alvo].y + x2[alvo].height - x2[alvo].imgHeight + 15, 22, 500, "view"));

							atacar = false;
						}
						else
							atacar = true;
					break;

					default:
						atacar = true;
					break;
				}

				x2[0].statusTurnos -= x2[0].status > -1 ? 1 : 0;
				if(x2[0].statusTurnos <= 0) x2[0].status = -1;

				if(atacar){
					dano = (float)(V.random.Next((int)x2[0].minForca, (int)x2[0].maxForca)*fraqueza);
					int alvo = V.random.Next(0, x1.Count()-1);

					x1[alvo].vida -= dano;
					turno = !turno;
					F.DesativarTecla("delayTurno", delayTurno);
					V.temptexts.Add(new TempText("-"+dano, x1[alvo].x + x1[alvo].imgWidth + 10, x1[alvo].y + x1[alvo].height - x1[alvo].imgHeight + 15, 22, 500, "view"));
				}
			}
		}
		
		public static void Fim(){
			if(x1[0].movimentar <= 0 && x1[0].vida > 0)
				CurrentScreen.Change("localmap");
			else if(x1[0].vidaA <= 0)
				x1[0].OnDie();
		}

		public static void LocalMapDraw(){
			V.window.SetView(V.view);

				foreach(Chao x in V.chao.ToList())
					x.Draw();

				foreach(Limite x in V.limites.ToList())
					x.Draw();

				V.objetos.Sort((x, y) => x.zindex.CompareTo(y.zindex));
				foreach(Geral x in V.objetos.ToList())
					x.Draw();

			V.window.SetView(V.hud);

				foreach(TempText x in V.temptexts.ToList())
					x.Draw();	

				Efeitos.BackdropRun();
		}
	}
}