/*
 * Created by SharpDevelop.
 * User: prog2
 * Date: 20.10.2016
 * Time: 9:22
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

namespace TestProject
{
	class Program
	{
		public static void Main(string[] args)
		{
			if (args.Length == 0) {
				Console.WriteLine("Пожалуйста введите параметры compress - для упаковки [имя источника] [имя исходника]" +
				"\ndecompress - для распаковки [имя источника] [имя исходника]\n" +
				"-tf [Имя файла] [размер файла в Гб] - создать тестовый файл в гб");
				Console.ReadKey(true);
				return;
			}
			switch (args[0]) {
				case "compress":
					{
						Gzip.Compress(args[1], args[2]);
						break;}
				case "decompress":
					{
						Gzip.Decompress(args[1], args[2]);
						break;}
				case "-tf":
					{
						Gzip.TestFile(args[1], args[2]);
						break;}
				default:
					Console.WriteLine("Пожалуйста введите параметры compress - для упаковки [имя источника] [имя исходника]\ndecompress - для распаковки [имя источника] [имя исходника]\n"+
					"-tf [Имя файла] [размер файла в Гб] - создать тестовый файл в гб");		
					break;
			}
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
	}
}