using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TSound.Web.Models.ViewModels.Track
{
    public class TrackLightViewModel
    {
        public Guid Id { get; set; }

        public string SpotifyId { get; set; }

        public string Name { get; set; }

        public string Artist { get; set; }

        public string PreviewUrl { get; set; }

        public string Uri { get; set; }

        public int DurationMs { get; set; }

        public string DurationInMinutes
        {
            get
            {
                int minutes = (int)Math.Floor((double)this.DurationMs / 60);
                int seconds = this.DurationMs % 60;

                string secondsString = seconds.ToString().PadLeft(2, '0');

                return $"{minutes}:{secondsString}";
            }
        }
    }
}
