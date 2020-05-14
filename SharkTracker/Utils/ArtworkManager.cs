using System.Collections.Generic;
using System.Windows.Media.Imaging;
using SharkTracker.Models;

namespace SharkTracker.Utils
{
    public class ArtworkManager
    {

        // ATTRIBUTES

        private static ArtworkManager _instance;

        public static ArtworkManager Instance => _instance ??= new ArtworkManager();


        private Dictionary<string, BitmapSource> _artwork;
        
        // CONSTRUCTORS

        private ArtworkManager()
        {
            _artwork = new Dictionary<string, BitmapSource>();
        }

        // public void async Init(List<Card> cards)
        // {
        //     foreach (Card c in cards)
        //     {
        //         _artwork.Add(c.Code, );
        //     }
        // }

        // METHODS

        public BitmapSource GetBitmapSource(string cardCode)
        {
            return _artwork.ContainsKey(cardCode) ? _artwork[cardCode] : null;
        }
    }
}