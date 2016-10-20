/*
 * Created by SharpDevelop.
 * User: prog2
 * Date: 20.10.2016
 * Time: 9:34
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.IO.Compression;
using System.Threading;

namespace TestProject
{
	/// <summary>
	/// Description of Gzip.
	/// </summary>
	public class Gzip
	{
		private static int countThread = Environment.ProcessorCount * 2;
		private static byte[][] dataArray = new byte[countThread][];
		private static byte[][] TempDataArray = new byte[countThread][];
		private static int buffersize = 1024 * 1024;
			
		
		public Gzip()
		{
		}
		public static void Compress(string inFile, string pathOutFile = "")
		{
			if (pathOutFile == "") {
				pathOutFile = "1.gz";
			}
			byte[] buffer = new byte[buffersize];
			int _buffersize = 10;
			Console.WriteLine("Сжатие началось");
			using (FileStream streaminFile = new FileStream(inFile, FileMode.Open)) {
				using (FileStream outFile = new FileStream(pathOutFile, FileMode.Create)) {
					while (streaminFile.Position < streaminFile.Length) {
						Thread[] arrayThreads = new Thread[countThread];
						for (int i = 0; (i < countThread) && (streaminFile.Position < streaminFile.Length); i++) {
							
							if (streaminFile.Length - streaminFile.Position <= buffersize) {
								_buffersize = (int)(streaminFile.Length - streaminFile.Position);
							} else {
								_buffersize = buffersize;
							}
							
							Console.CursorLeft = 0;
							Console.Write("{0:0.00} %", 100 * streaminFile.Position / streaminFile.Length);
							
							dataArray[i] = new byte[_buffersize];
							streaminFile.Read(dataArray[i], 0, _buffersize);
 
							arrayThreads[i] = new Thread(CompressBlock);
							arrayThreads[i].Start(i);
						}
	
						for (int i = 0; (i < countThread) && (arrayThreads[i] != null);) {
							if (arrayThreads[i].ThreadState == ThreadState.Stopped) {
								BitConverter.GetBytes(TempDataArray[i].Length + 1).CopyTo(TempDataArray[i], 4);
								outFile.Write(TempDataArray[i], 0, TempDataArray[i].Length);
								i++;
							}
	
						}						
					}
					outFile.Close();
				}
				streaminFile.Close();
			}
		}
		
		private static void CompressBlock(object ii)
		{
			int element = (int)ii;
			using (var ms = new MemoryStream(dataArray[element].Length)) {
				using (var compressionstream = new GZipStream(ms, CompressionMode.Compress)) {
					compressionstream.Write(dataArray[element], 0, dataArray[element].Length);
						
				}
				TempDataArray[element] = ms.ToArray();
			}
		}
	
		public static void Decompress(string PathInFile, string PathoutFile)
		{
			PathoutFile = PathoutFile == "" ? PathInFile.Remove(PathInFile.Length - 3) : PathoutFile;
			using (FileStream inFile = new FileStream(PathInFile, FileMode.Open)) {
				using (FileStream outFile = new FileStream(PathoutFile, FileMode.Create)) {
               
					int _dataPortionSize;
					int compressedBlockLength;
					Thread[] arrayThreads;
					Console.WriteLine("Decompressing...");
					byte[] buff = new byte[8];
                
					while (inFile.Position < inFile.Length) {
						arrayThreads = new Thread[countThread];
				 
						for (int i = 0; (i < countThread) && (inFile.Position < inFile.Length); i++) {
							inFile.Read(buff, 0, 8);
							compressedBlockLength = BitConverter.ToInt32(buff, 4);
							TempDataArray[i] = new byte[compressedBlockLength];
					
							buff.CopyTo(TempDataArray[i], 0);
							inFile.Read(TempDataArray[i], 8, compressedBlockLength - 9);
					
							_dataPortionSize = BitConverter.ToInt32(TempDataArray[i], compressedBlockLength - 5);
							dataArray[i] = new byte[_dataPortionSize];
					
							Console.CursorLeft = 0;
							Console.Write("{0:0.00} %", 100 * inFile.Position / inFile.Length);
					
							arrayThreads[i] = new Thread(DecompressBlock);
							arrayThreads[i].Start(i);
						}
						for (int i = 0; (i < countThread) && arrayThreads[i] != null;) {
							if (arrayThreads[i].ThreadState == ThreadState.Stopped) {
						
								outFile.Write(dataArray[i], 0, dataArray[i].Length);
								i++;
							}
						}
				 
					}
					outFile.Close();
				}
				inFile.Close();
			}
			
		}
		private static void DecompressBlock(object i)
		{
			using (MemoryStream input = new MemoryStream(TempDataArray[(int)i])) {
 
				using (GZipStream ds = new GZipStream(input, CompressionMode.Decompress)) {
					ds.Read(dataArray[(int)i], 0, dataArray[(int)i].Length);
				}
 
			}
		}
	
	
		public static void TestFile(string filename, string arg)
		{
			int gb = Int32.Parse(arg);
			using (FileStream stream = new FileStream(filename, FileMode.Create)) {
				byte[] array = System.Text.Encoding.UTF8.GetBytes("Любя, съешь щипцы, - вздохнёт мэр, - кайф жгуч.\n" +
				               "Шеф взъярён так щипцы с эхом гудбай Жюль.\n" +
				               "Эй, жлоб! Где туз? Прячь юных съёмщиц в шкаф." +
				               "\nЭкс-граф? Плюш изъят. Бьём чуждый цен хвощ!				" +
				               "\nЭх, чужак! Общий съём цен шляп (юфть) — вдрызг!				" +
				               "\nЭх, чужд кайф, сплющь объём вши, грызя цент.				" +
				               "\nЧушь: гид вёз кэб цапф, юный жмот съел хрящ.");
				for (int i = 0; i < 1910573 * gb; i++) {
					stream.Write(array, 0, array.Length);
					Console.CursorLeft = 0;
					if (i % 100 == 0) {
						Console.Write("{0:0.00} %", 100 * i / (1910573 * gb));
					}
				}
				
				stream.Close();
			}
			
		}
	}
}
