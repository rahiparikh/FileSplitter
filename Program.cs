using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

/*
 * 
 *  Developer   : Rahil Parikh ( rahil@rahilparikh.me )
 *  Date        : August 18, 2012
 *  
 *  Copyright (c) 2012, Rahil Parikh
 *
 *  As long as you retain this notice and credit author
 *  for his work you can do whatever you want with this
 *  stuff. 
 *  
 *  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 *  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 *  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 *  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 *  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 *  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 *  THE SOFTWARE.
 *  
 *  ToDo -
 *  1. Error Checking
 *  2. CommandLine Handling
 *  3. Support for XML
 * 
 */

namespace FileSplitter
{
    class Program
    {
        static void Main(string[] args)
        {

            double totalLines;
            int perFileLines;
            int pageSize;
            StreamReader streamReader;
            StreamWriter streamWriter;
            string tempFile;
            string inFile;
            string outFile;

            bool delIfExists;

            string line;
            double overAllProgress;
            int perFileProgress;

            //Cursor position in source file
            totalLines = 0;

            //Cursor position in current file
            perFileLines = 0;

            //Max number of lines in a splitted file
            pageSize = 20000;
            tempFile = Environment.GetEnvironmentVariable("temp") + @"\" + System.Guid.NewGuid().ToString();

            //The path to the source file
            inFile = @"<PATH_TO_FILE>";

            //Delete the file if exists; deletes on per file bases
            delIfExists = true;

            streamReader = new StreamReader(inFile);
            streamWriter = new StreamWriter(tempFile);
            Console.Write("Processing.........");

            //Read a line from source file
            while ((line = streamReader.ReadLine()) != null)
            {
                //Calculate overall progress
                overAllProgress = (totalLines / pageSize);

                //We need a new file
                if (overAllProgress % 1 == 0)
                {
                    perFileLines = 0;
                    streamWriter.Close();
                    outFile = Path.GetDirectoryName(inFile) + @"\" + Path.GetFileNameWithoutExtension(inFile) + "_" + (int)overAllProgress + Path.GetExtension(inFile);
                    if (delIfExists && File.Exists(outFile))
                    {
                        File.Delete(outFile);
                    }
                    streamWriter = new StreamWriter(outFile);
                    Console.WriteLine();
                    Console.Write("{0}\t\t\t", Path.GetFileName(outFile));
                }

                streamWriter.WriteLine(line);
                totalLines++;
                perFileLines++;
                perFileProgress = perFileLines * 100 / pageSize;

                if (perFileProgress % 1 == 0)
                {
                    Console.Write("\b\b\b\b\b\b\b\b\b");
                    Console.Write("\t{0}%", (int)perFileProgress);
                }

            }
            Console.Write("\b\b\b\b\b\b\b\b\b");
            Console.WriteLine("\t100%");
            File.Delete(tempFile);
            streamReader.Close();

            Console.WriteLine();
            Console.WriteLine("Total {0} lines processed", totalLines.ToString("N"));
            Console.ReadLine();
        }
    }
}
