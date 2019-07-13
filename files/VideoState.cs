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
	public class VideoState : Screen{

		private static Geral npc_;

		public static int step;
		public static int video;
		public static Geral npc;

		public static float npcX;
		public static float npcY;

		public static float cowOpacity;

		public static float atributoUm;
		public static float atributoDois;
		public static float atributoTres;

		public static int atributoUmY;
		public static int atributoDoisY;
		public static int atributoTresY;

		public override void Draw(){
			V.window.SetView(V.hud);

			TelaPrincipal();
		}

		public override void OnExit(){
			Idk.windowColor = new Color(000, 000, 000);
		}

		public override void OnStart(){
			Idk.windowColor = new Color(255, 255, 255);
			Efeitos.opacidade = 0;
			Efeitos.Backdrop("fade-in-white", 255, 2.55f);

			step 		= 0;
			cowOpacity 	= 0;
			npcX 		= npc.x-(V.view.Center.X - (Screen.width/2));
			npcY 		= (npc.y+npc.height-npc.imgHeight)-(V.view.Center.Y - (Screen.height/2));

			atributoUm 	= 0;
			atributoDois= 0;
			atributoTres= 0;

			atributoUmY = 20;
			atributoDoisY=20;
			atributoTresY=20;
		}

		public static void TelaPrincipal(){
			if(Efeitos.opacidade < 255)
				LocalMapDraw();

			switch(video){
				case 1:
					Video1();
				break;

				case 2:
					Video2();
				break;
			}
		}

		public static void Video1(){
			V.img[0].Texture 		= V.IMG_CAT[1][3];
			V.img[0].TextureRect	= new IntRect(0, 0, (int)npc.imgWidth, (int)npc.imgHeight);
			V.img[0].Position 		= new Vector2f(npcX, npcY);
			V.img[0].Color 			= new Color(255, 255, 255, 255);
			V.window.Draw(V.img[0]);

			V.img[0].Texture 		= V.IMG_CAT[13][0];
			V.img[0].TextureRect	= new IntRect(0, 0, 160, 96);
			V.img[0].Position 		= new Vector2f(npcX-350, npcY+150);
			V.img[0].Color 			= new Color(255, 255, 255, (byte)cowOpacity);
			V.window.Draw(V.img[0]);

			F.Escrever("-Nome", 	true, npcX-350+80-(F.TxtWidth("-Nome", 30, true)/2), npcY+280+atributoUmY, 	30, 000, 000, 000, (byte)atributoUm);
			F.Escrever("-Peso", 	true, npcX-350+80-(F.TxtWidth("-Peso", 30, true)/2), npcY+310+atributoDoisY, 30, 000, 000, 000, (byte)atributoDois);
			F.Escrever("-Altura", 	true, npcX-350+80-(F.TxtWidth("-Altura", 30, true)/2), npcY+340+atributoTresY, 30, 000, 000, 000, (byte)atributoTres);

			float o = cowOpacity > 200 ? 200 : cowOpacity;
			F.DesenharShape(npcX+180, npcY+145, 290, 120, 000, 000, 000, o);
			new RichText(npcX+190, npcY+150, 24, true, (byte)cowOpacity, @"#3da3efVaca #ffffffvaca #f92659= new #3da3efVaca#ffffff();/n/nvaca.nome #f92659= #e6db5a""Loló""#ffffff;/nvaca.peso #f92659= #ae81ff720#3da3eff#ffffff;/nvaca.altura #f92659= #ae81ff1.5#3da3eff#ffffff;");

			V.img[0].Color = new Color(255, 255, 255, 255);

			switch(step){
				case 0:
					if(Efeitos.opacidade >= 255 && npcY > 50)
						npcY--;
					else if(npcY <= 50 && Efeitos.opacidade >= 50) 
						step++;
				break;

				case 1:
					if(F.txtAlerta == "")
						F.Alerta("Até onde sabemos, a #ff0000torre #ffffffpossui somente um enigma. /nPara resolvê-lo, você precisará saber o que são #00ffffobjetos #ffffffe #00ffffatributos#ffffff.");
					if(F.Key("x") && !F.TeclaDesativada("x") && F.letraAtual >= F.txtAlerta.Length && Configuracoes.letraTempo == 50){
						F.Alerta("Em resumo, um #00ffffobjeto #ffffffé uma #ff0000variável #ffffffque pode armazenar vários valores./n/n/n/n/n/n/n/n~Uma #ff0000variável #ffffffé algo que pode armazenar um valor, como um número ou uma palavra~");
						step++;
					}
				break;

				case 2:
					if(F.Key("x") && !F.TeclaDesativada("x") && F.letraAtual >= F.txtAlerta.Length && Configuracoes.letraTempo == 50){
						F.Alerta("Chamamos esses valores de #00ffffatributos.");
						step++;
					}
				break;

				case 3:
					if(F.Key("x") && !F.TeclaDesativada("x") && F.letraAtual >= F.txtAlerta.Length && Configuracoes.letraTempo == 50){
						F.Alerta("Um exemplo?/nHmmm.../nAh, Claro!");
						step++;
					}
				break;

				case 4:
					if(F.Key("x") && !F.TeclaDesativada("x") && F.letraAtual >= F.txtAlerta.Length && Configuracoes.letraTempo == 50){
						F.Alerta("O #00ffffobjeto #ff0000vaca #ffffffpossui os #00ffffatributos #ffffffnome, peso e altura.");
						step++;
					}
				break;

				case 5:
					if(F.letraAtual >= 24 && cowOpacity < 255)
						cowOpacity += 2.55f; 

					if(F.letraAtual >= 70){
						if(atributoUm < 255) 	atributoUm += 2.55f;
						if(atributoUmY > 0) 	atributoUmY -= 1;
					}

					if(F.letraAtual >= 76){
						if(atributoDois < 255) 	atributoDois += 2.55f;
						if(atributoDoisY > 0) 	atributoDoisY -= 1;
					}

					if(F.letraAtual >= 83){
						if(atributoTres < 255) 	atributoTres += 2.55f;
						if(atributoTresY > 0) 	atributoTresY -= 1;
					}

					if(F.Key("x") && !F.TeclaDesativada("x") && F.letraAtual >= F.txtAlerta.Length && Configuracoes.letraTempo == 50){
						F.Alerta("O quê? Sim, uma vaca é um animal, e não um objeto.../nNão sei se conseguiria explicar isso, talvez alguém de outra cidade consiga.");
						step++;
					}
				break;

				case 6:
					if(F.Key("x") && !F.TeclaDesativada("x") && F.letraAtual >= F.txtAlerta.Length && Configuracoes.letraTempo == 50){
						cowOpacity = 255;
						atributoUm = 255;
						atributoDois = 255;
						atributoTres = 255;
						Efeitos.opacidade = 255;

						step++;
					}
				break;

				case 7:
					cowOpacity 		-= cowOpacity <= 0 ? cowOpacity : 2.55f;
					atributoUm 		-= atributoUm <= 0 ? atributoUm : 2.55f;
					atributoDois 	-= atributoDois <= 0 ? atributoDois : 2.55f;
					atributoTres 	-= atributoTres <= 0 ? atributoTres : 2.55f;

					if(cowOpacity <= 0){
						if(npcY < (npc.y+npc.height-npc.imgHeight)-(V.view.Center.Y - (Screen.height/2)))
							npcY++;
						else if(npcY >= (npc.y+npc.height-npc.imgHeight)-(V.view.Center.Y - (Screen.height/2)) && Efeitos.opacidade > 0){
							Efeitos.opacidade -= 2.55f;
							Efeitos.Backdrop("fade-out-white", 0, 2.55f);
							step++;
						}
					}
				break;

				case 8:
					if(Efeitos.opacidade <= 0){
						V.objetos[V.objetos.IndexOf(npc)].step = 0;
						CurrentScreen.Change("localmap");
					}
				break;
			}
		}

		public static void Video2(){
			V.img[0].Texture 		= V.IMG_CAT[1][6];
			V.img[0].TextureRect	= new IntRect(0, 0, (int)npc.imgWidth, (int)npc.imgHeight);
			V.img[0].Position 		= new Vector2f(npcX, npcY);
			V.img[0].Color 			= new Color(255, 255, 255, 255);
			V.window.Draw(V.img[0]);

			V.img[0].Texture 		= V.IMG_CAT[13][1];
			V.img[0].TextureRect	= new IntRect(0, 0, 100, 88);
			V.img[0].Position 		= new Vector2f(npcX-350, npcY+150);
			V.img[0].Color 			= new Color(255, 255, 255, (byte)cowOpacity);
			V.window.Draw(V.img[0]);

			F.Escrever("-Latir", 	true, npcX-370+80-(F.TxtWidth("-Latir", 30, true)/2), npcY+280+atributoUmY, 	30, 000, 000, 000, (byte)atributoUm);
			F.Escrever("-Dormir", 	true, npcX-370+80-(F.TxtWidth("-Dormir", 30, true)/2), npcY+310+atributoDoisY, 30, 000, 000, 000, (byte)atributoDois);
			F.Escrever("-Comer", 	true, npcX-370+80-(F.TxtWidth("-Comer", 30, true)/2), npcY+340+atributoTresY, 30, 000, 000, 000, (byte)atributoTres);

			float o = cowOpacity > 200 ? 200 : cowOpacity;
			F.DesenharShape(npcX+180, npcY+145, 355, 120, 000, 000, 000, o);
			new RichText(npcX+190, npcY+150, 24, true, (byte)cowOpacity, @"#f92659public static #3da3efvoid #a6e22eLatir#ffffff(){}/n/n#f92659public static #3da3efvoid #a6e22eDormir#ffffff(){}/n/n#f92659public static #3da3efvoid #a6e22eComer#ffffff(){}/n");

			V.img[0].Color = new Color(255, 255, 255, 255);

			switch(step){
				case 0:
					if(Efeitos.opacidade >= 255 && npcY > 50)
						npcY--;
					else if(npcY <= 50 && Efeitos.opacidade >= 50) 
						step++;
				break;

				case 1:
					if(F.txtAlerta == "")
						F.Alerta("Não quero perder meu tempo, então vou explicar resumidamente...");
					if(F.Key("x") && !F.TeclaDesativada("x") && F.letraAtual >= F.txtAlerta.Length && Configuracoes.letraTempo == 50){
						F.Alerta("Dentro da programação, #00ffffmétodos #fffffffuncionam como #ff0000scripts./n/n/n/n/n/n/n/n~#ffffffUm #ff0000script #ffffffé um conjunto de comandos~");
						step++;
					}
				break;

				case 2:
					if(F.Key("x") && !F.TeclaDesativada("x") && F.letraAtual >= F.txtAlerta.Length && Configuracoes.letraTempo == 50){
						F.Alerta("Por exemplo, o #00ffffobjeto #ff0000cachorro #ffffffpossui os #00ffffmétodos #ffffffLatir, Dormir e Comer.");
						step++;
					}
				break;

				case 3:
					if(F.letraAtual >= 24 && cowOpacity < 255)
						cowOpacity += 2.55f; 

					if(F.letraAtual >= 70){
						if(atributoUm < 255) 	atributoUm += 2.55f;
						if(atributoUmY > 0) 	atributoUmY -= 1;
					}

					if(F.letraAtual >= 76){
						if(atributoDois < 255) 	atributoDois += 2.55f;
						if(atributoDoisY > 0) 	atributoDoisY -= 1;
					}

					if(F.letraAtual >= 83){
						if(atributoTres < 255) 	atributoTres += 2.55f;
						if(atributoTresY > 0) 	atributoTresY -= 1;
					}

					if(F.Key("x") && !F.TeclaDesativada("x") && F.letraAtual >= F.txtAlerta.Length && Configuracoes.letraTempo == 50){
						F.Alerta("Bem, esse é o conceito de um #00ffffmétodo#ffffff, agora me deixe trabalhar.");
						step++;
					}
				break;

				case 4:
					if(F.Key("x") && !F.TeclaDesativada("x") && F.letraAtual >= F.txtAlerta.Length && Configuracoes.letraTempo == 50){
						cowOpacity = 255;
						atributoUm = 255;
						atributoDois = 255;
						atributoTres = 255;
						Efeitos.opacidade = 255;

						step++;
					}
				break;

				case 5:
					cowOpacity 		-= cowOpacity <= 0 ? cowOpacity : 2.55f;
					atributoUm 		-= atributoUm <= 0 ? atributoUm : 2.55f;
					atributoDois 	-= atributoDois <= 0 ? atributoDois : 2.55f;
					atributoTres 	-= atributoTres <= 0 ? atributoTres : 2.55f;

					if(cowOpacity <= 0){
						if(npcY < (npc.y+npc.height-npc.imgHeight)-(V.view.Center.Y - (Screen.height/2)))
							npcY++;
						else if(npcY >= (npc.y+npc.height-npc.imgHeight)-(V.view.Center.Y - (Screen.height/2)) && Efeitos.opacidade > 0){
							Efeitos.opacidade -= 2.55f;
							Efeitos.Backdrop("fade-out-white", 0, 2.55f);
							step++;
						}
					}
				break;

				case 6:
					if(Efeitos.opacidade <= 0){
						V.objetos[V.objetos.IndexOf(npc)].step = 0;
						CurrentScreen.Change("localmap");
					}
				break;
			}
		}

		public static void Set(Geral _npc, int _video){
			npc 	= _npc;
			npc_	= _npc;
			video 	= _video;
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