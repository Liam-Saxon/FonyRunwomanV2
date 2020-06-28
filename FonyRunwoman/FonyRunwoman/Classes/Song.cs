using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FonyRunwoman.Classes
{
    class Song
    {
        private string songName;
        private string songURL;
        private string songArtist;
        private string songAlbum;

        //public Song(string Name, int Number, string Title, string Artist, string Album)
        //{
        //    Name = songName;
        //    Number = songNumber;
        //    Title = songTitle;
        //    Artist = songArtist;
        //    Album = songAlbum;
        //}

        public string MysongURL { get; set; }
        public string MysongName { get; set; }
        public string MysongArtist { get; set; }
        public string MyAlbum { get; set; }

    }
}
