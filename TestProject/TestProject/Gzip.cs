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
		private static int countThread=Environment.ProcessorCount*2 ;
		private static byte[][] dataArray=new byte[countThread][];
		private static byte[][] TempDataArray=new byte[countThread][];
		private static int buffersize=1024*1024;
			
		
		public Gzip()
		{
		}
		public static void Compress(string inFile,string pathOutFile="")
		{
			if(pathOutFile=="")
			{
				pathOutFile="1.gz";
			}
			byte[] buffer=new byte[buffersize];
			int _buffersize=10;
			Console.WriteLine("Сжатие началось");
			using (FileStream streaminFile = new FileStream(inFile, FileMode.Open)) {
				using (FileStream outFile = new FileStream(pathOutFile, FileMode.Create)) {
					while (streaminFile.Position < streaminFile.Length) {
						Thread[] arrayThreads = new Thread[countThread];
						for (int i = 0; (i < countThread) && (streaminFile.Position < streaminFile.Length); i++) {
							
							if (streaminFile.Length - streaminFile.Position <= buffersize) 
							{
								_buffersize = (int)(streaminFile.Length - streaminFile.Position);
							} else {
								_buffersize = buffersize;
							}
							
							Console.CursorLeft=0;
							Console.Write("{0:0.00} %",100*streaminFile.Position/streaminFile.Length);
							
							dataArray[i] = new byte[_buffersize];
							streaminFile.Read(dataArray[i], 0, _buffersize);
 
							arrayThreads[i] = new Thread(CompressBlock);
							arrayThreads[i].Start(i);
						}
	
						for (int i = 0; (i < countThread) && (arrayThreads[i] != null);) 
						{
							if (arrayThreads[i].ThreadState==ThreadState.Stopped) {
								BitConverter.GetBytes(TempDataArray[i].Length+1).CopyTo(TempDataArray[i],4);
								outFile.Write(TempDataArray[i],0,TempDataArray[i].Length);
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
					/*while ((nRead = streaminFile.Read(buffer, 0, buffer.Length)) > 0) {
						compressionstream.Write(buffer, buffersize * i++, nRead);
					}*/
					compressionstream.Write(dataArray[element], 0, dataArray[element].Length);
						
				}
				TempDataArray[element] = ms.ToArray();
			}
		}
	}
}
