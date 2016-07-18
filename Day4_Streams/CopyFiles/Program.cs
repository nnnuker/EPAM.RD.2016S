using System;
using System.IO;
using System.Net;
using System.Text;

namespace FileStreams
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //if (args.Length < 2)
            //{
            //    Console.WriteLine("Arguments: <source> <destination>");
            //    return;
            //}

            //string source = args[0];
            //string destin = args[1];

            string source = @"D:\Valery_Larkovich\Projects\EPAM.RD.2016S.Larkovich\Day4_Streams\CopyFiles\TextFile.txt";
            string destin = @"D:\Valery_Larkovich\Projects\EPAM.RD.2016S.Larkovich\Day4_Streams\CopyFiles\TextFileDestin.txt";

            //ByteCopy(source, destin);
            //BlockCopy(source, destin);
            //LineCopy(source, destin);
            MemoryBufferCopy(source, destin);
            //WebClient();
        }

        public static void ByteCopy(string source, string destin)
        {
            int bytesCounter = 0;

            // TODO: Implement byte-copy here.

            using (var sourceStream = new FileStream(source, FileMode.Open))
            using (var destinStream = new FileStream(destin, FileMode.Create))
            {
                int b;
                while ((b = sourceStream.ReadByte()) != -1) // TODO: read byte
                {
                    bytesCounter++;
                    destinStream.WriteByte((byte)b); // TODO: write byte
                }
            }

            Console.WriteLine("ByteCopy() done. Total bytes: {0}", bytesCounter);
        }

        public static void BlockCopy(string source, string destin)
        {
            // TODO: Implement block copy via buffer.

            using (var sourceStream = new FileStream(source, FileMode.Open))
            using (var destinStream = new FileStream(destin, FileMode.Create))
            {
                byte[] buffer = new byte[1024];
                int bytesRead = 0;

                do
                {
                    bytesRead = sourceStream.Read(buffer, 0, buffer.Length); // TODO: read in buffer

                    Console.WriteLine("BlockCopy(): writing {0} bytes.", bytesRead);
                    destinStream.Write(buffer, 0, buffer.Length); // TODO: write to buffer
                }
                while (bytesRead == buffer.Length);
            }

        }

        public static void LineCopy(string source, string destin)
        {
            int linesCount = 0;

            // TODO: implement copying lines using StreamReader/StreamWriter.

            using (var sourceStream = new FileStream(source, FileMode.Open))
            using (var destinStream = new FileStream(destin, FileMode.Create))
            {
                using (var streamReader = new StreamReader(sourceStream))
                using (var streamWriter = new StreamWriter(destinStream))
                {

                    string line;
                    while (true)
                    {
                        linesCount++;
                        if ((line = streamReader.ReadLine()) == null) // TODO: read line
                        {
                            break;
                        }
                        streamWriter.WriteLine(line); // TODO: write line

                    }
                }
            }


            Console.WriteLine("LineCopy(): {0} lines.", linesCount);
        }

        public static void MemoryBufferCopy(string source, string destin)
        {
            var stringBuilder = new StringBuilder();

            string content;

            using (var textReader = (TextReader)new StreamReader(source)) // TODO: use StreamReader here
            {
                content = textReader.ReadToEnd(); // TODO: read entire file
            }

            using (var sourceReader = new StringReader(content)) // TODO: Use StringReader here with content
            using (var sourceWriter = new StringWriter(stringBuilder)) // TODO: Use StringWriter here with stringBuilder
            {
                string line = null;

                do
                {
                    line = sourceReader.ReadLine(); // TODO: read line
                    if (line != null)
                    {
                        sourceWriter.WriteLine(line); // TODO: write line
                    }

                } while (line != null);
            }

            Console.WriteLine("MemoryBufferCopy(): chars in StringBuilder {0}", stringBuilder.Length);

            const int blockSize = 1024;

            using (var stringReader = new StringReader(stringBuilder.ToString())) // TODO: Use StringReader to read from stringBuilder.
            using (var memoryStream = new MemoryStream(stringBuilder.Length))
            using (var streamWriter = new StreamWriter(memoryStream)) // TODO: Compose StreamWriter with memory stream.
            using (var destinStream = new FileStream(destin, FileMode.Create)) // TODO: Use file stream.
            {
                char[] buffer = new char[blockSize];
                int bytesRead;

                do
                {
                    bytesRead = stringReader.ReadBlock(buffer, 0, blockSize); // TODO: Read block from stringReader to buffer.
                    streamWriter.Write(buffer, 0, buffer.Length); // TODO: Write buffer to streamWriter.

                    //TODO: After implementing everythin check the content of NewTextFile. What's wrong with it, and how this may be fixed?

                }
                while (bytesRead == blockSize);

                var buf = memoryStream.GetBuffer();
                var arr = memoryStream.ToArray();
                destinStream.Write(buf, 0, buf.Length); // TODO: write memoryStream.GetBuffer() content to destination stream.
            }
        }

        public static void WebClient()
        {
            WebClient webClient = new WebClient();
            using (var stream = webClient.OpenRead("http://google.com"))
            {

                Console.WriteLine("WebClient(): CanRead = {0}", stream.CanRead); // TODO: print if it is possible to read from the stream
                Console.WriteLine("WebClient(): CanWrite = {0}", stream.CanWrite); // TODO: print if it is possible to write to the stream
                Console.WriteLine("WebClient(): CanSeek = {0}", stream.CanSeek); // TODO: print if it is possible to seek through the stream

                string destin = @"D:\Valery_Larkovich\Projects\EPAM.RD.2016S.Larkovich\Day4_Streams\CopyFiles\google_request.txt";

                using (var destinStream = new FileStream(destin, FileMode.Create))
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = 0;

                    do
                    {
                        bytesRead = stream.Read(buffer, 0, buffer.Length); // TODO: read in buffer

                        destinStream.Write(buffer, 0, buffer.Length); // TODO: write to buffer
                    }
                    while (bytesRead == buffer.Length);
                }
                // TODO: Save stream content to "google_request.txt" file.
            }
        }
    }
}