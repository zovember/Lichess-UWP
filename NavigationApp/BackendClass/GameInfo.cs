using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace NavigationApp
{
    class GameInfo
    {
        public string fullId { get; set; }
        public string gameId { get; set; }
        //public string fen { get; set; }
        public string color { get; set; }
        public string lastMove { get; set; }
        public string source { get; set; }
        public int statusId { get; set; }
        public string statusName { get; set; }
        public string speed { get; set; }
        public string perf { get; set; }
        public bool rated { get; set; }
        public bool hasMoved { get; set; }
        public string opponentId { get; set; }
        public string opponentUsername { get; set; }
        public int opponentRating { get; set; }
        public bool isMyTurn { get; set; }
        public int secondsLeft { get; set; }
        public string id { get; set; }

        public GameInfo(JObject jsonObj)
        {
            if (jsonObj.ToString() != "")
            {
                fullId = (string)jsonObj["game"]["fullId"];
                gameId = (string)jsonObj["game"]["gameId"];
                //fen = (string)jsonObj["game"]["fen"];
                color = (string)jsonObj["game"]["color"];
                lastMove = (string)jsonObj["game"]["lastMove"];
                source = (string)jsonObj["game"]["source"];
                statusId = (int)jsonObj["game"]["status"]["id"];
                statusName = (string)jsonObj["game"]["status"]["name"];
                speed = (string)jsonObj["game"]["speed"];
                perf = (string)jsonObj["game"]["perf"];
                rated = (bool)jsonObj["game"]["rated"];
                hasMoved = (bool)jsonObj["game"]["hasMoved"];
                opponentId = (string)jsonObj["game"]["opponent"]["id"];
                opponentUsername = (string)jsonObj["game"]["opponent"]["username"];
                //opponentRating = (int)jsonObj["game"]["opponent"]["rating"];
                isMyTurn = (bool)jsonObj["game"]["isMyTurn"];
                //secondsLeft = (int)jsonObj["game"]["secondsLeft"];
                id = (string)jsonObj["game"]["id"];
            }
        }

        public GameInfo() { }
    }
}
