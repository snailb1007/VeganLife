using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeganLife.Data.FireBaseData
{
    public class FirebaseRealtimeData
    {
        const string _linkOfFirebaseClient = "https://vegan-life-d1c9b-default-rtdb.firebaseio.com/";
        public FirebaseClient FirebaseDatabase { get; private set; }

        public FirebaseRealtimeData()
        {
            FirebaseDatabase = new FirebaseClient(_linkOfFirebaseClient);
        }

        public async Task<string> GetBackgroundImage(string goal)
        {
            var result = new Dictionary<string, string>();
            var data = await FirebaseDatabase.Child($"Backgrounds/Themes/{goal}").OnceAsync<string>();
            return data.Select(item => item.Object.ToString()).FirstOrDefault();
        }
    }
}
