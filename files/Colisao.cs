using System;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Main{
	public partial class F{
		public static float catX;
		public static float catY;
		public static float sumHalfHeight;
		public static float sumHalfWidth;
		public static float overlapX;
		public static float overlapY;
		
		public static void ColideBloco(Geral x1, Geral x2, bool ajustarZindex){
			catX  = (x1.x + (x1.width / 2)) - (x2.x + (x2.width / 2));
			catY = (x1.y + (x1.height / 2)) - (x2.y + (x2.height / 2));
									
			sumHalfWidth = (x1.width / 2) + (x2.width / 2);
			sumHalfHeight = (x1.height / 2) + (x2.height / 2);

			if(Math.Abs(catX ) <= sumHalfWidth && Math.Abs(catY) <= sumHalfHeight && !x1.pausado){
				overlapX = sumHalfWidth - Math.Abs(catX );
				overlapY = sumHalfHeight - Math.Abs(catY);
				x2.Interagir(0);

				if(overlapX >= overlapY){
					if(catY > 0){		
						x1.y = x2.y + x2.height;
						if(x == Configuracoes.controle && x1.direcao == 1 && F.Key("x") && !F.TeclaDesativada("x")){
							x2.Interagir(1);
							F.DesativarTecla("x", 500);
						}
					} 
					else{
						x1.y = x2.y - x1.height;
						if(x == Configuracoes.controle && x1.direcao == 0 && F.Key("x") && !F.TeclaDesativada("x")){
							x2.Interagir(1);
							F.DesativarTecla("x", 500);
						}
					}
				} 
				else{
					if(catX  > 0 && x1.x + (x1.width / 2) >= x2.x){
						x1.x = x2.x + x2.width;
						if(x == Configuracoes.controle && x1.direcao == 3 && F.Key("x") && !F.TeclaDesativada("x")){
							x2.Interagir(1);
							F.DesativarTecla("x", 500);
						}
					} 
					else{
						x1.x = x2.x - x1.width;
						if(x == Configuracoes.controle && x1.direcao == 2 && F.Key("x") && !F.TeclaDesativada("x")){
							x2.Interagir(1);
							F.DesativarTecla("x", 500);
						}
					}
				}	
			}

			if(ajustarZindex){
				catX  	= ((x1.x+x1.adcX) + (x1.imgWidth/2)) - ((x2.x+x2.adcX-140) + ((280+x2.imgWidth)/2));
				catY 	= ((x1.y+x1.height-x1.imgHeight) + (x1.imgHeight/2)) - ((x2.y+x2.height) + 70);
										
				sumHalfWidth 	= (x1.imgWidth/2) 	+  ((280+x2.imgWidth)/2);
				sumHalfHeight 	= (x1.imgHeight/2) 	+  70;

				if(Math.Abs(catX ) < sumHalfWidth && Math.Abs(catY) < sumHalfHeight){
					x1.zindex = x2.zindex + 1;
					x1.qtdObj++;
				}
			}
		}
		
		public static void ChaoLimite(Chao x1, Chao x2){		
			catX  = (x1.x + (x1.width / 2)) - (x2.x + (x2.width / 2));
			catY = (x1.y + (x1.height / 2)) - (x2.y + (x2.height / 2));
									
			sumHalfWidth = (x1.width / 2) + (x2.width / 2);
			sumHalfHeight = (x1.height / 2) + (x2.height / 2);
									
			if(Math.Abs(catX ) <= sumHalfWidth && Math.Abs(catY) <= sumHalfHeight){
				overlapX = sumHalfWidth - Math.Abs(catX );
				overlapY = sumHalfHeight - Math.Abs(catY);
										
				if(overlapX >= overlapY){
					if(catY > 0){
						x1.lCima 	= true;
						x2.lBaixo 	= true;
					}
					else{
						x1.lBaixo 	= true;
						x2.lCima 	= true;
					}
				} 
				else{
					if(catX  > 0){
						x1.lEsquerda 	= true;
						x2.lDireita 	= true;
					}
					else{
						x1.lDireita 	= true;
						x2.lEsquerda 	= true;
					}
				}
			}
		}
		
		public static void AtualizarZindex(Geral x1, Geral x2){
			catX  	= ((x1.x+x1.adcX) + (x1.imgWidth/2)) - ((x2.x+x2.adcX-140) + ((280+x2.imgWidth)/2));
			catY 	= ((x1.y+x1.height-x1.imgHeight) + (x1.imgHeight/2)) - ((x2.y+x2.height) + 70);
									
			sumHalfWidth 	= (x1.imgWidth/2) 	+  ((280+x2.imgWidth)/2);
			sumHalfHeight 	= (x1.imgHeight/2) 	+  70;
									
			if(Math.Abs(catX ) < sumHalfWidth && Math.Abs(catY) < sumHalfHeight)
				x1.zindex = x2.zindex + 2;
		}
	}
}