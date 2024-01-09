using Microsoft.AspNetCore.Mvc;
using DomainModel;
using System.Text.Json;
namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class PlayListController : ControllerBase
    {
        private static PlayList PL = new PlayList();

        [HttpGet("Get all songs")]
        //public IEnumerable<Song> GetAllSongs()
        //{
        //    return PL.Songs;
        //}
        public String [] GetAllSongs()
        {
            int length = PL.Songs.Count;
            string[] result = new string[length];
            int i = 0;
            foreach (Song s in PL.Songs)
            {
                result[i] = s.ToString();
                i++;
            }
            return result;
        }
        [HttpGet("Load from JSON")]
        public IActionResult FromJSON(string PlayListName)
        {
            if(PL.FromJSON(PlayListName))
            {
                return Ok();
            }
            return NotFound("JSON-file named " + PlayListName + ".json was not found");
        }
        [HttpGet("Load from XML")]
        public IActionResult FromXML(String PlayListName)
        {
            if(PL.FromXML(PlayListName))
            {
                return Ok();
            }
            return NotFound("XML-file named "+ PlayListName+".xml was not found");
        }
        [HttpGet("Load from SQL")]
        public IActionResult FromSQL(String PlayListName)
        {
            if (PL.FromSQL(PlayListName))
            {
                return Ok();
            }
            return NotFound("Playlist " + PlayListName + " was not found in database");
        }
        [HttpPost("Add song")]
        public IActionResult Add(String Author, String Title)
        {
            Song songToAdd = new Song(Author, Title);

            if (PL.AddSong(songToAdd))
            {
                return Ok();
            }
            return NotFound("This song has already been added to playlist");
        }

        [HttpPost("Save to JSON")]
        public IActionResult ToJSON(string PlayListName)
        {
            PL.ToJSON(PlayListName);
            return Ok();
        }
        [HttpPost("Save to XML")]
        public IActionResult ToXML(String PlayListName)
        {
            PL.ToXML(PlayListName);
            return Ok();
        }
        [HttpPost("Save to SQLite")]
        public IActionResult ToSQL(string PlayListName)
        {
            if(PL.ToSQL(PlayListName))
            {
                return Ok();
            }
            return NotFound("Playlist "+ PlayListName+" already exsists");
        }
        [HttpDelete("Delete song")]
        public IActionResult Delete(String Author, string Title) 
        {
            Song songToDelete = new Song(Author, Title);
            if (PL.DeleteSong(songToDelete))
            {
                return Ok();
            };
            return NotFound("No such song found in playlist");
        }
    }
}