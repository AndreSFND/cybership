using System;
using System.Linq;
using System.Collections.Generic;
using SFML;
using SFML.Window;
using SFML.Graphics;
using SFML.System;

namespace Main{
	public class RichText{

		public RichText(float x, float y, uint fontsize, bool fontType, string txt){
			for(int s=0;s<=txt.Length-1;s++){
				substring 	= txt.Substring(s, 1);
				charEfeito 	= true;

				switch(substring){
					case "*":
						if(efeitos.Any(n => n == 1))
							efeitos.Remove(1);
						else
							efeitos.Add(1);
					break;

					case "~":
						if(efeitos.Any(n => n == 2))
							efeitos.Remove(2);
						else
							efeitos.Add(2);
					break;

					case "_":
						if(efeitos.Any(n => n == 4))
							efeitos.Remove(4);
						else
							efeitos.Add(4);
					break;

					case "#":
						if(txt.Length > s+7){
							System.Drawing.Color c 	= System.Drawing.ColorTranslator.FromHtml(txt.Substring(s, 7));
							cor 					= new Color(c.R, c.G, c.B, c.A);

							effectCharCount = 7;
						}
						else
							effectCharCount = txt.Length-s;
					break;

					case "/":
						if(txt.Length > s+1 && txt.Substring(s, 2) == @"/n"){
							y += (int)F.TxtHeight(substring, fontsize, fontType)+5;
							effectCharCount = 2;
							newtext 		= "";
							chars 			= s;
							firstchar 		= true;
						}
					break;

					default:
						charEfeito = false;
					break;
				}

				if(effectCharCount > 0){
					charEfeito = true;
					effectCharCount--;
				}

				if(charEfeito)
					chars++;
				else{
					cspace 		= F.TxtWidth(newtext.Substring(0, s-chars), fontsize, fontType);
					Font font 	= fontType ? V.eBitBold : V.eBit;

					if(firstchar){
						float v1 	= F.TxtWidth(newtext.Substring(0, s-chars), fontsize, fontType);
						float v2 	= F.TxtWidth(newtext.Substring(0, s-chars)+newtext.Substring(0, s-chars), fontsize, fontType);

						cspace 		+= v2 - 2*v1;
					}

					Text texto 				= new Text(substring, font, fontsize);
		            texto.Position 			= new Vector2f((int)x+cspace, (int)y);
		            texto.FillColor 		= cor;

		            SFML.Graphics.Text.Styles q = (SFML.Graphics.Text.Styles)0;
		            foreach(int i in efeitos) q ^= (SFML.Graphics.Text.Styles)i;
		            texto.Style = q;
					
					V.window.Draw(texto);

					newtext += substring;
				}
			}
		}

		public RichText(float x, float y, uint fontsize, bool fontType, byte opacity, string txt){
			for(int s=0;s<=txt.Length-1;s++){
				substring 	= txt.Substring(s, 1);
				charEfeito 	= true;

				switch(substring){
					case "*":
						if(efeitos.Any(n => n == 1))
							efeitos.Remove(1);
						else
							efeitos.Add(1);
					break;

					case "~":
						if(efeitos.Any(n => n == 2))
							efeitos.Remove(2);
						else
							efeitos.Add(2);
					break;

					case "_":
						if(efeitos.Any(n => n == 4))
							efeitos.Remove(4);
						else
							efeitos.Add(4);
					break;

					case "#":
						if(txt.Length > s+7){
							System.Drawing.Color c 	= System.Drawing.ColorTranslator.FromHtml(txt.Substring(s, 7));
							cor 					= new Color(c.R, c.G, c.B, opacity);

							effectCharCount = 7;
						}
						else
							effectCharCount = txt.Length-s;
					break;

					case "/":
						if(txt.Length > s+1 && txt.Substring(s, 2) == @"/n"){
							y += (int)F.TxtHeight(substring, fontsize, fontType)+5;
							effectCharCount = 2;
							newtext 		= "";
							chars 			= s;
							firstchar 		= true;
						}
					break;

					default:
						charEfeito = false;
					break;
				}

				if(effectCharCount > 0){
					charEfeito = true;
					effectCharCount--;
				}

				if(charEfeito)
					chars++;
				else{
					cspace 		= F.TxtWidth(newtext.Substring(0, s-chars), fontsize, fontType);
					Font font 	= fontType ? V.eBitBold : V.eBit;

					if(firstchar){
						float v1 	= F.TxtWidth(newtext.Substring(0, s-chars), fontsize, fontType);
						float v2 	= F.TxtWidth(newtext.Substring(0, s-chars)+newtext.Substring(0, s-chars), fontsize, fontType);

						cspace 		+= v2 - 2*v1;
					}

					Text texto 				= new Text(substring, font, fontsize);
		            texto.Position 			= new Vector2f((int)x+cspace, (int)y);
		            texto.FillColor 		= cor;

		            SFML.Graphics.Text.Styles q = (SFML.Graphics.Text.Styles)0;
		            foreach(int i in efeitos) q ^= (SFML.Graphics.Text.Styles)i;
		            texto.Style = q;
					
					V.window.Draw(texto);

					newtext += substring;
				}
			}
		}

		string 	newtext		= "";
		string 	substring;

		int 	chars 		= 0;
		float 	cspace 		= 0;
		int 	effectCharCount = 0;
		bool 	charEfeito 	= false;
		bool 	firstchar  	= true;
		Color 	cor 		= new Color(255, 255, 255, 255);

		List<int> efeitos = new List<int>();
	}
}