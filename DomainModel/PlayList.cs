using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Xml.Serialization;

namespace DomainModel
{
    public class PlayList
    {
        public List<Song> playlist;
        public PlayList()
        {
            playlist = new List<Song>();
        }
        public bool AddSong(Song songToAdd)
        {
            if (SongInPlaylist(songToAdd))
            {
                return false;
            }

            playlist.Add(songToAdd);
            return true;
        }

        private bool SongInPlaylist(Song song)
        {
            foreach (var s in playlist)
            {
                if (song.Equals(s))
                {
                    return true;
                }
            }

            return false;
        }

        public bool DeleteSong(Song songsToDelete)
        {
            foreach (var song in playlist)
            {
                if (songsToDelete.Equals(song))
                {
                    playlist.Remove(song);
                    return true;
                }
            }

            return false;
        }
        public IEnumerable<Song> Search(string[] request)
        {
            List<Song> result = new List<Song>();

            foreach (var song in playlist)
            {
                for (int i = 0; i < request.Length; i++)
                {
                    if (song.Author.Contains(request[i], StringComparison.InvariantCultureIgnoreCase)
                        || song.Title.Contains(request[i], StringComparison.InvariantCultureIgnoreCase))
                    {
                        result.Add(song);
                        break;
                    }
                }
            }

            return result;
        }
        public List<Song> Songs
        {
            get { return playlist; }
            set { playlist = value; }
        }

        public void ToJSON(String PlayListName)
        {
            var jsonPlaylist = JsonSerializer.Serialize<PlayList>(this);
            jsonPlaylist = JsonSerializer.Serialize<PlayList>(this);
            File.WriteAllText(PlayListName+".json", jsonPlaylist);
        }
        public bool FromJSON(String PlayListName)
        {
            if(File.Exists(PlayListName+".json"))
            {
                this.playlist = JsonSerializer.Deserialize<PlayList>(File.ReadAllText(PlayListName + ".json")).Songs;
                return true;
            }
            return false;
            
        }

        public void ToXML(String PlayListName)
        {
            XmlSerializer xmlPLaylistSerializer = new XmlSerializer(typeof(PlayList));
            using (FileStream create = new FileStream(PlayListName+".xml", FileMode.OpenOrCreate))
            {
                xmlPLaylistSerializer.Serialize(create, this);
            }
        }
        public bool FromXML(String PlayListName)
        {
            XmlSerializer xmlPLaylistSerializer = new XmlSerializer(typeof(PlayList));
            if (File.Exists(PlayListName+".xml"))
            {
                using (FileStream read = new FileStream(PlayListName + ".xml", FileMode.Open))
                {
                    PlayList tmp = (PlayList)xmlPLaylistSerializer.Deserialize(read);
                    this.playlist = tmp.Songs;
                    return true;
                }
            }
            return false;
        }
        public bool ToSQL(String name)
        {
            using (var db = new BloggingContext())
            {
                if (db.Song.Where(b => b.PlayListName == name).Count() != 0)
                {
                    return false;
                }
                db.AddRange(this.playlist.Select(c => new Song()
                {
                    Author = c.Author,
                    Title = c.Title,
                    PlayListName = name
                }));
                db.SaveChanges();
                return true;
            }
        }
        public bool DeleteFromSQL(String name)
        {
            using (var db = new BloggingContext())
            {
                if (db.Song.Where(b => b.PlayListName == name).Count() == 0)
                {
                    return false;
                }
                IQueryable<Song> query = (db.Song.Where(b => b.PlayListName == name));
                db.RemoveRange(query);
                db.SaveChanges();
                return true;
            }
        }
        public bool FromSQL(string name)
        {
            using (var db = new BloggingContext())
            {
                IQueryable<Song> query = (db.Song.Where(b => b.PlayListName == name));
                if (query.Count() == 0)
                {
                    return false;
                }
                this.playlist = query.ToList();
                return true;
            }
        }
    }
}