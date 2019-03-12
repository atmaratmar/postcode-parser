
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {

            string street_name = "";
            string door_number = "";
            string City_post_cod = "";
            string City_name = "";
            Console.WriteLine("enter street");
            street_name = Console.ReadLine();
            Console.WriteLine("enter house number");
            door_number = Console.ReadLine();
            Console.WriteLine("enter enter post code");
            City_post_cod = Console.ReadLine();
            Console.WriteLine("enter city");
            City_name = Console.ReadLine();

            // The url or service we wish to call
            string url = "https://dawa.aws.dk/autocomplete?caretpos=28&fuzzy=&q=" + street_name + "+" + door_number + ",+" + City_post_cod + "+" + City_name + "&startfra=adresse&type=adresse";
            // The result from the call
            string text = "";
            // Create a request to the URL
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            // Get the text from the URL
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = null;

                if (response.CharacterSet == null)
                {
                    readStream = new StreamReader(receiveStream);
                }
                else
                {
                    readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                }
                // Read all the data from the URL by a stream
                text = readStream.ReadToEnd();
                // Close the response and the stream
                response.Close();
                readStream.Close();
            }
            // Parse and make a list of postcodes

            var postcodeList = JArray.Parse(text).
            Select(p =>
            new
            {
                street = p["data"]["vejnavn"],
                house_number = p["data"]["husnr"],
                floor = p["data"]["etage"],
                side_tv_th_mf_ect = p["data"]["dør"],
                post_code = p["data"]["postnr"],
                city = p["data"]["postnrnavn"],

            }

            );
            // Make an string to hold the SQL 
            //string str = "";
            //// Make the SQL from the list of data we have
            //foreach (var post in postcodeList)
            //{
            //    str += "INSERT INTO Postnumbers (Postnr, City)" + System.Environment.NewLine +
            //    "VALUES ('" + post.address + "" + post.addresnumber + "');" + System.Environment.NewLine;
            //}


            foreach (var post in postcodeList)
            {
                if (street_name.ToLower() == post.street.ToString().ToLower() && (door_number.ToLower() == post.house_number.ToString().ToLower()
                    && (City_post_cod.ToLower() == post.post_code.ToString().ToLower() && (City_name.ToLower() == post.city.ToString().ToLower()))))
                {
                    Console.WriteLine("--------------------------------------" );
                    Console.WriteLine(" .   .   .   "+post.street);
                    Console.Write(" .   .   .   " + post.house_number); Console.Write(" "+post.floor); Console.WriteLine(" "+post.side_tv_th_mf_ect);
                    Console.WriteLine(" .   .   .   " + post.post_code);
                    Console.WriteLine(" .   .   .   " + post.city);
                    Console.WriteLine("_______________________________________" );


                }
                else if ((door_number.ToLower() != post.house_number.ToString())) { }
                else
                {
                    Console.WriteLine("not found this street name " + street_name);
                    break;

                }



            }

            //str.Dump();
            //Console.WriteLine(str);
            Console.ReadKey();
        }
    }
}

